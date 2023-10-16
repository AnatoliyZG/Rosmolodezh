using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Input;
using RosMolExtension;

namespace RosMolApp.Pages;

public partial class RegistrationPage : ContentPage
{
    private string errorText = null;
    private string profilePhoto { get => General.Photo; set => General.Photo = value; } 

    public RegistrationPage()
    {
        InitializeComponent();
        PhoneField.ChangeEntryMode(TextField.entryType.Telephone);
    }

    private async void Registration_Clicked(object sender, EventArgs e)
    {
        if (!AcceptTerms.IsChecked)
        {
            errorText = "Принятие согласия на обработку персональных данных является обязательным условием для пользования приложением";
            DisplayError();
            return;
        }

        if (!CheckField(LoginField, "Логин", 6) | !CheckField(PassField, "Пароль", 6) | !CheckField(NameField, "Имя", 2) | !CheckField(PhoneField, "Телефон", 18))
        {
            DisplayError();
            return;
        }

        LoadingOverlay.ActiveLoading(true);

        try
        {
            string _login = LoginField.Text;
            string _pass = PassField.Text;

            RegisterRequest request = new RegisterRequest(_login, _pass)
            {
                city = CityField.Text,
                bornDate = BornField.Date,
                direction = DirectionField.Text,
                name = NameField.Text,
                vkLink = VkField.Text,
                phone = PhoneField.Text,
                photo = profilePhoto == null ? null : File.ReadAllBytes(profilePhoto),
            };

            if (await General.Register(request))
            {
                App.Current.MainPage = new NavigationPage();

                if (profilePhoto != null)
                {
                    string format = profilePhoto.Split('.')[^1];

                    File.Copy(profilePhoto, $"{FileSystem.Current.AppDataDirectory}/ico.{format}", true);
                }

                General.SaveAccountCache(_login, _pass);
            }
        }
        catch (ResponseExeption ex)
        {
            DisplayError(ex.DisplayMessage);
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

    private async void LoadPhoto_Clicked(object sender, EventArgs e)
    {
        PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.StorageRead>();

            if (status != PermissionStatus.Granted)
            {
#if IOS
                await Shell.Current.DisplayAlert("Ошибка разрешения", "Для использования данной функции предоставтье приложению доступ к хранилищу устройства в настройках приложений.", "Ok");
#endif
                return;
            }
        }
        var picker = await MediaPicker.Default.PickPhotoAsync(new MediaPickerOptions()
        {
            Title = "Выбор фото профиля",
        });

        if (picker == null)
            return;

        profilePhoto = picker.FullPath;
        ProfileImage.Source = profilePhoto;
        }
}
