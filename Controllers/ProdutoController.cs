using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestauranteAPI.Application.DTOs;
using RestauranteAPI.Domain.Entities;
using RestauranteAPI.Infrastructure.Database;

namespace RestauranteAPI.Controllers;

[ApiController]
[Route("produtos")]
public class ProdutoController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProdutoController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Produto>>> GetAll()
    {
        var produtos = await _context.Produtos.ToListAsync();
        return Ok(produtos);
    }

    [HttpPost]
    public async Task<ActionResult<Produto>> Create(ProdutoCreateDto dto)
    {
        var produto = new Produto
        {
            Nome = dto.Nome,
            Preco = dto.Preco,
            Tipo = dto.Tipo,
            Ativo = true
        };

        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();

        return Created("", produto);
    }
}