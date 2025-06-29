using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace PinCodeAuth.Features.PinCodeAuthentication.Presentation;

public enum PinAuthenticationStep
{
    EnterPin,
    ReEnterPin
}

/// <summary>
/// ViewModel for two-step PIN creation/setup flow.
/// </summary>
/// <remarks>
/// Testing Error Messages:
/// 1. PIN Mismatch: Enter "1234" → Enter "5678" → Should show "PINs do not match. Please try again." for 2 seconds
/// 2. Format Error: Unlikely with number pad, but should show "PIN must be exactly 4 digits (0-9)" if validation fails
/// 3. Navigation Error: Should show "Navigation failed. Please try again." if Shell navigation fails
/// </remarks>
public partial class PinCodeViewModel : ObservableObject
{
    [ObservableProperty]
    private string _enteredPin = string.Empty;

    [ObservableProperty]
    private string _firstPin = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private PinAuthenticationStep _currentStep = PinAuthenticationStep.EnterPin;

    public ObservableCollection<bool> PinDigits { get; } = [false, false, false, false];

    public string StepTitle => CurrentStep switch
    {
        PinAuthenticationStep.EnterPin => "Create PIN",
        PinAuthenticationStep.ReEnterPin => "Confirm PIN",
        _ => "Enter PIN"
    };

    public string StepDescription => CurrentStep switch
    {
        PinAuthenticationStep.EnterPin => "Please enter your 4-digit PIN",
        PinAuthenticationStep.ReEnterPin => "Please re-enter your PIN to confirm",
        _ => "Please enter your PIN"
    };

    public string LeftButtonText => CurrentStep switch
    {
        PinAuthenticationStep.EnterPin => "Clear",
        PinAuthenticationStep.ReEnterPin => "Back",
        _ => "Clear"
    };

    public IRelayCommand LeftButtonCommand => CurrentStep switch
    {
        PinAuthenticationStep.EnterPin => ClearCommand,
        PinAuthenticationStep.ReEnterPin => GoBackCommand,
        _ => ClearCommand
    };

    [RelayCommand]
    private void AddDigit(string digit)
    {
        if (EnteredPin.Length < 4 && !string.IsNullOrEmpty(digit))
        {
            EnteredPin += digit;
            PinDigits[EnteredPin.Length - 1] = true;

            if (!string.IsNullOrEmpty(ErrorMessage) && EnteredPin.Length == 1)
            {
                ErrorMessage = string.Empty;
            }

            if (EnteredPin.Length == 4)
            {
                _ = ProcessPinAsync();
            }
        }
    }

    [RelayCommand]
    private void RemoveDigit()
    {
        if (EnteredPin.Length <= 0)
        {
            return;
        }

        PinDigits[EnteredPin.Length - 1] = false; // Empty the corresponding dot
        EnteredPin = EnteredPin[..^1];

        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ErrorMessage = string.Empty;
        }
    }

    [RelayCommand]
    private void Clear()
    {
        EnteredPin = string.Empty;
        ResetAllDots();
        ErrorMessage = string.Empty;
    }

    private void ClearPinInput()
    {
        EnteredPin = string.Empty;
        ResetAllDots();
        // Note: ErrorMessage is NOT cleared here
    }

    private void ResetAllDots()
    {
        for (int i = 0; i < 4; i++)
        {
            PinDigits[i] = false;
        }
    }

    private async Task ProcessPinAsync()
    {
        IsLoading = true;
        ErrorMessage = string.Empty;

        try
        {
            if (!IsValidPinFormat(EnteredPin))
            {
                ErrorMessage = "PIN must be exactly 4 digits (0-9)";
                ClearPinInput();
                return;
            }

            switch (CurrentStep)
            {
                case PinAuthenticationStep.EnterPin:
                    await HandleFirstPinEntryAsync();
                    break;
                case PinAuthenticationStep.ReEnterPin:
                    await HandleSecondPinEntryAsync();
                    break;
            }
        }
        catch (Exception)
        {
            ErrorMessage = "An error occurred during authentication";
            ResetToFirstStep();
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task HandleFirstPinEntryAsync()
    {
        FirstPin = EnteredPin;
        CurrentStep = PinAuthenticationStep.ReEnterPin;

        EnteredPin = string.Empty;
        ResetAllDots();

        OnPropertyChanged(nameof(StepTitle));
        OnPropertyChanged(nameof(StepDescription));
        OnPropertyChanged(nameof(LeftButtonText));
        OnPropertyChanged(nameof(LeftButtonCommand));

        await Task.Delay(100);
    }

    private async Task HandleSecondPinEntryAsync()
    {
        if (EnteredPin == FirstPin)
        {
            await NavigateToHomePageAsync();
        }
        else
        {
            ErrorMessage = "PINs do not match. Please try again.";
            ClearPinInput();
            await Task.Delay(2000);
            ResetToFirstStep();
        }
    }

    private async Task NavigateToHomePageAsync()
    {
        try
        {
            await Shell.Current.GoToAsync(state: "//home");
        }
        catch (Exception)
        {
            ErrorMessage = "Navigation failed. Please try again.";
            ResetToFirstStep();
        }
    }

    private void ResetToFirstStep()
    {
        CurrentStep = PinAuthenticationStep.EnterPin;
        FirstPin = string.Empty;
        EnteredPin = string.Empty;
        ResetAllDots();
        ErrorMessage = string.Empty;

        OnPropertyChanged(nameof(StepTitle));
        OnPropertyChanged(nameof(StepDescription));
        OnPropertyChanged(nameof(LeftButtonText));
        OnPropertyChanged(nameof(LeftButtonCommand));
    }

    private static bool IsValidPinFormat(string pin) => !string.IsNullOrWhiteSpace(pin) &&
                                                        pin.Length == 4 &&
                                                        pin.All(char.IsDigit);

    [RelayCommand]
    private void GoBack()
    {
        if (CurrentStep == PinAuthenticationStep.ReEnterPin)
        {
            ResetToFirstStep();
        }
    }

    public void ResetForNewSession()
    {
        CurrentStep = PinAuthenticationStep.EnterPin;
        FirstPin = string.Empty;
        EnteredPin = string.Empty;
        ResetAllDots();
        ErrorMessage = string.Empty;
        IsLoading = false;

        OnPropertyChanged(nameof(StepTitle));
        OnPropertyChanged(nameof(StepDescription));
        OnPropertyChanged(nameof(LeftButtonText));
        OnPropertyChanged(nameof(LeftButtonCommand));
    }
}