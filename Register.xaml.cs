using Chekers.ViewModel;

namespace Chekers;

public partial class Register : ContentPage
{	
	public Register()
	{
		InitializeComponent();
        BindingContext = new ViewModel.RegisterVM();
    }
}