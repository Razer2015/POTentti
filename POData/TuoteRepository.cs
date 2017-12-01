//using System;
//using System.Collections.Generic;
//using POLuokat;
//using System.Data;
//using System.Data.SqlClient;

//namespace POData
//{
//    public class TuoteRepository : DataAccess
//    {
//        public TuoteRepository(string conString) : base(conString) { }

//        /// <summary>
//        /// Parsii Tuote-olion IDataReaderistä
//        /// </summary>
//        /// <param name="reader"></param>
//        /// <returns></returns>
//        private Tuote TeeRivistaTuote(IDataReader reader) {
//            var paluu = new TuoteProxy(int.Parse(reader["ProductID"].ToString()), reader["ProductName"].ToString()) {
//                ToimittajaId = (!(reader["SupplierID"] is DBNull) ? int.Parse(reader["SupplierID"].ToString()) : (int?)null),
//                RyhmaId = (!(reader["CategoryID"] is DBNull) ? int.Parse(reader["CategoryID"].ToString()) : (int?)null),
//                YksikkoKuvaus = (!(reader["QuantityPerUnit"] is DBNull) ? reader["QuantityPerUnit"].ToString() : null),
//                YksikkoHinta = (!(reader["UnitPrice"] is DBNull) ? double.Parse(reader["UnitPrice"].ToString().Replace('.', ',')) : (double?)null),
//                VarastoSaldo = (!(reader["UnitsInStock"] is DBNull) ? int.Parse(reader["UnitsInStock"].ToString()) : (int?)null),
//                TilausSaldo = (!(reader["UnitsOnOrder"] is DBNull) ? int.Parse(reader["UnitsOnOrder"].ToString()) : (int?)null),
//                HalytysRaja = (!(reader["ReorderLevel"] is DBNull) ? int.Parse(reader["ReorderLevel"].ToString()) : (int?)null),

//                EiKaytossa = bool.Parse(reader["Discontinued"].ToString())
//            };

//            //Toimittaja ja TuoteRyhma‐olioiden myöhempää populointia varten
//            ((TuoteProxy)paluu).ToimittajaRepository = new ToimittajaRepository(ConnectionString);
//            ((TuoteProxy)paluu).TuoteRyhmaRepository = new TuoteRyhmaRepository(ConnectionString);
//            return (paluu);
//        }

//        /// <summary>
//        /// Tekee tuotelistan
//        /// </summary>
//        /// <param name="reader"></param>
//        /// <returns></returns>
//        private List<Tuote> TeeTuoteLista(IDataReader reader) {
//            var tuotteet = new List<Tuote>();
//            while (reader.Read()) {
//                tuotteet.Add(TeeRivistaTuote(reader));
//            }
//            return (tuotteet);
//        }
//    }
//}
