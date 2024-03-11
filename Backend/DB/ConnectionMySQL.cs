using Backend.Models;
using Microsoft.EntityFrameworkCore;
namespace Backend.DB {

    public class ConnectionMySQL(DbContextOptions options) : DbContext(options) {
        public DbSet<UserViewModel> Users { get; set; }
        public DbSet<ProductsViewModel> Products { get; set; }
        public DbSet<VendasViewModel> Vendas { get; set; }
        public DbSet<VendasItemViewModel> Vendas_Item { get; set;}
        public DbSet<CategoriaProdutosViewModel> Categoria_Produtos { get; set; }
    }
}
