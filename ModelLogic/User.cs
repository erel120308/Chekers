
using Chekers.Models;
using Chekers.ModelLogic;

namespace Chekers.ModelLogic
{
    internal class User : UserModel
    {
        public override bool TheUser()
        {
            return true;
        }
        public override void Register()
        {
            fdb.CreateUserWithEmailAndPasswordAsync(Email, Password, Name, OnComplete);
        }

        private  void OnComplete(Task task)
        {
            if (task.IsCompletedSuccessfully)
                SaveToPreferences();

            else
                Shell.Current.DisplayAlert(Strings.CreatUserError, task.Exception?.Message, Strings.Ok);
        }

        private void SaveToPreferences()
        {
            Preferences.Set(Keys.NameKey, Name);
            Preferences.Set(Keys.EmailKey, Email);
            Preferences.Set(Keys.PasswordKey, Password);
        }
        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(Email);
        }
        public User()
        {
            Name = Preferences.Get(Keys.NameKey, string.Empty);
            Email = Preferences.Get(Keys.EmailKey, string.Empty);
            Password = Preferences.Get(Keys.PasswordKey, string.Empty);
        }
    }
}
