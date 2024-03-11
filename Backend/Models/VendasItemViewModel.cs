using System.ComponentModel.DataAnnotations;

namespace Backend.Models {
    public class VendasItemViewModel {

        [Key]
        public int Id_VendasItem { get; set; }
        public int Id_Vendas { get; set; }
        public int Id_product { get; set; }
        public int Quantidade { get; set; }
        public decimal Preco_Unidade { get; set; }
        public decimal Valor_Total { get; set; }


        public VendasItemViewModel() { }

        public VendasItemViewModel(int id_vendasItem, int id_vendas, int id_produto, int quantidade, decimal preco_unidade, decimal valor_total) {
            Id_VendasItem = id_vendasItem;
            Id_Vendas = id_vendas;
            Id_product = id_produto;
            Quantidade = quantidade;
            Preco_Unidade = preco_unidade;
            Valor_Total = valor_total;
        }
    }
}
