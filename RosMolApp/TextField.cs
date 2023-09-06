using Microsoft.Maui.Controls.Shapes;

namespace RosMolApp;

public class TextField : ContentView
{
    public TextField()
    {
        Entry entry = new Entry
        {
            Placeholder = "Поле ввода...",
            FontSize = 14,
            Keyboard = Keyboard.Email,
        };

        Content = new Border
        {
            Padding = new Thickness(15, 1),
            Margin = new Thickness(0, 5),
            StrokeShape = new RoundRectangle { CornerRadius=12},
            Stroke=new SolidColorBrush(Colors.White),
            Shadow = new Shadow
            {
                Brush = new SolidColorBrush(Colors.Black),
                Opacity= 0.2f,
                Offset=new Point(0, 2),
                Radius=4,
            },
            Content = entry,
        };
    }
}
public class DateField : ContentView
{
    public DateField()
    {
        DatePicker picker = new DatePicker();

        picker.DateSelected += (o, e) =>
        {
            picker.TextColor = Colors.Black;
        };

        Content = new Border
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
            Content = picker,
        };
    }
}