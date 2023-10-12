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
        await Navigation.PushAsync(registrationPage, true);
    }

    private async void Login_Clicked(object sender, EventArgs e)
    {
        try
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
        }catch(ResponseExeption ex)
        {
            await Shell.Current.DisplayAlert("Ошибка авторизации", ex.Message, "OK");
        }
    }
}
