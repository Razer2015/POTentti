using POLuokat;
using System.Collections.Generic;

namespace POData
{
    public class ProductProxy : Product
    {
        List<OrderDetail> _orderDetails;
        bool OrderDetailsRetrieved = false;

        public OrderDetailRepository OrderDetailRepository { get; set; }

        public override List<OrderDetail> OrderDetails
        {
            get
            {
                if (!OrderDetailsRetrieved) {
                    _orderDetails = OrderDetailRepository.SearchAllByProductID(ProductID);
                    OrderDetailsRetrieved = true;
                }
                return (_orderDetails);
            }
            set => base.OrderDetails = value;
        }

        public ProductProxy(int productID, string productName) 
            : base(productID, productName) {
        }
    }
}
