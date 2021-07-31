using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PropertyDemo.Service.Exceptions;
using PropertyDemo.Service.ViewModel;

namespace PropertyDemo.Data.DatabaseService.PropertyService
{
    public interface IPropertyService
    {
        List<PropertyViewModel> GetAllNonAdminPropertiesByUserId(string userId);

        List<PropertyViewModel> GetAllPropertiesByCheckingAdminRight(string userId);

        Task<PropertyViewModel> GetPropertyByPropertyIdAsync(int propertyId);

        Task<int> SaveChangesAsync(Model.Property property);

        Task<int> UpdateAsync(PropertyViewModel propertyVM);

        Task<int> DeleteAsync(int propertyId);
    }

    public class PropertyService : IPropertyService
    {
        private readonly IDataContext _dataContext;

        public PropertyService(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        #region Save Property

        public async Task<int> SaveChangesAsync(Model.Property property)
        {
            property.CreatedOn = DateTime.UtcNow;

            _dataContext.Properties.Add(property);

            var saveResult = await _dataContext.SaveChangesAsync(default);

            return saveResult;
        }

        public async Task<int> UpdateAsync(PropertyViewModel propertyVM)
        {
            var property = PropertyModelMapper(propertyVM);

            var saveResult = 0;

            try
            {
                property.UpdatedOn = DateTime.UtcNow;

                _dataContext.Properties.Update(property);

                saveResult = await _dataContext.SaveChangesAsync(default);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!_dataContext.Properties.Any(x => x.PropertyId == property.PropertyId))
                {
                    throw new CustomException($"Property not found: {ex.Message}");
                }
            }

            return saveResult;
        }

        public async Task<int> DeleteAsync(int propertyId)
        {
            var property = await _dataContext.Properties.FindAsync(propertyId);
            _dataContext.Properties.Remove(property);
            return await _dataContext.SaveChangesAsync(default);
        }
        #endregion

        #region Get properties

        /// <summary>
        /// Get all properties of an non-admin user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>A list of property view model</returns>
        public List<PropertyViewModel> GetAllNonAdminPropertiesByUserId(string userId)
        {
            var properties = _dataContext.Properties
                                .Where(x => x.ApplicationUserId == userId && x.ApplicationUser.IsAdministrator == false);

            return PropertyViewModelMapper(properties);
        }

        /// <summary>
        /// Get all properties of an admin user and all non-admin's properties
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>A list of property view model</returns>
        public List<PropertyViewModel> GetAllPropertiesByCheckingAdminRight(string userId)
        {
            var adminPropertiesQuery = from prop in _dataContext.Properties
                       join user in _dataContext.ApplicationUsers
                       on prop.ApplicationUserId equals user.Id
                       where prop.ApplicationUserId == userId && user.IsAdministrator == true
                       select prop;

            var nonAdminPropertiesQuery = from prop in _dataContext.Properties
                      join user in _dataContext.ApplicationUsers
                       on prop.ApplicationUserId equals user.Id
                      where user.IsAdministrator == false && prop.ApplicationUserId != userId
                      select prop;

            var adminProperties = PropertyViewModelMapper(adminPropertiesQuery);

            var nonAdminProperties = PropertyViewModelMapper(nonAdminPropertiesQuery);

            return adminProperties.Concat(nonAdminProperties).ToList();
        }

        /// <summary>
        /// Get a single property record by its Id
        /// </summary>
        /// <param name="propertyId">Property Id</param>
        /// <returns>A Property View Model</returns>
        public async Task<PropertyViewModel> GetPropertyByPropertyIdAsync(int propertyId)
        {
            var property = await _dataContext.Properties.FindAsync(propertyId);

            _ = property ?? throw new CustomException("Property not found");

            return PropertyViewModelMapper(property);
        }

        #endregion

        #region private functions

        /// <summary>
        /// A function to map IQuery of PropertDemo.Data.Model.Property to a list of PropertyDemo.Service.ViewModel.PropertyViewModel
        /// </summary>
        /// <param name="properties">IQueryable of Model.Property</param>
        /// <returns>A list of property view model</returns>
        private List<PropertyViewModel> PropertyViewModelMapper(IQueryable<Model.Property> properties)
        {
            var propertiesVM = new List<PropertyViewModel>();

            foreach (var item in properties)
            {
                propertiesVM.Add(
                    new PropertyViewModel()
                    {
                        PropertyId = item.PropertyId,
                        PropertyName = item.PropertyName,
                        Bedroom = item.Bedroom,
                        LeasePrice = item.LeasePrice,
                        SalePrice = item.SalePrice,
                        IsAvaliable = item.IsAvaliable,
                    });
            }

            return propertiesVM;
        }

        /// <summary>
        /// A function to map a single PropertDemo.Data.Model.Property to a PropertyDemo.Service.ViewModel.PropertyViewModel
        /// </summary>
        /// <param name="properties">IQueryable of Model.Property</param>
        /// <returns>A property view model</returns>
        private PropertyViewModel PropertyViewModelMapper(Model.Property property)
            => new PropertyViewModel()
            {
                PropertyId = property.PropertyId,
                PropertyName = property.PropertyName,
                Bedroom = property.Bedroom,
                LeasePrice = property.LeasePrice,
                SalePrice = property.SalePrice,
                IsAvaliable = property.IsAvaliable,
                UserId = property.ApplicationUserId
            };

        /// <summary>
        /// A function to map a single PropertyDemo.Service.ViewModel.PropertyViewModel to a PropertDemo.Data.Model.Property
        /// </summary>
        /// <param name="propertyViewModel">A Property View Model</param>
        /// <returns>A Property</returns>

        private Model.Property PropertyModelMapper(PropertyViewModel propertyViewModel)
            => new Model.Property()
            {
                PropertyId = propertyViewModel.PropertyId,
                PropertyName = propertyViewModel.PropertyName,
                Bedroom = propertyViewModel.Bedroom,
                LeasePrice = propertyViewModel.LeasePrice,
                SalePrice = propertyViewModel.SalePrice,
                IsAvaliable = propertyViewModel.IsAvaliable,
                ApplicationUserId = propertyViewModel.UserId
            };

        #endregion

    }
}
