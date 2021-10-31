namespace Domain.Shared.Event
{
    public class CustomerEmailAddressConfirmed : Event
    {
        public ID CustomerId { get; }

        private CustomerEmailAddressConfirmed(ID customerId)
        {
            CustomerId = customerId;
        }

        public static CustomerEmailAddressConfirmed Build(ID customerId)
        {
            return new CustomerEmailAddressConfirmed(customerId);
        }
    }
}