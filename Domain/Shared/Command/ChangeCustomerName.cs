namespace Domain.Shared.Command
{
    public class ChangeCustomerName
    {
        public PersonName Name { get; }
        public ID CustomerID { get; }

        private ChangeCustomerName(string customerId, string givenName, string familyName)
        {
            CustomerID = ID.Build(customerId);
            Name = PersonName.Build(givenName, familyName);
        }

        public static ChangeCustomerName Build(string customerId, string givenName, string familyName)
        {
            return new ChangeCustomerName(customerId, givenName, familyName);
        }
    }
}