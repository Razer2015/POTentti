using POLuokat;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace POData
{
    public class CustomerRepository : DataAccess
    {
        public CustomerRepository(string conString) : base(conString) { }

        /// <summary>
        /// Parses Customer from IDataReader
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private Customer CreateCustomer(IDataReader reader) {
            var customer = new CustomerProxy(reader["CustomerID"].ToString(), reader["CompanyName"].ToString()) {
                ContactName = (!(reader["ContactName"] is DBNull) ? reader["ContactName"].ToString() : null),
                ContactTitle = (!(reader["ContactTitle"] is DBNull) ? reader["ContactTitle"].ToString() : null),
                Address = (!(reader["Address"] is DBNull) ? reader["Address"].ToString() : null),
                City = (!(reader["City"] is DBNull) ? reader["City"].ToString() : null),
                Region = (!(reader["Region"] is DBNull) ? reader["Region"].ToString() : null),
                PostalCode = (!(reader["PostalCode"] is DBNull) ? reader["PostalCode"].ToString() : null),
                Country = (!(reader["Country"] is DBNull) ? reader["Country"].ToString() : null),
                Phone = (!(reader["Phone"] is DBNull) ? reader["Phone"].ToString() : null),
                Fax = (!(reader["Fax"] is DBNull) ? reader["Fax"].ToString() : null)
            };

            customer.OrderRepository = new OrderRepository(ConnectionString);

            return (customer);
        }

        /// <summary>
        /// Parses Customers from IDataReader
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private List<Customer> CreateCustomers(IDataReader reader) {
            var customers = new List<Customer>();
            while (reader.Read()) {
                customers.Add(CreateCustomer(reader));
            }
            return (customers);
        }

        /// <summary>
        /// Searches for a customer based on customerID
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public Customer Search(string customerID) {
            string sql = "SELECT * FROM dbo.Customers WHERE CustomerID = @CustomerID";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));
                        var reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                        reader.Read();
                        return (CreateCustomer(reader));
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"DatabaseError: {e.Message}");
            }
        }

        /// <summary>
        /// Searches all customers
        /// </summary>
        /// <returns></returns>
        public List<Customer> SearchAll() {
            string sql = "SELECT * FROM dbo.Customers";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        return (CreateCustomers(cmd.ExecuteReader()));
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"DatabaseError: {e.Message}");
            }
        }

        /// <summary>
        /// Searcher for a customer by a companyName
        /// </summary>
        /// <param name="companyName"></param>
        /// <returns></returns>
        public List<Customer> SearchByName(string companyName) {
            string sql = "SELECT * FROM dbo.Customers WHERE CompanyName LIKE @CompanyName";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@CompanyName", $"{companyName}%"));
                        return (CreateCustomers(cmd.ExecuteReader()));
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"DatabaseError: {e.Message}");
            }
        }

        /// <summary>
        /// Searches for a customer by a city
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public List<Customer> SearchByCity(string city) {
            string sql = "SELECT * FROM dbo.Customers WHERE City LIKE @City";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@City", $"{city}%"));
                        return (CreateCustomers(cmd.ExecuteReader()));
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"DatabaseError: {e.Message}");
            }
        }

        /// <summary>
        /// Searches for a customer by country
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        public List<Customer> SearchByCountry(string country) {
            string sql = "SELECT * FROM dbo.Customers WHERE Country LIKE @Country";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@Country", $"{country}%"));
                        return (CreateCustomers(cmd.ExecuteReader()));
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"DatabaseError: {e.Message}");
            }
        }

        /// <summary>
        /// Adds a new customer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public bool Add(Customer customer) {
            string sql = "INSERT INTO dbo.Customers(CustomerID, CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax) VALUES(@CustomerID, @CompanyName, @ContactName, @ContactTitle, @Address, @City, @Region, @PostalCode, @Country, @Phone, @Fax)";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@CustomerID", customer.CustomerID));
                        cmd.Parameters.Add(new SqlParameter("@CompanyName", customer.CompanyName));
                        cmd.Parameters.Add(new SqlParameter("@ContactName", customer.ContactName ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@ContactTitle", customer.ContactTitle ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@Address", customer.Address ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@City", customer.City ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@Region", customer.Region ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@PostalCode", customer.PostalCode ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@Country", customer.Country ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@Phone", customer.Phone ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@Fax", customer.Fax ?? (object)DBNull.Value));
                        return (cmd.ExecuteNonQuery() == 1 ? true : false);
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"DatabaseError: {e.Message}");
            }
        }

        /// <summary>
        /// Modifies the customer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public bool Change(Customer customer) {
            string sql = "UPDATE dbo.Customers SET CompanyName = @CompanyName, ContactName = @ContactName, ContactTitle = @ContactTitle, Address = @Address, City = @City, Region = @Region, PostalCode = @PostalCode, Country = @Country, Phone = @Phone, Fax = @Fax WHERE CustomerID = @CustomerID";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@CustomerID", customer.CustomerID));

                        cmd.Parameters.Add(new SqlParameter("@CompanyName", customer.CompanyName));
                        cmd.Parameters.Add(new SqlParameter("@ContactName", customer.ContactName ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@ContactTitle", customer.ContactTitle ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@Address", customer.Address ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@City", customer.City ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@Region", customer.Region ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@PostalCode", customer.PostalCode ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@Country", customer.Country ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@Phone", customer.Phone ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@Fax", customer.Fax ?? (object)DBNull.Value));
                        return (cmd.ExecuteNonQuery() == 1 ? true : false);
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"DatabaseError: {e.Message}");
            }
        }

        /// <summary>
        /// Deletes a customer based on customerID
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public bool Delete(string customerID) {
            string sql = "DELETE FROM dbo.Customers WHERE CustomerID = @CustomerID";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));
                        return (cmd.ExecuteNonQuery() == 1 ? true : false);
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"DatabaseError: {e.Message}");
            }
        }
    }
}
