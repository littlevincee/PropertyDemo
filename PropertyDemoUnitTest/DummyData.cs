using System;
using System.Linq;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using PropertyDemo.Data.Model;
using PropertyDemo.Data;

namespace PropertyDemoUnitTest
{
    public static class DummyData
    {
        public static async Task InsertDummyOwnerDetailDataAsync(IDataContext dataContext)
        {
            var dummyReccords = Builder<OwnerDetail>
                .CreateListOfSize(3)
                .All()
                    .With(r => r.FirstName = "Vinson")
                    .With(r => r.Surname = "Wong")
                    .With(r => r.ContactNumber = 12345)
                    .With(r => r.HongKongId = "R123")
                .Build()
                .ToList();

            await dataContext
                    .OwnerDetails
                    .AddRangeAsync(dummyReccords);

            await dataContext.SaveChangesAsync(default);
        }

        public static async Task InsertDummyPropertyDataAsync(IDataContext dataContext)
        {
            var dummyOwnerDetail = Builder<OwnerDetail>
                                    .CreateNew()
                                        .With(r => r.OwnerDetailId = 1)
                                        .With(r => r.FirstName = "Vinson")
                                        .With(r => r.Surname = "Wong")
                                        .With(r => r.ContactNumber = 12345)
                                        .With(r => r.HongKongId = "R123")
                                    .Build();

            var dummyProperty = Builder<Property>
                                   .CreateNew()
                                        .With(r => r.OwnerDetailId = dummyOwnerDetail.OwnerDetailId)
                                        .With(r => r.PropertyName = "Home")
                                    .Build();

            await dataContext.Properties.AddAsync(dummyProperty);

            await dataContext.SaveChangesAsync(default);
        }
    }
}
