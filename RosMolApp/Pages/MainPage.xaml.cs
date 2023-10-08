using Microsoft.Maui.Controls.Internals;
using RosMolApp.Pages.Templates;
using RosMolExtension;
using System.Diagnostics;

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
        try
        {
            FlyoutContentPage announcePage;

            if(!pages.TryGetValue(key, out announcePage))
            {
                announcePage = new FlyoutContentPage(title);
                pages.Add(key, announcePage);
            }

            await Navigation.PushAsync(announcePage, true);

            announcePage.Load(General.RequestData<AnnounceData>(new DataRequest(key)), (a) => new BigCard(title, key, a as AnnounceData)
            {
                expand = async (a) =>
                {
                    if(a == null)
                    {
                        await Navigation.PopAsync(true);
                    }
                    else
                    {
                        await Navigation.PushAsync(a, true);
                    }
                }
            });

        }
        catch (Exception ex)
        {
            await Navigation.PopAsync();
            await DisplayAlert("Ошибка", ex.Message, "Ок");
        }
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