using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using NukeProjectUtils.DataStructure.Enums;
using NukeProjectUtils.DataStructure.ValueObjects.Aggregates;
using NukeProjectUtils.DataStructure.ValueObjects.Atomics;
using NukeProjectUtils.ValueObjects.Base;

namespace NukeAuthentication.Entitys;

public record User : BaseEntity
{
    #region Base Properties
    public Person Person { get; init; }
    public Address Address { get; init; }
    public Email Email { get; init; }
    public Phone? Phone { get; init; }
    public Password Password { get; init; }
    public AccountStatus Status { get; init; }
    public Role Role { get; init; }
    #endregion

    private User() { }

    public User(Person person, Address address, Email email, Password password, Phone? phone, AccountStatus status = AccountStatus.Active, Role level = Role.User)
    {
        Person = person;
        Address = address;
        Email = email;
        Phone = phone;
        Password = password;
        Status = status;
        Role = level;
    }
}
