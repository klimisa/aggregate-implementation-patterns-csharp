namespace Domain.OOP.ES.Customer
{
    using System.Collections.Generic;
    using Shared;
    using Shared.Command;
    using Shared.Event;

    public class Customer4
    {
        private EmailAddress emailAddress;
        private Hash confirmationHash;
        private bool isEmailAddressConfirmed;
        private PersonName name;

        public List<Event> RecordedEvents { get; }

        private Customer4()
        {
            RecordedEvents = new List<Event>();
        }

        public static Customer4 Register(RegisterCustomer command)
        {
            Customer4 customer = new Customer4();

            // TODO

            return customer;
        }

        public static Customer4 Reconstitute(List<Event> events)
        {
            var customer = new Customer4();

            customer.Apply(events);

            return customer;
        }

        public void ConfirmEmailAddress(ConfirmCustomerEmailAddress command)
        {
            // TODO
        }

        public void ChangeEmailAddress(ChangeCustomerEmailAddress command)
        {
            // TODO
        }

        private void RecordThat(Event evt)
        {
            RecordedEvents.Add(evt);
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