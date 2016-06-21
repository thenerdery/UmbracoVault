using umbraco.editorControls;
using umbraco.interfaces;
using Umbraco.Core;

namespace ReferenceApiWeblessUmbraco.Application
{
    public class ReferenceApiBootManager : CoreBootManager
    {
        public ReferenceApiBootManager(UmbracoApplicationBase umbracoApplication, string baseDirectory)
            : base(umbracoApplication)
        {
            //This is only here to ensure references to the assemblies needed for the DataTypesResolver
            //otherwise they won't be loaded into the AppDomain.
            var interfacesAssemblyName = typeof(IDataType).Assembly.FullName;
            var editorControlsAssemblyName = typeof(uploadField).Assembly.FullName;

            base.InitializeApplicationRootPath(baseDirectory);
        }

    }
}