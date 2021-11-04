namespace Domain.Shared.Command
{
    public class ChangeCustomerEmailAddress
    {
        public ID CustomerId { get; }
        public EmailAddress EmailAddress { get; }
        public Hash ConfirmationHash { get; }

        private ChangeCustomerEmailAddress(string customerId, string emailAddress)
        {
            CustomerId = ID.Build(customerId);
            EmailAddress = EmailAddress.Build(emailAddress);
            ConfirmationHash = Hash.Generate();
        }

        public static ChangeCustomerEmailAddress Build(string customerId, string emailAddress)
        {
            return new ChangeCustomerEmailAddress(customerId, emailAddress);
        }
    }
}