using POLuokat;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace POData
{
    public class AsiakasRepository : DataAccess
    {
        public AsiakasRepository(string conString) : base(conString) { }

        /// <summary>
        /// Parsii Asiakas-olion IDataReaderistä
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private Asiakas TeeRivistaAsiakas(IDataReader reader) {
            var paluu = new AsiakasProxy(reader["CustomerID"].ToString(), reader["CompanyName"].ToString()) {
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


            paluu.TilausOtsikkoRepository = new TilausOtsikkoRepository(ConnectionString);

            return (paluu);
        }

        /// <summary>
        /// Tekee listan asiakkaista
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private List<Asiakas> TeeAsiakasLista(IDataReader reader) {
            var asiakkaat = new List<Asiakas>();
            while (reader.Read()) {
                asiakkaat.Add(TeeRivistaAsiakas(reader));
            }
            return (asiakkaat);
        }

        public Asiakas Hae(string id) {
            string sql = "SELECT * FROM dbo.Customers WHERE CustomerID = @CustomerID";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@CustomerID", id));
                        var reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                        reader.Read();
                        return (TeeRivistaAsiakas(reader));
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"Tietokantavirhe: {e.Message}");
            }
        }

        public List<Asiakas> HaeKaikki() {
            string sql = "SELECT * FROM dbo.Customers";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        return (TeeAsiakasLista(cmd.ExecuteReader()));
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"Tietokantavirhe: {e.Message}");
            }
        }

        public List<Asiakas> HaeByNimi(string nimi) {
            string sql = "SELECT * FROM dbo.Customers WHERE CompanyName LIKE @CompanyName";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@CompanyName", $"{nimi}%"));
                        return (TeeAsiakasLista(cmd.ExecuteReader()));
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"Tietokantavirhe: {e.Message}");
            }
        }

        public List<Asiakas> HaeByCity(string kaupunki) {
            string sql = "SELECT * FROM dbo.Customers WHERE City LIKE @City";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@City", $"{kaupunki}%"));
                        return (TeeAsiakasLista(cmd.ExecuteReader()));
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"Tietokantavirhe: {e.Message}");
            }
        }

        public List<Asiakas> HaeByCountry(string maa) {
            string sql = "SELECT * FROM dbo.Customers WHERE Country LIKE @Country";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@Country", $"{maa}%"));
                        return (TeeAsiakasLista(cmd.ExecuteReader()));
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"Tietokantavirhe: {e.Message}");
            }
        }

        public bool Lisaa(Asiakas item) {
            string sql = "INSERT INTO dbo.Customers(CustomerID, CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax) VALUES(@CustomerID, @CompanyName, @ContactName, @ContactTitle, @Address, @City, @Region, @PostalCode, @Country, @Phone, @Fax)";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@CustomerID", item.Id));
                        cmd.Parameters.Add(new SqlParameter("@CompanyName", item.CompanyName));
                        cmd.Parameters.Add(new SqlParameter("@ContactName", item.ContactName ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@ContactTitle", item.ContactTitle ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@Address", item.Address ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@City", item.City ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@Region", item.Region ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@PostalCode", item.PostalCode ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@Country", item.Country ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@Phone", item.Phone ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@Fax", item.Fax ?? (object)DBNull.Value));
                        return (cmd.ExecuteNonQuery() == 1 ? true : false);
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"Tietokantavirhe: {e.Message}");
            }
        }

        public bool Muuta(Asiakas item) {
            string sql = "UPDATE dbo.Customers SET CompanyName = @CompanyName, ContactName = @ContactName, ContactTitle = @ContactTitle, Address = @Address, City = @City, Region = @Region, PostalCode = @PostalCode, Country = @Country, Phone = @Phone, Fax = @Fax WHERE CustomerID = @CustomerID";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@CustomerID", item.Id));

                        cmd.Parameters.Add(new SqlParameter("@CompanyName", item.CompanyName));
                        cmd.Parameters.Add(new SqlParameter("@ContactName", item.ContactName ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@ContactTitle", item.ContactTitle ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@Address", item.Address ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@City", item.City ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@Region", item.Region ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@PostalCode", item.PostalCode ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@Country", item.Country ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@Phone", item.Phone ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqlParameter("@Fax", item.Fax ?? (object)DBNull.Value));
                        return (cmd.ExecuteNonQuery() == 1 ? true : false);
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"Tietokantavirhe: {e.Message}");
            }
        }

        public bool Poista(string id) {
            string sql = "DELETE FROM dbo.Customers WHERE CustomerID = @CustomerID";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@CustomerID", id));
                        return (cmd.ExecuteNonQuery() == 1 ? true : false);
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"Tietokantavirhe: {e.Message}");
            }
        }
    }
}
