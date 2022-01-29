using Ui.Models;

namespace Ui.Pages
{
    public partial class Test : IDisposable
    {
        public List<TestDto> TestLists { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Console.WriteLine("Test Render");
            _store.OnChange += StateHasChanged;
            _httpInterceptor.RegisterEvent();
            TestLists = await GetTestDto();
        }

        private async Task<List<TestDto>> GetTestDto()
        {
            var data = await _httpClient.GetAsync<List<TestDto>>("api/test");
            return data;
        }

        private async Task<bool> DeleteTest(string testId)
        {
            Console.WriteLine("test id is: " + testId);
            return default;
        }

        private async Task<bool> EditTest(TestDto item)
        {
            Console.WriteLine("test id is: " + item);
            return default;
        }

        public void Dispose()
        {
            _httpInterceptor.DisposeEvent();
            _store.OnChange -= StateHasChanged;
        }
    }
}
