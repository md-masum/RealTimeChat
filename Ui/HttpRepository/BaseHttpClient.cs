using System.Net.Http.Json;
using Ui.Response;
using Ui.Service;
using Ui.Store;

namespace Ui.HttpRepository
{
    public class BaseHttpClient
    {
        private readonly HttpClient _client;
        private readonly ToastService _toastService;
        private readonly StoreContainer _store;

        public BaseHttpClient(HttpClient client, ToastService toastService, StoreContainer store)
        {
            _client = client;
            _toastService = toastService;
            _store = store;
        }

        public async Task<TResponse> GetAsync<TResponse>(string url)
        {
            try
            {
                _store.IsLoading = true;
                var response = await _client.GetFromJsonAsync<ApiResponse<TResponse>>(url);
                _store.IsLoading = false;
                return response!.Data;
            }
            catch (Exception e)
            {
                _store.IsLoading = false;
                Console.WriteLine(e);
                throw;
            }
            
        }

        public async Task<TResponse> PostAsync<TResponse, TRequest>(string url, TRequest requestBody)
        {
            try
            {
                _store.IsLoading = true;
                var response = await _client.PostAsJsonAsync(url, requestBody);
                var content = await response.Content.ReadFromJsonAsync<ApiResponse<TResponse>>();
                _store.IsLoading = false;
                return content!.Data;
            }
            catch (Exception e)
            {
                _store.IsLoading = false;
                Console.WriteLine(e);
                throw;
            }

        }

        public async Task<bool> SignalRPost(string url, object requestBody)
        {
            try
            {
                var response = await _client.PostAsJsonAsync(url, requestBody);
                var content = await response.Content.ReadFromJsonAsync<bool>();
                return content;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

        }

        public async Task<TResponse> PutAsync<TResponse, TRequest>(string url, TRequest requestBody)
        {
            try
            {
                _store.IsLoading = true;
                var response = await _client.PutAsJsonAsync(url, requestBody);
                var content = await response.Content.ReadFromJsonAsync<ApiResponse<TResponse>>();
                _store.IsLoading = false;
                return content!.Data;
            }
            catch (Exception e)
            {
                _store.IsLoading = false;
                Console.WriteLine(e);
                throw;
            }

        }

        public async Task<bool> DeleteAsync(string url)
        {
            try
            {
                _store.IsLoading = true;
                var response = await _client.DeleteAsync(url);
                _store.IsLoading = false;
                if (response.IsSuccessStatusCode) return true;
                return false;
            }
            catch (Exception e)
            {
                _store.IsLoading = false;
                Console.WriteLine(e);
                throw;
            }

        }
    }
}
