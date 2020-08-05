using ExeconWeather.Models;
using ExeconWeather.Services.Interface;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ExeconWeather.Services.Implementation
{
    class LocationService : ILocationService
    {
        public async Task<LocationModel> GetLastKnownLocation()
        {
            LocationModel locationModel = new LocationModel();
            locationModel.IsSuccess = true;

            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    locationModel.Latitude = location.Latitude;
                    locationModel.Longitude = location.Longitude;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                locationModel.IsSuccess = false;
                locationModel.ExMessage = fnsEx.Message;
            }
            catch (FeatureNotEnabledException fneEx)
            {
                locationModel.IsSuccess = false;
                locationModel.ExMessage = fneEx.Message;
            }
            catch (PermissionException pEx)
            {
                locationModel.IsSuccess = false;
                locationModel.ExMessage = pEx.Message;
            }
            catch (Exception ex)
            {
                locationModel.IsSuccess = false;
                locationModel.ExMessage = ex.Message;
            }

            return locationModel;
        }

        public async Task<LocationModel> GetCurrentLocation()
        {
            LocationModel locationModel = new LocationModel();
            locationModel.IsSuccess = true;

            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    locationModel.Latitude = location.Latitude;
                    locationModel.Longitude = location.Longitude;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                locationModel.IsSuccess = false;
                locationModel.ExMessage = fnsEx.Message;
            }
            catch (FeatureNotEnabledException fneEx)
            {
                locationModel.IsSuccess = false;
                locationModel.ExMessage = fneEx.Message;
            }
            catch (PermissionException pEx)
            {
                locationModel.IsSuccess = false;
                locationModel.ExMessage = pEx.Message;
            }
            catch (Exception ex)
            {
                locationModel.IsSuccess = false;
                locationModel.ExMessage = ex.Message;
            }

            return locationModel;
        }
    }
}
