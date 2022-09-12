using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Informe seu nome.")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Informe seu e-mail.")]
        [EmailAddress(ErrorMessage ="E-mail inválido.")]
        public string Email { get; set; }
    }
}
