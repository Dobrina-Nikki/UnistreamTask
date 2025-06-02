namespace UnistreamTaskWebAPI.Models.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public DateTime InsertDateTime { get; set; }
    }
}
