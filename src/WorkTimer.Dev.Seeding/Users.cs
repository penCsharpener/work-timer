using System.Collections.Generic;
using WorkTimer.Domain.Models;

namespace WorkTimer.Dev.Seeding;

public static class Users
{
    private static readonly string _pwd = "123123";
    private static List<AppUser> _appUsers;

    public static List<AppUser> GetUsers()
    {
        _appUsers ??= new List<AppUser> {
                new AppUser("admin@gmail.com", _pwd) { Id = 1 },
                new AppUser("john.doe@gmail.com", _pwd) { Id = 2 },
                new AppUser("jane.doe@gmail.com", _pwd) { Id = 3 }
            };

        return _appUsers;
    }
}