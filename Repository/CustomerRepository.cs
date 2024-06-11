using BankAccount.Model;
using BankAccount.Repository.Interface;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using static BankAccount.Model.AdminModel;

namespace BankAccount.Repository
{
    public class CustomerRepository : ICustomer
    {

        private readonly IConfiguration _configuration;
        private SqlConnection _connect;

        public CustomerRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Connection()
        {
            string connects = _configuration.GetConnectionString("connect") ?? "";
            _connect = new SqlConnection(connects);
        }

        public async Task<List<GetCustomer>> GetCustomerById(int CustomerId)
        {
            try
            {
                Connection();
                _connect.Open();
                List<GetCustomer> customerProfile = new List<GetCustomer>();
                SqlCommand command = new SqlCommand("GetCustomerById", _connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CustomerId", CustomerId);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                foreach (DataRow list in dt.Rows)
                {
                    customerProfile.Add(new GetCustomer
                    {
                        CustomerId = Convert.ToInt32(list["customer_id"]),
                        CustomerName = list["customer_name"].ToString(),
                        Age = Convert.ToInt32(list["age"]),
                        Gender = list["gender"].ToString(),
                        Address = list["address"].ToString(),
                        Email = list["email"].ToString(),
                        PhoneNumber = list["phone_number"].ToString(),
                        IdentificationType = list["identification_type"].ToString(),
                        IdentificationNumber = list["identification_number"].ToString()
                    });



                }
                return customerProfile;
            }
            finally
            {
                _connect.Close();
            }
        }


        public async Task<decimal?> GetAccountBalanceAsync(long accountId)
        {
            try
            {
                Connection();
                await _connect.OpenAsync();
                decimal? balance = null;

                SqlCommand command = new SqlCommand("SP_GetAccountBalance", _connect)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@AccountId", accountId);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    await reader.ReadAsync();
                    balance = Convert.ToDecimal(reader["balance"]);
                }

                return balance;
            }
            finally
            {
                if (_connect != null && _connect.State != ConnectionState.Closed)
                {
                    await _connect.CloseAsync();
                }
            }
        }




        public async Task<List<GetAccount>> GetAccountById(long account_number)
        {
            try
            {
                Connection();
                await _connect.OpenAsync();
                List<GetAccount> AccountDetails = new List<GetAccount>();

                SqlCommand command = new SqlCommand("SP_GetAccountByAccountNumber", _connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@account_number", account_number);

                SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    AccountDetails.Add(new GetAccount
                    {
                        AccountId = Convert.ToInt32(reader["account_id"]),
                        CustomerId = Convert.ToInt32(reader["customer_id"]),
                        AccountNumber = Convert.ToInt64(reader["account_number"]),
                        AccountType = reader["account_type"].ToString() ?? "",
                        Balance = Convert.ToDecimal(reader["balance"]),
                        CreatedDate = Convert.ToDateTime(reader["created_date"]),
                        Status = reader["status"].ToString() ?? "",
                        CardType = reader["card_type"].ToString() ?? "",
                        IsActiveCard = Convert.ToBoolean(reader["isActive_card"]),
                        LastTransactionDate = reader["last_transaction_date"] != DBNull.Value ? Convert.ToDateTime(reader["last_transaction_date"]) : (DateTime?)null,
                        CardNumber = reader["card_number"] != DBNull.Value ? reader["card_number"].ToString() : null,
                        CardExpiryDate = reader["card_expiry_date"] != DBNull.Value ? Convert.ToDateTime(reader["card_expiry_date"]) : (DateTime?)null
                    });
                }

                await reader.CloseAsync();
                return AccountDetails;
            }
            finally
            {
                await _connect.CloseAsync();
            }
        }

