using UmbracoVault.Attributes;

namespace UmbracoVault.Tests.Models
{
    [UmbracoEntity(AutoMap = true)]
    public class ExampleRecursiveAttributesViewModel : BaseRecursiveAttributesViewModel
    {
    }

    [UmbracoEntity(AutoMap = true)]
    public class BaseRecursiveAttributesViewModel
    {
    }
}
