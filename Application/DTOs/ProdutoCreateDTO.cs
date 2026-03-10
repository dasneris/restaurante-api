using RestauranteAPI.Domain.Enums;

namespace RestauranteAPI.Application.DTOs;

public class ProdutoCreateDto
{
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public TipoProduto Tipo { get; set; }
}