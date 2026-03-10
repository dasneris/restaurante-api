using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestauranteAPI.Application.DTOs;
using RestauranteAPI.Infrastructure.Database;

namespace RestauranteAPI.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginRequestDto dto)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == dto.Email && u.Senha == dto.Senha);

        if (usuario == null)
        {
            return Unauthorized(new
            {
                mensagem = "Email ou senha inválidos."
            });
        }

        return Ok(new
        {
            mensagem = "Login realizado com sucesso.",
            usuario = new
            {
                usuario.Id,
                usuario.Nome,
                usuario.Email,
                usuario.Perfil
            }
        });
    }
}