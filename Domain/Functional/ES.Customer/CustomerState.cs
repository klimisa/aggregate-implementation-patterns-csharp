namespace Domain.Functional.ES.Customer
{
    using System.Collections.Generic;
    using Shared;
    using Shared.Event;

    public class CustomerState
    {
        public EmailAddress EmailAddress { get; private set; }
        public Hash ConfirmationHash { get; private set; }
        public PersonName Name { get; private set; }

        public bool IsEmailAddressConfirmed { get; private set; }

        private CustomerState()
        {
        }

        public static CustomerState Reconstitute(List<Event> events)
        {
            var customer = new CustomerState();

            customer.Apply(events);

            return customer;
        }

        private void Apply(List<Event> events)
        {
            foreach (var evt in events)
            {
                switch (evt)
                {
                    case CustomerRegistered e:
                        EmailAddress = e.EmailAddress;
                        ConfirmationHash = e.ConfirmationHash;
                        Name = e.Name;
                        break;
                    case CustomerEmailAddressConfirmed e:
                        IsEmailAddressConfirmed = true;
                        break;
                    case CustomerEmailAddressChanged e:
                        EmailAddress = e.EmailAddress;
                        ConfirmationHash = e.ConfirmationHash;
                        IsEmailAddressConfirmed = false;
                        break;
                }
            }
        }
    }
}