using Chekers.Models;


namespace Chekers.ModelLogic
{
    class User : UserModel
    {
        public override bool TheUser()
        {
            return true;
        }
        public override void Register()
        {
            Preferences.Set(Keys.NameKey, Name);
        }
        public User()
        {
            Name = Preferences.Get(Keys.NameKey, string.Empty);
        }
    }
}
