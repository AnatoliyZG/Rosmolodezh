namespace RosMolApp.Pages;

public partial class ProfilePage : ContentPage
{
    public string UserId => $"ID: {General.UserId}";
	public ProfilePage()
	{
        BindingContext = this;
		InitializeComponent();
	}

    private void Exit_Clicked(object sender, EventArgs e)
    {
        General.DeleteAccounteCache();

        Application.Current.MainPage = new AppShell();
    }
}
