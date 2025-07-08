using System.ComponentModel.DataAnnotations;

namespace Student_Record_Management_System.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username is required.")]

        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required.")]

        public string Password { get; set; }
    }

}
