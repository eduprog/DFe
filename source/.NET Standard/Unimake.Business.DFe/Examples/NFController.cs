using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Unimake.Business.DFe.Xml.NFe;
using Unimake.Business.DFe.Xml.NFe.Examples;

namespace Unimake.Business.DFe.Examples.Controllers
{
    /// <summary>
    /// Exemplo de controller usando os DTOs gerados automaticamente
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class NFController : ControllerBase
    {
        /// <summary>
        /// Incluir nova NFe usando DTO gerado automaticamente
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> IncluirNFe([FromBody] EnviNFeIncluirRequest request)
        {
            try
            {
                // Validação automática via Data Annotations
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Conversão do DTO para classe XML
                var enviNFe = new EnviNFe
                {
                    Versao = request.Versao,
                    IdLote = request.IdLote,
                    IndSinc = request.IndSinc,
                    NFe = request.NFe ?? new List<NFe>()
                };

                // Gerar XML
                var xml = enviNFe.GerarXML();

                // Aqui você processaria a NFe...
                // - Validar regras de negócio
                // - Enviar para SEFAZ
                // - Salvar no banco de dados
                // etc.

                return Ok(new { 
                    Sucesso = true, 
                    IdLote = request.IdLote,
                    Mensagem = "NFe processada com sucesso" 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    Erro = "Erro interno do servidor", 
                    Detalhes = ex.Message 
                });
            }
        }

        /// <summary>
        /// Atualizar NFe existente
        /// </summary>
        [HttpPut("{idLote}")]
        public async Task<IActionResult> AtualizarNFe(
            string idLote, 
            [FromBody] EnviNFeAtualizarRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Verificar se o lote existe
                // var loteExistente = await _nfeService.BuscarPorIdLote(idLote);
                // if (loteExistente == null)
                //     return NotFound();

                // Aplicar apenas as propriedades que foram enviadas
                var enviNFe = new EnviNFe
                {
                    IdLote = request.IdLote,
                    Versao = request.Versao ?? "4.00", // valor padrão se não informado
                    IndSinc = request.IndSinc ?? SimNao.Nao,
                    NFe = request.NFe ?? new List<NFe>()
                };

                // Processar atualização...

                return Ok(new { 
                    Sucesso = true, 
                    IdLote = request.IdLote,
                    Mensagem = "NFe atualizada com sucesso" 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    Erro = "Erro interno do servidor", 
                    Detalhes = ex.Message 
                });
            }
        }

        /// <summary>
        /// Consultar NFe
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ConsultarNFe([FromQuery] EnviNFeConsultarRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Simular consulta
                var resultado = new
                {
                    IdLote = request.IdLote,
                    Resultados = new[]
                    {
                        new { Id = 1, Status = "Autorizada", DataEmissao = DateTime.Now.AddDays(-1) },
                        new { Id = 2, Status = "Pendente", DataEmissao = DateTime.Now }
                    },
                    Total = 2
                };

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    Erro = "Erro interno do servidor", 
                    Detalhes = ex.Message 
                });
            }
        }

        /// <summary>
        /// Excluir NFe
        /// </summary>
        [HttpDelete("{idLote}")]
        public async Task<IActionResult> ExcluirNFe(string idLote)
        {
            try
            {
                // Verificar se existe
                // Processar exclusão
                
                return Ok(new { 
                    Sucesso = true, 
                    Mensagem = $"Lote {idLote} excluído com sucesso" 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    Erro = "Erro interno do servidor", 
                    Detalhes = ex.Message 
                });
            }
        }
    }

    /// <summary>
    /// Exemplo de serviço para conversão entre XML e DTO
    /// </summary>
    public class NFService
    {
        /// <summary>
        /// Converter DTO para classe XML
        /// </summary>
        public EnviNFe ConverterParaXml(EnviNFeIncluirRequest dto)
        {
            return new EnviNFe
            {
                Versao = dto.Versao,
                IdLote = dto.IdLote,
                IndSinc = dto.IndSinc,
                NFe = dto.NFe ?? new List<NFe>()
            };
        }

        /// <summary>
        /// Converter classe XML para DTO
        /// </summary>
        public EnviNFeIncluirRequest ConverterParaDto(EnviNFe xml)
        {
            return new EnviNFeIncluirRequest
            {
                Versao = xml.Versao,
                IdLote = xml.IdLote,
                IndSinc = xml.IndSinc,
                NFe = xml.NFe ?? new List<NFe>()
            };
        }

        /// <summary>
        /// Validar regras específicas de NFe
        /// </summary>
        public ValidationResult ValidarNFe(EnviNFeIncluirRequest dto)
        {
            var errors = new List<string>();

            // Exemplo de validações específicas
            if (string.IsNullOrEmpty(dto.IdLote))
                errors.Add("IdLote é obrigatório");

            if (dto.NFe?.Count == 0)
                errors.Add("Pelo menos uma NFe deve ser informada");

            if (dto.NFe?.Count > 50)
                errors.Add("Máximo de 50 NFes por lote");

            return new ValidationResult
            {
                IsValid = errors.Count == 0,
                Errors = errors
            };
        }
    }

    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
