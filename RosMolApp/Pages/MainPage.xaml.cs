using Microsoft.Maui.Controls.Internals;
using RosMolApp.Pages.Templates;
using RosMolExtension;

namespace RosMolApp.Pages;

public partial class MainPage : ContentPage
{

    private FlyoutContentPage announcePage = new FlyoutContentPage();

    public MainPage()
	{
		InitializeComponent();
	}

    public async void LoadAnnonces(string key)
    {
        try
        {
            await Navigation.PushAsync(announcePage, true);

            announcePage.Load(General.RequestData<AnnounceData>(new DataRequest("", key)), (a) => new BigCard(a as AnnounceData));

        }catch (Exception ex)
        {
            await Navigation.PopAsync();
            await DisplayAlert("Œ¯Ë·Í‡", ex.Message, "ŒÍ");
        }
    }

    private async void Goskommol_Clicked(object sender, EventArgs e)
    {
        LoadAnnonces("Announces");
    }

    public async void OpenVk(object sender, EventArgs e)
    {
        await Browser.Default.OpenAsync("https://vk.com");
    }
    public async void OpenYandex(object sender, EventArgs e)
    {
        await Browser.Default.OpenAsync("https://yandex.ru");
    }
    public async void OpenTelegram(object sender, EventArgs e)
    {
        await Browser.Default.OpenAsync("https://t.me/telegram");
    }
}