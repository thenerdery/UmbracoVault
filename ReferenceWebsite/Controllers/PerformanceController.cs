using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

using ReferenceWebsite.Models;

using UmbracoVault;
using UmbracoVault.Controllers;
using UmbracoVault.Proxy.Concrete;
using UmbracoVault.Reflection;
// ReSharper disable All

namespace ReferenceWebsite.Controllers
{
    public class PerformanceController : VaultDefaultGenericController
    {
        // Maintains load times by property type
        private static Dictionary<Type, List<long>> _paramLoadTimes;

        // Used to ensure only proxy-loaded properties are included in param load times
        private static bool _currentIsProxy;

        public PerformanceController()
        {
            _paramLoadTimes = new Dictionary<Type, List<long>>();
        }

        public ActionResult RunProxyPerfTests(int iterations = 500)
        {
            var output = new List<string>();

            var blogReferences = LoadPosts();
            var largeDocReference = LoadLargeDocs();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            RunDefaultBlogLoad(iterations, output, blogReferences);
            AddEmptyLines(output, 2);

            RunProxyBlogLoad(iterations, output, blogReferences);
            AddEmptyLines(output, 4);

            RunDefaultLargeDocLoad(iterations, output, largeDocReference);
            AddEmptyLines(output, 2);

            RunProxyLargeDocLoad(iterations, output, largeDocReference);
            AddEmptyLines(output, 4);

            AddPropertyLoadTimes(output);

            return new ContentResult
            {
                Content = string.Join("<br/>", output.ToArray()),
                ContentType = "text/html"
            };
        }

        private static void AddPropertyLoadTimes(List<string> output)
        {
            foreach (var time in _paramLoadTimes)
            {
                output.Add($"Type {time.Key.Name} loaded in average time {time.Value.Average()} ticks");
            }
        }

        private static void AddEmptyLines(List<string> output, int count)
        {
            for (int i = 0; i < count; i++)
            {
               output.Add(string.Empty);
            }
        }

        private static void RunProxyLargeDocLoad(int iterations, List<string> output, List<LargeDocumentViewModel> largeDocReference)
        {
            ClassConstructor.SetInstanceFactory(new ProxyFactory());
            _currentIsProxy = true;
            RunIterations(LoadLargeDocs, _largeDocumentReaders, iterations, output, "proxy", largeDocReference);
            ClassConstructor.SetInstanceFactory(new DefaultInstanceFactory());
        }

        private static void RunDefaultLargeDocLoad(int iterations, List<string> output, List<LargeDocumentViewModel> largeDocReference)
        {
            _currentIsProxy = false;
            RunIterations(LoadLargeDocs, _largeDocumentReaders, iterations, output, "default", largeDocReference);
        }

        private static void RunProxyBlogLoad(int iterations, List<string> output, List<BlogEntryViewModel> blogReferences)
        {
            ClassConstructor.SetInstanceFactory(new ProxyFactory());
            _currentIsProxy = true;
            RunIterations(LoadPosts, _blogPostReaders, iterations, output, "proxy", blogReferences);
            ClassConstructor.SetInstanceFactory(new DefaultInstanceFactory());
        }

        private static void RunDefaultBlogLoad(int iterations, List<string> output, List<BlogEntryViewModel> blogReferences)
        {
            _currentIsProxy = false;
            RunIterations(LoadPosts, _blogPostReaders, iterations, output, "default", blogReferences);
        }

        private static void RunIterations<T>(Func<List<T>> loader, Dictionary<string, Action<List<T>, T>> readers,  int iterations, ICollection<string> content, string proxyLabel, List<T> referenceContent)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            foreach (var reader in readers)
            {
                var watch = Stopwatch.StartNew();
                var count = 0;
                for (var i = 0; i < iterations; i++)
                {
                    var docs = loader();
                    count = docs.Count;
                    foreach (var post in docs)
                    {
                        reader.Value(referenceContent, post);
                    }
                }
                watch.Stop();
                content.Add($"Loaded {count} {typeof(T).Name} using {proxyLabel} {iterations} times reading {reader.Key}, taking {watch.ElapsedMilliseconds}ms");
            }
        }

        private static List<LargeDocumentViewModel> LoadLargeDocs()
        {
            return Vault.Context.GetByDocumentType<LargeDocumentViewModel>().ToList();
        } 

        private static List<BlogEntryViewModel> LoadPosts()
        {
            return Vault.Context.GetByDocumentType<BlogEntryViewModel>().ToList();
        }


        #region Content readers - vary how much content is read

        private static void LoadAndTime(Type paramType, Func<bool> paramReadAction)
        {
            if (!_currentIsProxy)
            {
                return;
            }

            if (!_paramLoadTimes.ContainsKey(paramType))
            {
                _paramLoadTimes.Add(paramType, new List<long>());
            }

            var watch = Stopwatch.StartNew();
            Debug.Assert(paramReadAction());
            watch.Stop();

            _paramLoadTimes[paramType].Add(watch.ElapsedTicks);
        }

