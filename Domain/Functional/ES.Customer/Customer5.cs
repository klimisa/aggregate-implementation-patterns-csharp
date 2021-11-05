namespace Domain.Functional.ES.Customer
{
    using System.Collections.Generic;
    using Shared;
    using Shared.Command;
    using Shared.Event;

    public class Customer5
    {
        public static CustomerRegistered Register(RegisterCustomer command)
        {
            return CustomerRegistered.Build(
                command.CustomerId,
                command.EmailAddress,
                command.ConfirmationHash,
                command.Name
            );
        }

        public static List<Event> ConfirmEmailAddress(List<Event> eventStream, ConfirmCustomerEmailAddress command)
        {
            bool isEmailAddressConfirmed = false;
            Hash confirmationHash = default;
            foreach (var evt in eventStream)
            {
                switch (evt)
                {
                    case CustomerRegistered e:
                        confirmationHash = e.ConfirmationHash;
                        break;
                    case CustomerEmailAddressConfirmed e:
                        isEmailAddressConfirmed = true;
                        break;
                    case CustomerEmailAddressChanged e:
                        confirmationHash = e.ConfirmationHash;
                        isEmailAddressConfirmed = false;
                        break;
                }
            }

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

        public static List<Event> ChangeEmailAddress(List<Event> eventStream, ChangeCustomerEmailAddress command)
        {
            EmailAddress emailAddress = default;
            foreach (var evt in eventStream)
            {
                switch (evt)
                {
                    case CustomerRegistered e:
                        emailAddress = e.EmailAddress;
                        break;
                    case CustomerEmailAddressChanged e:
                        emailAddress = e.EmailAddress;
                        break;
                }
            }

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
    }
}