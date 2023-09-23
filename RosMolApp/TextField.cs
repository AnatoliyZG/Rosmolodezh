using Microsoft.Maui.Controls.Shapes;

namespace RosMolApp;

public class TextField : ContentView, IDataField
{
    public string Text { get => entry.Text; }

    public Brush StrokeColor { get => border.Stroke; set => border.Stroke = value; }

    private Entry entry;

    private Border border;

    public TextField()
    {
        entry = new Entry
        {
            Placeholder = "Поле ввода...",
            FontSize = 14,
            Keyboard = Keyboard.Email,
        };

        border = new Border
        {
            Padding = new Thickness(15, 1),
            Margin = new Thickness(0, 5),
            StrokeShape = new RoundRectangle { CornerRadius = 12 },
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
}
public class DateField : ContentView, IDataField
{
    public Brush StrokeColor { get; set; } = new SolidColorBrush(Colors.White);

    public DateTime Date => picker.Date;

    private DatePicker picker;

    public DateField()
    {
        picker = new DatePicker();

        picker.DateSelected += (o, e) =>
        {
            picker.TextColor = Colors.Black;
        };

        Content = new Border
        {
            Padding = new Thickness(15, 1),
            Margin = new Thickness(0, 5),
            StrokeShape = new RoundRectangle { CornerRadius = 12 },
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