using POLuokat;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace POData
{
    public class TilausRiviRepository : DataAccess
    {
        public TilausRiviRepository(string conString) : base(conString) { }

        /// <summary>
        /// Parsii TilausRivi-olion IDataReaderistä
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private TilausRivi TeeRivistaTilausRivi(IDataReader reader) {
            var paluu = new TilausRiviProxy(int.Parse(reader["OrderID"].ToString())) {
                ProductID = (!(reader["ProductID"] is DBNull) ? int.Parse(reader["ProductID"].ToString()) : (int?)null),
                UnitPrice = (!(reader["UnitPrice"] is DBNull) ? double.Parse(reader["UnitPrice"].ToString().Replace('.', ',')) : (double?)null),
                Quantity = (!(reader["Quantity"] is DBNull) ? int.Parse(reader["Quantity"].ToString()) : (int?)null),
                Discount = (!(reader["Discount"] is DBNull) ? float.Parse(reader["Discount"].ToString()) : (float?)null)
            };

            paluu.TilausOtsikkoRepository = new TilausOtsikkoRepository(ConnectionString);
            paluu.TuoteRepository = new TuoteRepository(ConnectionString);

            return (paluu);
        }

        /// <summary>
        /// Tekee listan tilausriveistä
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private List<TilausRivi> TeeTilausRiviLista(IDataReader reader) {
            var asiakkaat = new List<TilausRivi>();
            while (reader.Read()) {
                asiakkaat.Add(TeeRivistaTilausRivi(reader));
            }
            return (asiakkaat);
        }

        public List<TilausRivi> HaeTilauksenKaikki(int id) {
            string sql = "SELECT * FROM dbo.[Order Details] WHERE OrderID = @OrderID";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@OrderID", id));
                        return (TeeTilausRiviLista(cmd.ExecuteReader()));
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"Tietokantavirhe: {e.Message}");
            }
        }

        public List<TilausRivi> HaeTuotteenKaikki(int id) {
            string sql = "SELECT * FROM dbo.[Order Details] WHERE ProductID = @ProductID";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@ProductID", id));
                        return (TeeTilausRiviLista(cmd.ExecuteReader()));
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"Tietokantavirhe: {e.Message}");
            }
        }
    }
}
