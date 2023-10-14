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
        private delegate FlyoutContentPage ExpandAction();

        public static readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(ImageSource), typeof(MediumCard));

        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public Action<FlyoutContentPage> expand;

        private ExpandAction expandAction;

        public BigCard(string title, string key, AnnounceData announce, bool expanded = false, ImageSource source = null)
        {
            BindingContext = this;

            var button = new Button()
            {
                Margin = new Thickness(20, 15, 20, 25),
                Text = expanded ? "Назад" : "Подробнее",
            };

            if (!expanded)
            {
                expandAction = () =>
                {
                    FlyoutContentPage page = new FlyoutContentPage(title);
                    page.AddView(new BigCard(title, key, announce, true, Source)
                    {
                        expand = (a) => expand.Invoke(a),
                        expandAction = () => null,
                    });
                    return page;
                };
            }

            button.Clicked += (_, _) => expand.Invoke(expandAction());

            var image = new Image()
            {
                HorizontalOptions=LayoutOptions.Fill,
                Aspect = Aspect.Fill,
                Background = new LinearGradientBrush(new GradientStopCollection()
                {
                    new GradientStop(new Color(244, 113, 255),0),
                    new GradientStop(new Color(182, 63, 255),1),
                }, new Point(.5, 0), new Point(.5, 1))
            };

            image.SetBinding(Image.SourceProperty, "Source");

            if (source == null)
            {
                LoadImage(key, announce);
            }
            else
            {
                Source = source;
            }

            var view = new VerticalStackLayout
            {
                Children = {
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
                        TextType= TextType.Html,
                        Margin = new Thickness(20, 15, 20, 10),
                        FontSize = 16,
                        Text = announce.name,
                        FontAttributes = FontAttributes.Bold,
                    }
                }
            };

            if (announce is NewsData news)
            {
                var textMargin = new Thickness(3, 0, 8, 0);

                var line = new Grid()
                {
                    Margin = new Thickness(20, 0, 20, 12),
                    HorizontalOptions = LayoutOptions.Fill,
                    ColumnDefinitions = new ColumnDefinitionCollection(
                                                new ColumnDefinition(),
                                                new ColumnDefinition()),
                    Children =
                    {
                        new HorizontalStackLayout(){
                            Children = {
                                new Image()
                                {
                                    Source = "event.svg",
                                    HeightRequest=13,
                                    WidthRequest=13,
                                },
                                new Label()
                                {
                                    Text = news is EventData ? $"{news.startDate?.ToString("d.MM.yyyy")} - {news.endDate?.ToString("dd.MM.yyyy")}"
                                    : $"{news.startDate?.ToString("d.MM.yyyy")}",
                                    FontSize = 12,
                                    TextColor = new Color(122, 122, 122),
                                    Margin = textMargin,
                                },
                                new Image()
                                {
                                    Source = "time.svg",
                                    HeightRequest=13,
                                    WidthRequest=13,
                                },
                                new Label()
                                {
                                    Text = $"{news.startDate?.ToString("HH:mm")}",
                                    FontSize = 12,
                                    TextColor = new Color(122, 122, 122),
                                    Margin = textMargin,
                                },
                            }
                        }
                    }
                };

                if (news is EventData eventData)
                {
                    line.Add(new HorizontalStackLayout()
                    {
                        HorizontalOptions = LayoutOptions.End,
                        Children =
                        {
                            new Image()
                            {
                                Source = "star.svg",
                                HeightRequest = 13,
                                WidthRequest = 13,
                            },
                            new Label()
                            {
                                Text = $"Баллы: {eventData.score}",
                                FontSize = 12,
                                Margin = textMargin,
                            }
                        }
                    }, 1, 0);
                }

                view.Add(line);
            }

            string smr =
#if IOS
                expanded ? announce.description : announce.summary;
#else
                expanded ? announce.description.Replace("</P>", "</P><BR>").Replace("<LI>", "<LI>\t") : announce.summary.Replace("</P>", "</P><BR>").Replace("<LI>", "<LI>\t");
#endif
            if (smr.EndsWith("<BR>"))
            {
                smr = smr.Remove(smr.Length - 4);
            }

            view.Add(new Label()
            {
                TextType= TextType.Html,
                Margin = new Thickness(20, 0),
                LineHeight=1.2,
                FontSize = 12,
                Text = smr,
            });

            view.Add(button);

            var MainContent = new Frame()
            {
                IsClippedToBounds=true,
                CornerRadius = 25,
                Padding = 0,
                Margin = 5,
                BorderColor = Colors.Transparent,
                Content = view,
            };

            Content = MainContent;
        }

        public async void LoadImage(string key, AnnounceData announce)
        {
            Source = await General.RequestImage(new PhotoRequest(key, announce.id.ToString()));
        }
    }
}
