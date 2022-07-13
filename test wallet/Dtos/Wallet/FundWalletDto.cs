using System.ComponentModel.DataAnnotations;


namespace test_wallet.Dtos.Wallet
{
    public class FundWalletDto
    {
        [Required(ErrorMessage = "Input the recipient field")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please input the amount to be transfered")]
        public decimal? Balance { get; set; }
       
    }
}
