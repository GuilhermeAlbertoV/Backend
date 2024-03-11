using System.ComponentModel.DataAnnotations;

namespace Backend.Models {
    public class CategoriaProdutosViewModel {
        [Key]
        public int Id_Categoria { get; set; }
        public string Nome_Categoria { get; set; }
        public string Descricao { get; set; }

        public CategoriaProdutosViewModel() { }

        public CategoriaProdutosViewModel(int id_categoria, string nome_categoria, string descricao) {
            Id_Categoria = id_categoria;
            Nome_Categoria = nome_categoria;
            Descricao = descricao;
        }

    }
}
