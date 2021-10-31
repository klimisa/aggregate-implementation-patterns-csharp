namespace Domain.Shared.Event
{
    public class CustomerEmailAddressChanged : Event
    {
        public ID CustomerId { get; }
        public EmailAddress EmailAddress { get; }
        public Hash ConfirmationHash { get; }

        private CustomerEmailAddressChanged(ID customerId, EmailAddress emailAddress, Hash confirmationHash)
        {
            CustomerId = customerId;
            EmailAddress = emailAddress;
            ConfirmationHash = confirmationHash;
        }

        public static CustomerEmailAddressChanged Build(ID customerId, EmailAddress emailAddress, Hash confirmationHash)
        {
            return new CustomerEmailAddressChanged(customerId, emailAddress, confirmationHash);
        }
    }
}