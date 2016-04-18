using System.Web.Http;
using Umbraco.Core;
using Umbraco.Core.Services;
using UmbracoVault;

namespace ReferenceApiWeblessUmbraco.Controllers.Base
{
    public abstract class UmbracoDataController : ApiController
    {
        protected readonly ServiceContext ServiceContext;

        protected UmbracoDataController()
        {
            ServiceContext = ApplicationContext.Current.Services;
        }

        protected TEntity GetById<TEntity>(int id)
        {
            var viewModel = Vault.Context.GetContentById<TEntity>(id);

            return viewModel;
        }
    }
}