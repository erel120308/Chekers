using Chekers.ModelLogic;
namespace Chekers.Models
{
    abstract class UserModel
    {
        protected FbData fdb = new();

        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public abstract bool TheUser();
        public abstract bool IsValid();
        public string Email { get; set; } = string.Empty;
        public bool IsRegistered => !string.IsNullOrWhiteSpace(Name);
        public string Name { get; set; } = string.Empty;
        public abstract void Register();
    }
}
