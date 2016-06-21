using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ReferenceApiWeblessUmbraco.Enums;
using ReferenceApiWeblessUmbraco.Models;

using Umbraco.Core;
using Umbraco.Core.Services;

using UmbracoVault;

namespace ReferenceApiWeblessUmbraco.Controllers
{
    [RoutePrefix("api/vault")]
    public class UmbracoDataController : ApiController
    {
        protected readonly ServiceContext ServiceContext;

        protected UmbracoDataController()
        {
            ServiceContext = ApplicationContext.Current.Services;
        }

        [HttpGet]
        [Route("numeric")]
        public HttpResponseMessage NumericTypes()
        {
            return GetById<NumericTypesViewModel>(UmbracoId.NumericTypes);
        }

        [HttpGet]
        [Route("text")]
        public HttpResponseMessage TextTypes()
        {
            return GetById<TextTypesViewModel>(UmbracoId.TextTypes);
        }

        [HttpGet]
        [Route("arrays")]
        public HttpResponseMessage Arrays()
        {
            return GetById<ArraysViewModel>(UmbracoId.Arrays);
        }

        [HttpGet]
        [Route("objects")]
        public HttpResponseMessage Objects()
        {
            return GetById<ObjectsViewModel>(UmbracoId.Objects);
        }

        [HttpGet]
        [Route("enums")]
        public HttpResponseMessage Enums()
        {
            return GetById<EnumsViewModel>(UmbracoId.Enums);
        }

        [HttpGet]
        [Route("members")]
        public HttpResponseMessage Members()
        {
            var viewModel = Vault.Context.GetMemberById<WebsiteUser>((int)UmbracoId.SingleUser);

            var response = Request.CreateResponse(HttpStatusCode.OK, viewModel);
            return response;
        }

        protected HttpResponseMessage GetById<TEntity>(UmbracoId id)
        {
            var viewModel = Vault.Context.GetContentById<TEntity>((int)id);

            var response = Request.CreateResponse(HttpStatusCode.OK, viewModel);
            return response;
        }
    }
}