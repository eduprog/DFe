using System;

namespace Unimake.Business.DFe.SourceGenerators.Attributes
{
    /// <summary>
    /// Atributo para excluir propriedades específicas da geração do DTO
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcludeFromDtoAttribute : Attribute
    {
        /// <summary>
        /// Tipos de DTO dos quais a propriedade deve ser excluída
        /// </summary>
        public DtoType[]? ExcludeFromTypes { get; }

        public ExcludeFromDtoAttribute(params DtoType[]? excludeFromTypes)
        {
            ExcludeFromTypes = excludeFromTypes;
        }
    }

    /// <summary>
    /// Atributo para customizar o nome da propriedade no DTO
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DtoPropertyNameAttribute : Attribute
    {
        public string Name { get; }

        public DtoPropertyNameAttribute(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// Atributo para marcar propriedades como obrigatórias no DTO
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DtoRequiredAttribute : Attribute
    {
        /// <summary>
        /// Tipos de DTO onde a propriedade é obrigatória
        /// </summary>
        public DtoType[]? RequiredForTypes { get; }

        public DtoRequiredAttribute(params DtoType[]? requiredForTypes)
        {
            RequiredForTypes = requiredForTypes;
        }
    }
}
