using NukeAuthentication.Scr.Domain.ValueObjects.Base;
using NukeAuthentication.Scr.Shared.Exceptions;

namespace NukeAuthentication.Scr.Domain.ValueObjects;

public record Person
{
    public Name Name { get; init; }
    public DateOnly BirthDate { get; init; }
    public CPF Cpf { get; init; }
     
    #region Geters
    public string FirstName => Name.FirstName;
    public string LastName => Name.LastName;
    public string FullName => Name.FullName;

    public string Numbers => Cpf.Numbers;
    public string Validators => Cpf.Validators;
    public string UnformattedCpf => Cpf.UnformattedCpf;
    public string FormattedCpf => Cpf.FormattedCpf;
    public string MaskedCpf => Cpf.MaskedCpf;
    #endregion

    private Person(){ }

    public Person(Name name, DateOnly birthDate, CPF cpf)
    {
        Name = name;
        Cpf = cpf;

        ValidateAge(birthDate);

        BirthDate = birthDate;
    }

    private void ValidateAge(DateOnly date)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var age = today.Year - date.Year;

        if (date > today.AddYears(-age))
            age--;

        if (age < 18)
            throw new UnderageException();

        if (age > 120)
            throw new OveragedException();
    }
}
