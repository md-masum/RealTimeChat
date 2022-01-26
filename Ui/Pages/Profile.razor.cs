using Microsoft.AspNetCore.Components;
using Ui.HttpRepository;
using Ui.Models;

namespace Ui.Pages
{
    public partial class Profile
    {
        private UserUpdateDto _userUpdate = new UserUpdateDto();

        [Inject]
        public IUserService UserService { get; set; }

        [Inject]
        public HttpInterceptorService Interceptor { get; set; }

        public UserDto UserData { get; set; } = new();

        public bool ShowErrorMessage { get; set; }
        public string Error { get; set; } = String.Empty;

        public bool IsInEditState { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Interceptor.RegisterEvent();

            var data = await UserService.GetCurrentUser();
            if (data.IsSuccess)
            {
                UserData = data.Data;
            }
            else
            {
                Error = data.Message;
                ShowErrorMessage = true;
            }
            IsInEditState = false;
        }

        public async Task Update()
        {
            var result = await UserService.UpdateUser(_userUpdate);
            if (!result.IsSuccess)
            {
                Error = result.Message;
                ShowErrorMessage = true;
            }

            UserData = result.Data;
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

        public void Dispose() => Interceptor.DisposeEvent();
    }
}
