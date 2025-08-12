using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unimake.DFe.Database.Models;

namespace Unimake.DFe.Database.Data.Mapping;

/// <summary>
/// Configuração de mapeamento para DestinatarioEntity
/// </summary>
public class DestinatarioMapping : IEntityTypeConfiguration<DestinatarioEntity>
{
    public void Configure(EntityTypeBuilder<DestinatarioEntity> builder)
    {
        // Configuração da tabela
        builder.ToTable("destinatario");

        // Chave primária
        builder.HasKey(e => e.Id);

        // Mapeamento das colunas para snake_lowercase
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.CNPJ).HasColumnName("cnpj").HasMaxLength(14);
        builder.Property(e => e.CPF).HasColumnName("cpf").HasMaxLength(11);
        builder.Property(e => e.IdEstrangeiro).HasColumnName("id_estrangeiro").HasMaxLength(20);
        builder.Property(e => e.RazaoSocial).HasColumnName("razao_social").HasMaxLength(60);
        builder.Property(e => e.IndicadorIE).HasColumnName("indicador_ie");
        builder.Property(e => e.InscricaoEstadual).HasColumnName("inscricao_estadual").HasMaxLength(14);
        builder.Property(e => e.ISUF).HasColumnName("isuf").HasMaxLength(9);
        builder.Property(e => e.InscricaoMunicipal).HasColumnName("inscricao_municipal").HasMaxLength(15);
        builder.Property(e => e.Email).HasColumnName("email").HasMaxLength(60);
        builder.Property(e => e.Logradouro).HasColumnName("logradouro").HasMaxLength(60);
        builder.Property(e => e.Numero).HasColumnName("numero").HasMaxLength(60);
        builder.Property(e => e.Complemento).HasColumnName("complemento").HasMaxLength(60);
        builder.Property(e => e.Bairro).HasColumnName("bairro").HasMaxLength(60);
        builder.Property(e => e.CodigoMunicipio).HasColumnName("codigo_municipio");
        builder.Property(e => e.NomeMunicipio).HasColumnName("nome_municipio").HasMaxLength(60);
        builder.Property(e => e.UF).HasColumnName("uf").HasMaxLength(2);
        builder.Property(e => e.CEP).HasColumnName("cep").HasMaxLength(8);
        builder.Property(e => e.CodigoPais).HasColumnName("codigo_pais");
        builder.Property(e => e.NomePais).HasColumnName("nome_pais").HasMaxLength(60);
        builder.Property(e => e.Telefone).HasColumnName("telefone").HasMaxLength(14);
        builder.Property(e => e.DataCriacao).HasColumnName("data_criacao");
        builder.Property(e => e.DataAtualizacao).HasColumnName("data_atualizacao");

        // Índices
        builder.HasIndex(e => e.CNPJ).HasFilter("\"cnpj\" IS NOT NULL");
        builder.HasIndex(e => e.CPF).HasFilter("\"cpf\" IS NOT NULL");
    }
}