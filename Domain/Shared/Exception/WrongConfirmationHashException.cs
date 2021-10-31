namespace Domain.Shared.Exception
{
    using System;

    public class WrongConfirmationHashException: Exception
    {
        public WrongConfirmationHashException(): base("Confirmation hash does not match")
        {
            
        }
    }
}