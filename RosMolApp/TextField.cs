using Microsoft.Maui.Controls.Shapes;
using System;

namespace RosMolApp;

public class TextField : ContentView, IDataField
{
    public static readonly BindableProperty LenghtProperty = BindableProperty.Create(nameof(Lenght), typeof(int), typeof(TextField), 16);

    public int Lenght
    {
        get => (int)GetValue(LenghtProperty);
        set => SetValue(LenghtProperty, value);
    }


    public string Text { get => entry.Text; }

    public Brush StrokeColor { get => border.Stroke; set => border.Stroke = value; }

    public Entry entry;

    private Border border;

    public enum entryType
    {
        Simple,
        Telephone,
    }

    public TextField()
    {
        BindingContext = this;

        entry = new Entry
        {
            Placeholder = "Поле ввода...",
            Keyboard = Keyboard.Email,
            FontSize = 14,
        };

        entry.SetBinding(Entry.MaxLengthProperty, "Lenght");

        border = new Border
        {
            Padding = new Thickness(15, 1),
            Margin = new Thickness(0, 5),
            StrokeShape = new RoundRectangle { CornerRadius = 12 },
            BackgroundColor = Colors.White,
            Stroke = new SolidColorBrush(Colors.White),
            Shadow = new Shadow
            {
                Brush = new SolidColorBrush(Colors.Black),
                Opacity = 0.2f,
                Offset = new Point(0, 2),
                Radius = 4,
            },
            Content = entry,
        };

        Content = border;
    }

    public void ChangeEntryMode(entryType entryType)
    {
        switch (entryType)
        {
            case entryType.Telephone:
                entry.Keyboard = Keyboard.Telephone;
                entry.Text = "+7";
                entry.Completed += (a, b) => entryRefresh();
                entry.Unfocused += (a, b) => entryRefresh();
                entry.MaxLength = 18;

                void entryRefresh()
                {
                    int nums = 0;
                    ulong phone = ulong.Parse(entry.Text.Where(x => char.IsDigit(x)).Skip(1).TakeWhile(a => nums++ < 10).ToArray());

                    string endf = "(###) ###-##-##";
                    int letters = Math.Min(15, nums + (nums > 8 ? 5 : nums > 6 ? 4 : nums > 3 ? 3 : 1));

                    entry.Text = string.Format($"{{0:+7 {endf.Substring(0, letters)}}}", phone);
                }

                entry.TextChanged += (a, b) =>
                {
                    if (!b.NewTextValue.StartsWith("+7"))
                    {
                        entry.Text = b.OldTextValue;
                    }
                };
                return;
        }
    }
}
public class DateField : ContentView, IDataField
{
    public Brush StrokeColor { get; set; } = new SolidColorBrush(Colors.White);

    public DateTime Date => picker.Date;

    private DatePicker picker;

    public DateField()
    {
        picker = new DatePicker()
        {
            VerticalOptions= LayoutOptions.Center,
            TextColor=Colors.Gray,
        };

        picker.DateSelected += (o, e) =>
        {
            picker.TextColor = Colors.Black;
        };

        Content = new Border
        {
            Padding = new Thickness(15, 1),
            Margin = new Thickness(0, 5),
            StrokeShape = new RoundRectangle { CornerRadius = 12 },
            BackgroundColor = Colors.White,
            Stroke = StrokeColor,
            Shadow = new Shadow
            {
                Brush = new SolidColorBrush(Colors.Black),
                Opacity = 0.2f,
                Offset = new Point(0, 2),
                Radius = 4,
            },
            Content = picker,
        };
    }

}

public interface IDataField
{
    public Brush StrokeColor { get; set; }
}
