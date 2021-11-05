namespace Domain.Tests.OOP.ES.Customer
{
    using System;
    using System.Linq;
    using Domain.OOP.ES.Customer;
    using FluentAssertions;
    using Shared;
    using Shared.Command;
    using Shared.Event;
    using Xunit;

    public class Customer4Tests
    {
        private ID customerId;
        private readonly EmailAddress emailAddress;
        private readonly EmailAddress changedEmailAddress;
        private Hash confirmationHash;
        private readonly Hash wrongConfirmationHash;
        private readonly Hash changedConfirmationHash;
        private readonly PersonName name;
        private Customer4 registeredCustomer;

        public Customer4Tests()
        {
            customerId = ID.Generate();
            emailAddress = EmailAddress.Build("john@doe.com");
            changedEmailAddress = EmailAddress.Build("john+changed@doe.com");
            confirmationHash = Hash.Generate();
            wrongConfirmationHash = Hash.Generate();
            changedConfirmationHash = Hash.Generate();
            name = PersonName.Build("John", "Doe");
        }

        [Fact]
        void RegisterCustomer4()
        {
            WHEN_RegisterCustomer();
            THEN_CustomerRegistered();
        }

        [Fact]
        void ConfirmEmailAddress()
        {
            GIVEN(CustomerIsRegistered());
            WHEN_ConfirmEmailAddress_With(confirmationHash);
            THEN_EmailAddressConfirmed();
        }

        [Fact]
        void ConfirmEmailAddress_WithWrongConfirmationHash()
        {
            GIVEN(CustomerIsRegistered());
            WHEN_ConfirmEmailAddress_With(wrongConfirmationHash);
            THEN_EmailAddressConfirmationFailed();
        }

        [Fact]
        void ConfirmEmailAddress_WhenItWasAlreadyConfirmed()
        {
            GIVEN(CustomerIsRegistered(),
                __and_EmailAddressWasConfirmed());
            WHEN_ConfirmEmailAddress_With(confirmationHash);
            THEN_NothingShouldHappen();
        }

        [Fact]
        void ConfirmEmailAddress_WithWrongConfirmationHash_whenItWasAlreadyConfirmed()
        {
            GIVEN(CustomerIsRegistered(),
                __and_EmailAddressWasConfirmed());
            WHEN_ConfirmEmailAddress_With(wrongConfirmationHash);
            THEN_EmailAddressConfirmationFailed();
        }

        [Fact]
        void ChangeEmailAddress()
        {
            // Given
            GIVEN(CustomerIsRegistered());
            WHEN_ChangeEmailAddress_With(changedEmailAddress);
            THEN_EmailAddressChanged();
        }

        [Fact]
        void ChangeEmailAddress_WithUnchangedEmailAddress()
        {
            GIVEN(CustomerIsRegistered());
            WHEN_ChangeEmailAddress_With(emailAddress);
            THEN_NothingShouldHappen();
        }

        [Fact]
        void ChangeEmailAddress_WhenItWasAlreadyChanged()
        {
            GIVEN(CustomerIsRegistered(),
                __and_EmailAddressWasChanged());
            WHEN_ChangeEmailAddress_With(changedEmailAddress);
            THEN_NothingShouldHappen();
        }

        [Fact]
        void ConfirmEmailAddress_WhenItWasPreviouslyConfirmedAndThenChanged()
        {
            GIVEN(CustomerIsRegistered(),
                __and_EmailAddressWasConfirmed(),
                __and_EmailAddressWasChanged());
            WHEN_ConfirmEmailAddress_With(changedConfirmationHash);
            THEN_EmailAddressConfirmed();
        }

        /*
         * Methods for GIVEN
         */
        private void GIVEN(params Event[] events)
        {
            registeredCustomer = Customer4.Reconstitute(events.ToList());
        }

        private CustomerRegistered CustomerIsRegistered()
        {
            return CustomerRegistered.Build(customerId, emailAddress, confirmationHash, name);
        }

        private CustomerEmailAddressConfirmed __and_EmailAddressWasConfirmed()
        {
            return CustomerEmailAddressConfirmed.Build(customerId);
        }

        private CustomerEmailAddressChanged __and_EmailAddressWasChanged()
        {
            return CustomerEmailAddressChanged.Build(customerId, changedEmailAddress, changedConfirmationHash);
        }

        /*
         * Methods for WHEN
         */
        private void WHEN_RegisterCustomer()
        {
            var registerCustomer = RegisterCustomer.Build(emailAddress.Value, name.GivenName, name.FamilyName);
            registeredCustomer = Customer4.Register(registerCustomer);
            customerId = registerCustomer.CustomerId;
            confirmationHash = registerCustomer.ConfirmationHash;
        }

        private void WHEN_ConfirmEmailAddress_With(Hash confirmationHash)
        {
            var command = ConfirmCustomerEmailAddress.Build(customerId.Value, confirmationHash.Value);
            try
            {
                registeredCustomer.ConfirmEmailAddress(command);
            }
            catch
            {
                throw new Exception(THelper.PropertyIsNull("ConfirmationHash"));
            }
        }

        private void WHEN_ChangeEmailAddress_With(EmailAddress emailAddress)
        {
            var command = ChangeCustomerEmailAddress.Build(customerId.Value, emailAddress.Value);
            try
            {
                registeredCustomer.ChangeEmailAddress(command);
            }
            catch
            {
                throw new Exception(THelper.PropertyIsNull("EmailAddress"));
            }
        }

        /*
         * Methods for THEN
         */
        void THEN_CustomerRegistered()
        {
            var method = "Register";
            var eventName = "CustomerRegistered";
            var recordedEvents = registeredCustomer.RecordedEvents;
            recordedEvents.Should().HaveCount(1, THelper.NoEventWasRecorded(method, eventName));
            var evt = recordedEvents[0] as CustomerRegistered;
            evt.Should().NotBeNull(THelper.EventIsNull(method, eventName));
            evt.Should().BeOfType<CustomerRegistered>(THelper.EventOfWrongTypeWasRecorded(method));
            customerId.Should().Be(evt?.CustomerId, THelper.PropertyIsWrong(method, "CustomerId"));
            emailAddress.Should().Be(evt?.EmailAddress, THelper.PropertyIsWrong(method, "EmailAddress"));
            confirmationHash.Should().Be(evt?.ConfirmationHash, THelper.PropertyIsWrong(method, "ConfirmationHash"));
            name.Should().Be(evt?.Name, THelper.PropertyIsWrong(method, "Name"));
        }

        void THEN_EmailAddressConfirmed()
        {
            var method = "ConfirmEmailAddress";
            var eventName = "CustomerEmailAddressConfirmed";
            var recordedEvents = registeredCustomer.RecordedEvents;
            recordedEvents.Should().HaveCount(1, THelper.NoEventWasRecorded(method, eventName));
            var evt = recordedEvents[0] as CustomerEmailAddressConfirmed;
            evt.Should().NotBeNull(THelper.EventIsNull(method, eventName));
            evt.Should().BeOfType<CustomerEmailAddressConfirmed>(THelper.EventOfWrongTypeWasRecorded(method));
            customerId.Should().Be(evt?.CustomerId, THelper.PropertyIsWrong(method, "CustomerId"));
        }

        void THEN_EmailAddressConfirmationFailed()
        {
            var method = "ConfirmEmailAddress";
            var eventName = "CustomerEmailAddressConfirmationFailed";
            var recordedEvents = registeredCustomer.RecordedEvents;
            recordedEvents.Should().HaveCount(1, THelper.NoEventWasRecorded(method, eventName));
            var evt = recordedEvents[0] as CustomerEmailAddressConfirmationFailed;
            evt.Should().NotBeNull(THelper.EventIsNull(method, eventName));
            evt.Should().BeOfType<CustomerEmailAddressConfirmationFailed>(THelper.EventOfWrongTypeWasRecorded(method));
            customerId.Should().Be(evt?.CustomerId, THelper.PropertyIsWrong(method, "CustomerId"));
        }

        private void THEN_EmailAddressChanged()
        {
            var method = "ChangeEmailAddress";
            var eventName = "CustomerEmailAddressChanged";
            var recordedEvents = registeredCustomer.RecordedEvents;
            recordedEvents.Should().HaveCount(1, THelper.NoEventWasRecorded(method, eventName));
            var evt = recordedEvents[0] as CustomerEmailAddressChanged;
            evt.Should().NotBeNull(THelper.EventIsNull(method, eventName));
            evt.Should().BeOfType<CustomerEmailAddressChanged>(THelper.EventOfWrongTypeWasRecorded(method));
            customerId.Should().Be(evt?.CustomerId, THelper.PropertyIsWrong(method, "CustomerId"));
            changedEmailAddress.Should().Be(evt?.EmailAddress, THelper.PropertyIsWrong(method, "EmailAddress"));
        }

        void THEN_NothingShouldHappen()
        {
            var recordedEvents = registeredCustomer.RecordedEvents;
            recordedEvents.Should().HaveCount(0, THelper.NoEventShouldHaveBeenRecorded(THelper.TypeOfFirst(recordedEvents)));
        }
    }
}