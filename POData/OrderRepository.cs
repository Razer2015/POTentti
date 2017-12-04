using POLuokat;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace POData
{
    public class OrderRepository : DataAccess
    {
        public OrderRepository(string conString) : base(conString) { }

        /// <summary>
        /// Parses Order from IDataReader
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private Order CreateOrder(IDataReader reader) {
            var order = new OrderProxy(int.Parse(reader["OrderID"].ToString())) {
                CustomerID = (!(reader["CustomerID"] is DBNull) ? reader["CustomerID"].ToString() : null),
                EmployeeID = (!(reader["EmployeeID"] is DBNull) ? int.Parse(reader["EmployeeID"].ToString()) : (int?)null),
                OrderDate = (!(reader["OrderDate"] is DBNull) ? DateTime.Parse(reader["OrderDate"].ToString()) : (DateTime?)null),
                RequiredDate = (!(reader["RequiredDate"] is DBNull) ? DateTime.Parse(reader["RequiredDate"].ToString()) : (DateTime?)null),
                ShippedDate = (!(reader["ShippedDate"] is DBNull) ? DateTime.Parse(reader["ShippedDate"].ToString()) : (DateTime?)null),
                ShipVia = (!(reader["ShipVia"] is DBNull) ? int.Parse(reader["ShipVia"].ToString()) : (int?)null),
                Freight = (!(reader["Freight"] is DBNull) ? decimal.Parse(reader["Freight"].ToString().Replace('.', ',')) : (decimal?)null),
                ShipName = (!(reader["ShipName"] is DBNull) ? reader["ShipName"].ToString() : null),
                ShipAddress = (!(reader["ShipAddress"] is DBNull) ? reader["ShipAddress"].ToString() : null),
                ShipCity = (!(reader["ShipCity"] is DBNull) ? reader["ShipCity"].ToString() : null),
                ShipRegion = (!(reader["ShipRegion"] is DBNull) ? reader["ShipRegion"].ToString() : null),
                ShipPostalCode = (!(reader["ShipPostalCode"] is DBNull) ? reader["ShipPostalCode"].ToString() : null),
                ShipCountry = (!(reader["ShipCountry"] is DBNull) ? reader["ShipCountry"].ToString() : null)
            };

            order.CustomerRepository = new CustomerRepository(ConnectionString);
            order.OrderDetailRepository = new OrderDetailRepository(ConnectionString);

            return (order);
        }

        /// <summary>
        /// Parses Orders from IDataReader
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private List<Order> CreateOrders(IDataReader reader) {
            var orders = new List<Order>();
            while (reader.Read()) {
                orders.Add(CreateOrder(reader));
            }
            return (orders);
        }

        /// <summary>
        /// Searches for an order based on orderID
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public Order Search(int orderID) {
            string sql = "SELECT * FROM dbo.Orders WHERE OrderID = @OrderID";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@OrderID", orderID));
                        var reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                        reader.Read();
                        return (CreateOrder(reader));
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"DatabaseError: {e.Message}");
            }
        }

        /// <summary>
        /// Searches for orders based on customerID
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public List<Order> SearchAllByCustomerID(string customerID) {
            string sql = "SELECT * FROM dbo.Orders WHERE CustomerID = @CustomerID";

            try {
                // Using block kutsuu Dispose metodia, joka puolestaan kutsuu myös Close metodia (Myös virheen sattuessa)
                using (var sqlCon = new SqlConnection(ConnectionString)) {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand(sql, sqlCon)) {
                        cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));
                        return (CreateOrders(cmd.ExecuteReader()));
                    }
                }
            }
            catch (Exception e) {
                throw new ApplicationException($"DatabaseError: {e.Message}");
            }
        }
    }
}
