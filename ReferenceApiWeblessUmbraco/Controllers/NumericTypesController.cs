using ReferenceApiWeblessUmbraco.Controllers.Base;
using ReferenceApiWeblessUmbraco.Enums;
using ReferenceApiWeblessUmbraco.Models;

namespace ReferenceApiWeblessUmbraco.Controllers
{
    public class NumericTypesController : UmbracoDataController
    {
        public NumericTypesViewModel Get()
        {
            var item = GetById<NumericTypesViewModel>((int)UmbracoId.NumericTypes);
            return item;
        }
    }
}
