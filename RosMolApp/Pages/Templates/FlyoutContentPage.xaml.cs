namespace RosMolApp.Pages.Templates;

public partial class FlyoutContentPage : ContentPage
{
	public FlyoutContentPage()
	{
		InitializeComponent();
	}

    private async void Back_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync(true);
    }
}