using test_wallet.Model;
using test_wallet.Dtos.Wallet;

namespace test_wallet.Repositories
{
    public interface IWalletRepository
    {
        List<WalletModel> GetWallet();
        WalletModel GetWallet(int id);

        WalletModel CreateWallet(NewWalletDto Wallet);
    }
}
