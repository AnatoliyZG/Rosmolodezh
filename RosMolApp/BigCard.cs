using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using RosMolApp.Pages;
using RosMolApp.Pages.Templates;
using RosMolExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RosMolApp
{
    public class BigCard : ContentView
    {
        public static readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(ImageSource), typeof(MediumCard));

        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public Action<string, AnnounceData> action;


        public BigCard(string key, AnnounceData announce, bool expanded = false)
        {
            BindingContext = this;

            var button = new Button()
            {
                Margin = new Thickness(20, 15, 20, 25),
                Text = expanded ? "Назад" : "Подробнее",
            };

            button.Clicked += (_, _) => action.Invoke(key, announce);

            var image = new Image()
            {
                HorizontalOptions = LayoutOptions.Fill,
                Aspect = Aspect.Center,
                Background = new LinearGradientBrush(new GradientStopCollection()
                {
                    new GradientStop(new Color(244, 113, 255),0),
                    new GradientStop(new Color(182, 63, 255),1),
                }, new Point(.5, 0), new Point(.5, 1))
            };

            image.SetBinding(Image.SourceProperty, "Source");

            LoadImage(key,announce);

            var MainContent = new Frame()
            {
                CornerRadius = 25,
                Padding = 0,
                Margin = 5,
                BorderColor = Colors.Transparent,
                Content = new VerticalStackLayout
                {
                    Children =
                    {
                        new Frame()
                        {
                            HeightRequest = 170,
                            Padding = 0,
                            BorderColor = Colors.Transparent,
                            CornerRadius = 0,
                            Content = image,
                        },
                        new Label()
                        {
                            Margin = new Thickness(20, 15, 20, 10),
                            FontSize = 16,
                            Text = announce.name,
                            FontAttributes = FontAttributes.Bold,
                        },
                        new Label()
                        {
                            Margin=new Thickness(20,0),
                            FontSize=12,
                            Text = expanded ? announce.description : announce.summary,
                        },
                        button,
                    }
                }
            };

            Content = MainContent;
        }

        public async void LoadImage(string key, AnnounceData announce)
        {
            Source = await General.RequestImage(new PhotoRequest(key, announce.name));
        }
    }
}
