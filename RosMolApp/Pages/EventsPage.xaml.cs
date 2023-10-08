using RosMolExtension;

namespace RosMolApp.Pages;

public partial class EventsPage : ContentPage
{
	public EventsPage()
	{
		InitializeComponent();

        Load();
	}

    public async void Load()
    {
        LoadingOverlay.ActiveLoading(true);

        try
        {
            var news = await General.RequestData<EventData>(new DataRequest("Events"), true);

            foreach (var item in news)
            {
                ContentView.Add(new BigCard("События", "Events", item)
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

        }finally { 
            LoadingOverlay.ActiveLoading(false);
        }
    }
}