using POLuokat;

namespace POData
{
    public class OrderDetailProxy : OrderDetail
    {
        Order _order;
        Product _product;
        bool OrderRetrieved = false;
        bool ProductRetrieved = false;

        public OrderRepository OrderRepository { get; set; }
        public ProductRepository ProductRepository { get; set; }

        public override Order Order
        {
            get
            {
                if (!OrderRetrieved) {
                    _order = OrderRepository.Search(OrderID);
                    OrderRetrieved = true;
                }
                return (_order);
            }
            set => base.Order = value;
        }

        public override Product Product
        {
            get
            {
                if (!ProductRetrieved) {
                    _product = ProductRepository.Search((int)ProductID);
                    ProductRetrieved = true;
                }
                return (_product);
            }
            set => base.Product = value;
        }

        public OrderDetailProxy(int orderID, int productID) 
            : base(orderID, productID) {

        }
    }
}
