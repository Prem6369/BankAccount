using BankAccount.Model;
using static BankAccount.Model.AdminModel;

namespace BankAccount.Repository.Interface
{
    public interface ICustomer
    {
        Task<List<GetCustomer>> GetCustomerById(int CustomerId);
        Task<(string resultMessage, int customerId)> AddNewUser(PostCustomer user);
        Task<List<GetAccount>> GetAccountById(long account_number);
        Task<(string resultMessage, long? accountNumber)> CreateAccountAsync(CreateAccountRequest account);
        Task<List<Transaction>> GetTransactionsByAccountNumber(long accountNumber);

        Task<decimal?> GetAccountBalanceAsync(long accountId);

        Task<string> WithdrawAsync(WithdrawalRequest request);
        Task<string> DepositAsync(WithdrawalRequest request);
    }
}
