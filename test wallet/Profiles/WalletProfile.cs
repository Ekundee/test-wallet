using test_wallet.Model;
using test_wallet.Dtos.Wallet;
using AutoMapper;

namespace test_wallet.Profiles
{
    public class WalletProfile : Profile
    {
        public WalletProfile()
        {
            CreateMap<FundWalletDto, WalletModel>();
            CreateMap<NewWalletDto, WalletModel>();

            CreateMap<WalletLoginResponseDto, WalletModel>();
            CreateMap<UpdateUserDto, WalletModel>();
            CreateMap<ChangePassword, WalletModel>();
        }
    }
}
