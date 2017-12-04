using System;
using System.Collections.Generic;
using POLuokat;
using System.Data;
using System.Data.SqlClient;

namespace POData
{
    public class ProductRepository : DataAccess
    {
        public ProductRepository(string conString) : base(conString) { }

        /// <summary>
        /// Parses Product from IDataReader
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private Product CreateProduct(IDataReader reader) {
            var product = new ProductProxy(int.Parse(reader["ProductID"].ToString()), reader["ProductName"].ToString()) {
                SupplierID = (!(reader["SupplierID"] is DBNull) ? int.Parse(reader["SupplierID"].ToString()) : (int?)null),
                CategoryID = (!(reader["CategoryID"] is DBNull) ? int.Parse(reader["CategoryID"].ToString()) : (int?)null),
                QuantityPerUnit = (!(reader["QuantityPerUnit"] is DBNull) ? reader["QuantityPerUnit"].ToString() : null),
                UnitPrice = (!(reader["UnitPrice"] is DBNull) ? decimal.Parse(reader["UnitPrice"].ToString().Replace('.', ',')) : (decimal?)null),
                UnitsInStock = (!(reader["UnitsInStock"] is DBNull) ? int.Parse(reader["UnitsInStock"].ToString()) : (int?)null),
                UnitsOnOrder = (!(reader["UnitsOnOrder"] is DBNull) ? int.Parse(reader["UnitsOnOrder"].ToString()) : (int?)null),
                ReorderLevel = (!(reader["ReorderLevel"] is DBNull) ? int.Parse(reader["ReorderLevel"].ToString()) : (int?)null),

                Discontinued = bool.Parse(reader["Discontinued"].ToString())
            };

            product.OrderDetailRepository = new OrderDetailRepository(ConnectionString);

            return (product);
        }

        /// <summary>
        /// Tekee tuotelistan
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private List<Product> CreateProducts(IDataReader reader) {
            var products = new List<Product>();
            while (reader.Read()) {
                products.Add(CreateProduct(reader));
            }
            return (products);
        }

        /// <summary>
        /// Searches for an product by productID
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public Product Search(int productID) {
            string sql = "SELECT * FROM dbo.Products WHERE ProductID = @ProductID";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@ProductID", productID));
                        var reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                        reader.Read();
                        return (CreateProduct(reader));
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"DatabaseError: {e.Message}");
            }
        }
    }
}
