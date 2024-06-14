namespace BankAccount.Model
{
    public class AdminModel
    {
        public class GetAccount
        {
            public int AccountId { get; set; }
            public int CustomerId { get; set; }
            public long AccountNumber { get; set; }
            public string AccountType { get; set; }
            public decimal Balance { get; set; }
            public DateTime CreatedDate { get; set; }
            public string Status { get; set; }
            public string CardType { get; set; }
            public bool IsActiveCard { get; set; }
            public DateTime? LastTransactionDate { get; set; }
            public string CardNumber { get; set; }
            public DateTime? CardExpiryDate { get; set; }
        }


        public class CreateAccountRequest
        {
            public int customerId { get; set; }
            public string email { get; set; }

            public string accountType { get; set; }
            public decimal balance { get; set; }
            public string cardType { get; set; }

        }

        public class AccountDetails
        {
            public int AccountId { get; set; }
            public int CustomerId { get; set; }
            public long AccountNumber { get; set; }
            public string AccountType { get; set; }
            public decimal Balance { get; set; }
            public DateTime CreatedDate { get; set; }
            public string Status { get; set; }
            public string CardType { get; set; }
            public bool IsActiveCard { get; set; }
            public DateTime? LastTransactionDate { get; set; }
            public string CardNumber { get; set; }
            public DateTime? CardExpiryDate { get; set; }
        }


        public class Customer
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

        public class CardDetails
        {
            public int CustomerId { get; set; }
            public string CardType { get; set; }
            public string CardNumber { get; set; }
            public DateTime CardExpiryDate { get; set; }
        }

        public class VaildateCustomer
        {
            public string PaymentMethod { get; set; }
            public string Email { get; set; }
            public decimal Amount { get; set; }
        }

    }
}
