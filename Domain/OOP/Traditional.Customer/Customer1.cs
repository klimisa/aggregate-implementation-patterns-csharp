namespace Domain.OOP.Traditional.Customer
{
    using Shared;
    using Shared.Command;
    using Shared.Exception;

    public class Customer1
    {
        public ID Id { get; }
        public EmailAddress EmailAddress { get; private set; }
        public Hash ConfirmationHash { get; private set; }
        public PersonName Name { get; }
        public bool IsEmailAddressConfirmed { get; private set; }

        private Customer1(ID id, EmailAddress emailAddress, Hash confirmationHash, PersonName name)
        {
            Id = id;
            EmailAddress = emailAddress;
            ConfirmationHash = confirmationHash;
            Name = name;
        }

        public static Customer1 Register(RegisterCustomer command)
        {
            return new Customer1(
                command.CustomerId,
                command.EmailAddress,
                command.ConfirmationHash,
                command.Name
            );
        }

        public void ConfirmEmailAddress(ConfirmCustomerEmailAddress command)
        {
            if (command.ConfirmationHash != ConfirmationHash)
                throw new WrongConfirmationHashException();

            IsEmailAddressConfirmed = true;
        }

        public void ChangeEmailAddress(ChangeCustomerEmailAddress command)
        {
            EmailAddress = command.EmailAddress;
            ConfirmationHash = command.ConfirmationHash;
            IsEmailAddressConfirmed = false;
        }
    }
}