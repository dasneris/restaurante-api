using RestauranteAPI.Domain.Enums;

namespace RestauranteAPI.Domain.Entities;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public TipoProduto Tipo { get; set; }
    public bool Ativo { get; set; } = true;
}