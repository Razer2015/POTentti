using System.Collections.Generic;

namespace POLuokat
{
    public class Product
    {
        public int ProductID { get; private set; }
        public string ProductName { get; set; }

        public int? SupplierID { get; set; }
        public int? CategoryID { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public int? UnitsInStock { get; set; }
        public int? UnitsOnOrder { get; set; }
        public int? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }

        // Navigation properties
        public virtual List<OrderDetail> OrderDetails { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Product() {
            OrderDetails = new List<OrderDetail>();
        }

        /// <summary>
        /// Additional Constructor
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="productName"></param>
        public Product(int productID, string productName) : this() {
            this.ProductID = productID;
            this.ProductName = productName;
        }

        public override string ToString() {
            return ($"{ProductID} {ProductName} ({OrderDetails.Count})");
        }
    }
}
