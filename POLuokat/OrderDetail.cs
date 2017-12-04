namespace POLuokat
{
    public class OrderDetail
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public float Discount { get; set; }

        // Navigation properties
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderDetail() {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderDetail(int orderID, int productID) : this() {
            this.OrderID = orderID;
            this.ProductID = productID;
        }

        public override string ToString() {
            return ($"{OrderID}");
        }
    }
}
