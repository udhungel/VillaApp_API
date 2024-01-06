using MagicVilla_Utility;
using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services
{
    public class VillaNumberService : BaseService, IVillaNumberService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string villaUrl;

        public VillaNumberService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");

        }

        public Task<T> CreateAsync<T>(VillaNumberCreateDTO dto,string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url= villaUrl + "api/VillaNumberAPI",
                token = token

            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,             
                Url = villaUrl + "api/VillaNumberAPI/" + id,
                token = token

            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,               
                Url = villaUrl + "api/VillaNumberAPI",
                token = token
            });
        }

        public Task<T> GetAllAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            { ApiType = SD.ApiType.GET,
              Url = villaUrl + "api/VillaNumberAPI/" + id,
                token = token
            });
        }

        public Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = villaUrl + "api/VillaNumberAPI/" + dto.VillaNo,
                token = token
            });
        }
    }
}
