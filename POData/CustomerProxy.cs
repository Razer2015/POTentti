using POLuokat;
using System.Collections.Generic;

namespace POData
{
    public class CustomerProxy : Customer
    {
        List<Order> _orders;
        bool OrdersRetrieved = false;

        public OrderRepository OrderRepository { get; set; }

        public override List<Order> Orders
        {
            get
            {
                if (!OrdersRetrieved) {
                    _orders = OrderRepository.SearchAllByCustomerID(CustomerID);
                    OrdersRetrieved = true;
                }
                return (_orders);
            }
            set => base.Orders = value;
        }

        public CustomerProxy(string id, string nimi) 
            : base(id, nimi) {
        }
    }
}
