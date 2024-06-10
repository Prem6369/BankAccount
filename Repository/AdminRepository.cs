using System.Data.SqlClient;

namespace BankAccount.Repository
{
    public class AdminRepository
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


    }
}
