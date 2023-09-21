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
		if(!CheckField(LoginField, "�����", 6) | !CheckField(PassField, "������", 6) | !CheckField(NameField, "���", 2))
		{
            DisplayError();
			return;
		}

		if(General.Register(NameField.Text, LoginField.Text, PassField.Text))
		{
            await Navigation.PopModalAsync(true);

            App.Current.MainPage = new NavigationPage();
        }
    }

	private bool CheckField(TextField field, string fieldName, int minLength = 3)
	{
		if(field.Text == null || field.Text.Length < minLength)
		{
            field.StrokeColor = new SolidColorBrush(Colors.Red);

			if(errorText == null)
			{
				errorText = $"���� \"{fieldName}\" ������ ��������� {minLength} ��� ����� ��������!";
            }

            return false;
		}

		field.StrokeColor = new SolidColorBrush(Colors.White);

        return true;
	}

	private async void DisplayError()
	{
		if (errorText != null)
		{
			await DisplayAlert("������", errorText, "��");
		}
		errorText = null;
	}
}