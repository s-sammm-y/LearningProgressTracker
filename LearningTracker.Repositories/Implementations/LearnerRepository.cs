
using LearningTracker.Data;
using LearningTracker.Models.Entities;
using LearningTracker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LearningTracker.Repositories.Implementations;

public class LearnerRepository : ILearnerRepository
{
    private readonly ApplicationDbContext _context;

    public LearnerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Learner?> GetByIdAsync(Guid id)
    {
        return await _context.Learners
            .Include(l => l.Account)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<Learner?> GetByAccountIdAsync(Guid accountId)
    {
        return await _context.Learners
            .FirstOrDefaultAsync(l => l.AccountId == accountId);
    }

    public async Task<List<Learner>> GetAllAsync()
    {
        return await _context.Learners
            .Include(l => l.Account)
            .OrderBy(l => l.DisplayName)
            .ToListAsync();
    }

    public async Task AddAsync(Learner learner)
    {
        await _context.Learners.AddAsync(learner);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<int> CountAsync()
    {
        return await _context.Learners.CountAsync();
    }

    public async Task<List<Learner>> GetAllWithAssignmentsAsync()
    {
        return await _context.Learners
            .Include(l => l.Assignments)
            .OrderBy(l => l.Cohort)
            .ThenBy(l => l.DisplayName)
            .ToListAsync();
    }
}
