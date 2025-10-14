using Firebase.Auth;
using Firebase.Auth.Providers;
using Plugin.CloudFirestore;
using Chekers.Models;

namespace Chekers.ModelLogic
{
    class FbData:FbDataModel
    {
        public override async void CreateUserWithEmailAndPasswordAsync(string email, string password, string name, Action<System.Threading.Tasks.Task> OnComplete)
        {
            try
            {
                await facl.CreateUserWithEmailAndPasswordAsync(email, password, name).ContinueWith(OnComplete);
                await Shell.Current.DisplayAlert("Success", "User registered!", "OK");

            }
            catch (Exception ex)
            {
                // כאן אפשר לבדוק אם ההודעה כוללת "EMAIL_EXISTS" או משהו דומה
                if (ex.Message.Contains("EMAIL_EXISTS"))
                {
                    await Shell.Current.DisplayAlert("Registration Failed", "Email already exists.", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
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
