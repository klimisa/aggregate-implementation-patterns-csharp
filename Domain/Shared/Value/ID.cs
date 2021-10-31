namespace Domain.Shared
{
    using System;

    public record ID
    {
        public string Value { get; }

        private ID(string value)
        {
            Value = value;
        }

        public static ID Generate()
        {
            return new ID(Guid.NewGuid().ToString());
        }

        public static ID Build(string id)
        {
            return new ID(id);
        }
    };
}