using System;

namespace Unimake.Business.DFe.SourceGenerators.Attributes
{
    /// <summary>
    /// Atributo para configurar geração de DTOs em classes wrapper/herança
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SourceGeneratorConfigAttribute : Attribute
    {
        /// <summary>
        /// Tipo da classe base para extrair propriedades
        /// </summary>
        public Type BaseClassType { get; }

        /// <summary>
        /// Tipos de DTO a serem gerados
        /// </summary>
        public DtoType[] Types { get; }

        /// <summary>
        /// Namespace customizado para os DTOs (opcional)
        /// </summary>
        public string? CustomNamespace { get; set; }

        /// <summary>
        /// Prefixo customizado para o nome da classe (opcional)
        /// </summary>
        public string? ClassPrefix { get; set; }

        /// <summary>
        /// Se deve usar o nome da classe wrapper ou da classe base
        /// </summary>
        public bool UseWrapperName { get; set; } = false;

        public SourceGeneratorConfigAttribute(Type baseClassType, params DtoType[] types)
        {
            BaseClassType = baseClassType ?? throw new ArgumentNullException(nameof(baseClassType));
            Types = types ?? new[] { DtoType.Request };
        }
    }

    /// <summary>
    /// Atributo para configurar propriedades específicas no wrapper
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class PropertyConfigAttribute : Attribute
    {
        /// <summary>
        /// Nome da propriedade na classe base
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Tipos de DTO dos quais a propriedade deve ser excluída
        /// </summary>
        public DtoType[]? ExcludeFromTypes { get; set; }

        /// <summary>
        /// Tipos de DTO onde a propriedade é obrigatória
        /// </summary>
        public DtoType[]? RequiredForTypes { get; set; }

        /// <summary>
        /// Nome customizado para a propriedade no DTO
        /// </summary>
        public string? CustomName { get; set; }

        public PropertyConfigAttribute(string propertyName)
        {
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
        }
    }
}
