using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;


namespace MagicVilla_Web.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse responseModel { get; set; }

        public IHttpClientFactory httpClientFactory { get; set; }   
        public BaseService(IHttpClientFactory httpClientFactory) 
        {
            this.responseModel = new();
            this.httpClientFactory = httpClientFactory; 
        }     

        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                var client = httpClientFactory.CreateClient("MagicAPI");
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
                if (apiRequest.ContentType == SD.ContentType.MultipartFormData)
                {
                    httpRequestMessage.Headers.Add("Accept", "*/*"); //accept anything
                }  
                else
                {
                    httpRequestMessage.Headers.Add("Accept", "application/json");
                }
               

                    httpRequestMessage.RequestUri =  new Uri(apiRequest.Url);
                if (apiRequest.ContentType == SD.ContentType.MultipartFormData)
                {
                    var content = new MultipartFormDataContent();
                    foreach (var prop in apiRequest.Data.GetType().GetProperties())
                    {
                        var value = prop.GetValue(apiRequest);
                        if (value is FormFile)
                        {
                            var file = (FormFile)value;
                            if (file != null)
                            {
                                content.Add(new StreamContent(file.OpenReadStream()), prop.Name, file.FileName);

                            }
                            else
                            {
                                content.Add(new StringContent(value == null ? "" : value.ToString()), prop.Name);

                            }
                            httpRequestMessage.Content = content;
                        }
                    }

                }
                else 
                {
                    if (apiRequest.Data != null)
                    {
                        //not null HTTP POST/PUT
                        httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");
                    }
                }
                switch (apiRequest.ApiType)
                {
                    case SD.ApiType.POST:
                        httpRequestMessage.Method = HttpMethod.Post;
                        break;
                    case SD.ApiType.PUT:
                        httpRequestMessage.Method = HttpMethod.Put;
                        break;
                    case SD.ApiType.DELETE:
                        httpRequestMessage.Method = HttpMethod.Delete;
                        break;
                    default:
                        httpRequestMessage.Method = HttpMethod.Get;
                        break;
                }
                HttpResponseMessage apiResponse = null;
                if (!string.IsNullOrEmpty(apiRequest.token))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiRequest.token);

                }

               apiResponse = await client.SendAsync(httpRequestMessage);

                var apiContent = await apiResponse.Content.ReadAsStringAsync();

                try
                {
                    APIResponse ApiResponse = JsonConvert.DeserializeObject<APIResponse>(apiContent);
                    if (apiResponse.StatusCode == System.Net.HttpStatusCode.BadRequest || apiResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        ApiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                        ApiResponse.IsSuccess = false;
                        var res = JsonConvert.SerializeObject(ApiResponse);
                        var resultObj = JsonConvert.DeserializeObject<T>(res);
                        return resultObj;
                    }
                }
                catch (Exception)
                {

                    var exceptionAPIResponse = JsonConvert.DeserializeObject<T>(apiContent);
                    return exceptionAPIResponse;
                }
                var APIReponse = JsonConvert.DeserializeObject<T>(apiContent);
                return APIReponse; 

            }
            catch (Exception e)
            {
                var dto = new APIResponse
                {
                    ErrorMessage = new List<string> { Convert.ToString(e.Message) },
                    IsSuccess = false,
                };
                var res = JsonConvert.SerializeObject(dto);
                var APIResponse = JsonConvert.DeserializeObject<T>(res);
                return APIResponse;
            }
        }
    }
}
