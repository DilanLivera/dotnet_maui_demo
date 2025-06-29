using PinCodeAuth.Domain.Common;

namespace PinCodeAuth.Features.PinCodeAuthentication.Application;

public sealed class AuthenticatePinHandler
{
    private readonly string _correctPin = "1234"; // In a real app, this would come from secure storage

    public Task<Result<bool>> HandleAsync(AuthenticatePinCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (!command.PinCode.IsValid)
        {
            return Task.FromResult(Result<bool>.Failure(
                                                        Error.Validation("InvalidPin", "PIN code is not valid")));
        }

        bool isAuthenticated = command.PinCode.Value == _correctPin;

        if (!isAuthenticated)
        {
            return Task.FromResult(Result<bool>.Failure(
                                                        Error.Validation("IncorrectPin", "PIN code is incorrect")));
        }

        return Task.FromResult(Result<bool>.Success(true));
    }
}