using System.ComponentModel.DataAnnotations;

namespace InsureYouAI.DTOs.UserDtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "İsim alanı boş bırakılamaz")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Soyisim alanı boş bırakılamaz")]
        public string Surname { get; set; }


        [Required(ErrorMessage = "Email alanı boş bırakılamaz")]
        [EmailAddress(ErrorMessage ="Geçerli formatta email adresi giriniz")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz")]
        [MinLength(8, ErrorMessage = "Şifre en az 6 karakter olmalı")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Devam etmek için sözleşmeyi kabul etmelisiniz")]
        public bool AcceptTerms { get; set; }
    }
}
