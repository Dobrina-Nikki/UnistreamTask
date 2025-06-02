CREATE TABLE Transactions (
    Id UUID PRIMARY KEY,
    TransactionDate TIMESTAMP NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    InsertDateTime TIMESTAMP NOT NULL
);