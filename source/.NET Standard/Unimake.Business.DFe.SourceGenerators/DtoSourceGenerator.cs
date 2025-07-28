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

                // Verificar primeiro se é uma configuração de Source Generator (método wrapper)
                var sourceGeneratorConfig = GetSourceGeneratorConfigAttribute(classSymbol);
                if (sourceGeneratorConfig != null)
                {
                    ProcessSourceGeneratorConfig(context, classSymbol, sourceGeneratorConfig);
                    continue;
                }

                // Método original para classes marcadas diretamente
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

        private SourceGeneratorConfigData? GetSourceGeneratorConfigAttribute(INamedTypeSymbol classSymbol)
        {
            var attribute = classSymbol.GetAttributes()
                .FirstOrDefault(attr => attr.AttributeClass?.Name == "SourceGeneratorConfigAttribute");

            if (attribute == null)
                return null;

            // Extrair tipo da classe base
            var baseClassType = attribute.ConstructorArguments[0].Value as INamedTypeSymbol;
            if (baseClassType == null)
                return null;

            // Extrair tipos de DTO
            var types = new List<DtoType>();
            if (attribute.ConstructorArguments.Length > 1)
            {
                var typesArray = attribute.ConstructorArguments[1].Values;
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

            // Extrair argumentos nomeados
            var customNamespace = attribute.NamedArguments
                .FirstOrDefault(arg => arg.Key == "CustomNamespace").Value.Value?.ToString();

            var classPrefix = attribute.NamedArguments
                .FirstOrDefault(arg => arg.Key == "ClassPrefix").Value.Value?.ToString();

            var useWrapperName = false;
            var useWrapperNameArg = attribute.NamedArguments
                .FirstOrDefault(arg => arg.Key == "UseWrapperName");
            if (useWrapperNameArg.Key != null)
            {
                bool.TryParse(useWrapperNameArg.Value.Value?.ToString(), out useWrapperName);
            }

            // Extrair configurações de propriedades
            var propertyConfigs = GetPropertyConfigs(classSymbol);

            return new SourceGeneratorConfigData
            {
                BaseClassType = baseClassType,
                Types = types.ToArray(),
                CustomNamespace = customNamespace,
                ClassPrefix = classPrefix,
                UseWrapperName = useWrapperName,
                PropertyConfigs = propertyConfigs
            };
        }

        private Dictionary<string, PropertyConfigData> GetPropertyConfigs(INamedTypeSymbol classSymbol)
        {
            var configs = new Dictionary<string, PropertyConfigData>();

            var propertyConfigAttributes = classSymbol.GetAttributes()
                .Where(attr => attr.AttributeClass?.Name == "PropertyConfigAttribute");

            foreach (var attr in propertyConfigAttributes)
            {
                if (attr.ConstructorArguments.Length == 0)
                    continue;

                var propertyName = attr.ConstructorArguments[0].Value?.ToString();
                if (string.IsNullOrEmpty(propertyName))
                    continue;

                var config = new PropertyConfigData { PropertyName = propertyName };

                // Extrair ExcludeFromTypes
                var excludeFromArg = attr.NamedArguments
                    .FirstOrDefault(arg => arg.Key == "ExcludeFromTypes");
                if (excludeFromArg.Key != null && excludeFromArg.Value.Values != null)
                {
                    var excludeTypes = new List<DtoType>();
                    foreach (var value in excludeFromArg.Value.Values)
                    {
                        if (Enum.TryParse<DtoType>(value.Value?.ToString(), out var dtoType))
                        {
                            excludeTypes.Add(dtoType);
                        }
                    }
                    config.ExcludeFromTypes = excludeTypes.ToArray();
                }

                // Extrair RequiredForTypes
                var requiredForArg = attr.NamedArguments
                    .FirstOrDefault(arg => arg.Key == "RequiredForTypes");
                if (requiredForArg.Key != null && requiredForArg.Value.Values != null)
                {
                    var requiredTypes = new List<DtoType>();
                    foreach (var value in requiredForArg.Value.Values)
                    {
                        if (Enum.TryParse<DtoType>(value.Value?.ToString(), out var dtoType))
                        {
                            requiredTypes.Add(dtoType);
                        }
                    }
                    config.RequiredForTypes = requiredTypes.ToArray();
                }

                // Extrair CustomName
                var customNameArg = attr.NamedArguments
                    .FirstOrDefault(arg => arg.Key == "CustomName");
                if (customNameArg.Key != null)
                {
                    config.CustomName = customNameArg.Value.Value?.ToString();
                }

                configs[propertyName] = config;
            }

            return configs;
        }

        private void ProcessSourceGeneratorConfig(GeneratorExecutionContext context, INamedTypeSymbol wrapperClass, SourceGeneratorConfigData config)
        {
            foreach (var dtoType in config.Types)
            {
                var dtoCode = GenerateDtoFromBaseClass(config.BaseClassType, wrapperClass, dtoType, config);
                if (!string.IsNullOrEmpty(dtoCode))
                {
                    var baseClassName = config.UseWrapperName ? wrapperClass.Name : config.BaseClassType.Name;
                    var className = $"{config.ClassPrefix}{baseClassName}{GetDtoSuffix(dtoType, null)}";
                    var fileName = $"{className}.g.cs";
                    context.AddSource(fileName, SourceText.From(dtoCode, Encoding.UTF8));
                }
            }
        }

        private string GenerateDtoFromBaseClass(INamedTypeSymbol baseClass, INamedTypeSymbol wrapperClass, DtoType dtoType, SourceGeneratorConfigData config)
        {
            var namespaceName = config.CustomNamespace ?? $"{baseClass.ContainingNamespace}.Dtos";
            var baseClassName = config.UseWrapperName ? wrapperClass.Name : baseClass.Name;
            var className = $"{config.ClassPrefix}{baseClassName}{GetDtoSuffix(dtoType, null)}";

            var sb = new StringBuilder();

            // Using statements
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.ComponentModel.DataAnnotations;");
            sb.AppendLine("using System.Text.Json.Serialization;");
            sb.AppendLine($"using {baseClass.ContainingNamespace};");
            sb.AppendLine();

            // Namespace
            sb.AppendLine($"namespace {namespaceName}");
            sb.AppendLine("{");

            // Class declaration
            sb.AppendLine($"    /// <summary>");
            sb.AppendLine($"    /// DTO gerado automaticamente para {baseClass.Name} - Tipo: {dtoType}");
            sb.AppendLine($"    /// Baseado na configuração da classe wrapper: {wrapperClass.Name}");
            sb.AppendLine($"    /// </summary>");
            sb.AppendLine($"    public partial class {className}");
            sb.AppendLine("    {");

            // Properties
            var properties = GetFilteredPropertiesFromBaseClass(baseClass, dtoType, config);
            foreach (var property in properties)
            {
                GeneratePropertyFromBaseClass(sb, property, dtoType, config);
            }

            // Métodos auxiliares de conversão
            GenerateConversionMethods(sb, baseClass, className, dtoType);

            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private IEnumerable<IPropertySymbol> GetFilteredPropertiesFromBaseClass(INamedTypeSymbol baseClass, DtoType dtoType, SourceGeneratorConfigData config)
        {
            return baseClass.GetMembers()
                .OfType<IPropertySymbol>()
                .Where(p => p.DeclaredAccessibility == Accessibility.Public)
                .Where(p => !ShouldExcludePropertyFromBaseClass(p, dtoType, config));
        }

        private bool ShouldExcludePropertyFromBaseClass(IPropertySymbol property, DtoType dtoType, SourceGeneratorConfigData config)
        {
            var propertyName = property.Name;

            // Verificar configuração específica da propriedade
            if (config.PropertyConfigs.TryGetValue(propertyName, out var propertyConfig))
            {
                if (propertyConfig.ExcludeFromTypes?.Contains(dtoType) == true)
                    return true;
            }

            // Excluir propriedades com XmlIgnore
            if (property.GetAttributes().Any(attr => attr.AttributeClass?.Name == "XmlIgnoreAttribute"))
                return true;

            // Regras padrão baseadas no tipo do DTO
            return dtoType switch
            {
                DtoType.IncluirRequest => propertyName is "Id" or "DataCriacao" or "DataAtualizacao" or "Chave",
                DtoType.AtualizarRequest => propertyName is "DataCriacao" or "Chave",
                DtoType.ConsultarRequest => propertyName is "DataAtualizacao",
                _ => false
            };
        }

        private void GeneratePropertyFromBaseClass(StringBuilder sb, IPropertySymbol property, DtoType dtoType, SourceGeneratorConfigData config)
        {
            var propertyName = GetPropertyNameFromConfig(property, config);
            var propertyType = GetPropertyType(property);
            var isRequired = IsPropertyRequiredFromConfig(property, dtoType, config);

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

        private string GetPropertyNameFromConfig(IPropertySymbol property, SourceGeneratorConfigData config)
        {
            if (config.PropertyConfigs.TryGetValue(property.Name, out var propertyConfig) &&
                !string.IsNullOrEmpty(propertyConfig.CustomName))
            {
                return propertyConfig.CustomName;
            }

            return property.Name;
        }

        private bool IsPropertyRequiredFromConfig(IPropertySymbol property, DtoType dtoType, SourceGeneratorConfigData config)
        {
            if (config.PropertyConfigs.TryGetValue(property.Name, out var propertyConfig))
            {
                if (propertyConfig.RequiredForTypes?.Contains(dtoType) == true)
                    return true;
            }

            // Regras padrão
            return dtoType switch
            {
                DtoType.IncluirRequest => !property.Type.CanBeReferencedByName || 
                                        property.GetAttributes().Any(attr => attr.AttributeClass?.Name == "XmlAttributeAttribute"),
                _ => false
            };
        }

        private void GenerateConversionMethods(StringBuilder sb, INamedTypeSymbol baseClass, string dtoClassName, DtoType dtoType)
        {
            var baseClassName = baseClass.Name;

            sb.AppendLine($"        /// <summary>");
            sb.AppendLine($"        /// Converte o DTO para a classe {baseClassName}");
            sb.AppendLine($"        /// </summary>");
            sb.AppendLine($"        public {baseClassName} To{baseClassName}()");
            sb.AppendLine("        {");
            sb.AppendLine($"            return new {baseClassName}");
            sb.AppendLine("            {");
            sb.AppendLine("                // TODO: Implementar mapeamento automático");
            sb.AppendLine("                // Esta implementação será aprimorada em versões futuras");
            sb.AppendLine("            };");
            sb.AppendLine("        }");
            sb.AppendLine();

            sb.AppendLine($"        /// <summary>");
            sb.AppendLine($"        /// Cria um DTO a partir de uma instância de {baseClassName}");
            sb.AppendLine($"        /// </summary>");
            sb.AppendLine($"        public static {dtoClassName} From{baseClassName}({baseClassName} source)");
            sb.AppendLine("        {");
            sb.AppendLine($"            return new {dtoClassName}");
            sb.AppendLine("            {");
            sb.AppendLine("                // TODO: Implementar mapeamento automático");
            sb.AppendLine("                // Esta implementação será aprimorada em versões futuras");
            sb.AppendLine("            };");
            sb.AppendLine("        }");
            sb.AppendLine();
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
            return classSymbol.GetMembers()
                .OfType<IPropertySymbol>()
                .Where(p => p.DeclaredAccessibility == Accessibility.Public)
                .Where(p => !ShouldExcludeProperty(p, dtoType));
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

        private class SourceGeneratorConfigData
        {
            public INamedTypeSymbol BaseClassType { get; set; } = null!;
            public DtoType[] Types { get; set; } = Array.Empty<DtoType>();
            public string? CustomNamespace { get; set; }
            public string? ClassPrefix { get; set; }
            public bool UseWrapperName { get; set; }
            public Dictionary<string, PropertyConfigData> PropertyConfigs { get; set; } = new();
        }

        private class PropertyConfigData
        {
            public string PropertyName { get; set; } = string.Empty;
            public DtoType[]? ExcludeFromTypes { get; set; }
            public DtoType[]? RequiredForTypes { get; set; }
            public string? CustomName { get; set; }
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
