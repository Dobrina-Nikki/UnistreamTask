using Xunit;
using Moq;
using UnistreamTaskWebAPI.Services;
using UnistreamTaskWebAPI.Data;
using UnistreamTaskWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using UnistreamTaskWebAPI.Models.Exceptions;

namespace UnistreamTaskWebAPI.UnitTests
{
    public class TransactionServiceTests
    {
        private readonly Mock<ILogger<TransactionService>> _loggerMock = new();
        private readonly DbContextOptions<AppDbContext> _dbOptions;

        public TransactionServiceTests()
        {
            _dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;
        }

        [Fact]
        public async Task CreateTransaction_ExistingId_ReturnsExistingDate()
        {
            // Arrange
            var existingId = Guid.NewGuid();
            var existingDate = DateTime.UtcNow.AddDays(-1);

            using var context = new AppDbContext(_dbOptions);
            context.Transactions.Add(new Models.Entities.Transaction { Id = existingId, InsertDateTime = existingDate });
            await context.SaveChangesAsync();

            var service = new TransactionService(context, _loggerMock.Object);

            // Act
            var result = await service.CreateTransactionAsync(new Transaction(existingId, DateTime.Now, 100, DateTime.Now));

            // Assert
            Assert.Equal(existingDate, result.InsertDateTime);
        }

        [Theory]
        [InlineData(-100)]
        [InlineData(0)]
        public async Task CreateTransaction_InvalidAmount_ThrowsException(decimal amount)
        {
            // Arrange
            using var context = new AppDbContext(_dbOptions);
            var service = new TransactionService(context, _loggerMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ApiException>(() =>
                service.CreateTransactionAsync(new Transaction(Guid.NewGuid(), DateTime.Now, amount, DateTime.Now)));
        }

        [Fact]
        public async Task CreateTransaction_ReachesLimit_ThrowsException()
        {
            // Arrange
            using var context = new AppDbContext(_dbOptions);
            for (int i = 0; i < 100; i++)
            {
                context.Transactions.Add(new Models.Entities.Transaction { Id = Guid.NewGuid() });
            }
            await context.SaveChangesAsync();

            var service = new TransactionService(context, _loggerMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ApiException>(() =>
                service.CreateTransactionAsync(new Transaction(Guid.NewGuid(), DateTime.Now, 100, DateTime.Now)));
        }
    }

}
