namespace Domain.Tests.OOP.Traditional.Customer
{
    using System;
    using Domain.OOP.Traditional.Customer;
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
        public void RegisterCustomer1()
        {
            // When
            var command = RegisterCustomer.Build(emailAddress.Value, name.GivenName, name.FamilyName);
            var customer = Customer1.Register(command);

            // Then it should succeed
            // and should have the expected state
            Assert.NotNull(customer);
            Assert.Equal(customer.Id, command.CustomerId);
            Assert.Equal(customer.Name, command.Name);
            Assert.Equal(customer.EmailAddress, command.EmailAddress);
            Assert.Equal(customer.ConfirmationHash, command.ConfirmationHash);
            Assert.False(customer.IsEmailAddressConfirmed);
        }

        [Fact]
        public void Confirm_EmailAddress()
        {
            // Given
            GivenARegisteredCustomer();

            // When ConfirmCustomerEmailAddress
            // Then it should succeed
            var command = ConfirmCustomerEmailAddress.Build(customerId.Value, confirmationHash.Value);
            var exception = Record.Exception(() => registeredCustomer.ConfirmEmailAddress(command));
            Assert.Null(exception);

            // and the emailAddress should be confirmed
            Assert.True(registeredCustomer.IsEmailAddressConfirmed);
        }

        [Fact]
        public void ConfirmEmailAddress_WithWrongConfirmationHash()
        {
            // Given
            GivenARegisteredCustomer();

            // When ConfirmCustomerEmailAddress
            // Then it should throw WrongConfirmationHashException
            var command = ConfirmCustomerEmailAddress.Build(customerId.Value, wrongConfirmationHash.Value);
            Assert.Throws<WrongConfirmationHashException>(() => registeredCustomer.ConfirmEmailAddress(command));

            // and the emailAddress should not be confirmed
            Assert.False(registeredCustomer.IsEmailAddressConfirmed);
        }

        [Fact]
        public void ChangeEmailAddress()
        {
            // Given
            GivenARegisteredCustomer();

            // When changeCustomerEmailAddress
            var command = ChangeCustomerEmailAddress.Build(customerId.Value, changedEmailAddress.Value);
            registeredCustomer.ChangeEmailAddress(command);

            // Then the emailAddress and confirmationHash should be changed and the emailAddress should be unconfirmed
            Assert.Equal(registeredCustomer.EmailAddress, command.EmailAddress);
            Assert.Equal(registeredCustomer.ConfirmationHash, command.ConfirmationHash);
            Assert.False(registeredCustomer.IsEmailAddressConfirmed);
        }

        [Fact]
        public void ConfirmEmailAddress_WhenItWasPreviouslyConfirmedAndThenChanged()
        {
            // Given
            GivenARegisteredCustomer();
            GivenEmailAddressWasConfirmed();
            GivenEmailAddressWasChanged();

            // When confirmCustomerEmailAddress
            // Then it should succeed
            var command = ConfirmCustomerEmailAddress.Build(customerId.Value, changedConfirmationHash.Value);
            var exception = Record.Exception(() => registeredCustomer.ConfirmEmailAddress(command));
            Assert.Null(exception);

            // and the emailAddress should be confirmed
            Assert.True(registeredCustomer.IsEmailAddressConfirmed);
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
                throw new Exception("unexpected error in givenEmailAddressWasConfirmed: " + e.Message);
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