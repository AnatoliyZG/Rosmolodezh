using RosMolExtension;

namespace RosMolApp.Pages;

public partial class AuthPage : ContentPage
{
	private RegistrationPage registrationPage;

	public AuthPage()
	{
		InitializeComponent();

        CheckPermissions();

        (string, string)? accInfo = General.LoadAccountCache();

        if (accInfo != null)
        {
            Task.Run(async () =>
            {
                try
                {
                    if (await General.LoginAccount(new LoginRequest(accInfo.Value.Item1, accInfo.Value.Item2)))
                    {
                        App.Current.MainPage = new NavigationPage();
                        return;
                    }
                }catch(ResponseExeption ex)
                {
                    if(ex.status == Response.Status.LoginFailed)
                        General.DeleteAccounteCache();
                }
                catch
                {
                    General.DeleteAccounteCache();
                }
            }).Wait();
        }

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
        string _login = login.Text;
        string _pass = password.Text;

        if(await LoginAccount(_login, _pass))
        {
            General.SaveAccountCache(_login, _pass);
        }
    }

    public async Task<bool> LoginAccount(string login, string password)
    {
        loadingScreen.ActiveLoading(true);

        try
        {
            if (await General.LoginAccount(new LoginRequest(login, password)))
            {
                App.Current.MainPage = new NavigationPage();
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (ResponseExeption ex)
        {
            await Shell.Current.DisplayAlert("Ошибка авторизации", ex.DisplayMessage, "OK");
            return false;
        }
        finally
        {
            loadingScreen.ActiveLoading(false);
        }
    }
}
