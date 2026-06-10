using System.ComponentModel.DataAnnotations;

namespace InsureYouAI.DTOs.UserDtos
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Mevcut şifreyi giriniz.")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Yeni şifreyi giriniz.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Yeni şifre tekrarını giriniz.")]
        [Compare("NewPassword", ErrorMessage = "Şifreler uyuşmuyor.")]
        public string ConfirmPassword { get; set; }
    }
}
