using Ui.Models;

namespace Ui.Pages
{
    public partial class Profile
    {
        private UserUpdateDto _userUpdate = new UserUpdateDto();
        public UserDto UserData { get; set; } = new();

        public bool IsInEditState { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _httpInterceptor.RegisterEvent();

            var data = await _httpClient.GetAsync<UserDto>("api/user");
            if (data != null) UserData = data;
            IsInEditState = false;
        }

        public async Task Update()
        {
            var result = await _httpClient.PutAsync<UserDto, UserUpdateDto>("api/user", _userUpdate);
            if (result != null)
            {
                UserData = result;
            }
            IsInEditState = false;
        }

        public void EnableEditState()
        {
            _userUpdate = new UserUpdateDto
            {
                FirstName = UserData.FirstName,
                LastName = UserData.LastName,
                PresentAddress = UserData.PresentAddress,
                PermanentAddress = UserData.PermanentAddress,
                DateOfBirth = UserData.DateOfBirth
            };
            IsInEditState = true;
        }

        public void DisabledEditState()
        {
            _userUpdate = new UserUpdateDto();
            IsInEditState = false;
        }

        public void Dispose() => _httpInterceptor.DisposeEvent();
    }
}
