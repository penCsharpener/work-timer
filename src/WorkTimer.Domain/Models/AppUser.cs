using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace WorkTimer.Domain.Models;
public class AppUser : IdentityUser<int>
{
    public AppUser() { }

    public AppUser(string email, string password)
    {
        UserName = Email = email;
        NormalizedUserName = NormalizedEmail = email.ToUpper();
        LockoutEnabled = false;
        EmailConfirmed = true;
        SecurityStamp = Guid.NewGuid().ToString();
        PasswordHash = new PasswordHasher<AppUser>().HashPassword(this, password);
    }

    public ICollection<Contract>? Contracts { get; set; }
    public ICollection<Todo> Todos { get; set; }
    public ICollection<Note> Notes { get; set; }
}