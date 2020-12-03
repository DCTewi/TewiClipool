using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace TewiClipool.Client.Services
{
    public abstract class HttpClientServiceBase<TModel> : HttpClientServiceBase<TModel, int>
    {
        protected HttpClientServiceBase(HttpClient http) : base(http)
        {
        }
    }

    public abstract class HttpClientServiceBase<TModel, TKey>
    {
        protected readonly HttpClient http;
        public HttpClientServiceBase(HttpClient http)
        {
            this.http = http;
        }

        protected abstract string GetApiUrl();

        public virtual async Task<TKey> Create(TModel model)
        {
            var response = await http.PostAsJsonAsync(GetApiUrl(), model);
            return await response.Content.ReadFromJsonAsync<TKey>();
        }

        public virtual async Task<bool> Delete(TKey id)
        {
            var response = await http.DeleteAsync($"{GetApiUrl()}/id/{id}");
            return response.IsSuccessStatusCode;
        }

        public virtual async Task<bool> Update(TModel model)
        {
            var response = await http.PutAsJsonAsync(GetApiUrl(), model);
            return response.IsSuccessStatusCode;
        }

        public virtual async Task<TModel> Get(TKey id)
        {
            return await http.GetFromJsonAsync<TModel>($"{GetApiUrl()}/id/{id}");
        }

        public virtual async Task<List<TModel>> GetAll()
        {
            return await http.GetFromJsonAsync<List<TModel>>(GetApiUrl());
        }
    }
}