        private static readonly Dictionary<string, Action<List<BlogEntryViewModel>, BlogEntryViewModel>> _blogPostReaders =
            new Dictionary<string, Action<List<BlogEntryViewModel>, BlogEntryViewModel>>
            {
                { "none", (r, b) => { } },
                {
                    "title", (r, b) =>
                    {
                        var refPost = r.First(o => o.CmsContent.Id == b.CmsContent.Id);
                        LoadAndTime(refPost.Title.GetType(), () => refPost.Title.Equals(b.Title));
                    }
                },
                {
                    "titleContent", (r, b) =>
                    {
                        var refPost = r.First(o => o.CmsContent.Id == b.CmsContent.Id);
                        LoadAndTime(refPost.Title.GetType(), () => refPost.Title.Equals(b.Title));
                        LoadAndTime(refPost.Content.GetType(), () => refPost.Content.Equals(b.Content));
                    }
                },
                {
                    "all", (r, b) =>
                    {
                        var refPost = r.First(o => o.CmsContent.Id == b.CmsContent.Id);
                        LoadAndTime(refPost.Title.GetType(), () => refPost.Title.Equals(b.Title));
                        LoadAndTime(refPost.Content.GetType(), () => refPost.Content.Equals(b.Content));
                        LoadAndTime(refPost.PostDate.GetType(), () => refPost.PostDate.Equals(b.PostDate));
                        LoadAndTime(typeof(Image), () =>
                        {
                            return (refPost.PostImage == null && b.PostImage == null) ||
                            // ReSharper disable once PossibleNullReferenceException
                            (refPost.PostImage.Url.Equals(b.PostImage.Url));
                        });
                    }
                }
            };

        private static readonly Dictionary<string, Action<List<LargeDocumentViewModel>, LargeDocumentViewModel>> _largeDocumentReaders =
           new Dictionary<string, Action<List<LargeDocumentViewModel>, LargeDocumentViewModel>>
           {
                { "none", (r, b) => { } },
                {
                    "one", (r, b) =>
                    {
                        var refPost = r.First(o => o.CmsContent.Id == b.CmsContent.Id);
                        LoadAndTime(refPost.Text.GetType(), () => refPost.Text.Equals(b.Text));
                    }
                },
                {
                    "six", (r, b) =>
                    {
                        var refPost = r.First(o => o.CmsContent.Id == b.CmsContent.Id);
                        LoadAndTime(refPost.Text.GetType(), () => refPost.Text.Equals(b.Text));
                        LoadAndTime(refPost.Text2.GetType(), () => refPost.Text2.Equals(b.Text2));
                        LoadAndTime(refPost.Text3.GetType(), () => refPost.Text3.Equals(b.Text3));
                        LoadAndTime(refPost.ContentPicker.GetType(), () => refPost.ContentPicker.Equals(b.ContentPicker));
                        LoadAndTime(refPost.DictionaryPicker.GetType(), () => refPost.DictionaryPicker.Length.Equals(b.DictionaryPicker.Length));
                        LoadAndTime(refPost.CustomColorSelect.GetType(), () => refPost.CustomColorSelect.SequenceEqual(b.CustomColorSelect));
                    }
                },
                {
                    "all", (r, b) =>
                    {
                        var refPost = r.First(o => o.CmsContent.Id == b.CmsContent.Id);
                        LoadAndTime(refPost.Text.GetType(), () => refPost.Text.Equals(b.Text));
                        LoadAndTime(refPost.Text2.GetType(), () => refPost.Text2.Equals(b.Text2));
                        LoadAndTime(refPost.Text3.GetType(), () => refPost.Text3.Equals(b.Text3));
                        LoadAndTime(refPost.Text4.GetType(), () => refPost.Text4.Equals(b.Text4));
                        LoadAndTime(refPost.Text5.GetType(), () => refPost.Text5.Equals(b.Text5));

                        LoadAndTime(refPost.ContentPicker.GetType(), () => refPost.ContentPicker.Equals(b.ContentPicker));
                        LoadAndTime(refPost.ContentPicker2.GetType(), () => refPost.ContentPicker2.Equals(b.ContentPicker2));
                        LoadAndTime(refPost.DictionaryPicker.GetType(), () => refPost.DictionaryPicker.Length.Equals(b.DictionaryPicker.Length));
                        LoadAndTime(refPost.CustomColorSelect.GetType(), () => refPost.CustomColorSelect.SequenceEqual(b.CustomColorSelect));
                        LoadAndTime(refPost.IntArray.GetType(), () => refPost.IntArray.SequenceEqual(b.IntArray));
                        LoadAndTime(refPost.StringArray.GetType(), () => refPost.StringArray.SequenceEqual(b.StringArray));
                        LoadAndTime(refPost.CheckboxList.GetType(), () => refPost.CheckboxList.SequenceEqual(b.CheckboxList));
                        LoadAndTime(refPost.DropDownListMultiple.GetType(), () => refPost.DropDownListMultiple.SequenceEqual(b.DropDownListMultiple));
                        LoadAndTime(refPost.DropDownListMultiplePublishKeys.GetType(), () => refPost.DropDownListMultiplePublishKeys.Equals(b.DropDownListMultiplePublishKeys));
                        LoadAndTime(refPost.StaffList.GetType(), () => refPost.StaffList.Select(m => m.Name).SequenceEqual(b.StaffList.Select(m => m.Name)));
                    }
                }
           };

        #endregion

    }
}