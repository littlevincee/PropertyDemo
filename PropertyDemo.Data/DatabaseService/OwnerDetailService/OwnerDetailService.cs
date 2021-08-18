using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PropertyDemo.Data.Model;
using PropertyDemo.Service.Exceptions;
using PropertyDemo.Service.ViewModel;

namespace PropertyDemo.Data.DatabaseService.OwnerDetailService
{
    public interface IOwnerDetailService
    {
        List<OwnerDetailViewModel> GetAllOwnerDetails();

        OwnerDetailViewModel GetOwnerDetailById(int id);

        Task<int> SaveChangesAsync(OwnerDetailViewModel ownerDetailViewModel);

        Task<int> UpdateAsync(OwnerDetailViewModel ownerDetailViewModel);
    }

    public class OwnerDetailService : IOwnerDetailService
    {
        private readonly IDataContext _dataContext;

        public OwnerDetailService(IDataContext dataContext) => _dataContext = dataContext;

        public async Task<int> SaveChangesAsync(OwnerDetailViewModel ownerDetailViewModel)
        {
            var result = 0;

            if (ownerDetailViewModel is null)
            {
                return result;
            }

            ownerDetailViewModel.CreatedOn = DateTime.Now;

            var ownerDetail = OwnerDetailModelMapper(ownerDetailViewModel);

            _dataContext.OwnerDetails.Add(ownerDetail);

            result = await _dataContext.SaveChangesAsync(default);


            return result;
        }

        public async Task<int> UpdateAsync(OwnerDetailViewModel ownerDetailViewModel)
        {
            var result = 0;

            if (ownerDetailViewModel is null)
            {
                return result;
            }
            
            var ownerDetail = OwnerDetailModelMapper(ownerDetailViewModel);

            try
            {
                ownerDetail.UpdatedOn = DateTime.Now;

                _dataContext.OwnerDetails.Update(ownerDetail);

                result = await _dataContext.SaveChangesAsync(default);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!_dataContext.OwnerDetails.Any(x => x.OwnerDetailId == ownerDetail.OwnerDetailId))
                {
                    throw new CustomException($"Property not found: {ex.Message}");
                }
            }

            return result;

        }

        public OwnerDetailViewModel GetOwnerDetailById(int id) => OwnerDetailViewModelMapper(_dataContext.OwnerDetails.Where(s => s.OwnerDetailId == id).First());

        public List<OwnerDetailViewModel> GetAllOwnerDetails() => OwnerDetailViewModelMapper(_dataContext.OwnerDetails.AsQueryable()).ToList();

        private List<OwnerDetailViewModel> OwnerDetailViewModelMapper(IQueryable<OwnerDetail> ownerDetails)
        {
            var ownerDetailViewModels = new List<OwnerDetailViewModel>();

            foreach (var item in ownerDetails)
            {
                ownerDetailViewModels.Add(
                  new OwnerDetailViewModel()
                  {
                      OwnerDetailId = item.OwnerDetailId,
                      Title = item.Title,
                      FirstName = item.FirstName,
                      Surname = item.Surname,
                      HongKongId = item.HongKongId,
                      ContactNumber = item.ContactNumber
                  });
            }

            return ownerDetailViewModels;
        }

        private OwnerDetailViewModel OwnerDetailViewModelMapper(OwnerDetail ownerDetail)
        {
            if (ownerDetail is null)
            {
                return new OwnerDetailViewModel();
            }

            return new OwnerDetailViewModel()
            {
                OwnerDetailId = ownerDetail.OwnerDetailId,
                Title = ownerDetail.Title,
                FirstName = ownerDetail.FirstName,
                Surname = ownerDetail.Surname,
                HongKongId = ownerDetail.HongKongId,
                ContactNumber = ownerDetail.ContactNumber
            };
        }

        private OwnerDetail OwnerDetailModelMapper(OwnerDetailViewModel ownerDetailViewModel)
        {
            if (ownerDetailViewModel is null)
            {
                return new OwnerDetail();
            }

            return new OwnerDetail()
            {
                OwnerDetailId = ownerDetailViewModel.OwnerDetailId,
                Title = ownerDetailViewModel.Title,
                FirstName = ownerDetailViewModel.FirstName,
                Surname = ownerDetailViewModel.Surname,
                HongKongId = ownerDetailViewModel.HongKongId,
                ContactNumber = ownerDetailViewModel.ContactNumber
            };
        }
    }
}
