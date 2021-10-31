namespace Domain.Shared
{
    using System;

    public record Hash
    {
        public string Value { get; }

        private Hash(string value)
        {
            Value = value;
        }

        public static Hash Generate()
        {
            return new Hash(Guid.NewGuid().ToString());
        }

        public static Hash Build(string hash)
        {
            return new Hash(hash);
        }
    };
}