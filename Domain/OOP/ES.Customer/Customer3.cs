namespace Domain.OOP.ES.Customer
{
    using System.Collections.Generic;
    using Shared;
    using Shared.Command;
    using Shared.Event;

    public class Customer3
    {
        private EmailAddress emailAddress;
        private Hash confirmationHash;
        private bool isEmailAddressConfirmed;
        private PersonName name;

        private Customer3()
        {
        }

        public static CustomerRegistered Register(RegisterCustomer command)
        {
            return CustomerRegistered.Build(
                command.CustomerId,
                command.EmailAddress,
                command.ConfirmationHash,
                command.Name
            );
        }

        public static Customer3 Reconstitute(List<Event> events)
        {
            var customer = new Customer3();

            customer.Apply(events);

            return customer;
        }

        public List<Event> ConfirmEmailAddress(ConfirmCustomerEmailAddress command)
        {
            if (command.ConfirmationHash != confirmationHash)
            {
                return new List<Event>()
                {
                    CustomerEmailAddressConfirmationFailed.Build(command.CustomerId)
                };
            }

            if (isEmailAddressConfirmed)
                return new List<Event>();

            return new List<Event>
            {
                CustomerEmailAddressConfirmed.Build(command.CustomerId)
            };
        }

        public List<Event> ChangeEmailAddress(ChangeCustomerEmailAddress command)
        {
            if (command.EmailAddress == emailAddress)
                return new List<Event>();

            return new List<Event>()
            {
                CustomerEmailAddressChanged.Build(
                    command.CustomerId, 
                    command.EmailAddress, 
                    command.ConfirmationHash)
            };
        }

        private void Apply(List<Event> events)
        {
            foreach (var evt in events)
            {
                Apply(evt);
            }
        }

        private void Apply(Event evt)
        {
            switch (evt)
            {
                case CustomerRegistered e:
                    name = e.Name;
                    emailAddress = e.EmailAddress;
                    confirmationHash = e.ConfirmationHash;
                    break;
                case CustomerEmailAddressConfirmed e:
                    isEmailAddressConfirmed = true;
                    break;
                case CustomerEmailAddressChanged e:
                    emailAddress = e.EmailAddress;
                    confirmationHash = e.ConfirmationHash;
                    isEmailAddressConfirmed = false;
                    break;
            }
        }
    }
}