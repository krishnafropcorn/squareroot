namespace CardReader.Interfaces
{
    public interface IMagStripeResult
    {
        string PrimaryAccountNumberSecure { get; set; }

        void DestroyPrimaryAccountNumber();
    }
}
