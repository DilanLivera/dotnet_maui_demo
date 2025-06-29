using System.Diagnostics;

namespace PinCodeAuth.Domain.ValueObjects;

public sealed record PinCode(string Value)
{
    public static PinCode Create(string value)
    {
        Debug.Assert(!string.IsNullOrWhiteSpace(value), "PinCode cannot be null or empty");
        Debug.Assert(value.Length >= 4 && value.Length <= 8, "PinCode must be between 4 and 8 digits");
        Debug.Assert(value.All(char.IsDigit), "PinCode must contain only digits");

        return new PinCode(value.Trim());
    }

    public bool IsValid => !string.IsNullOrWhiteSpace(Value) &&
                           Value.Length >= 4 &&
                           Value.Length <= 8 &&
                           Value.All(char.IsDigit);
}