using System;
using System.Collections.Generic;

namespace POLuokat
{
    public class Order
    {
        public int OrderID { get; set; }
        public string CustomerID { get; set; }
        public int? EmployeeID { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public int? ShipVia { get; set; }
        public decimal? Freight { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }

        // Navigation properties
        public virtual Customer Customer { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Order() {
            OrderDetails = new List<OrderDetail>();
        }

        /// <summary>
        /// Additional Constructor
        /// </summary>
        /// <param name="orderID"></param>
        public Order(int orderID) : this() {
            this.OrderID = orderID;
        }

        public override string ToString() {
            return ($"{OrderID} {CustomerID}");
        }
    }
}
