using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Unimake.Business.DFe.SourceGenerators.Tests
{
    public class DtoSourceGeneratorTests
    {
        [Fact]
        public void Generator_Should_Generate_Request_Dto()
        {
            // Arrange
            var source = @"
using System;
using System.Collections.Generic;
using Unimake.Business.DFe.SourceGenerators.Attributes;

namespace TestNamespace
{
    [GenerateDto(DtoType.Request)]
    public class TestClass
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public List<string> Items { get; set; }
    }
}";

            // Act
            var result = RunGenerator(source);

            // Assert
            Assert.True(result.GeneratedSources.Length > 0);
            var generatedSource = result.GeneratedSources[0].SourceText.ToString();
            
            Assert.Contains("public partial class TestClassRequest", generatedSource);
            Assert.Contains("[JsonPropertyName(\"name\")]", generatedSource);
            Assert.Contains("public string Name { get; set; }", generatedSource);
        }

        [Fact]
        public void Generator_Should_Exclude_XmlIgnore_Properties()
        {
            // Arrange
            var source = @"
using System;
using System.Xml.Serialization;
using Unimake.Business.DFe.SourceGenerators.Attributes;

namespace TestNamespace
{
    [GenerateDto(DtoType.Request)]
    public class TestClass
    {
        public string Name { get; set; }
        
        [XmlIgnore]
        public string IgnoredProperty { get; set; }
    }
}";

            // Act
            var result = RunGenerator(source);

            // Assert
            var generatedSource = result.GeneratedSources[0].SourceText.ToString();
            
            Assert.Contains("public string Name { get; set; }", generatedSource);
            Assert.DoesNotContain("IgnoredProperty", generatedSource);
        }

        [Fact]
        public void Generator_Should_Respect_ExcludeFromDto_Attribute()
        {
            // Arrange
            var source = @"
using System;
using Unimake.Business.DFe.SourceGenerators.Attributes;

namespace TestNamespace
{
    [GenerateDto(DtoType.Request)]
    public class TestClass
    {
        public string Name { get; set; }
        
        [ExcludeFromDto(DtoType.Request)]
        public string ExcludedProperty { get; set; }
    }
}";

            // Act
            var result = RunGenerator(source);

            // Assert
            var generatedSource = result.GeneratedSources[0].SourceText.ToString();
            
            Assert.Contains("public string Name { get; set; }", generatedSource);
            Assert.DoesNotContain("ExcludedProperty", generatedSource);
        }

        [Fact]
        public void Generator_Should_Add_Required_Attribute()
        {
            // Arrange
            var source = @"
using System;
using System.ComponentModel.DataAnnotations;
using Unimake.Business.DFe.SourceGenerators.Attributes;

namespace TestNamespace
{
    [GenerateDto(DtoType.Request)]
    public class TestClass
    {
        [DtoRequired(DtoType.Request)]
        public string RequiredProperty { get; set; }
        
        public string OptionalProperty { get; set; }
    }
}";

            // Act
            var result = RunGenerator(source);

            // Assert
            var generatedSource = result.GeneratedSources[0].SourceText.ToString();
            
            Assert.Contains("[Required]", generatedSource);
            Assert.Contains("public string RequiredProperty { get; set; }", generatedSource);
        }

        [Fact]
        public void Generator_Should_Use_Custom_Property_Name()
        {
            // Arrange
            var source = @"
using System;
using Unimake.Business.DFe.SourceGenerators.Attributes;

namespace TestNamespace
{
    [GenerateDto(DtoType.Request)]
    public class TestClass
    {
        [DtoPropertyName(""customName"")]
        public string OriginalName { get; set; }
    }
}";

            // Act
            var result = RunGenerator(source);

            // Assert
            var generatedSource = result.GeneratedSources[0].SourceText.ToString();
            
            Assert.Contains("[JsonPropertyName(\"customName\")]", generatedSource);
            Assert.Contains("public string customName { get; set; }", generatedSource);
        }

        private static GeneratorDriverRunResult RunGenerator(string source)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(source);

            var references = new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.ComponentModel.DataAnnotations.RequiredAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Xml.Serialization.XmlIgnoreAttribute).Assembly.Location)
            };

            var compilation = CSharpCompilation.Create(
                "TestAssembly",
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            var generator = new DtoSourceGenerator();

            var driver = CSharpGeneratorDriver.Create(generator);
            return driver.RunGeneratorsAndUpdateCompilation(compilation, out _, out _);
        }
    }
}
