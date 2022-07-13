using System.ComponentModel.DataAnnotations;

namespace test_wallet.Dtos.Wallet
{
    public class WalletLoginDto
    {
        [Required(ErrorMessage = "Email field cannot be empty")]
        public string Email { get; set; } = String.Empty;

        [Required(ErrorMessage = "Password Field cannot be empty")]
        public string Password { get; set; } = String.Empty;
    }
}
