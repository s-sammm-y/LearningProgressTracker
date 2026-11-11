using LearningTracker.Models.Entities;
using Microsoft.AspNetCore.Identity;

Console.Write("Enter Coordinator password: ");
var password = Console.ReadLine();

if (string.IsNullOrWhiteSpace(password))
{
    Console.WriteLine("Password cannot be empty.");
    return;
}

var account = new Account
{
    Id = Guid.NewGuid(),
    NormalizedEmail = "coordinator2@example.com"
};

var passwordHasher = new PasswordHasher<Account>();

var passwordHash = passwordHasher.HashPassword(
    account,
    password
);

Console.WriteLine("\nGenerated Password Hash:");
Console.WriteLine(passwordHash);

/*
using LearningTracker.Models.Entities;
using Microsoft.AspNetCore.Identity;

Console.Write("Enter the original password: ");
var password = Console.ReadLine();

Console.Write("Paste the generated hash: ");
var hash = Console.ReadLine();

var account = new Account
{
    Id = Guid.NewGuid(),
    NormalizedEmail = "COORDINATOR1@EXAMPLE.COM"
};

var hasher = new PasswordHasher<Account>();

var result = hasher.VerifyHashedPassword(
    account,
    hash!,
    password!
);

Console.WriteLine($"\nVerification result: {result}");
*/