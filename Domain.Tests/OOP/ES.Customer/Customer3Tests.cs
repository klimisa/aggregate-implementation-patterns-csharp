namespace Domain.Tests.OOP.ES.Customer
{
    using System;
    using System.Collections.Generic;
    using Domain.OOP.ES.Customer;
    using FluentAssertions;
    using Shared;
    using Shared.Command;
    using Shared.Event;
    using Xunit;

    public class Customer3Tests
    {
        private ID customerId;
        private readonly EmailAddress emailAddress;
        private readonly EmailAddress changedEmailAddress;
        private Hash confirmationHash;
        private readonly Hash wrongConfirmationHash;
        private readonly Hash changedConfirmationHash;
        private readonly PersonName name;
        private CustomerRegistered customerRegistered;
        private List<Event> recordedEvents;
        private Customer3 registeredCustomer;

        public Customer3Tests()
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
        void RegisterCustomer3()
        {
            WHEN_RegisterCustomer();
            THEN_CustomerRegistered();
        }

        [Fact]
        void ConfirmEmailAddress()
        {
            GIVEN_CustomerRegistered();
            WHEN_ConfirmEmailAddress_With(confirmationHash);
            THEN_EmailAddressConfirmed();
        }

        [Fact]
        void ConfirmEmailAddress_WithWrongConfirmationHash()
        {
            GIVEN_CustomerRegistered();
            WHEN_ConfirmEmailAddress_With(wrongConfirmationHash);
            THEN_EmailAddressConfirmationFailed();
        }
        
        [Fact]
        void ConfirmEmailAddress_WhenItWasAlreadyConfirmed() {
            GIVEN_CustomerRegistered();
            __and_EmailAddressWasConfirmed();
            WHEN_ConfirmEmailAddress_With(confirmationHash);
            THEN_NothingShouldHappen();
        }
        
        [Fact]
        void ChangeEmailAddress() {
            GIVEN_CustomerRegistered();
            WHEN_ChangeEmailAddress_With(changedEmailAddress);
            THEN_EmailAddressChanged();
        }
        
        [Fact]
        void ChangeEmailAddress_WithUnchangedEmailAddress() {
            GIVEN_CustomerRegistered();
            WHEN_ChangeEmailAddress_With(emailAddress);
            THEN_NothingShouldHappen();
        }
        
        [Fact]
        void ChangeEmailAddress_WhenItWasAlreadyChanged() {
            GIVEN_CustomerRegistered();
            __and_EmailAddressWasChanged();
            WHEN_ChangeEmailAddress_With(changedEmailAddress);
            THEN_NothingShouldHappen();
        }
        
        [Fact]
        void ConfirmEmailAddress_WhenItWasPreviouslyConfirmedAndThenChanged() {
            GIVEN_CustomerRegistered();
            __and_EmailAddressWasConfirmed();
            __and_EmailAddressWasChanged();
            WHEN_ConfirmEmailAddress_With(changedConfirmationHash);
            THEN_EmailAddressConfirmed();
        }
        
        /*
         * Methods for GIVEN
         */

        private void GIVEN_CustomerRegistered()
        {
            registeredCustomer = Customer3.Reconstitute(
                new List<Event>
                {
                    CustomerRegistered.Build(customerId, emailAddress, confirmationHash, name)
                }
            );
        }
        
        private void __and_EmailAddressWasConfirmed() {
            // TODO: Discuss it, cause Apply() it shouldn't be public
            registeredCustomer.Apply(
                CustomerEmailAddressConfirmed.Build(customerId)
            );
        }
        
        private void __and_EmailAddressWasChanged() {
            registeredCustomer.Apply(
                CustomerEmailAddressChanged.Build(customerId, changedEmailAddress, changedConfirmationHash)
            );
        }

        /*
         * Methods for WHEN
         */

        private void WHEN_RegisterCustomer()
        {
            var registerCustomer = RegisterCustomer.Build(emailAddress.Value, name.GivenName, name.FamilyName);
            customerRegistered = Customer3.Register(registerCustomer);
            customerId = registerCustomer.CustomerId;
            confirmationHash = registerCustomer.ConfirmationHash;
        }

        private void WHEN_ConfirmEmailAddress_With(Hash confirmationHash)
        {
            var command = ConfirmCustomerEmailAddress.Build(customerId.Value, confirmationHash.Value);
            try
            {
                recordedEvents = registeredCustomer.ConfirmEmailAddress(command);
            }
            catch
            {
                throw new Exception(THelper.PropertyIsNull("ConfirmationHash"));
            }
        }

        private void WHEN_ChangeEmailAddress_With(EmailAddress emailAddress) {
            var command = ChangeCustomerEmailAddress.Build(customerId.Value, emailAddress.Value);
            try 
            {
                recordedEvents = registeredCustomer.ChangeEmailAddress(command);
            } 
            catch 
            {
                throw new Exception(THelper.PropertyIsNull("EmailAddress"));
            }
        }
        
        /*
         * Methods for THEN
         */
        private void THEN_CustomerRegistered()
        {
            var method = "Register";
            var eventName = "CustomerRegistered";
            customerRegistered.Should().NotBeNull(THelper.EventIsNull("Register", eventName));
            customerId.Should().Be(customerRegistered.CustomerId, THelper.PropertyIsWrong(method, "CustomerID"));
            emailAddress.Should().Be(customerRegistered.EmailAddress, THelper.PropertyIsWrong(method, "EmailAddress"));
            confirmationHash.Should().Be(customerRegistered.ConfirmationHash, THelper.PropertyIsWrong(method, "ConfirmationHash"));
            name.Should().Be(customerRegistered.Name, THelper.PropertyIsWrong(method, "Name"));
        }

        private void THEN_EmailAddressConfirmed()
        {
            var method = "ConfirmEmailAddress";
            var eventName = "CustomerEmailAddressConfirmed";
            recordedEvents.Should().HaveCount(1, THelper.NoEventWasRecorded(method, eventName));
            var evt = recordedEvents[0] as CustomerEmailAddressConfirmed;
            evt.Should().NotBeNull(THelper.EventIsNull(method, eventName));
            evt.Should().BeOfType<CustomerEmailAddressConfirmed>(THelper.EventOfWrongTypeWasRecorded(method));
            customerId.Should().Be(evt?.CustomerId, THelper.PropertyIsWrong(method, "CustomerId"));
        }

        private void THEN_EmailAddressConfirmationFailed() {
            var method = "ConfirmEmailAddress";
            var eventName = "CustomerEmailAddressConfirmationFailed";
            recordedEvents.Should().HaveCount(1, THelper.NoEventWasRecorded(method, eventName));
            var evt = recordedEvents[0] as CustomerEmailAddressConfirmationFailed;
            evt.Should().NotBeNull(THelper.EventIsNull(method, eventName));
            evt.Should().BeOfType<CustomerEmailAddressConfirmationFailed>(THelper.EventOfWrongTypeWasRecorded(method));
            customerId.Should().Be(evt?.CustomerId, THelper.PropertyIsWrong(method, "CustomerId"));
        }
        
        private void THEN_EmailAddressChanged() {
            var method = "ChangeEmailAddress";
            var eventName = "CustomerEmailAddressChanged";
            recordedEvents.Should().HaveCount(1, THelper.NoEventWasRecorded(method, eventName));
            var evt = recordedEvents[0] as CustomerEmailAddressChanged;
            evt.Should().NotBeNull(THelper.EventIsNull(method, eventName));
            evt.Should().BeOfType<CustomerEmailAddressChanged>(THelper.EventOfWrongTypeWasRecorded(method));
            customerId.Should().Be(evt?.CustomerId, THelper.PropertyIsWrong(method, "CustomerId"));
            changedEmailAddress.Should().Be(evt?.EmailAddress, THelper.PropertyIsWrong(method, "EmailAddress"));
        }
        
        private void THEN_NothingShouldHappen() {
            recordedEvents.Should().HaveCount(0, THelper.NoEventShouldHaveBeenRecorded(THelper.TypeOfFirst(recordedEvents)));
        }
    }
}