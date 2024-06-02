using Xunit;
using Microsoft.EntityFrameworkCore;
using Dierentuin_App.Data;
using System.Data;

public class UnitTest1
{
    [Fact]
    public async Task CanOpenConnectionToDatabase()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<Dierentuin_AppContext>()
            .UseSqlite("Data Source=Dierentuin_AppContext-8936df66-a63e-4347-9020-18b0e3d246f1.db")
            .Options;

        // Act
        using (var context = new Dierentuin_AppContext(options))
        {
            await context.Database.OpenConnectionAsync();

            // Assert
            Assert.Equal(ConnectionState.Open, context.Database.GetDbConnection().State);
        }
    }
}