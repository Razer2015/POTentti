using POLuokat;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace POData
{
    public class TilausOtsikkoRepository : DataAccess
    {
        public TilausOtsikkoRepository(string conString) : base(conString) { }

        /// <summary>
        /// Parsii TilausOtsikko-olion IDataReaderistä
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private TilausOtsikko TeeRivistaTilausOtsikko(IDataReader reader) {
            var paluu = new TilausOtsikkoProxy(int.Parse(reader["OrderID"].ToString()), reader["CustomerID"].ToString()) {
                EmployeeID = (!(reader["EmployeeID"] is DBNull) ? int.Parse(reader["EmployeeID"].ToString()) : (int?)null),
                OrderDate = (!(reader["OrderDate"] is DBNull) ? DateTime.Parse(reader["OrderDate"].ToString()) : (DateTime?)null),
                RequiredDate = (!(reader["RequiredDate"] is DBNull) ? DateTime.Parse(reader["RequiredDate"].ToString()) : (DateTime?)null),
                ShippedDate = (!(reader["ShippedDate"] is DBNull) ? DateTime.Parse(reader["ShippedDate"].ToString()) : (DateTime?)null),
                ShipVia = (!(reader["ShipVia"] is DBNull) ? int.Parse(reader["ShipVia"].ToString()) : (int?)null),
                Freight = (!(reader["Freight"] is DBNull) ? double.Parse(reader["Freight"].ToString().Replace('.', ',')) : (double?)null),
                ShipName = (!(reader["ShipName"] is DBNull) ? reader["ShipName"].ToString() : null),
                ShipAddress = (!(reader["ShipAddress"] is DBNull) ? reader["ShipAddress"].ToString() : null),
                ShipCity = (!(reader["ShipCity"] is DBNull) ? reader["ShipCity"].ToString() : null),
                ShipRegion = (!(reader["ShipRegion"] is DBNull) ? reader["ShipRegion"].ToString() : null),
                ShipPostalCode = (!(reader["ShipPostalCode"] is DBNull) ? reader["ShipPostalCode"].ToString() : null),
                ShipCountry = (!(reader["ShipCountry"] is DBNull) ? reader["ShipCountry"].ToString() : null)
            };

            paluu.AsiakasRepository = new AsiakasRepository(ConnectionString);
            paluu.TilausRiviRepository = new TilausRiviRepository(ConnectionString);

            return (paluu);
        }

        /// <summary>
        /// Tekee listan tilausotsikoista
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private List<TilausOtsikko> TeeTilausOtsikkoLista(IDataReader reader) {
            var asiakkaat = new List<TilausOtsikko>();
            while (reader.Read()) {
                asiakkaat.Add(TeeRivistaTilausOtsikko(reader));
            }
            return (asiakkaat);
        }

        public TilausOtsikko Hae(int id) {
            string sql = "SELECT * FROM dbo.Orders WHERE OrderID = @OrderID";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@OrderID", id));
                        var reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                        reader.Read();
                        return (TeeRivistaTilausOtsikko(reader));
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"Tietokantavirhe: {e.Message}");
            }
        }

        public List<TilausOtsikko> HaeAsiakkaanKaikki(string id) {
            string sql = "SELECT * FROM dbo.Orders WHERE CustomerID = @CustomerID";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@CustomerID", id));
                        return (TeeTilausOtsikkoLista(cmd.ExecuteReader()));
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"Tietokantavirhe: {e.Message}");
            }
        }
    }
}
