using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;
using PropertyDemo.Data;
using PropertyDemo.Data.DatabaseService.OwnerDetailService;

namespace PropertyDemoUnitTest
{
    [TestClass]
    public class OwnerDetailTests
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
        public async Task CreateOwnerDetailRecords_SuccessAsync()
        {
            await DummyData.InsertDummyOwnerDetailDataAsync(_dataContext);

            var count = await _dataContext.OwnerDetails.CountAsync(default);

            Assert.AreEqual(3, count);

            var result = await _dataContext
                                .OwnerDetails
                                .Where(s => s.FirstName == "Vinson" && s.Surname == "Wong")
                                .FirstOrDefaultAsync(default);

            Assert.AreEqual("Vinson", result.FirstName);
            Assert.AreEqual("Wong", result.Surname);
        }
    }
}
