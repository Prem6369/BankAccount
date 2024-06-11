using static BankAccount.Model.AdminModel;

namespace BankAccount.Repository.Interface
{
    public interface IAdmin
    {
        Task<IEnumerable<AccountDetails>> GetAllByAccountAsync();
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<bool> UpdateBankActiveStatusAsync(int customerId);
        Task<string> UpdateCardDetailsAsync(CardDetails cardDetails);
    }
}
