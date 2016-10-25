using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ReferenceWebsite.Models;

using UmbracoVault;
using UmbracoVault.Proxy;
using UmbracoVault.Reflection;

namespace ReferenceWebsite.Controllers
{
    public class ProxyTestsController : Controller
    {
        public ActionResult ValueShouldMatchCmsContent()
        {
            ClassConstructor.SetInstanceFactory(new DefaultInstanceFactory());
            var referenceModel = Vault.Context.GetByDocumentType<BlogEntryViewModel>().FirstOrDefault();

            ClassConstructor.SetInstanceFactory(new ProxyInstanceFactory());
            var proxyModel = Vault.Context.GetContentById<BlogEntryViewModel>(referenceModel.CmsContent.Id);

            var result = Content(FormatResult(referenceModel.Content, referenceModel.Content, proxyModel.Content), "text/html");

            ClassConstructor.SetInstanceFactory(new DefaultInstanceFactory());

            return result;
        }

        public ActionResult ValueShouldBeOverWritten()
        {
            const string OverrideContent = "This content has been overwritten";

            ClassConstructor.SetInstanceFactory(new DefaultInstanceFactory());
            var referenceModel = Vault.Context.GetByDocumentType<BlogEntryViewModel>().FirstOrDefault();

            ClassConstructor.SetInstanceFactory(new ProxyInstanceFactory());
            var proxyModel = Vault.Context.GetContentById<BlogEntryViewModel>(referenceModel.CmsContent.Id);
            proxyModel.Content = OverrideContent;

            var result = Content(FormatResult(referenceModel.Content, OverrideContent, proxyModel.Content), "text/html");

            ClassConstructor.SetInstanceFactory(new DefaultInstanceFactory());

            return result;
        }

        private static string FormatResult(string cmsContent, string expectedContent, string actualContent)
        {
            return string.Format("{0}{1}<br/><br/>{2}{3}<br/><br/>{4}{5}", FormatTitle("CMS Content:"), cmsContent, FormatTitle("Expected Content:"), expectedContent, FormatTitle("Actual Content:"), actualContent);
        }

        private static string FormatTitle(string title)
        {
            return string.Format("<p><strong>{0}</strong></p><br/>", title);
        }
    }
}