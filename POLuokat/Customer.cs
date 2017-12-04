using System.Collections.Generic;

namespace POLuokat
{
    public class Customer
    {
        public string CustomerID { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }

        // Navigation properties
        public virtual List<Order> Orders { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Customer() {
            Orders = new List<Order>();
        }

        /// <summary>
        /// Additional Constructor
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="companyName"></param>
        public Customer(string customerID, string companyName) : this() {
            this.CustomerID = customerID;
            this.CompanyName = companyName;
        }

        public override string ToString() {
            return ($"{CustomerID} {CompanyName} ({Orders.Count})");
        }
    }
}
