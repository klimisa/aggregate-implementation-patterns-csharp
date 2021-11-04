namespace Domain.Tests.Functional.Traditional.Customer
{
    using System;
    using Domain.Functional.Traditional.Customer;
    using FluentAssertions;
    using Shared;
    using Shared.Command;
    using Shared.Exception;
    using Xunit;

    public class Customer2Tests
    {
        private ID customerId;
        private Hash confirmationHash;
        private readonly PersonName name;
        private readonly EmailAddress emailAddress;
        private readonly EmailAddress changedEmailAddress;
        private readonly Hash wrongConfirmationHash;
        private Hash changedConfirmationHash;
        private CustomerState registeredCustomer;

        public Customer2Tests()
        {
            emailAddress = EmailAddress.Build("john@doe.com");
            changedEmailAddress = EmailAddress.Build("john+changed@doe.com");
            wrongConfirmationHash = Hash.Generate();
            changedConfirmationHash = Hash.Generate();
            name = PersonName.Build("John", "Doe");
        }

        [Fact]
        void RegisterCustomer2()
        {
            // When
            var command = RegisterCustomer.Build(emailAddress.Value, name.GivenName, name.FamilyName);
            var customer = Customer2.Register(command);

            // Then it should succeed
            // and should have the expected state
            customer.Should().NotBeNull();
            customer.Id.Should().Be(command.CustomerId);
            customer.Name.Should().Be(command.Name);
            customer.EmailAddress.Should().Be(command.EmailAddress);
            customer.ConfirmationHash.Should().Be(command.ConfirmationHash);
            customer.IsEmailAddressConfirmed.Should().BeFalse();
        }

        [Fact]
        void ConfirmEmailAddress()
        {
            // Given
            GivenARegisteredCustomer();

            // When ConfirmCustomerEmailAddress
            // Then it should succeed
            var command = ConfirmCustomerEmailAddress.Build(customerId.Value, confirmationHash.Value);
            CustomerState changedCustomer = default;
            Action act = () => changedCustomer = Customer2.ConfirmEmailAddress(registeredCustomer, command);
            act.Should().NotThrow();

            // and the emailAddress should be confirmed
            Assert.True(changedCustomer.IsEmailAddressConfirmed);
        }

        [Fact]
        void ConfirmEmailAddress_WithWrongConfirmationHash()
        {
            // Given
            GivenARegisteredCustomer();

            // When confirmCustomerEmailAddress
            // Then it should throw WrongConfirmationHashException
            var command = ConfirmCustomerEmailAddress.Build(customerId.Value, wrongConfirmationHash.Value);
            Action act = () => Customer2.ConfirmEmailAddress(registeredCustomer, command);
            act.Should().Throw<WrongConfirmationHashException>();
        }

        [Fact]
        void ChangeEmailAddress()
        {
            // Given
            GivenARegisteredCustomer();

            // When changeCustomerEmailAddress
            var command = ChangeCustomerEmailAddress.Build(customerId.Value, changedEmailAddress.Value);
            var changedCustomer = Customer2.ChangeEmailAddress(registeredCustomer, command);

            // Then the emailAddress and confirmationHash should be changed and the emailAddress should be unconfirmed
            command.EmailAddress.Should().Be(changedCustomer.EmailAddress);
            command.ConfirmationHash.Should().Be(changedCustomer.ConfirmationHash);
            changedCustomer.IsEmailAddressConfirmed.Should().BeFalse();
        }

        [Fact]
        void ConfirmEmailAddress_WhenItWasPreviouslyConfirmedAndThenChanged()
        {
            // Given
            GivenARegisteredCustomer();
            GivenEmailAddressWasConfirmed();
            GivenEmailAddressWasChanged();

            // When confirmEmailAddress
            // Then it should throw WrongConfirmationHashException
            var command = ConfirmCustomerEmailAddress.Build(customerId.Value, changedConfirmationHash.Value);
            CustomerState changedCustomer = default;
            Action act = () => changedCustomer = Customer2.ConfirmEmailAddress(registeredCustomer, command);
            act.Should().NotThrow();

            // and the emailAddress of the changed Customer should be confirmed
            changedCustomer.IsEmailAddressConfirmed.Should().BeTrue();
        }

        /**
         * Helper methods to set up the Given state
         */
        private void GivenARegisteredCustomer()
        {
            var register = RegisterCustomer.Build(emailAddress.Value, name.GivenName, name.FamilyName);
            customerId = register.CustomerId;
            confirmationHash = register.ConfirmationHash;
            registeredCustomer = Customer2.Register(register);
        }

        private void GivenEmailAddressWasConfirmed()
        {
            var command = ConfirmCustomerEmailAddress.Build(customerId.Value, confirmationHash.Value);

            try
            {
                registeredCustomer = Customer2.ConfirmEmailAddress(registeredCustomer, command);
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
            registeredCustomer = Customer2.ChangeEmailAddress(registeredCustomer, command);
        }
    }
}