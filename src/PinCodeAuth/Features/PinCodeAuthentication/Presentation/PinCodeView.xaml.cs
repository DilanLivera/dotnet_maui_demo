namespace PinCodeAuth.Features.PinCodeAuthentication.Presentation;

public partial class PinCodeView : ContentPage
{
    private readonly PinCodeViewModel _viewModel;

    public PinCodeView(PinCodeViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _viewModel.ResetForNewSession();
    }
}