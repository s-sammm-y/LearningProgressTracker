
using LearningTracker.Data;
using LearningTracker.Models.Entities;
using LearningTracker.Models.Enums;
using LearningTracker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LearningTracker.Repositories.Implementations;

public class AssignmentRepository : IAssignmentRepository
{
    private readonly ApplicationDbContext _context;

    public AssignmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Assignment?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Assignments
            .Include(a => a.Learner)
            .Include(a => a.Activity)
            .FirstOrDefaultAsync(
                a => a.Id == id,
                cancellationToken);
    }

    public async Task<List<Assignment>> GetByLearnerIdAsync(
        Guid learnerId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Assignments
            .Include(a => a.Activity)
            .Where(a => a.LearnerId == learnerId)
            .OrderBy(a => a.Activity.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        Guid learnerId,
        Guid activityId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Assignments
            .AnyAsync(
                a => a.LearnerId == learnerId &&
                     a.ActivityId == activityId,
                cancellationToken);
    }

    public async Task AddAsync(
        Assignment assignment,
        CancellationToken cancellationToken = default)
    {
        await _context.Assignments.AddAsync(
            assignment,
            cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Assignments.CountAsync(cancellationToken);
    }

    public async Task<int> CountCompletedAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Assignments.CountAsync(
            a => a.Status == AssignmentStatus.Completed,
            cancellationToken);
    }
}
