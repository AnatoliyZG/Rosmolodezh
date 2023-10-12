using RosMolExtension;

namespace RosMolApp.Pages;

public partial class AuthPage : ContentPage
{
	private RegistrationPage registrationPage;

	public AuthPage()
	{
		InitializeComponent();

        CheckPermissions();

        registrationPage = new RegistrationPage();

    }

    private async void CheckPermissions()
    {
        PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.NetworkState>();

        if(status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.NetworkState>();

            if (status != PermissionStatus.Granted)
            {
#if IOS
                await Shell.Current.DisplayAlert("Ошибка разрешения", "Для использования приложения необходимо предоставить доступ к интернет соединению в настройках приложений.", "Ok");
#endif
            }
        }
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
