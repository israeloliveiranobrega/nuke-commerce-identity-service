using NukeAuthentication.Scr.Shared.Exceptions.Base;

namespace NukeAuthentication.Scr.Shared.Exceptions;

public class InvalidEmailFormatExceptions : DomainException
{
    public InvalidEmailFormatExceptions() : base("O formato do email é inválido.") { }
}
