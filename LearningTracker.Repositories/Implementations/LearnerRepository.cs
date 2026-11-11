
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

    public async Task<Learner?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Learners
            .Include(l => l.Account)
            .FirstOrDefaultAsync(
                l => l.Id == id,
                cancellationToken);
    }

    public async Task<Learner?> GetByAccountIdAsync(
        Guid accountId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Learners
            .FirstOrDefaultAsync(
                l => l.AccountId == accountId,
                cancellationToken);
    }

    public async Task<List<Learner>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Learners
            .Include(l => l.Account)
            .OrderBy(l => l.DisplayName)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        Learner learner,
        CancellationToken cancellationToken = default)
    {
        await _context.Learners.AddAsync(learner, cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Learners.CountAsync(cancellationToken);
    }

    public async Task<List<Learner>> GetAllWithAssignmentsAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Learners
            .Include(l => l.Assignments)
            .OrderBy(l => l.Cohort)
            .ThenBy(l => l.DisplayName)
            .ToListAsync(cancellationToken);
    }
}
