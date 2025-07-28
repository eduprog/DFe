using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unimake.Business.DFe.SourceGenerators
{
    [Generator]
    public class DtoSourceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not SyntaxReceiver receiver)
                return;

            foreach (var candidateClass in receiver.CandidateClasses)
            {
                var model = context.Compilation.GetSemanticModel(candidateClass.SyntaxTree);
                var classSymbol = model.GetDeclaredSymbol(candidateClass) as INamedTypeSymbol;

                if (classSymbol == null)
                    continue;

                var generateDtoAttribute = GetGenerateDtoAttribute(classSymbol);
                if (generateDtoAttribute == null)
                    continue;

                foreach (var dtoType in generateDtoAttribute.Types)
                {
                    var dtoCode = GenerateDto(classSymbol, dtoType, generateDtoAttribute);
                    if (!string.IsNullOrEmpty(dtoCode))
                    {
                        var fileName = $"{classSymbol.Name}{GetDtoSuffix(dtoType, generateDtoAttribute.CustomSuffix)}.g.cs";
                        context.AddSource(fileName, SourceText.From(dtoCode, Encoding.UTF8));
                    }
                }
            }
        }

        private GenerateDtoAttributeData? GetGenerateDtoAttribute(INamedTypeSymbol classSymbol)
        {
            var attribute = classSymbol.GetAttributes()
                .FirstOrDefault(attr => attr.AttributeClass?.Name == "GenerateDtoAttribute");

            if (attribute == null)
                return null;

            var types = new List<DtoType>();
            if (attribute.ConstructorArguments.Length > 0)
            {
                var typesArray = attribute.ConstructorArguments[0].Values;
                foreach (var typeValue in typesArray)
                {
                    if (Enum.TryParse<DtoType>(typeValue.Value?.ToString(), out var dtoType))
                    {
                        types.Add(dtoType);
                    }
                }
            }

            if (types.Count == 0)
                types.Add(DtoType.Request);

            var customNamespace = attribute.NamedArguments
                .FirstOrDefault(arg => arg.Key == "CustomNamespace").Value.Value?.ToString();

            var customSuffix = attribute.NamedArguments
                .FirstOrDefault(arg => arg.Key == "CustomSuffix").Value.Value?.ToString();

            return new GenerateDtoAttributeData
            {
                Types = types.ToArray(),
                CustomNamespace = customNamespace,
                CustomSuffix = customSuffix
            };
        }

        private string GenerateDto(INamedTypeSymbol classSymbol, DtoType dtoType, GenerateDtoAttributeData attributeData)
        {
            var namespaceName = attributeData.CustomNamespace ?? $"{classSymbol.ContainingNamespace}.Dtos";
            var className = $"{classSymbol.Name}{GetDtoSuffix(dtoType, attributeData.CustomSuffix)}";

            var sb = new StringBuilder();

            // Using statements
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.ComponentModel.DataAnnotations;");
            sb.AppendLine("using System.Text.Json.Serialization;");
            sb.AppendLine();

            // Namespace
            sb.AppendLine($"namespace {namespaceName}");
            sb.AppendLine("{");

            // Class declaration
            sb.AppendLine($"    /// <summary>");
            sb.AppendLine($"    /// DTO gerado automaticamente para {classSymbol.Name} - Tipo: {dtoType}");
            sb.AppendLine($"    /// </summary>");
            sb.AppendLine($"    public partial class {className}");
            sb.AppendLine("    {");

            // Properties
            var properties = GetFilteredProperties(classSymbol, dtoType);
            foreach (var property in properties)
            {
                GenerateProperty(sb, property, dtoType);
            }

            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private IEnumerable<IPropertySymbol> GetFilteredProperties(INamedTypeSymbol classSymbol, DtoType dtoType)
        {
            var properties = classSymbol.GetMembers()
                .OfType<IPropertySymbol>()
                .Where(p => p.DeclaredAccessibility == Accessibility.Public)
                .Where(p => !ShouldExcludeProperty(p, dtoType));

            return properties;
        }

        private bool ShouldExcludeProperty(IPropertySymbol property, DtoType dtoType)
        {
            // Excluir propriedades com XmlIgnore
            if (property.GetAttributes().Any(attr => attr.AttributeClass?.Name == "XmlIgnoreAttribute"))
                return true;

            // Excluir propriedades marcadas com ExcludeFromDto
            var excludeAttribute = property.GetAttributes()
                .FirstOrDefault(attr => attr.AttributeClass?.Name == "ExcludeFromDtoAttribute");

            if (excludeAttribute != null)
            {
                // Se não especificou tipos, exclui de todos
                if (excludeAttribute.ConstructorArguments.Length == 0)
                    return true;

                // Verifica se o tipo atual está na lista de exclusão
                var excludeTypes = excludeAttribute.ConstructorArguments[0].Values;
                foreach (var excludeType in excludeTypes)
                {
                    if (Enum.TryParse<DtoType>(excludeType.Value?.ToString(), out var excludeDtoType) && 
                        excludeDtoType == dtoType)
                    {
                        return true;
                    }
                }
            }

            // Excluir algumas propriedades específicas baseadas no tipo do DTO
            var propertyName = property.Name;
            return dtoType switch
            {
                DtoType.IncluirRequest => propertyName is "Id" or "DataCriacao" or "DataAtualizacao",
                DtoType.AtualizarRequest => propertyName is "DataCriacao",
                _ => false
            };
        }

        private void GenerateProperty(StringBuilder sb, IPropertySymbol property, DtoType dtoType)
        {
            var propertyName = GetPropertyName(property);
            var propertyType = GetPropertyType(property);
            var isRequired = IsPropertyRequired(property, dtoType);

            // Summary
            sb.AppendLine("        /// <summary>");
            sb.AppendLine($"        /// {property.Name}");
            sb.AppendLine("        /// </summary>");

            // Attributes
            if (isRequired)
            {
                sb.AppendLine("        [Required]");
            }

            sb.AppendLine($"        [JsonPropertyName(\"{ToCamelCase(propertyName)}\")]");

            // Property declaration
            sb.AppendLine($"        public {propertyType} {propertyName} {{ get; set; }}");
            sb.AppendLine();
        }

        private string GetPropertyName(IPropertySymbol property)
        {
            var nameAttribute = property.GetAttributes()
                .FirstOrDefault(attr => attr.AttributeClass?.Name == "DtoPropertyNameAttribute");

            if (nameAttribute?.ConstructorArguments.Length > 0)
            {
                return nameAttribute.ConstructorArguments[0].Value?.ToString() ?? property.Name;
            }

            return property.Name;
        }

        private string GetPropertyType(IPropertySymbol property)
        {
            var type = property.Type;
            
            // Converter tipos XML para tipos apropriados para JSON
            var typeName = type.ToDisplayString();

            // Mapear alguns tipos específicos
            return typeName switch
            {
                "System.Xml.XmlDocument" => "string",
                "System.Xml.XmlElement" => "string",
                _ when typeName.StartsWith("System.Xml") => "object",
                _ => typeName
            };
        }

        private bool IsPropertyRequired(IPropertySymbol property, DtoType dtoType)
        {
            var requiredAttribute = property.GetAttributes()
                .FirstOrDefault(attr => attr.AttributeClass?.Name == "DtoRequiredAttribute");

            if (requiredAttribute != null)
            {
                // Se não especificou tipos, é obrigatório em todos
                if (requiredAttribute.ConstructorArguments.Length == 0)
                    return true;

                // Verifica se o tipo atual está na lista de obrigatórios
                var requiredTypes = requiredAttribute.ConstructorArguments[0].Values;
                foreach (var requiredType in requiredTypes)
                {
                    if (Enum.TryParse<DtoType>(requiredType.Value?.ToString(), out var reqDtoType) && 
                        reqDtoType == dtoType)
                    {
                        return true;
                    }
                }
            }

            // Regras padrão baseadas no tipo
            return dtoType switch
            {
                DtoType.IncluirRequest => !property.Type.CanBeReferencedByName || 
                                        property.GetAttributes().Any(attr => attr.AttributeClass?.Name == "XmlAttributeAttribute"),
                _ => false
            };
        }

        private string GetDtoSuffix(DtoType dtoType, string? customSuffix)
        {
            if (!string.IsNullOrEmpty(customSuffix))
                return customSuffix;

            return dtoType switch
            {
                DtoType.Request => "Request",
                DtoType.Response => "Response",
                DtoType.IncluirRequest => "IncluirRequest",
                DtoType.AtualizarRequest => "AtualizarRequest",
                DtoType.ExcluirRequest => "ExcluirRequest",
                DtoType.ConsultarRequest => "ConsultarRequest",
                _ => "Dto"
            };
        }

        private static string ToCamelCase(string input)
        {
            if (string.IsNullOrEmpty(input) || char.IsLower(input[0]))
                return input;

            return char.ToLowerInvariant(input[0]) + input.Substring(1);
        }

        private class SyntaxReceiver : ISyntaxReceiver
        {
            public List<ClassDeclarationSyntax> CandidateClasses { get; } = new();

            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                if (syntaxNode is ClassDeclarationSyntax classDeclaration &&
                    classDeclaration.AttributeLists.Count > 0)
                {
                    CandidateClasses.Add(classDeclaration);
                }
            }
        }

        private class GenerateDtoAttributeData
        {
            public DtoType[] Types { get; set; } = Array.Empty<DtoType>();
            public string? CustomNamespace { get; set; }
            public string? CustomSuffix { get; set; }
        }

        private enum DtoType
        {
            Request,
            Response,
            IncluirRequest,
            AtualizarRequest,
            ExcluirRequest,
            ConsultarRequest
        }
    }
}
