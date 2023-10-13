namespace RosMolApp.Pages;

public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
		InitializeComponent();
	}

    private void Exit_Clicked(object sender, EventArgs e)
    {
        General.DeleteAccounteCache();

        Application.Current.MainPage = new AppShell();
    }
}
