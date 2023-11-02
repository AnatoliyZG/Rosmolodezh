namespace RosMolApp.Pages;

public partial class ProfilePage : ContentPage
{
    public string UserName => General.Name;
    public string UserId => $" #{General.UserId}";

    public string Direction => General.Direction;

    public string Raiting => "9 999 б.";

    public string Patriot => "999 б.";

    public string Dobrovol => "999 б.";

    public string Media => "999 б.";

    public string Predprinimatel => "999 б.";

    public string Creator => "999 б.";

    public string Molodesh => "999 б.";

	public ProfilePage()
	{
        BindingContext = this;
		InitializeComponent();
        LoadInfo();
    }

    public void LoadInfo()
    {
        telephone.Text = General.PhoneNumber;
        borndate.Text = General.BornDate.ToString("dd.MM.yyyy");
        city.Text = General.City;
        vklink.Text = General.VkLink;

        string photo = $"{FileSystem.Current.AppDataDirectory}/ico";
        if (File.Exists(photo))
        ProfileImage.Source = photo;
    }

    private void Exit_Clicked(object sender, EventArgs e)
    {
        General.DeleteAccounteCache();

        Application.Current.MainPage = new AppShell();
    }
}
