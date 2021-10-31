namespace Domain.Shared
{
    public record EmailAddress
    {
        public string Value { get; }

        private EmailAddress(string value)
        {
            Value = value;
        }

        public static EmailAddress Build(string emailAddress)
        {
            return new EmailAddress(emailAddress);
        }
    };
}