using System.ComponentModel.DataAnnotations;
using test_wallet.Model;

namespace test_wallet.Dtos.Wallet
{
    public class WalletLoginDto
    {
        [Required(ErrorMessage = "Email field cannot be empty")]
        public string Email { get; set; } = String.Empty;

        [Required(ErrorMessage = "Password Field cannot be empty")]
        public string Password { get; set; } = String.Empty;
    }

    public class WalletLoginResponseDto
    {
        public WalletModel? CurrentUser { get; set; }

        public string AuthToken { get; set; }

    }
}