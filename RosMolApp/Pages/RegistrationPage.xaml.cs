namespace RosMolApp.Pages;

public partial class RegistrationPage : ContentPage
{
    private string errorText = null;

    public RegistrationPage()
    {
        InitializeComponent();
    }

    private async void Registration_Clicked(object sender, EventArgs e)
    {
        if (!CheckField(LoginField, "Логин", 6) | !CheckField(PassField, "Пароль", 6) | !CheckField(NameField, "Имя", 2))
        {
            DisplayError();
            return;
        }
        try
        {
            if (await General.Register(NameField.Text, LoginField.Text, PassField.Text, PhoneField.Text))
            {
                await Navigation.PopModalAsync(true);

                App.Current.MainPage = new NavigationPage();
            }
        }
        catch (ResponseExeption ex)
        {
            DisplayError(ex.Message);
        }
    }

    private bool CheckField(TextField field, string fieldName, int minLength = 3)
    {
        if (field.Text == null || field.Text.Length < minLength)
        {
            field.StrokeColor = new SolidColorBrush(Colors.Red);

            if (errorText == null)
            {
                errorText = $"Поле \"{fieldName}\" должно содержать {minLength} или более символов!";
            }

            return false;
        }

        field.StrokeColor = new SolidColorBrush(Colors.White);

        return true;
    }

    private void DisplayError(string errorText)
    {
        this.errorText = errorText;
        DisplayError();
    }

    private async void DisplayError()
    {
        if (errorText != null)
        {
            await DisplayAlert("Ошибка", errorText, "Ок");
        }
        errorText = null;
    }
}