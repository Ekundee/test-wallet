using System.ComponentModel.DataAnnotations;

namespace test_wallet.Dtos.Wallet
{
    public class NewWalletDto
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = String.Empty;

        [Required]
        public string? Username { get; set; } = String.Empty;

        [Required]
        public string? Email { get; set; } = String.Empty;

        [Required]
        public string? Password { get; set; } = String.Empty;

        [Required]
        public string? DateOfBirth { get; set; } = String.Empty;

        [Required]
        public string? CurrencyType { get; set; } = "NGN";

        [Required]
        [Range(1,5000000000)]
        public decimal? Balance { get; set; } = 0;

        
    }
}
