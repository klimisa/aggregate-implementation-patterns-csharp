namespace Domain.Tests.OOP.Traditional.Customer
{
    using System;
    using Domain.OOP.Traditional.Customer;
    using FluentAssertions;
    using Shared;
    using Shared.Command;
    using Shared.Exception;
    using Xunit;

    public class Customer1Tests
    {
        private ID customerId;
        private Hash confirmationHash;
        private readonly PersonName name;
        private readonly EmailAddress emailAddress;
        private readonly EmailAddress changedEmailAddress;
        private readonly Hash wrongConfirmationHash;
        private Hash changedConfirmationHash;
        private Customer1 registeredCustomer;

        public Customer1Tests()
        {
            emailAddress = EmailAddress.Build("john@doe.com");
            changedEmailAddress = EmailAddress.Build("john+changed@doe.com");
            wrongConfirmationHash = Hash.Generate();
            changedConfirmationHash = Hash.Generate();
            name = PersonName.Build("John", "Doe");
        }

        [Fact]
        void RegisterCustomer1()
        {
            // When
            var command = RegisterCustomer.Build(emailAddress.Value, name.GivenName, name.FamilyName);
            var customer = Customer1.Register(command);

            // Then it should succeed
            // and should have the expected state
            customer.Should().NotBeNull();
            customer.Id.Should().Be(command.CustomerId);
            customer.Name.Should().Be( command.Name);
            customer.EmailAddress.Should().Be( command.EmailAddress);
            customer.ConfirmationHash.Should().Be( command.ConfirmationHash);
            customer.IsEmailAddressConfirmed.Should().BeFalse();
        }

        [Fact]
        void Confirm_EmailAddress()
        {
            // Given
            GivenARegisteredCustomer();

            // When ConfirmCustomerEmailAddress
            // Then it should succeed
            var command = ConfirmCustomerEmailAddress.Build(customerId.Value, confirmationHash.Value);
            Action act = () => registeredCustomer.ConfirmEmailAddress(command);
            act.Should().NotThrow();

            // and the emailAddress should be confirmed
            Assert.True(registeredCustomer.IsEmailAddressConfirmed);
        }

        [Fact]
        void ConfirmEmailAddress_WithWrongConfirmationHash()
        {
            // Given
            GivenARegisteredCustomer();

            // When ConfirmCustomerEmailAddress
            // Then it should throw WrongConfirmationHashException
            var command = ConfirmCustomerEmailAddress.Build(customerId.Value, wrongConfirmationHash.Value);
            Action act = () => registeredCustomer.ConfirmEmailAddress(command);
            act.Should().Throw<WrongConfirmationHashException>();

            // and the emailAddress should not be confirmed
            registeredCustomer.IsEmailAddressConfirmed.Should().BeFalse();
        }

        [Fact]
        void ChangeEmailAddress()
        {
            // Given
            GivenARegisteredCustomer();

            // When changeCustomerEmailAddress
            var command = ChangeCustomerEmailAddress.Build(customerId.Value, changedEmailAddress.Value);
            registeredCustomer.ChangeEmailAddress(command);

            // Then the emailAddress and confirmationHash should be changed and the emailAddress should be unconfirmed
            registeredCustomer.EmailAddress.Should().Be( command.EmailAddress);
            registeredCustomer.ConfirmationHash.Should().Be( command.ConfirmationHash);
            registeredCustomer.IsEmailAddressConfirmed.Should().BeFalse();
        }

        [Fact]
        void ConfirmEmailAddress_WhenItWasPreviouslyConfirmedAndThenChanged()
        {
            // Given
            GivenARegisteredCustomer();
            GivenEmailAddressWasConfirmed();
            GivenEmailAddressWasChanged();

            // When confirmCustomerEmailAddress
            // Then it should succeed
            var command = ConfirmCustomerEmailAddress.Build(customerId.Value, changedConfirmationHash.Value);
            Action act = () => registeredCustomer.ConfirmEmailAddress(command);
            act.Should().NotThrow();

            // and the emailAddress should be confirmed
            registeredCustomer.IsEmailAddressConfirmed.Should().BeTrue();
        }

        /*
         * Helper methods to set up the Given state
         */
        private void GivenARegisteredCustomer()
        {
            var register = RegisterCustomer.Build(emailAddress.Value, name.GivenName, name.FamilyName);
            customerId = register.CustomerId;
            confirmationHash = register.ConfirmationHash;
            registeredCustomer = Customer1.Register(register);
        }

        private void GivenEmailAddressWasConfirmed()
        {
            var command = ConfirmCustomerEmailAddress.Build(customerId.Value, confirmationHash.Value);

            try
            {
                registeredCustomer.ConfirmEmailAddress(command);
            }
            catch (WrongConfirmationHashException e)
            {
                throw new Exception("Unexpected error in GivenEmailAddressWasConfirmed: " + e.Message);
            }
        }

        private void GivenEmailAddressWasChanged()
        {
            var command = ChangeCustomerEmailAddress.Build(customerId.Value, changedEmailAddress.Value);
            changedConfirmationHash = command.ConfirmationHash;
            registeredCustomer.ChangeEmailAddress(command);
        }
    }
}