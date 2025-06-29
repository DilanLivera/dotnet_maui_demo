using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PinCodeAuth.Domain.Common;
using PinCodeAuth.Domain.ValueObjects;
using PinCodeAuth.Features.PinCodeAuthentication.Application;

namespace PinCodeAuth.Features.PinCodeAuthentication.Presentation;

public partial class PinCodeViewModel : ObservableObject
{
    private readonly AuthenticatePinHandler _handler;

    [ObservableProperty]
    private string _enteredPin = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private bool _isAuthenticated;

    public ObservableCollection<string> PinDigits { get; } = new();

    public PinCodeViewModel(AuthenticatePinHandler handler)
    {
        ArgumentNullException.ThrowIfNull(handler);
        _handler = handler;
    }

    [RelayCommand]
    private void AddDigit(string digit)
    {
        if (EnteredPin.Length < 6 && !string.IsNullOrEmpty(digit))
        {
            EnteredPin += digit;
            PinDigits.Add("•");
            ErrorMessage = string.Empty;

            if (EnteredPin.Length == 4)
            {
                _ = AuthenticateAsync();
            }
        }
    }

    [RelayCommand]
    private void RemoveDigit()
    {
        if (EnteredPin.Length > 0)
        {
            EnteredPin = EnteredPin[..^1];
            PinDigits.RemoveAt(PinDigits.Count - 1);
            ErrorMessage = string.Empty;
        }
    }

    [RelayCommand]
    private void Clear()
    {
        EnteredPin = string.Empty;
        PinDigits.Clear();
        ErrorMessage = string.Empty;
        IsAuthenticated = false;
    }

    private async Task AuthenticateAsync()
    {
        IsLoading = true;
        ErrorMessage = string.Empty;

        try
        {
            PinCode pinCode = PinCode.Create(EnteredPin);
            AuthenticatePinCommand command = new(pinCode);
            Result<bool> result = await _handler.HandleAsync(command);

            if (result.IsFailure)
            {
                ErrorMessage = result.Error.Description;
                Clear();
            }
            else
            {
                IsAuthenticated = true;
                // In a real app, you might navigate to the main app or save authentication state
                await Shell.Current.DisplayAlert("Success", "PIN authenticated successfully!", "OK");
            }
        }
        catch (Exception)
        {
            ErrorMessage = "An error occurred during authentication";
            Clear();
        }
        finally
        {
            IsLoading = false;
        }
    }
}