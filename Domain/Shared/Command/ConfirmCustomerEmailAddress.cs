namespace Domain.Shared.Command
{
    public class ConfirmCustomerEmailAddress
    {
        public ID CustomerId { get; }
        public Hash ConfirmationHash { get; }

        private ConfirmCustomerEmailAddress(string customerId, string confirmationHash)
        {
            CustomerId = ID.Build(customerId);
            ConfirmationHash = Hash.Build(confirmationHash);
        }

        public static ConfirmCustomerEmailAddress Build(string customerID, string confirmationHash)
        {
            return new ConfirmCustomerEmailAddress(customerID, confirmationHash);
        }
    }
}