namespace Domain.Shared.Event
{
    public class CustomerRegistered : Event
    {
        public ID CustomerId { get; }
        public EmailAddress EmailAddress { get; }
        public Hash ConfirmationHash { get; }
        public PersonName Name { get; }

        private CustomerRegistered(ID customerID, EmailAddress emailAddress, Hash confirmationHash, PersonName name)
        {
            CustomerId = customerID;
            EmailAddress = emailAddress;
            ConfirmationHash = confirmationHash;
            Name = name;
        }

        public static CustomerRegistered Build(
            ID id,
            EmailAddress emailAddress,
            Hash confirmationHash,
            PersonName name
        )
        {
            return new CustomerRegistered(id, emailAddress, confirmationHash, name);
        }
    }
}