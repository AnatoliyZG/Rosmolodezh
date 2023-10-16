using RosMolExtension;

namespace RosMolApp.Pages;

public partial class NewsPage : ContentPage
{
    public NewsPage()
    {
        InitializeComponent();

        Load();
    }

    public async void Load()
    {
        LoadingOverlay.ActiveLoading(true);

        try
        {
            var news = await General.RequestData<NewsData>(new DataRequest("News"), true);

            foreach (var item in news)
            {
                ContentView.Add(new BigCard("Новости", "News", item)
                {
                    expand = async (a) =>
                    {
                        if (a == null)
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

        }catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
        }
        finally
        {
            LoadingOverlay.ActiveLoading(false);
        }
    }
}
