using System;
using System.Collections.Generic;

namespace POLuokat
{
    public class TilausOtsikko
    {
        public int Id { get; set; }
        public string CustomerID { get; set; }
        public int? EmployeeID { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public int? ShipVia { get; set; }
        public double? Freight { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }

        public virtual List<TilausRivi> TilausRivit { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public TilausOtsikko() {
            TilausRivit = new List<TilausRivi>();
        }

        /// <summary>
        /// Additional Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyName"></param>
        public TilausOtsikko(int id, string customerID) : this() {
            this.Id = id;
            this.CustomerID = customerID;
        }

        public override string ToString() {
            return ($"{Id} {CustomerID}");
        }
    }
}
