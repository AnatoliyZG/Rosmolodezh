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
    public class EventCard : Frame
    {

        public EventCard(EventData EventData)
        {
            Padding = 0;
            CornerRadius = 25;


            var main = new Grid()
            {
                RowDefinitions = new RowDefinitionCollection(
                                            new RowDefinition(52),
                                            new RowDefinition()),
            };

            var header = new Grid()
            {
                Margin = new Thickness(20, 0),
                ColumnDefinitions = new ColumnDefinitionCollection(
                                                new ColumnDefinition(),
                                                new ColumnDefinition()),
            };

            header.Add(new Label()
            {
                Text = "Участвую:",
                FontSize = 14,
                TextColor = Colors.White,
                VerticalOptions = LayoutOptions.Center,
            }, 0, 0);
            header.Add(new Label()
            {
                Text = "Направление:",
                FontSize = 14,
                TextColor = Colors.White,
                HorizontalTextAlignment = TextAlignment.End,
                VerticalOptions = LayoutOptions.Center,
            }, 1, 0);

            var info = new Grid()
            {
                Margin = new Thickness(0, 15),
                ColumnDefinitions = new ColumnDefinitionCollection(
                                            new ColumnDefinition(), new ColumnDefinition()),
                RowDefinitions = new RowDefinitionCollection(new RowDefinition(), new RowDefinition(60)),

                Children =
                {
                    new HorizontalStackLayout(){
                        new Image()
                        {
                            Source="event",
                            HeightRequest=13,
                            WidthRequest=13,
                            Margin=new Thickness(0,0,5,0),
                        },
                        new Label()
                        {
                            Text = $"{EventData.startDate?.ToString("d.MM.yyyy")} - {EventData.endDate?.ToString("dd.MM.yyyy")}",
                            FontSize = 12,
                            TextColor = new Color(122, 122, 122),
                            Margin = new Thickness(3, 0, 8, 0),
                        },
                        new Image()
                        {
                            Source = "time",
                            HeightRequest=13,
                            WidthRequest=13,
                        },
                        new Label()
                        {
                            Text = $"{EventData.startDate?.ToString("HH:mm")}",
                            FontSize = 12,
                            TextColor = new Color(122, 122, 122),
                            Margin = new Thickness(3, 0, 8, 0),
                        },
                    },
                },
            };
            info.Add(new HorizontalStackLayout()
            {
                HorizontalOptions = LayoutOptions.End,
                Children =
                {
                    new Image()
                    {
                        Source = "star",
                        HeightRequest = 13,
                        WidthRequest = 13,
                    },
                    new Label()
                    {
                        Text = $"Баллы: {EventData.score}",
                        FontSize = 12,
                        Margin = new Thickness(3, 0, 8, 0),
                    }
                }
            }, 1, 0);

            info.Add(new Button()
            {
                Text = "Отказаться",
                Margin = new Thickness(0, 15, 5, 0),
                BorderColor = Colors.Red,
            }, 0, 1);

            info.Add(new Button()
            {
                Text = "Подробнее",
                Margin = new Thickness(5, 15, 0, 0),
            }, 1, 1);

            var body = new VerticalStackLayout()
            {
                Margin = new Thickness(20, 15, 20, 25),
                Children =
                {
                    new Label()
                    {
                        FontSize=16,
                        FontAttributes = FontAttributes.Bold,
                        Text="Крымский патриотический форум \r\n\"Родина в сердце\"",
                    },
                    info,
                },
            };


            main.Add(new BoxView()
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                Color = Colors.LimeGreen,
            }, 0, 0);

            main.Add(header, 0, 0);


            main.Add(body, 0, 1);

            Content = main;
        }
    }
}
