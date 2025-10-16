using System.Net.Http;
using System.Text;
using System.Text.Json;
using Chekers.ModelLogic;
using Chekers.Models;

using System.Windows.Input;     

namespace Chekers.ViewModel
{
    internal partial class MainPageVM : ObservableObject
    {

        private User user = new();
        public ICommand LoginCommand { get; }
        public ICommand ToggleIsPasswordCommand { get; }
        public bool IsBusy { get; set; } = false;
        public string UserName
        {
            get => user.UserName;
            set
            {
                user.UserName = value;
                (LoginCommand as Command)?.ChangeCanExecute();
            }
        }
        public string Email
        {
            get => user.Email;
            set
            {
                user.Email = value;
                OnPropertyChanged(nameof(Email));
                (LoginCommand as Command)?.ChangeCanExecute();
            }
        }
        public string Password
        {
            get => user.Password;
            set
            {
                user.Password = value;
                (LoginCommand as Command)?.ChangeCanExecute();
            }
        }
        public bool IsPassword { get; set; } = true;
        public ICommand GoToRegisterCommand { get; }
        public MainPageVM()
        {
            LoginCommand = new Command(async () => await Login(), CanLogin);
            ToggleIsPasswordCommand = new Command(ToggleIsPassword);
            GoToRegisterCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new Register());
            }); 
        }

        private void ToggleIsPassword()
        {
            IsPassword = !IsPassword;
            OnPropertyChanged(nameof(IsPassword));
        }

        private async Task Login()
        {
            IsBusy = true;
            OnPropertyChanged(nameof(IsBusy));

            try
            {
                var httpClient = new HttpClient();
                var apiKey = "AIzaSyDb6javGjKtQTeJESlpm3cRz-pmQfMejBc"; // 🔁 שים את המפתח שלך כאן

                var requestData = new
                {
                    email = Email,
                    password = Password,
                    returnSecureToken = true
                };

                var json = JsonSerializer.Serialize(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(
                    $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={apiKey}",
                    content
                );

                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // התחברות הצליחה – מעבר לעמוד הבית
                    await Application.Current.MainPage.Navigation.PushAsync(new HomePage());
                }
                else
                {
                    var errorDoc = JsonDocument.Parse(responseContent);
                    var errorCode = errorDoc.RootElement
                        .GetProperty("error")
                        .GetProperty("message")
                        .GetString();

                    string errorMessage = errorCode switch
                    {
                        "EMAIL_NOT_FOUND" => "המשתמש לא קיים.",
                        "INVALID_PASSWORD" => "סיסמה שגויה.",
                        "USER_DISABLED" => "המשתמש הושבת.",
                        _ => "שגיאה כללית בהתחברות."
                    };

                    await Application.Current.MainPage.DisplayAlert("שגיאה", errorMessage, "אישור");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("שגיאה", $"בעיה בחיבור: {ex.Message}", "סגור");
            }
            finally
            {
                IsBusy = false;
                OnPropertyChanged(nameof(IsBusy));
            }
        }


        private bool CanLogin()
        {
            return (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(Email));
        }


    }
}
