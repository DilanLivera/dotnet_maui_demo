namespace PinCodeAuth;

public partial class HomePage : ContentPage
{
    public HomePage() => InitializeComponent();

    private async void OnMainAppClicked(object sender, EventArgs e) => await DisplayAlert(title: "Navigation", message: "This would navigate to your main app content", "OK");

    private async void OnSignOutClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync(state: "//pincode");
}