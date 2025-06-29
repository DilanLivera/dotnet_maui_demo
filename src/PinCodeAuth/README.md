# PIN Code Authentication Template

This template provides a complete PIN code authentication system for .NET MAUI applications, following Domain-Driven Design (DDD) and Vertical Slice Architecture patterns.

## Features

- **Modern UI**: Clean, responsive PIN entry interface with visual feedback
- **Secure Architecture**: Domain-driven design with proper separation of concerns
- **Error Handling**: Comprehensive error handling using the Result pattern
- **Cross-Platform**: Works on iOS, Android, macOS, and Windows
- **MVVM Pattern**: Uses CommunityToolkit.Mvvm for clean data binding
- **Validation**: Robust PIN validation with clear user feedback

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

### Default PIN

The template comes with a default PIN: **1234**

### Integration

1. The PIN authentication page is automatically set as the main page
2. Upon successful authentication, users see a success message
3. Failed attempts show clear error messages
4. The interface automatically clears after failed attempts

### Customization

#### Change the PIN

Update the `_correctPin` field in `AuthenticatePinHandler.cs`:

```csharp
private readonly string _correctPin = "your-new-pin";
```

#### Modify PIN Length

Adjust validation in `PinCode.cs`:

```csharp
Debug.Assert(value.Length >= 4 && value.Length <= 8, "PinCode must be between 4 and 8 digits");
```

#### Customize UI

- **Colors**: Modify `Resources/Styles/Colors.xaml`
- **Button Styles**: Update `NumberButtonStyle` and `ActionButtonStyle` in `Resources/Styles/Styles.xaml`
- **Layout**: Edit `PinCodeView.xaml` for different arrangements

#### Add Biometric Support

Extend the `AuthenticatePinHandler` to include fingerprint/face recognition as an alternative authentication method.

## Components

### Core Files

- `PinCode.cs` - Value object with validation
- `AuthenticatePinHandler.cs` - Authentication business logic
- `PinCodeViewModel.cs` - UI state management
- `PinCodeView.xaml` - User interface
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
