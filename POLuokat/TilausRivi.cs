namespace POLuokat
{
    public class TilausRivi
    {
        public int Id { get; set; }
        public int? ProductID { get; set; }
        public double? UnitPrice { get; set; }
        public int? Quantity { get; set; }
        public float? Discount { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public TilausRivi() {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TilausRivi(int id) : this() {
            this.Id = id;
        }

        public override string ToString() {
            return ($"{Id}");
        }
    }
}
