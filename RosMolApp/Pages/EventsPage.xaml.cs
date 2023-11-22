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

    private void Direct_open(object sender, EventArgs e)
    {
        Direction.IsVisible = true;
        var anim = new Animation(a => Direction.Opacity = a, 0, 1);
        anim.Commit(this, "SimpleAnimation", 15, 200, Easing.Linear);
    }

    private void Direct_close(object sender, EventArgs e)
    {
        var anim = new Animation(a => Direction.Opacity = a, 1, 0);
        anim.Commit(this, "SimpleAnimation", 15, 200, Easing.Linear, (a,b) => Direction.IsVisible = false);
    }
}
