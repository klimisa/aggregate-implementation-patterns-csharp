namespace Domain.Shared.Event
{
    public class CustomerEmailAddressConfirmationFailed : Event
    {
        public ID CustomerId { get; }

        private CustomerEmailAddressConfirmationFailed(ID customerId)
        {
            CustomerId = customerId;
        }

        public static CustomerEmailAddressConfirmationFailed Build(ID customerId)
        {
            return new CustomerEmailAddressConfirmationFailed(customerId);
        }
    }
}