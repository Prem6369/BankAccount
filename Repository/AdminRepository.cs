using BankAccount.Repository.Interface;
using System.Data;
using System.Data.SqlClient;
using static BankAccount.Model.AdminModel;

namespace BankAccount.Repository
{
    public class AdminRepository:IAdmin
    {
        private readonly IConfiguration _configuration;
        private SqlConnection _connect;

        public AdminRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Connection()
        {
            string connects = _configuration.GetConnectionString("connect")??"";
            _connect = new SqlConnection(connects);
        }


        public async Task<IEnumerable<AccountDetails>> GetAllByAccountAsync()
        {
            try
            {
                Connection();
                await _connect.OpenAsync();

                SqlCommand command = new SqlCommand("SP_GetByAccountDetails", _connect)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                List<AccountDetails> accountDetailsList = new List<AccountDetails>();
                foreach (DataRow row in dt.Rows)
                {
                    accountDetailsList.Add(new AccountDetails
                    {
                        AccountId = Convert.ToInt32(row["account_id"]),
                        CustomerId = Convert.ToInt32(row["customer_id"]),
                        AccountNumber = Convert.ToInt64(row["account_number"]),
                        AccountType = row["account_type"].ToString(),
                        Balance = Convert.ToDecimal(row["balance"]),
                        CreatedDate = Convert.ToDateTime(row["created_date"]),
                        Status = row["status"].ToString(),
                        CardType = row["card_type"].ToString(),
                        IsActiveCard = Convert.ToBoolean(row["isActive_card"]),
                        LastTransactionDate = row["last_transaction_date"] != DBNull.Value ? Convert.ToDateTime(row["last_transaction_date"]) : (DateTime?)null,
                        CardNumber = row["card_number"].ToString(),
                        CardExpiryDate = row["card_expiry_date"] != DBNull.Value ? Convert.ToDateTime(row["card_expiry_date"]) : (DateTime?)null
                    });
                }

                return accountDetailsList;
            }
            finally
            {
                await _connect.CloseAsync();
            }
        }



        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            Connection();
            await _connect.OpenAsync();

            SqlCommand command = new SqlCommand("SP_GetAllCustomers", _connect)
            {
                CommandType = CommandType.StoredProcedure
            };

            var customers = new List<Customer>();
            var reader = await command.ExecuteReaderAsync();
                
            while (await reader.ReadAsync())
            {
                var customer = new Customer
                {
                    CustomerId = reader.GetInt32(reader.GetOrdinal("customer_id")),
                    CustomerName = reader.GetString(reader.GetOrdinal("customer_name")),
                    Age = reader.GetInt32(reader.GetOrdinal("age")),
                    Gender = reader.GetString(reader.GetOrdinal("gender")),
                    Address = reader.GetString(reader.GetOrdinal("address")),
                    Email = reader.GetString(reader.GetOrdinal("email")),
                    PhoneNumber = reader.GetString(reader.GetOrdinal("phone_number")),
                    IdentificationType = reader.GetString(reader.GetOrdinal("identification_type")),
                    IdentificationNumber = reader.GetString(reader.GetOrdinal("identification_number"))
                };
                customers.Add(customer);
            }
                

         return customers;
          
        }



        public async Task<bool> UpdateBankActiveStatusAsync(int customerId)
        {
            try
            {
                Connection();

                await _connect.OpenAsync();

                var command = new SqlCommand("SP_UpdateBankActiveStatus", _connect)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@customer_id", customerId);

                int rowsAffected = await command.ExecuteNonQueryAsync();

                return rowsAffected > 0; 
            }
            finally
            {
                if (_connect != null && _connect.State != ConnectionState.Closed)
                {
                    _connect.Close();
                }
            }
        }

        public async Task<string> UpdateCardDetailsAsync(CardDetails cardDetails)
        {
            try
            {
                Connection();
                await _connect.OpenAsync();

                var command = new SqlCommand("SP_UpdateCardDetails", _connect)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@customer_id", cardDetails.CustomerId);
                command.Parameters.AddWithValue("@card_type", cardDetails.CardType);
                command.Parameters.AddWithValue("@card_number", cardDetails.CardNumber);
                command.Parameters.AddWithValue("@card_expiry_date", cardDetails.CardExpiryDate);

                var resultMessageParameter = command.Parameters.Add("@resultMessage", SqlDbType.NVarChar, 255);
                resultMessageParameter.Direction = ParameterDirection.Output;

                await command.ExecuteNonQueryAsync();

                return resultMessageParameter.Value.ToString();
            }
            finally
            {
                if (_connect != null && _connect.State != ConnectionState.Closed)
                {
                    _connect.Close();
                }
            }
        }





    }



}
