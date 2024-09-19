using System.ComponentModel.DataAnnotations;

namespace EmployeePortal.Models
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "Электронная почта обязательна")]
        [EmailAddress(ErrorMessage = "Не корректный адрес электронной почты")]
        [Display(Name = "Электронная почта")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string ConfirmPassword { get; set; }
    }
}
