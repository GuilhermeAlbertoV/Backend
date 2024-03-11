using System.ComponentModel.DataAnnotations;

namespace Backend.Models {
    public class VendasViewModel {
        [Key]
        public int Id_Vendas { get; set; }
        public int Id_User { get; set; }
        public int Id_Product { get; set; }
        public DateTime DataVenda { get; set; }
        public int Quantidade { get; set; }
        public decimal Preco_Unidade { get; set; }
        public decimal Valor_total { get; set; }

        

        public VendasViewModel() { }

        public VendasViewModel(int id_vendas, int id_user, int id_product, DateTime dataVenda, int quantidade, decimal preco_unidade, decimal valor_total) {
            Id_Vendas = id_vendas;
            Id_User = id_user;
            Id_Product = id_product;
            DataVenda = dataVenda;
            Quantidade = quantidade;
            Preco_Unidade = preco_unidade;
            Valor_total = valor_total;
        }
    }
}
