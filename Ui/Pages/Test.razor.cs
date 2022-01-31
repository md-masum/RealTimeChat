using Ui.Models;

namespace Ui.Pages
{
    public partial class Test : IDisposable
    {
        public List<TestDto> TestLists { get; set; }
        private bool _isShowDialog = false;
        private string _testId = null;

        private bool _isShowEditDialog = false;
        private TestDto _editItem;

        private bool _isShowCreateDialog = false;
        private TestDto _createItem;

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

        private void DeleteTest(string testId)
        {
            _testId = testId;
            _isShowDialog = true;
        }

        private async Task OnModalOkClick()
        {
            var result = await _httpClient.DeleteAsync($"api/test/{_testId}");
            if (result)
            {
                TestLists = TestLists.Where(c => c.Id != _testId).ToList();
                StateHasChanged();
            }
            _isShowDialog = false;
        }

        private void EditTest(TestDto item)
        {
            _isShowEditDialog = true;
            _editItem = item;
        }
        private async Task OnEditTestSubmit()
        {
            var result = await _httpClient.PostAsync<TestDto, TestDto>("api/test", _editItem);
            var test = TestLists.FirstOrDefault(c => c.Id == _editItem.Id);
            if (test != null)
            {
                test.Name = result.Name;
                test.Email = result.Email;
            }

            _isShowEditDialog = false;
        }

        private void OnCreateClick()
        {
            _createItem = new TestDto();
            _isShowCreateDialog = true;
        }
        private async Task OnCreateTestSubmit()
        {
            var result = await _httpClient.PostAsync<TestDto, TestDto>("api/test", _createItem);
            if (result != null)
            {
                TestLists.Add(result);
                _toastService.ShowSuccess("Test created successfully", 5000);
            }
            
            _isShowCreateDialog = false;
        }
        public void Dispose()
        {
            _httpInterceptor.DisposeEvent();
            _store.OnChange -= StateHasChanged;
        }
    }
}
