using NukeAuthentication.Scr.Shared.Exceptions;
using System.Text.RegularExpressions;

namespace NukeAuthentication.Scr.Domain.ValueObjects.Base;

public record Email
{
    public string Address { get; init; }
    public string Domain { get; init; }

    public string FullEmail => $"{Address}@{Domain}";
    public string MaskedEmail
    {
        get
        {
            var asterisks = string.Empty;

            for (int i = 0; i < (Address.Length - 1); i++)
                asterisks += '*';

            return $"{Address[0]}{asterisks}@{Domain}";
        }
    }

    private Email() { }

    public Email(string address, string domain)
    {
        ValidEmail(address, domain);

        Address = address;
        Domain = domain;
    }

    private static void ValidEmail(string address, string domain)
    {
        string regexPattern = @"^[\w%+-]+(\.[\w%+-]+)*@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        if (!Regex.IsMatch($"{address}@{domain}", regexPattern))
            throw new InvalidEmailFormatExceptions();
    }
}
