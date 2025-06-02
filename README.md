<h2>Тестовое задание T3</h2>
Дан record, описывающий транзакцию:
```
    public record class Transaction
    {
        public Guid Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
    }
```

Напишите Web API, позволяющее создавать и сохранять транзакции и получать ранее созданные транзакции по Id. В сервисе должно быть 2 запроса:
```
  POST /api/v1/Transaction
{
	"id": "1afa615f-af61-4d8a-b891-bc874c937772",
	"transactionDate": "2024-10-25T00:00:00+05:00",
	"amount": 12.34
}
Response 200 OK:
{
	"insertDateTime": "2024-10-25T12:03:34+05:00"
}
```
