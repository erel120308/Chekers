using Chekers.ModelLogic;
using System.Windows.Input;



namespace Chekers.ViewModels
{
    internal class RegisterVM
    {
        private readonly User user = new();
        public ICommand RegisterCommand { get; }

        public bool CanRegister()
        {
            return !string.IsNullOrWhiteSpace(user.Name);
        }

        private void Register()
        {
            user.Register();
        }

        public string Name
        {
            get => user.Name;
            set
            {
                if (user.Name != value)
                {
                    user.Name = value;
                    (RegisterCommand as Command)?.ChangeCanExecute();
                }
            }
        }

        public RegisterVM()
        {
            RegisterCommand = new Command(Register, CanRegister);

        }

    }
}

