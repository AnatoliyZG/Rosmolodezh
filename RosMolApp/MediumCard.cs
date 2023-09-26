using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RosMolApp;

public class MediumCard : ContentView
{
    public static readonly BindableProperty StartColorProperty = BindableProperty.Create(nameof(StartColor), typeof(Color), typeof(MediumCard), Colors.Red);
    public static readonly BindableProperty EndColorProperty = BindableProperty.Create(nameof(EndColor), typeof(Color), typeof(MediumCard), Colors.Red);
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(MediumCard), "");
    public static readonly BindableProperty SummaryProperty = BindableProperty.Create(nameof(Summary), typeof(string), typeof(MediumCard), "");
    public static readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(ImageSource), typeof(MediumCard));
    public static readonly BindableProperty ClickedProperty = BindableProperty.Create(nameof(Clicked), typeof(EventHandler), typeof(MediumCard));

    public event EventHandler Clicked;

    public Color StartColor
    {
        get => (Color)GetValue(StartColorProperty);
        set => SetValue(StartColorProperty, value);
    }

    public Color EndColor
    {
        get => (Color)GetValue(EndColorProperty);
        set => SetValue(EndColorProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string Summary
    {
        get => (string)GetValue(SummaryProperty);
        set => SetValue(SummaryProperty, value);
    }

    public ImageSource Source
    {
        get => (ImageSource)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public MediumCard()
    {
        BindingContext = this;

        Button button = new Button()
        {
            Margin = new Thickness(0, 15, 0, 0),
            BorderWidth = 1.5,
            BorderColor = Colors.White,
            CornerRadius = 8,
            BackgroundColor = Colors.Transparent,
            FontSize = 12,
            TextColor = Colors.White,
            Text = "Подробнее",
        };
        button.Clicked += (a,b) => Clicked.Invoke(a, b);

        var Gradient1 = new GradientStop(StartColor, 0);
        var Gradient2 = new GradientStop(EndColor, 1);

        Gradient1.SetBinding(GradientStop.ColorProperty, "StartColor");
        Gradient2.SetBinding(GradientStop.ColorProperty, "EndColor");

        var HeadLabel = new Label()
        {
            TextColor = Colors.White,
            FontSize = 16,
        };

        HeadLabel.SetBinding(Label.TextProperty, "Text");

        var SummaryLabel = new Label()
        {
            Margin = new Thickness(0, 10, 0, 0),
            TextColor = Colors.White,
            FontSize = 12
        };

        SummaryLabel.SetBinding(Label.TextProperty, "Summary");

        var Ico = new Image()
        {
            HeightRequest = 113,
            VerticalOptions = LayoutOptions.Center,

        };

        Ico.SetBinding(Image.SourceProperty, "Source");

        var grid = new Grid()
        {
            ColumnSpacing = 20,
            ColumnDefinitions =
            {
                new ColumnDefinition(),
                new ColumnDefinition{Width=113}
            },
            Children =
            {
                new VerticalStackLayout()
                {
                    VerticalOptions= LayoutOptions.Start,
                    Children = {
                        HeadLabel,
                        SummaryLabel,
                        button,
                    }
                },
            },
        };

        grid.Add(Ico, 1, 0);

        Border border = new Border()
        {
            StrokeShape = new RoundRectangle { CornerRadius = 25 },

            Background = new LinearGradientBrush(new GradientStopCollection() {
                Gradient1,
                Gradient2,
                }, new Point(0, 1), new Point(1, 0)),

            Stroke = null,
            Shadow = new Shadow
            {
                Brush = new SolidColorBrush(Colors.Black),
                Opacity = .15f,
                Offset = new Point(4, 6),
                Radius = 4,
            },
            Padding = 20,
            Margin=new Thickness(0,3),
            Content = grid,
        };

        Content = border;
    }
}

