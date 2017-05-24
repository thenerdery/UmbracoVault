using UmbracoVault.Attributes;

namespace UmbracoVault.Tests.Models
{
    [UmbracoEntity("recursiveModelWithAliases")]
    public class RecursiveModelWithAliases : ExampleRecursiveAttributesViewModel
    {
    }

    [UmbracoEntity("secondLevelRecursiveModelWithAliases")]
    public class SecondLevelRecursiveModelWithAliases : RecursiveModelWithAliases
    {
        
    }
}
