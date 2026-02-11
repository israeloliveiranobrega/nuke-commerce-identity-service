using Microsoft.AspNetCore.Mvc.Infrastructure;
using NukeAuthentication.Scr.Shared.Exceptions;
using NukeAuthentication.Scr.Shared.ExtensionMethods;
using static NukeAuthentication.Scr.Shared.ExtensionMethods.StringExtensionHelper;

namespace NukeAuthentication.Scr.Domain.ValueObjects.Base;

public record Phone
{
    public string CountryCode { get; init; }
    public string Number { get; init; }

    public ulong FullPhone => ulong.Parse($"{CountryCode}{Number}");
    public string UnformattedNumber => $"{CountryCode}{Number}";
    public string FormattedNumber => $"({CountryCode}) {UnformattedNumber[2]} {UnformattedNumber[3..7]}-{UnformattedNumber[7..11]}";
    public string MaskedPhone => $"({CountryCode}) * ****-**{FormattedNumber[^2..]}";

    private Phone() { }

    public Phone(string countryCode, string number)
    {
        ValidCountryCode(countryCode);
        ValidNumber(number);

        CountryCode = countryCode;
        Number = number;
    }

    private static void ValidCountryCode(string countryCode)
    {
        if (!countryCode.HasContent())
            throw new ArgumentNullException(nameof(countryCode));

        if (!countryCode.IsOnlyLettersOrNumbers(CheckType.OnlyNumbers))
            throw new InvalidPhoneCountryCodeFormatExceptions();

        if (!countryCode.HasLength(2))
            throw new InvalidPhoneCountryCodeLengthExceptions();
    }
    private static void ValidNumber(string number)
    {
        if (!number.HasContent())
            throw new ArgumentNullException(nameof(number));

        if (!number.IsOnlyLettersOrNumbers(CheckType.OnlyNumbers))
            throw new InvalidPhoneCountryCodeFormatExceptions();

        if (!number.HasLength(9))
            throw new InvalidPhoneCountryCodeLengthExceptions();
    }
}
