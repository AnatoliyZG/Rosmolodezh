using RosMolApp.Pages.Templates;

namespace RosMolApp.Pages;

public partial class MainPage : ContentPage
{
    private FlyoutContentPage _flyoutContentPage;

	public MainPage()
	{
		InitializeComponent();
        _flyoutContentPage = new FlyoutContentPage();
        var v = Shell.GetTitleView(this);
        v.BackgroundColor = Colors.Violet;
	}

    private async void Goskommol_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(_flyoutContentPage, true);
    }
}