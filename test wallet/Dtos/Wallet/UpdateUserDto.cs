using System.ComponentModel.DataAnnotations.Schema;

namespace test_wallet.Dtos.Wallet
{
    public class UpdateUserDto
    {
        public int Id { get; init; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }


        public string? Password { get; set; }

        public string? Username { get; set; } = "User";

        public string? DateOfBirth { get; set; }

        public string? CurrencyType { get; set; } = "NGN";

    }
}
