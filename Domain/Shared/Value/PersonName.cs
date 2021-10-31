namespace Domain.Shared
{
    using System;

    public record PersonName
    {
        public string GivenName { get; }
        public string FamilyName { get; }

        private PersonName(string givenName, string familyName)
        {
            GivenName = givenName;
            FamilyName = familyName;
        }

        public static PersonName Build(string givenName, string familyName)
        {
            return new PersonName(givenName, familyName);
        }
    }
}