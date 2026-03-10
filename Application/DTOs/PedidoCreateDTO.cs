namespace RestauranteAPI.Application.DTOs;

public class PedidoCreateDto
{
    public int Mesa { get; set; }
    public string Solicitante { get; set; } = string.Empty;
    public List<ItemPedidoCreateDto> Itens { get; set; } = new();
}