        public async Task<List<Transaction>> GetTransactionsByAccountNumber(long accountNumber)
        {
            List<Transaction> transactions = new List<Transaction>();
            try
            {
                Connection();
                await _connect.OpenAsync();

                SqlCommand command = new SqlCommand("SP_GetTransactionsByAccountNumber", _connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@account_number", accountNumber);


                SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    transactions.Add(new Transaction
                    {
                        TransactionId = Convert.ToInt32(reader["transaction_id"]),
                        AccountNumber = Convert.ToInt64(reader["account_number"]),
                        TransactionType = reader["transaction_type"].ToString(),
                        Amount = Convert.ToDecimal(reader["amount"]),
                        TransactionDate = Convert.ToDateTime(reader["transaction_date"]),
                        Description = reader["description"].ToString()
                    });
                }

                await reader.CloseAsync();
            }
            finally
            {
                await _connect.CloseAsync();
            }

            return transactions;
        }

        #region Post method
        public async Task<(string resultMessage, long? accountNumber)> CreateAccountAsync(CreateAccountRequest account)
        {
            try
            {
                Connection();
                await _connect.OpenAsync();

                SqlCommand command = new SqlCommand("SP_CreateAccount", _connect);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@customer_id", account.customerId);
                command.Parameters.AddWithValue("@account_type", account.accountType);
                command.Parameters.AddWithValue("@balance", account.balance);
                command.Parameters.AddWithValue("@card_type", account.cardType);

                SqlParameter resultMessageParam = new SqlParameter("@resultMessage", SqlDbType.NVarChar, 255)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(resultMessageParam);

                SqlParameter accountNumberParam = new SqlParameter("@generatedAccountNumber", SqlDbType.BigInt)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(accountNumberParam);

                await command.ExecuteNonQueryAsync();

                string resultMessage = resultMessageParam.Value.ToString();
                long? accountNumber = accountNumberParam.Value != DBNull.Value ? (long?)accountNumberParam.Value : null;

                return (resultMessage, accountNumber);
            }
            finally
            {
                await _connect.CloseAsync();
            }
        }





        public async Task<string> AddNewUser(PostCustomer user)
        {
            try
            {
                Connection();
                _connect.Open();
                SqlCommand command = new SqlCommand("SP_AddCustomer", _connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@customer_name", user.CustomerName);
                command.Parameters.AddWithValue("@age", user.Age);
                command.Parameters.AddWithValue("@gender", user.Gender);
                command.Parameters.AddWithValue("@address", user.Address);
                command.Parameters.AddWithValue("@email", user.Email);
                command.Parameters.AddWithValue("@phone_number", user.PhoneNumber);
                command.Parameters.AddWithValue("@identification_type", user.IdentificationType);
                command.Parameters.AddWithValue("@identification_number", user.IdentificationNumber);
                var outputMessage = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 250)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputMessage);

                await command.ExecuteNonQueryAsync();

                return outputMessage.Value.ToString();

            }
            finally
            {
                _connect.Close();
            }
        }


        public async Task<string> WithdrawAsync(WithdrawalRequest request)
        {
            try
            {
                Connection();
                await _connect.OpenAsync();

                SqlCommand command = new SqlCommand("SP_Withdraw", _connect)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@account_number", request.AccountNumber);
                command.Parameters.AddWithValue("@amount", request.Amount);
                command.Parameters.AddWithValue("@description", request.Description);

                SqlParameter resultMessageParam = new SqlParameter("@resultMessage", SqlDbType.NVarChar, 255)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(resultMessageParam);

                await command.ExecuteNonQueryAsync();

                return resultMessageParam.Value.ToString();
            }
            finally
            {
                await _connect.CloseAsync();
            }
        }

        public async Task<string> DepositAsync(WithdrawalRequest request)
        {
            try
            {
                Connection();
                await _connect.OpenAsync();

                SqlCommand command = new SqlCommand("SP_Deposit", _connect)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@account_number", request.AccountNumber);
                command.Parameters.AddWithValue("@amount", request.Amount);
                command.Parameters.AddWithValue("@description", request.Description);

                SqlParameter resultMessageParam = new SqlParameter("@resultMessage", SqlDbType.NVarChar, 255)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(resultMessageParam);

                await command.ExecuteNonQueryAsync();

                return resultMessageParam.Value.ToString();
            }
            finally
            {
                await _connect.CloseAsync();
            }
        }
        #endregion






    }
}
