using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestauranteAPI.Application.DTOs;
using RestauranteAPI.Domain.Entities;
using RestauranteAPI.Infrastructure.Database;
using RestauranteAPI.Domain.Enums;
namespace RestauranteAPI.Controllers;

[ApiController]
[Route("pedidos")]
public class PedidoController : ControllerBase
{
    private readonly AppDbContext _context;

    public PedidoController(AppDbContext context)
    {
        _context = context;
    }

  [HttpPost]
public async Task<ActionResult> Create(PedidoCreateDto dto)
{
    if (dto.Itens == null || dto.Itens.Count == 0)
    {
        return BadRequest("O pedido deve ter pelo menos um item.");
    }

    if (dto.Mesa <= 0)
    {
        return BadRequest("O número da mesa deve ser maior que zero.");
    }

    if (dto.Itens.Any(i => i.Quantidade <= 0))
    {
        return BadRequest("A quantidade dos itens deve ser maior que zero.");
    }

    var produtosIds = dto.Itens.Select(i => i.ProdutoId).ToList();

    var produtos = await _context.Produtos
        .Where(p => produtosIds.Contains(p.Id))
        .ToListAsync();

    if (produtos.Count != produtosIds.Count)
    {
        return BadRequest("Um ou mais produtos informados não existem.");
    }

    var pedido = new Pedido
    {
        Mesa = dto.Mesa,
        Solicitante = dto.Solicitante
    };

    foreach (var itemDto in dto.Itens)
    {
        var itemPedido = new ItemPedido
        {
            ProdutoId = itemDto.ProdutoId,
            Quantidade = itemDto.Quantidade
        };

        pedido.Itens.Add(itemPedido);
    }

    _context.Pedidos.Add(pedido);
    await _context.SaveChangesAsync();

    return Created($"/pedidos/{pedido.Id}", new
{
    pedido.Id,
    pedido.Mesa,
    pedido.Solicitante,
    pedido.Status,
    pedido.CriadoEm
});
}

    [HttpGet]
    public async Task<ActionResult<List<Pedido>>> GetAll()
    {
        var pedidos = await _context.Pedidos
            .Include(p => p.Itens)
            .ThenInclude(i => i.Produto)
            .ToListAsync();
        return Ok(pedidos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Pedido>> GetById(int id)
    {
        var pedido = await _context.Pedidos
            .Include(p => p.Itens)
            .ThenInclude(i => i.Produto)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pedido == null)
        {
            return NotFound();
        }

        return Ok(pedido);
    }

    [HttpGet("cozinha")]
public async Task<ActionResult> GetPedidosCozinha()
{
    var pedidos = await _context.Pedidos
        .Include(p => p.Itens)
        .ThenInclude(i => i.Produto)
        .ToListAsync();

    var resultado = pedidos.Select(p => new
    {
        p.Id,
        p.Mesa,
        p.Solicitante,
        p.Status,
        p.CriadoEm,
        Itens = p.Itens
            .Where(i => i.Produto != null && i.Produto.Tipo == Domain.Enums.TipoProduto.Prato)
            .Select(i => new
            {
                i.ProdutoId,
                Produto = i.Produto!.Nome,
                i.Quantidade
            })
    })
    .Where(p => p.Itens.Any())
    .ToList();

    return Ok(resultado);
}

[HttpGet("copa")]
public async Task<ActionResult> GetPedidosCopa()
{
    var pedidos = await _context.Pedidos
        .Include(p => p.Itens)
        .ThenInclude(i => i.Produto)
        .ToListAsync();

    var resultado = pedidos.Select(p => new
    {
        p.Id,
        p.Mesa,
        p.Solicitante,
        p.Status,
        p.CriadoEm,
        Itens = p.Itens
            .Where(i => i.Produto != null && i.Produto.Tipo == Domain.Enums.TipoProduto.Bebida)
            .Select(i => new
            {
                i.ProdutoId,
                Produto = i.Produto!.Nome,
                i.Quantidade
            })
    })
    .Where(p => p.Itens.Any())
    .ToList();

    return Ok(resultado);
}

[HttpPatch("{id}/status")]
public async Task<ActionResult> AtualizarStatus(int id, AtualizarStatusPedidoDto dto)
{
    var pedido = await _context.Pedidos.FindAsync(id);

    if (pedido == null)
    {
        return NotFound("Pedido não encontrado.");
    }

    if (!Enum.IsDefined(typeof(StatusPedido), dto.Status))
    {
        return BadRequest("Status inválido.");
    }

    pedido.Status = dto.Status;

    await _context.SaveChangesAsync();

    return Ok(new
    {
        mensagem = "Status atualizado com sucesso.",
        pedido.Id,
        pedido.Status
    });
}

[HttpGet("finalizados")]
public async Task<ActionResult> GetPedidosFinalizados()
{
    var pedidos = await _context.Pedidos
        .Include(p => p.Itens)
        .ThenInclude(i => i.Produto)
        .Where(p => p.Status == StatusPedido.Entregue)
        .Select(p => new
        {
            p.Id,
            p.Mesa,
            p.Solicitante,
            p.Status,
            p.CriadoEm,
            Itens = p.Itens.Select(i => new
            {
                    i.ProdutoId,
                    Produto = i.Produto != null ? i.Produto.Nome : null,
                    Tipo = i.Produto != null ? i.Produto.Tipo.ToString() : null,
                    i.Quantidade
            })
        })
        .ToListAsync();

    return Ok(pedidos);
}

}
