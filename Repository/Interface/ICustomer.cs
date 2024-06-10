using BankAccount.Model;
using static BankAccount.Model.AdminModel;

namespace BankAccount.Repository.Interface
{
    public interface ICustomer
    {
        Task<List<GetCustomer>> GetCustomerById(int CustomerId);
        Task<string> AddNewUser(PostCustomer user);
        Task<List<GetAccount>> GetAccountById(int account_number);
    }
}
