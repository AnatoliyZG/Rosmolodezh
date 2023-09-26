using System.Diagnostics;
using RosMolExtension;

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
        if (!CheckField(LoginField, "�����", 6) | !CheckField(PassField, "������", 6) | !CheckField(NameField, "���", 2))
        {
            DisplayError();
            return;
        }

        LoadingOverlay.ActiveLoading(true);

        try
        {
            RegisterRequest request = new RegisterRequest(LoginField.Text, PassField.Text)
            {
                city = CityField.Text,
                bornDate = BornField.Date,
                direction = DirectionField.Text,
                name = NameField.Text,
                vkLink = VkField.Text,
                phone = PhoneField.Text,
            };

            if (await General.Register(request))
            {
                await Navigation.PopModalAsync(true);

                App.Current.MainPage = new NavigationPage();
            }
        }
        catch (ResponseExeption ex)
        {
            DisplayError(ex.Message);
        }

        LoadingOverlay.ActiveLoading(false);
    }

    private bool CheckField(TextField field, string fieldName, int minLength = 3)
    {
        if (field.Text == null || field.Text.Length < minLength)
        {
            field.StrokeColor = new SolidColorBrush(Colors.Red);

            if (errorText == null)
            {
                errorText = $"���� \"{fieldName}\" ������ ��������� {minLength} ��� ����� ��������!";
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
            await DisplayAlert("������", errorText, "��");
        }
        errorText = null;
    }

    private async void LoadPhoto_Clicked(object sender, EventArgs e)
    {
        PermissionStatus status = await Permissions.RequestAsync<Permissions.StorageRead>();

        if (status != PermissionStatus.Granted)
        {
            return;
        }

        var picker = await FilePicker.Default.PickAsync(new PickOptions()
        {
            PickerTitle="����� ���� �������",
            FileTypes = FilePickerFileType.Images
        });
        var imageSource = picker.FullPath.ToString();
        Console.WriteLine("IMAGE: " + imageSource);
        ProfileImage.Source = imageSource;

        ProfileStandart.IsVisible = false;
    }
}