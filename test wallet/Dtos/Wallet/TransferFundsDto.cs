namespace test_wallet.Dtos.Wallet
{
    public class TransferFundsDto
    {
        public int RecipientId { get; set; }
        public int SenderId { get; set; }

        public decimal FundsSent { get; set; }  
    }
}
