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
            public DateTime LastTransactionDate { get; set; }
        }

    }
}
