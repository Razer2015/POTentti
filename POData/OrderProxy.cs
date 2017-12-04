using POLuokat;
using System.Collections.Generic;

namespace POData
{
    public class OrderProxy : Order
    {
        Customer _customer;
        List<OrderDetail> _orderDetails;
        bool CustomerRetrieved = false;
        bool OrderDetailsRetrieved = false;

        public CustomerRepository CustomerRepository { get; set; }
        public OrderDetailRepository OrderDetailRepository { get; set; }

        public override Customer Customer
        {
            get
            {
                if (!CustomerRetrieved) {
                    _customer = CustomerRepository.Search(CustomerID);
                    CustomerRetrieved = true;
                }
                return (_customer);
            }
            set => base.Customer = value;
        }

        public override List<OrderDetail> OrderDetails
        {
            get
            {
                if (!OrderDetailsRetrieved) {
                    _orderDetails = OrderDetailRepository.SearchAllByOrderID(OrderID);
                    OrderDetailsRetrieved = true;
                }
                return (_orderDetails);
            }
            set => base.OrderDetails = value;
        }

        public OrderProxy(int orderID) 
            : base(orderID) {
        }
    }
}
