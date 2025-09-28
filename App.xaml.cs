namespace Chekers
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
            Register = new NavigationPage(new Register());

            MainPage = new AppShell();
        }
    }
}
