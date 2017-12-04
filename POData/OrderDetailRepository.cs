using POLuokat;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace POData
{
    public class OrderDetailRepository : DataAccess
    {
        public OrderDetailRepository(string conString) : base(conString) { }

        /// <summary>
        /// Parses OrderDetail from IDataReader
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private OrderDetail CreateOrderDetail(IDataReader reader) {
            var orderDetail = new OrderDetailProxy(int.Parse(reader["OrderID"].ToString()), int.Parse(reader["ProductID"].ToString())) {
                UnitPrice = decimal.Parse(reader["UnitPrice"].ToString().Replace('.', ',')),
                Quantity = int.Parse(reader["Quantity"].ToString()),
                Discount = float.Parse(reader["Discount"].ToString())
            };

            orderDetail.OrderRepository = new OrderRepository(ConnectionString);
            orderDetail.ProductRepository = new ProductRepository(ConnectionString);

            return (orderDetail);
        }

        /// <summary>
        /// Parses OrderDetails from IDataReader
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private List<OrderDetail> CreateOrderDetails(IDataReader reader) {
            var orderDetails = new List<OrderDetail>();
            while (reader.Read()) {
                orderDetails.Add(CreateOrderDetail(reader));
            }
            return (orderDetails);
        }

        /// <summary>
        /// Searches for all orderDetails by orderID
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public List<OrderDetail> SearchAllByOrderID(int orderID) {
            string sql = "SELECT * FROM dbo.[Order Details] WHERE OrderID = @OrderID";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@OrderID", orderID));
                        return (CreateOrderDetails(cmd.ExecuteReader()));
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"DatabaseError: {e.Message}");
            }
        }

        /// <summary>
        /// Searches for all orderDetails by productID
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public List<OrderDetail> SearchAllByProductID(int productID) {
            string sql = "SELECT * FROM dbo.[Order Details] WHERE ProductID = @ProductID";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@ProductID", productID));
                        return (CreateOrderDetails(cmd.ExecuteReader()));
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"DatabaseError: {e.Message}");
            }
        }
    }
}
