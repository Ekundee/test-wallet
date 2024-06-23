using Microsoft.AspNetCore.Mvc;
using test_wallet.Repositories;
using test_wallet.Model;
using test_wallet.Dtos.Wallet;
using test_wallet.DbContexts;
using Microsoft.AspNetCore.Identity;
using test_wallet.HelperFunctions;
using AutoMapper;
using test_wallet.HelperFunctions.Implementation;

namespace test_wallet.Controllers
{
    [ApiController]
    [Route("wallet")]
    public class WalletController : ControllerBase
    {
        private readonly IWalletRepository walletRepo;
        private readonly IMapper mapper;
        public ICustomPasswordHasher _passwordHasher;
        public ITokenAuthorization _tokenAuthorization;

        private WalletDbContext walletDbContext;
        public WalletController (IWalletRepository walletRepo, WalletDbContext context, IMapper mapper, ICustomPasswordHasher _passwordHasher, ITokenAuthorization tokenAuthorization)
        {
            this.walletRepo = walletRepo;
            this.walletDbContext = context;
            this.mapper = mapper;
            this._passwordHasher = _passwordHasher;
            this._tokenAuthorization = tokenAuthorization;
        }

        /// <summary>
        /// Get all user wallet
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<List<WalletModel>> GetWallets()
        {
            // var wallets = walletRepo.GetWallet();
            var wallets = walletDbContext.DbWallets.ToList();
            return Ok(wallets);
        }

        /// <summary>
        /// Get individual wallet
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<WalletModel> GetWallet(int id)
        {
            //var wallet = walletRepo.GetWallet(id);
            var wallet = walletDbContext.DbWallets.FirstOrDefault(wallet => wallet.Id == id);
            if (wallet == null)
            {
                return NotFound();
            }

            return Ok(wallet);
        }

        /// <summary>
        /// Create wallet
        /// </summary>
        [HttpPost("create")]
        public ActionResult<NewWalletDto> CreateWallet(NewWalletDto newWallet)
        {

            newWallet.CurrencyType = newWallet.CurrencyType.ToUpper();

            // Check for duplicate
            var duplicateWallet = walletDbContext.DbWallets.FirstOrDefault(wallet => wallet.Email == newWallet.Email);
            if(duplicateWallet != null)
            {
                return Problem("User exist already");
            }

            WalletModel wallet = mapper.Map<WalletModel>(newWallet);
            wallet.History = $"You account was created {DateTime.Now}";

            string newPassword = _passwordHasher.hashPassword(wallet.Password);
            wallet.Password = newPassword;
           

            walletDbContext.DbWallets.Add(wallet);
            //walletRepo.CreateWallet(wallet);

            walletDbContext.SaveChanges();

            return CreatedAtAction(nameof(CreateWallet), wallet);
        }

        /// <summary>
        /// Login to wallet
        /// </summary>
        /// <param name="userlogin"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public ActionResult<WalletLoginResponseDto> LoginToWallet(WalletLoginDto userlogin)
        {
            WalletLoginResponseDto LoginResponse = new();

            WalletModel currentUser = walletDbContext.DbWallets.FirstOrDefault(wallet => wallet.Email == userlogin.Email);
            if(currentUser == null)
            {
                return Unauthorized("User does not exist");
            }

            Console.WriteLine(userlogin.Password);

            string hashedPassword = _passwordHasher.hashPassword(userlogin.Password);

            Console.WriteLine(hashedPassword);
            Console.WriteLine(currentUser.Password);
            if (!currentUser.Password.Equals(hashedPassword))
            {
                return Unauthorized("Passwords do not match");
            }

            string token = _tokenAuthorization.CreateToken(currentUser);
            
            LoginResponse.CurrentUser = currentUser;
            LoginResponse.AuthToken = token;
            // return  Ok(currentUser);

            return  Ok(LoginResponse);
        }

