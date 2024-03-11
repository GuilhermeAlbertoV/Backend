using System.ComponentModel.DataAnnotations;

namespace Backend.Models {
    public class ProductsViewModel {


        [Key]
        public int Id_Product { get; set; }

        public string Produto { get; set; }

        public string Descricao { get; set; }

        public decimal Valor { get; set; }

       

        public ProductsViewModel() { }

        public ProductsViewModel(int id_product, string produto, string descricao, decimal valor) {
            Id_Product = id_product;
            Produto = produto;
            Descricao = descricao;
            Valor = valor;
        }
    }
}
