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
            return null; // TODO
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
                        // TODO
                        break;
                    case CustomerEmailAddressConfirmed e:
                        // TODO
                        break;
                    case CustomerEmailAddressChanged e:
                        // TODO
                        break;
                }
            }

            // TODO

            return new List<Event>(); // TODO
        }

        public static List<Event> ChangeEmailAddress(List<Event> eventStream, ChangeCustomerEmailAddress command)
        {
            EmailAddress emailAddress = default;
            foreach (var evt in eventStream)
            {
                switch (evt)
                {
                    case CustomerRegistered e:
                        // TODO
                        break;
                    case CustomerEmailAddressChanged e:
                        // TODO
                        break;
                }
            }

            // TODO

            return new List<Event>(); // TODO
        }
    }
}