        /// <summary>
        /// fund wallet
        /// </summary>
        /// <param name="walletToFund"></param>
        /// <returns></returns>
        [HttpPut("fund")]
        public ActionResult<NewWalletDto> FundUserWallet(FundWalletDto walletToFund)
        {
            var userId = walletToFund.Id;
            var wallet = walletDbContext.DbWallets.FirstOrDefault(wallet => wallet.Id == userId);
            wallet.Balance = wallet.Balance + walletToFund.Balance;
            wallet.History = wallet.History + ";" + $"You funded your account with {walletToFund.Balance}";
            walletDbContext.DbWallets.Update(wallet);
            //walletRepo.CreateWallet(wallet);
            walletDbContext.SaveChanges();
            return CreatedAtAction(nameof(FundUserWallet), wallet);
        }


        /// <summary>
        /// Update user wallet
        /// </summary>
        [HttpPut("/update")]
        public ActionResult<WalletModel> UpdateUserInfo(UpdateUserDto updateUser)
        {
            var userId = updateUser.Id;
            var wallet = walletDbContext.DbWallets.FirstOrDefault(wallet => wallet.Id == userId);
            if (wallet == null) return NotFound();
            if (updateUser.FirstName != null) wallet.FirstName = updateUser.FirstName;
            if (updateUser.LastName != null) wallet.LastName = updateUser.LastName;
            if (updateUser.Password != null) wallet.Password = updateUser.Password;
            if (updateUser.Username != null) wallet.Username = updateUser.Username;
            if (updateUser.DateOfBirth != null) wallet.DateOfBirth = updateUser.DateOfBirth;
            if (updateUser.CurrencyType != null) wallet.CurrencyType = updateUser.CurrencyType;
            wallet.History = wallet.History + ";" + $"Updated profile at {DateTime.Now}";
            walletDbContext.DbWallets.Update(wallet);
            walletDbContext.SaveChanges();
            return Ok(wallet);
        }  
        
        /// <summary>
        /// Update user wallet
        /// </summary>
        [HttpPut("/change/password")]
        public ActionResult<WalletModel> ChangePassword(ChangePassword changePassword)
        {
            var userId = changePassword.Id;
            var wallet = walletDbContext.DbWallets.FirstOrDefault(wallet => wallet.Id == userId);
            if (wallet == null) return NotFound();
            if (changePassword.Password != null) wallet.FirstName = changePassword.Password;
            wallet.History = wallet.History + ";" + $"Updated password at {DateTime.Now}";
            walletDbContext.DbWallets.Update(wallet);
            walletDbContext.SaveChanges();
            return Ok(wallet);
        }

        /// <summary>
        /// Transfer to another wallet
        /// </summary>
        /// <param name="transferFunds"></param>
        /// <returns></returns>
        [HttpPut("fund/transfer")]
        public ActionResult<NewWalletDto> TransferFunds(TransferFundsDto transferFunds)
        {
            using(var transaction = walletDbContext.Database.BeginTransaction())
            {

                try
                {
                    var recipientId = transferFunds.RecipientId;
                    var senderId = transferFunds.SenderId;

                    var recipientWallet = walletDbContext.DbWallets.FirstOrDefault(wallet => wallet.Id == recipientId);
                    var senderWallet = walletDbContext.DbWallets.FirstOrDefault(wallet => wallet.Id == senderId);
                    recipientWallet.Balance = recipientWallet.Balance + transferFunds.FundsSent;
                    recipientWallet.History = recipientWallet.History + ";" + $"Your received {recipientWallet.CurrencyType} {transferFunds.FundsSent} from {senderWallet.FirstName} at {DateTime.Now}";
                    
                    senderWallet.Balance = senderWallet.Balance - transferFunds.FundsSent;
                    senderWallet.History = senderWallet.History + ";" + $"You transfered {senderWallet.CurrencyType} {transferFunds.FundsSent} to {recipientWallet.FirstName} at {DateTime.Now}";

                    walletDbContext.DbWallets.Update(recipientWallet);
                    walletDbContext.DbWallets.Update(senderWallet);
                    //walletRepo.CreateWallet(wallet);


                    walletDbContext.SaveChanges();

                    transaction.Commit();   
                    return CreatedAtAction(nameof(FundUserWallet), recipientWallet);

                }catch (Exception ex)
                {
                    transaction.Rollback();
                    return Problem("Please try again");

                }
            }
        }
    }


}
