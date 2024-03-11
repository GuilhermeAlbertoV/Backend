using System.ComponentModel.DataAnnotations;

namespace Backend.Models {
    public class UserViewModel {
        [Key]
        public int Id_User { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

       

        public UserViewModel() { }

        public UserViewModel(int id_user, string username, string email, string password) {
            Id_User = id_user;
            UserName = username;
            Email = email;
            Password = password;
        }
    }
}
