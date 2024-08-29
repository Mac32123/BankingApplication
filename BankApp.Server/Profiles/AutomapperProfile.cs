using AutoMapper;
using BankApp.Server.Models;

namespace BankApp.Server.Profiles
{
	public class AutomapperProfile : Profile
	{
        public AutomapperProfile()
        {
            CreateMap<RegisterNewAccountModel, Account>();

            CreateMap<UpdateAccountModel, Account>();
            CreateMap<Account, GetAccountModel>();
            CreateMap<TransactionRequestModel, Transaction>();
            CreateMap<Account, AuthenticatedAccountModel>();
            CreateMap<Account, CreatedAccountModel>();
        }
    }
}
