# PIN Code Authentication Template

This template provides a complete PIN code authentication system for .NET MAUI applications, following Domain-Driven Design (DDD) and Vertical Slice Architecture patterns.

## Features

- **Two-Step Authentication**: Create PIN, then confirm by re-entering
- **Modern UI**: Clean, responsive PIN entry interface with 4-dot visual feedback
- **Smart Navigation**: Automatic flow between entry steps and navigation to home page
- **Error Handling**: Clear error messages for mismatched PINs or invalid input
- **Cross-Platform**: Works on iOS, Android, macOS, and Windows
- **MVVM Pattern**: Uses CommunityToolkit.Mvvm for clean data binding
- **Dynamic UI**: Context-aware buttons (Clear/Back) based on current step
- **Professional PIN Dots**: 4-dot display with proper empty/filled states (circle borders with blue fill)

## Architecture

### Domain Layer (`Domain/`)

- **PinCode Value Object**: Encapsulates PIN validation logic
- **Result Pattern**: Type-safe error handling without exceptions
- **Error Types**: Structured error handling with codes and descriptions

### Application Layer (`Features/.../Application/`)

- **Commands**: Data transfer objects for operations
- **Handlers**: Business logic implementation
- **Authentication Logic**: PIN validation and verification

### Presentation Layer (`Features/.../Presentation/`)

- **ViewModels**: UI state management using MVVM
- **Views**: XAML-based user interface
- **Commands**: User interaction handling

## Usage

### Two-Step Authentication Flow

1. **Create PIN**: User enters a 4-digit PIN
2. **Confirm PIN**: User re-enters the same PIN to confirm
3. **Success**: If PINs match, user is redirected to the home page
4. **Error Handling**: If PINs don't match, clear error message is displayed and the process resets

### Navigation

- **PIN → Home**: Automatic redirect after successful PIN confirmation
- **Home → PIN**: Sign out button returns to PIN authentication with fresh state
- **Step Navigation**: Back button allows return to first step during confirmation
- **Auto-Reset**: PIN page automatically resets when navigated to (clears previous state)

### Customization

#### Modify PIN Length

To change from 4-digit to a different length, update these locations:

1. **ViewModel validation** in `PinCodeViewModel.cs`:
```csharp
if (EnteredPin.Length < 6) // Change from 4 to desired length
pin.Length == 6 && // Update validation
```

2. **Value object validation** in `PinCode.cs`:
```csharp
Debug.Assert(value.Length >= 4 && value.Length <= 8, "PinCode must be between 4 and 8 digits");
```

#### Customize UI

- **Colors**: Modify `Resources/Styles/Colors.xaml`
- **Button Styles**: Update `NumberButtonStyle` and `ActionButtonStyle` in `Resources/Styles/Styles.xaml`
- **Layout**: Edit `PinCodeView.xaml` for different arrangements

#### Add Biometric Support

Extend the authentication flow in `PinCodeViewModel` to include fingerprint/face recognition as an alternative authentication method before PIN entry.

## Components

### Core Files

- `PinCode.cs` - Value object with validation
- `PinCodeViewModel.cs` - Two-step authentication logic and UI state management
- `PinCodeView.xaml` - Dynamic user interface with step-aware controls
- `HomePage.xaml` - Success page after authentication
- `Result.cs` - Error handling pattern

### Supporting Files

- Built-in Community Toolkit converters for XAML data binding
- Custom button styles for number pad
- Dependency injection setup in `MauiProgram.cs`

## Security Considerations

⚠️ **Production Notes**:

- Store actual PINs securely (encrypted database, secure keychain)
- Implement attempt limiting and lockout mechanisms
- Add PIN complexity requirements
- Consider adding PIN expiration
- Implement secure PIN reset functionality

## Dependencies

- .NET 8.0
- .NET MAUI
- CommunityToolkit.Mvvm 8.2.2
- CommunityToolkit.Maui 7.0.1

## Build & Run

```bash
dotnet build
dotnet run
```

The application will launch with the PIN authentication screen ready for testing.
