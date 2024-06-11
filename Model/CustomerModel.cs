namespace BankAccount.Model
{
    public class CustomerModel
    {
    }

    public class GetCustomer
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
    }


    public class PostCustomer
    {
        public string CustomerName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
    }


    public class Transaction
    {
        public int TransactionId { get; set; }
        public long AccountNumber { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
    }

    public class WithdrawalRequest
    {
        public long AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }




}
