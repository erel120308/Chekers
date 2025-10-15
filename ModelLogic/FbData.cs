using Firebase.Auth;
using Firebase.Auth.Providers;
using Plugin.CloudFirestore;
using Chekers.Models;

namespace Chekers.ModelLogic
{
    class FbData:FbDataModel
    {
        public override async void CreateUserWithEmailAndPasswordAsync(
    string email, string password, string name,
    Action<Task> OnComplete)
        {
            try
            {
               
                var task = facl.CreateUserWithEmailAndPasswordAsync(email, password, name);
                await task;

                OnComplete?.Invoke(task);

                if (task.IsCompletedSuccessfully)
                {
                    Preferences.Set(Keys.NameKey, name);
                    Preferences.Set(Keys.EmailKey, email);
                    Preferences.Set(Keys.PasswordKey, password);

                    await Shell.Current.DisplayAlert(Strings.Success, Strings.RegisterSuccessMassege, Strings.Ok);
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message ?? string.Empty;

                if (msg.Contains("EMAIL_EXISTS", StringComparison.OrdinalIgnoreCase))
                {
                    await Shell.Current.DisplayAlert(Strings.CreatUserError, Strings.EmailAlreadyExists, Strings.Ok);
                }
                else if (msg.Contains("INVALID_EMAIL", StringComparison.OrdinalIgnoreCase))
                {
                    await Shell.Current.DisplayAlert(Strings.CreatUserError, Strings.InvalidEmailAddress, Strings.Ok);
                }
                else if (msg.Contains("WEAK_PASSWORD", StringComparison.OrdinalIgnoreCase))
                {
                    await Shell.Current.DisplayAlert(Strings.CreatUserError, "Password is too weak.", Strings.Ok);
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", msg, Strings.Ok);
                }
            }
        }

        public override async void SignInWithEmailAndPasswordAsync(string email, string password, Action<System.Threading.Tasks.Task> OnComplete)
        {
            await facl.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(OnComplete);
        }
        public override string DisplayName
        {
            get
            {
                string dn = string.Empty;
                if (facl.User != null)
                    dn = facl.User.Info.DisplayName;
                return dn;
            }
        }
        public override string UserId
        {
            get
            {
                return facl.User.Uid;
            }
        }
    }
}
