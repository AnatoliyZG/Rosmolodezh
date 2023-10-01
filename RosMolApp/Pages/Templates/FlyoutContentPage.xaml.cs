using Android.OS;
using RosMolExtension;
using System.Collections;

#pragma warning disable CS4014

namespace RosMolApp.Pages.Templates;

public partial class FlyoutContentPage : ContentPage
{

	public FlyoutContentPage()
	{
		InitializeComponent();
	}

    public async void Load<Data>(Task<Data[]> data, Func<object, View> content) where Data : ReadableData
    {
        try
        {
            ContentView.Children.Clear();

            LoadingOverlay.ActiveLoading(true);

            var datas = await data;


            foreach (var item in datas)
            {
                ContentView.Children.Add(await Task<View>.Factory.StartNew(content, item));
            }

            LoadingOverlay.ActiveLoading(false);

        }catch(Exception ex) { 
            Console.WriteLine(ex.ToString());

            LoadingOverlay.ActiveLoading(false);
        }

    }

    private async void Back_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync(true);
    }
}