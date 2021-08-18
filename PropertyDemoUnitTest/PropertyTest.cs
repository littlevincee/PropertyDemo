using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;
using PropertyDemo.Data;
using PropertyDemo.Data.DatabaseService.PropertyService;

namespace PropertyDemoUnitTest
{
    [TestClass]
    public class PropertyTests
    {
        private IDataContext _dataContext;

        [TestInitialize]
        public void Setup()
        {
            var opts = new DbContextOptionsBuilder<DataContext>()
                        .UseInMemoryDatabase(databaseName: $"In_Memory_Ms_SQL_Db-{Guid.NewGuid()}")
                        .Options;

            _dataContext = new DataContext(opts);
        }

        [TestMethod]
        public async Task CreatePropertyRecord_SuccessAsync()
        {
            await DummyData.InsertDummyPropertyDataAsync(_dataContext);

            var count = await _dataContext.Properties.CountAsync(default);

            Assert.AreEqual(1, count);

            var result = await _dataContext
                                .Properties
                                .Where(s => s.OwnerDetailId == 1 && s.PropertyName == "Home")
                                .FirstOrDefaultAsync(default);

            Assert.AreEqual("Home", result.PropertyName);
        }
    }
}
