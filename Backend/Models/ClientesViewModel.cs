using System.ComponentModel.DataAnnotations;

namespace Backend.Models {
    public class ClientesViewModel {
        [Key]

        public int Id_Cliente { get; set; }
        public string Cliente { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public DateOnly DataDeNascimento { get; set; }
        public char Sexo {  get; set; }

        public ClientesViewModel() { }
        public ClientesViewModel(int id, string cliente, string email,string telefone,string endereco, DateOnly dataDeNascimento,char sexo) {
            Id_Cliente = id;
            Cliente = cliente;
            Email = email;
            Telefone = telefone;
            Endereco = endereco;
            DataDeNascimento = dataDeNascimento;
            Sexo = sexo;
        }
    }
}
