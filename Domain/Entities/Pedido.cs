using RestauranteAPI.Domain.Enums;

namespace RestauranteAPI.Domain.Entities;

public class Pedido
{
    public int Id { get; set; }
    public int Mesa { get; set; }
    public string Solicitante { get; set; } = string.Empty;
    public StatusPedido Status { get; set; } = StatusPedido.EmPreparo;
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public List<ItemPedido> Itens { get; set; } = new();
}