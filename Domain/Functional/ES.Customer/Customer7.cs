namespace Domain.Functional.ES.Customer
{
    using System.Collections.Generic;
    using Shared.Command;
    using Shared.Event;

    public class Customer7
    {
        public static CustomerRegistered Register(RegisterCustomer command)
        {
            return null; // TODO
        }

        public static List<Event> ConfirmEmailAddress(CustomerState current, ConfirmCustomerEmailAddress command)
        {
            // TODO

            return new List<Event>(); // TODO
        }

        public static List<Event> ChangeEmailAddress(CustomerState current, ChangeCustomerEmailAddress command)
        {
            // TODO

            return new List<Event>(); // TODO
        }
    }
}