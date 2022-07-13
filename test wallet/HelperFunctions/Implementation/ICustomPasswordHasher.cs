namespace test_wallet.HelperFunctions.Implementation
{
    public interface ICustomPasswordHasher
    {
        string hashPassword(string password);
    }
}