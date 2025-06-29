namespace PinCodeAuth.Features.PinCodeAuthentication.Presentation;

public partial class PinCodeView : ContentPage
{
    public PinCodeView(PinCodeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}