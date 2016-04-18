using System.Web.Http;
using ReferenceApiWeblessUmbraco.Enums;
using ReferenceApiWeblessUmbraco.Models;
using umbraco;
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
        public NumericTypesViewModel NumericTypes()
        {
            var item = GetById<NumericTypesViewModel>(UmbracoId.NumericTypes);
            return item;
        }

        [HttpGet]
        [Route("text")]
        public TextTypesViewModel TextTypes()
        {
            var item = GetById<TextTypesViewModel>(UmbracoId.TextTypes);
            return item;
        }

        protected TEntity GetById<TEntity>(UmbracoId id)
        {
            var viewModel = Vault.Context.GetContentById<TEntity>((int)id);

            return viewModel;
        }
    }
}