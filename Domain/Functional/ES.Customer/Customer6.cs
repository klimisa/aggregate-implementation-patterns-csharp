namespace Domain.Functional.ES.Customer
{
    using System.Collections.Generic;
    using Shared.Command;
    using Shared.Event;

    public class Customer6
    {
        public static CustomerRegistered Register(RegisterCustomer command)
        {
            return null; // TODO
        }

        public static List<Event> ConfirmEmailAddress(List<Event> eventStream, ConfirmCustomerEmailAddress command)
        {
            var current = CustomerState.Reconstitute(eventStream);

            // TODO

            return new List<Event>(); // TODO
        }

        public static List<Event> ChangeEmailAddress(List<Event> eventStream, ChangeCustomerEmailAddress command)
        {
            var current = CustomerState.Reconstitute(eventStream);

            // TODO

            return new List<Event>(); // TODO
        }
    }
}