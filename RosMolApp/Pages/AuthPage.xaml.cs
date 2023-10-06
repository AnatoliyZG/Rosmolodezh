using RosMolExtension;

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

    private async void Login_Clicked(object sender, EventArgs e)
    {
        if (await General.LoginAccount(new LoginRequest(login.Text, password.Text)))
        {
           // await Navigation.PopModalAsync(true);

            App.Current.MainPage = new NavigationPage();

            /*
            if (profilePhoto != null)
            {
                string format = profilePhoto.Split('.')[^1];

                File.Copy(profilePhoto, $"{FileSystem.Current.AppDataDirectory}/ico.{format}", true);
            }
            */
        }
    }
}