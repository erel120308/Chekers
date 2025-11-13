using Chekers.ViewModel;

namespace Chekers
{
    public partial class MainPage : ContentPage
    {
       

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new ViewModel.MainPageVM();
        }

        
    }

}
