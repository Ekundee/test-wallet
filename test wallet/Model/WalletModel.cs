using System.ComponentModel.DataAnnotations.Schema;

namespace test_wallet.Model

{
    public class WalletModel
    {
        public int Id { get; init; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? Username { get; set; } = "User";

        public string? DateOfBirth { get; set; }

        public string? CurrencyType { get; set; } = "NGN";

        [Column(TypeName = "decimal(18,4)")]
        public decimal? Balance { get; set; } = 0;

    }
}
