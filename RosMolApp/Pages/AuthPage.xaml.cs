namespace RosMolApp.Pages;

public partial class AuthPage : ContentPage
{
	private RegistrationPage registrationPage;

	public AuthPage()
	{
		InitializeComponent();

        registrationPage = new RegistrationPage();

    }

    private async void Register_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(registrationPage, true);
    }
}