using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exata.Domain.DTO;

public class PaginacaoDTO
{
    /// <summary>
    /// Página de Retorno
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Insira um número de página válido.")]
    [Required]
    public int Pagina { get; set; }

    /// <summary>
    /// Quantidade de registro por página
    /// </summary>
    [Range(5, 100, ErrorMessage = "Registros por página deve ser de 5 a 100.")]
    [Required]
    public int RegistroPorPagina { get; set; }

    /// <summary>
    /// Campo de Ordenação dos dados
    /// </summary>
    public string OrderCampo { get; set; }

    /// <summary>
    /// Tipo de Ordenação dos dados
    /// </summary>
    public bool OrderTipoAsc { get; set; } = true;

    /// <summary>
    /// Propriedade de Pesquisa dos Dados
    /// </summary>
    public string PesquisarCampo { get; set; }

    /// <summary>
    /// Propriedade de Pesquisa dos Dados
    /// </summary>
    public string PesquisarValor { get; set; }

    /// <summary>
    /// Propriedade de Pesquisa Somente dos Itens Ativos se Existir na pesquisa
    /// </summary>
    public bool? Ativos { get; set; }
}
