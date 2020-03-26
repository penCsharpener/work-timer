using System.ComponentModel.DataAnnotations;

namespace WorkTimer.Api.Models {
    public class User {

        public User() { }

        public User(int id, string firstName, string lastName, string email, string username) {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UserName = username;
        }

        public User(int id, string firstName, string lastName, string email, string username, int roleId) {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UserName = username;
            RoleId = roleId;
        }

        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public int RoleId { get; set; }
    }
}
