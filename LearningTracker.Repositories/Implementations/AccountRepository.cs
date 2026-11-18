
using LearningTracker.Data;
using LearningTracker.Models.Entities;
using LearningTracker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LearningTracker.Repositories.Implementations;

public class AccountRepository : IAccountRepository
{
    private readonly ApplicationDbContext _context;

    public AccountRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Account?> GetByNormalizedEmailAsync(string normalizedEmail)
    {
        return await _context.Accounts
            .FirstOrDefaultAsync(
                a => a.NormalizedEmail == normalizedEmail);
    }

    public async Task<Account?> GetByIdAsync(Guid id)
    {
        return await _context.Accounts
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task AddAsync(Account account)
    {
        await _context.Accounts.AddAsync(account);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
