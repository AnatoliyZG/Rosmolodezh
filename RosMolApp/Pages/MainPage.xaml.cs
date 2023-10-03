using Microsoft.Maui.Controls.Internals;
using RosMolApp.Pages.Templates;
using RosMolExtension;

namespace RosMolApp.Pages;

public partial class MainPage : ContentPage
{

    private Dictionary<string, FlyoutContentPage> pages = new();

    public MainPage()
    {
        InitializeComponent();
    }

    public async void LoadAnnonces(string key)
    {
        try
        {
            FlyoutContentPage announcePage;

            if(!pages.TryGetValue(key, out announcePage))
            {
                announcePage = new FlyoutContentPage();
                pages.Add(key, announcePage);
            }

            await Navigation.PushAsync(announcePage, true);

            announcePage.Load(General.RequestData<AnnounceData>(new DataRequest(key)), (a) => new BigCard(key, a as AnnounceData)
            {
                action = ExpandCard,
            });

        }
        catch (Exception ex)
        {
            await Navigation.PopAsync();
            await DisplayAlert("Îøèáêà", ex.Message, "Îê");
        }
    }

    public async void ExpandCard(string key, AnnounceData announce)
    {
        var page = new FlyoutContentPage();

        page.AddView(new BigCard(key, announce, true)
        {
            action = async (_, _) =>
            {
                await Navigation.PopAsync(true);
            }
        });

        await Navigation.PushAsync(page, true);
    }

    private async void Goskommol_Clicked(object sender, EventArgs e)
    {
        LoadAnnonces("Announces");
    }

    private async void Wishes_Clicked(object sender, EventArgs e)
    {
        LoadAnnonces("Wishes");
    }

    private async void Options_Clicked(object sender, EventArgs e)
    {
        LoadAnnonces("Options");
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