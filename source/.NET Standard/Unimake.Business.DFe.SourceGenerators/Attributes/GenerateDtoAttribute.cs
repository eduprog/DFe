using System;

namespace Unimake.Business.DFe.SourceGenerators.Attributes
{
    /// <summary>
    /// Atributo para marcar classes que devem gerar DTOs automaticamente
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class GenerateDtoAttribute : Attribute
    {
        /// <summary>
        /// Tipos de DTO a serem gerados
        /// </summary>
        public DtoType[] Types { get; }

        /// <summary>
        /// Namespace customizado para os DTOs (opcional)
        /// </summary>
        public string? CustomNamespace { get; set; }

        /// <summary>
        /// Sufixo customizado para o nome da classe (opcional)
        /// </summary>
        public string? CustomSuffix { get; set; }

        public GenerateDtoAttribute(params DtoType[] types)
        {
            Types = types ?? new[] { DtoType.Request };
        }
    }

    /// <summary>
    /// Tipos de DTO que podem ser gerados
    /// </summary>
    public enum DtoType
    {
        Request,
        Response,
        IncluirRequest,
        AtualizarRequest,
        ExcluirRequest,
        ConsultarRequest
    }
}
