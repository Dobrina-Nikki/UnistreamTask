namespace UnistreamTaskWebAPI.Models
{
    // На собеседовании заходил вопрос о "новых" технологиях
    // record Transaction - сократили до позиционной 
    public record Transaction(Guid Id, DateTime TransactionDate, decimal Amount, DateTime InsertDateTime);
}
