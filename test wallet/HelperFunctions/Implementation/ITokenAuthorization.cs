using test_wallet.Model;

namespace test_wallet.HelperFunctions.Implementation
{
    public interface ITokenAuthorization
    {
        string CreateToken(WalletModel user);
    }
}