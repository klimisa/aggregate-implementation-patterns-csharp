namespace Domain.Functional.Traditional.Customer
{
    using Shared;

    public class CustomerState
    {
        public ID Id { get; }
        public EmailAddress EmailAddress { get; }
        public Hash ConfirmationHash { get; }
        public PersonName Name { get; }
        public bool IsEmailAddressConfirmed { get; }

        public CustomerState(ID id, EmailAddress emailAddress, Hash confirmationHash, PersonName name, bool isEmailAddressConfirmed = false)
        {
            Id = id;
            EmailAddress = emailAddress;
            ConfirmationHash = confirmationHash;
            Name = name;
            IsEmailAddressConfirmed = isEmailAddressConfirmed;
        }
    }
}