using PinCodeAuth.Domain.ValueObjects;

namespace PinCodeAuth.Features.PinCodeAuthentication.Application;

public sealed record AuthenticatePinCommand(PinCode PinCode);