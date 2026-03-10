using RestauranteAPI.Domain.Entities;
using RestauranteAPI.Domain.Enums;

namespace RestauranteAPI.Infrastructure.Database;

public static class DbInitializer
{
    public static void Seed(AppDbContext context)
    {
        context.Database.EnsureCreated();

        if (!context.Produtos.Any())
        {
            var produtos = new List<Produto>
            {
                new Produto { Nome = "Hamburguer", Preco = 25.90m, Tipo = TipoProduto.Prato, Ativo = true },
                new Produto { Nome = "Batata Frita", Preco = 15.00m, Tipo = TipoProduto.Prato, Ativo = true },
                new Produto { Nome = "Refrigerante", Preco = 7.50m, Tipo = TipoProduto.Bebida, Ativo = true },
                new Produto { Nome = "Suco de Laranja", Preco = 8.00m, Tipo = TipoProduto.Bebida, Ativo = true }
            };

            context.Produtos.AddRange(produtos);
        }

        if (!context.Usuarios.Any())
        {
            var usuarios = new List<Usuario>
            {
                new Usuario { Nome = "Administrador", Email = "admin@restaurante.com", Senha = "123456", Perfil = "Admin" },
                new Usuario { Nome = "Cozinha", Email = "cozinha@restaurante.com", Senha = "123456", Perfil = "Cozinha" },
                new Usuario { Nome = "Copa", Email = "copa@restaurante.com", Senha = "123456", Perfil = "Copa" }
            };

            context.Usuarios.AddRange(usuarios);
        }

        context.SaveChanges();
    }
}