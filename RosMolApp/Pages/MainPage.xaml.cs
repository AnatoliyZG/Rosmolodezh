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

    public async void LoadAnnonces(string title, string key)
    {
        PermissionStatus status = await Permissions.RequestAsync<Permissions.StorageWrite>();

        if(status != PermissionStatus.Granted)
        {
            return;
        }

        try
        {
            FlyoutContentPage announcePage;

            if(!pages.TryGetValue(key, out announcePage))
            {
                announcePage = new FlyoutContentPage(title);
                pages.Add(key, announcePage);
            }

            await Navigation.PushAsync(announcePage, true);

            announcePage.Load(General.RequestData<AnnounceData>(new DataRequest(key)), (a) => new BigCard(key, a as AnnounceData)
            {
                action = (a,b) => ExpandCard(title, a, b),
            });

        }
        catch (Exception ex)
        {
            await Navigation.PopAsync();
            await DisplayAlert("Ошибка", ex.Message, "Ок");
        }
    }

    public async void ExpandCard(string title, string key, AnnounceData announce)
    {
        var page = new FlyoutContentPage(title);

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
        LoadAnnonces("Госкоммол", "Announces");
    }

    private async void Wishes_Clicked(object sender, EventArgs e)
    {
        LoadAnnonces("Куда я хочу?", "Wishes");
    }

    private async void Options_Clicked(object sender, EventArgs e)
    {
        LoadAnnonces("Возможности", "Options");
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