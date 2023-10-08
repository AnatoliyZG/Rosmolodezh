using RosMolExtension;
using System.Collections;

#pragma warning disable CS4014

namespace RosMolApp.Pages.Templates;

public partial class FlyoutContentPage : ContentPage
{

    public string Title { get; set; }

    public DateTime lastUpdate = DateTime.MinValue;

    public int UpdateRate =
#if DEBUG
        0;
#else
        120;
#endif

    public FlyoutContentPage(string Title)
    {
        this.Title = Title;
        BindingContext = this;
        InitializeComponent();
    }

    public void AddView(View content)
    {
        ContentView.Children.Add(content);
    }

    public async void Load<Data>(Task<Data[]> data, Func<object, View> content) where Data : ReadableData
    {
        try
        {
            if (DateTime.UtcNow.Subtract(lastUpdate).TotalSeconds > UpdateRate)
            {
                LoadingOverlay.ActiveLoading(true);

                var datas = await data;

                ContentView.Children.Clear();

                foreach (var item in datas)
                {
                    ContentView.Children.Add(await Task<View>.Factory.StartNew(content, item));
                }

                lastUpdate = DateTime.UtcNow;
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        finally
        {
            LoadingOverlay.ActiveLoading(false);
        }

    }

    private async void Back_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync(true);
    }
}