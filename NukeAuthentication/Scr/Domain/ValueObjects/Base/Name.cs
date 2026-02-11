using NukeAuthentication.Scr.Shared.Exceptions;
using NukeAuthentication.Scr.Shared.ExtensionMethods;
using static NukeAuthentication.Scr.Shared.ExtensionMethods.StringExtensionHelper;

namespace NukeAuthentication.Scr.Domain.ValueObjects.Base;

public record Name
{
    public string FirstName { get; init; }
    public string LastName { get; init; }

    public string FullName => $"{FirstName} {LastName}";

    private Name() { }

    public Name(string firstName, string lastName)
    {
        ValidateNamePart(firstName);
        ValidateNamePart(lastName);

        FirstName = firstName;
        LastName = lastName;
    }

    private void ValidateNamePart(string namePart)
    {
        if (!namePart.HasContent() || !namePart.HasMinLength(3))
            throw new InvalidNameLengthExceptions(nameof(namePart));

        if (!namePart.IsOnlyLettersOrNumbers(CheckType.OnlyLetters))
            throw new InvalidNameCharacterExceptions(nameof(namePart));
    }
}
