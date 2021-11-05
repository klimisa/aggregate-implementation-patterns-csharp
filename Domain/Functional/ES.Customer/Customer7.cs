namespace Domain.Functional.ES.Customer
{
    using System.Collections.Generic;
    using Shared.Command;
    using Shared.Event;

    public class Customer7
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

        public static List<Event> ConfirmEmailAddress(CustomerState current, ConfirmCustomerEmailAddress command)
        {
            if (command.ConfirmationHash != current.ConfirmationHash)
            {
                return new List<Event>()
                {
                    CustomerEmailAddressConfirmationFailed.Build(command.CustomerId)
                };
            }

            if (current.IsEmailAddressConfirmed)
                return new List<Event>();

            return new List<Event>
            {
                CustomerEmailAddressConfirmed.Build(command.CustomerId)
            };
        }

        public static List<Event> ChangeEmailAddress(CustomerState current, ChangeCustomerEmailAddress command)
        {
            if (command.EmailAddress == current.EmailAddress)
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