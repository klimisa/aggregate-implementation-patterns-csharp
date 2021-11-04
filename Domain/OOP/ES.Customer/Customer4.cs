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

            customer.RecordThat(
                CustomerRegistered.Build(
                    command.CustomerId,
                    command.EmailAddress,
                    command.ConfirmationHash,
                    command.Name));

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
            if (command.ConfirmationHash != confirmationHash)
            {
                RecordThat(CustomerEmailAddressConfirmationFailed.Build(command.CustomerId));
                return;
            }

            if (!isEmailAddressConfirmed)
                RecordThat(CustomerEmailAddressConfirmed.Build(command.CustomerId));
        }

        public void ChangeEmailAddress(ChangeCustomerEmailAddress command)
        {
            if (command.EmailAddress != emailAddress)
                RecordThat(
                    CustomerEmailAddressChanged.Build(
                        command.CustomerId,
                        command.EmailAddress,
                        command.ConfirmationHash));
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

        // TODO: Discuss it, cause Apply() it shouldn't be public
        public void Apply(Event evt)
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