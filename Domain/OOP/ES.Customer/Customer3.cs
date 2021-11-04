namespace Domain.OOP.ES.Customer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
            return null; // TODO
        }

        public static Customer3 Reconstitute(List<Event> events)
        {
            var customer = new Customer3();

            customer.Apply(events);

            return customer;
        }

        public List<Event> ConfirmEmailAddress(ConfirmCustomerEmailAddress command)
        {
            // TODO

            return new List<Event>(); // TODO
        }

        public List<Event> ChangeEmailAddress(ChangeCustomerEmailAddress command)
        {
            // TODO

            return new List<Event>(); // TODO
        }

        private void Apply(List<Event> events)
        {
            foreach (var evt in events)
            {
                Apply(evt);
            }
        }

        public void Apply(Event evt)
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
    }
}