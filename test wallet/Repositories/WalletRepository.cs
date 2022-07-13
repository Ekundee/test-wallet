using System.Linq;
using test_wallet.Model;
using test_wallet.Dtos.Wallet;
using test_wallet.Repositories;

namespace test_wallet.Repositories
{
  
    public class WalletRepository : IWalletRepository
    {
        public List<WalletModel> wallets = new()
        {
            new WalletModel { Id = 1, FirstName = "Isaiah", LastName = "Ekundayo", Username = "Kundee", CurrencyType = "Dol", DateOfBirth = DateTime.Now.ToString(), Balance = 500 },
            new WalletModel { Id = 2, FirstName = "Chizaram", LastName = "Onuorah", Username = "Boobs", CurrencyType = "Dol", DateOfBirth = DateTime.Now.ToString(), Balance = 590 }
        };

        public List<WalletModel> GetWallet()
        {
            return wallets;
        }

        public WalletModel GetWallet(int id)
        {
            return wallets.SingleOrDefault(wallet => wallet.Id == id);
        }

        public WalletModel CreateWallet(NewWalletDto newWallet)
        {
            WalletModel wallet = new()
            {
                CurrencyType = newWallet.CurrencyType,
                Balance = newWallet.Balance,
                FirstName = newWallet.FirstName,
                LastName = newWallet.LastName,
                DateOfBirth = newWallet.DateOfBirth,
            };
            wallets.Add(wallet);
            return wallet;
        }
    }
}
