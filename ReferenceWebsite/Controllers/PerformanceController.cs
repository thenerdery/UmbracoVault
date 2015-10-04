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

namespace ReferenceWebsite.Controllers
{
    public class PerformanceController : VaultDefaultGenericController
    {
        #region Content readers - vary how much content is read
        private static readonly Dictionary<string, Action<List<BlogEntryViewModel>, BlogEntryViewModel>> _blogPostReaders =
            new Dictionary<string, Action<List<BlogEntryViewModel>, BlogEntryViewModel>>
            {
                { "none", (r, b) => { } },
                {
                    "title", (r, b) =>
                    {
                        var refPost = r.First(o => o.CmsContent.Id == b.CmsContent.Id);
                        Debug.Assert(refPost.Title.Equals(b.Title));
                    }
                },
                {
                    "titleContent", (r, b) =>
                    {
                        var refPost = r.First(o => o.CmsContent.Id == b.CmsContent.Id);
                        Debug.Assert(refPost.Title.Equals(b.Title));
                        Debug.Assert(refPost.Content.Equals(b.Content));
                    }
                },
                {
                    "all", (r, b) =>
                    {
                        var refPost = r.First(o => o.CmsContent.Id == b.CmsContent.Id);
                        Debug.Assert(refPost.Title.Equals(b.Title));
                        Debug.Assert(refPost.Content.Equals(b.Content));
                        Debug.Assert(refPost.PostDate.Equals(b.PostDate));
                        Debug.Assert(
                            (refPost.PostImage == null && b.PostImage == null) ||
                            (refPost.PostImage.Url.Equals(b.PostImage.Url)));
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
                        Debug.Assert(refPost.Text.Equals(b.Text));
                    }
                },
                {
                    "six", (r, b) =>
                    {
                        var refPost = r.First(o => o.CmsContent.Id == b.CmsContent.Id);
                        Debug.Assert(refPost.Text.Equals(b.Text));
                        Debug.Assert(refPost.Text2.Equals(b.Text2));
                        Debug.Assert(refPost.Text3.Equals(b.Text3));
                        Debug.Assert(refPost.ContentPicker.CmsContent.Id.Equals(b.ContentPicker.CmsContent.Id));
                        Debug.Assert(refPost.DictionaryPicker.Length.Equals(b.DictionaryPicker.Length));
                        Debug.Assert(refPost.CustomColorSelect.SequenceEqual(b.CustomColorSelect));
                    }
                },
                {
                    "all", (r, b) =>
                    {
                        var refPost = r.First(o => o.CmsContent.Id == b.CmsContent.Id);
                        Debug.Assert(refPost.Text.Equals(b.Text));
                        Debug.Assert(refPost.Text2.Equals(b.Text2));
                        Debug.Assert(refPost.Text3.Equals(b.Text3));
                        Debug.Assert(refPost.Text4.Equals(b.Text4));
                        Debug.Assert(refPost.Text5.Equals(b.Text5));
                        Debug.Assert(refPost.ContentPicker.CmsContent.Id.Equals(b.ContentPicker.CmsContent.Id));
                        Debug.Assert(refPost.ContentPicker2.CmsContent.Id.Equals(b.ContentPicker2.CmsContent.Id));
                        Debug.Assert(refPost.DictionaryPicker.Length.Equals(b.DictionaryPicker.Length));
                        Debug.Assert(refPost.CustomColorSelect.SequenceEqual(b.CustomColorSelect));
                        Debug.Assert(refPost.IntArray.SequenceEqual(b.IntArray));
                        Debug.Assert(refPost.StringArray.SequenceEqual(b.StringArray));
                        Debug.Assert(refPost.CheckboxList.SequenceEqual(b.CheckboxList));
                        Debug.Assert(refPost.DropDownListMultiple.SequenceEqual(b.DropDownListMultiple));
                        Debug.Assert(refPost.DropDownListMultiplePublishKeys.Equals(b.DropDownListMultiplePublishKeys));
                        Debug.Assert(refPost.StringArray.SequenceEqual(b.StringArray));
                        Debug.Assert(refPost.StaffList.Select(m => m.Name).SequenceEqual(b.StaffList.Select(m => m.Name)));
                    }
                }
           };
        #endregion

        public ActionResult RunProxyPerfTests(int iterations = 500)
        {
            var content = new List<string>();

            var blogReferences = LoadPosts();
            var largeDocReference = LoadLargeDocs();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Blog Post default loader
            RunIterations(LoadPosts, _blogPostReaders, iterations, content, "default", blogReferences);
            content.AddRange(new[] { string.Empty, string.Empty });

            // Blog Post proxy Loader
            ClassConstructor.SetInstanceFactory(new ProxyFactory());
            RunIterations(LoadPosts, _blogPostReaders, iterations, content, "proxy", blogReferences);
            ClassConstructor.SetInstanceFactory(new DefaultInstanceFactory());
            content.AddRange(new[] { string.Empty, string.Empty, string.Empty });

            // Large docs default loader
            RunIterations(LoadLargeDocs, _largeDocumentReaders, iterations, content, "default", largeDocReference);
            content.AddRange(new[] { string.Empty, string.Empty });

            // Large docs proxy loader
            ClassConstructor.SetInstanceFactory(new ProxyFactory());
            RunIterations(LoadLargeDocs, _largeDocumentReaders, iterations, content, "proxy", largeDocReference);
            ClassConstructor.SetInstanceFactory(new DefaultInstanceFactory());

            return new ContentResult
            {
                Content = string.Join("<br/>", content.ToArray()),
                ContentType = "text/html"
            };
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
    }
}