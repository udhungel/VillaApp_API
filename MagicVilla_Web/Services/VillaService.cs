﻿using MagicVilla_Utility;
using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services
{
    public class VillaService : BaseService, IVillaService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string villaUrl;

        public VillaService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");

        }

        public Task<T> CreateAsync<T>(VillaCreateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url= villaUrl + "api/VillaAPI",
                token= token    

            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,             
                Url = villaUrl + "api/VillaAPI/" + id,
                token = token

            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,               
                Url = villaUrl + "api/VillaAPI",
                token = token
            });
        }

        public Task<T> GetAllAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            { ApiType = SD.ApiType.GET,
              Url = villaUrl + "api/VillaAPI/" + id,
              token = token
            });
        }

        public Task<T> UpdateAsync<T>(VillaUpdateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = villaUrl + "api/VillaAPI/" + dto.Id,
                token = token
            });
        }
    }
}
