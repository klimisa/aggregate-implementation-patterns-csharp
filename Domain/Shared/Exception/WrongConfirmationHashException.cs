namespace Domain.Shared.Exception
{
    using System;

    public class WrongConfirmationHashException: Exception
    {
        public WrongConfirmationHashException(): base("confirmation hash does not match")
        {
            
        }
    }
}