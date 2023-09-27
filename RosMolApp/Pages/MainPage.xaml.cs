using Microsoft.Maui.Controls.Internals;
using RosMolApp.Pages.Templates;

namespace RosMolApp.Pages;

public partial class MainPage : ContentPage
{
    private FlyoutContentPage _flyoutContentPage;

	public MainPage()
	{
		InitializeComponent();
        _flyoutContentPage = new FlyoutContentPage();
	}

    private async void Goskommol_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(_flyoutContentPage, true);
    }
}