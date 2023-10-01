using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
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
        public BigCard(AnnounceData announce)
        {
            Frame fr = new Frame()
            {
                CornerRadius = 25,
                Padding = 0,
                Margin = 5,
                BorderColor = Colors.Transparent,
                Content = new VerticalStackLayout
                {
                    Children =
                    {
                        new Frame(){
                            HeightRequest=170,
                            Padding = 0,
                            BorderColor = Colors.Transparent,
                            CornerRadius = 0,
                            Content = new Image()
                            {
                                HorizontalOptions = LayoutOptions.Fill,
                                Aspect= Aspect.Center,
                                Source = "test_b.png",
                            },
                        },
                        new Label()
                        {
                            Margin=new Thickness(20,15,20,10),
                            FontSize=16,
                            Text = announce.name,
                            FontAttributes = FontAttributes.Bold,
                        },
                        new Label()
                        {
                            Margin=new Thickness(20,0),
                            FontSize=12,
                            Text = announce.summary,
                        },
                        new Button()
                        {
                            Margin=new Thickness(20,15,20,25),
                            Text ="Подробнее",
                        }
                    }
                }
            };

            Content = fr;
        }
    }
}
