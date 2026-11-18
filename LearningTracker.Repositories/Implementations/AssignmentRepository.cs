
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

    public async Task<Assignment?> GetByIdAsync(Guid id)
    {
        return await _context.Assignments
            .Include(a => a.Learner)
            .Include(a => a.Activity)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<Assignment>> GetByLearnerIdAsync(Guid learnerId)
    {
        return await _context.Assignments
            .Include(a => a.Activity)
            .Where(a => a.LearnerId == learnerId)
            .OrderBy(a => a.Activity.Title)
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(Guid learnerId, Guid activityId)
    {
        return await _context.Assignments
            .AnyAsync(
                a => a.LearnerId == learnerId &&
                     a.ActivityId == activityId);
    }

    public async Task AddAsync(Assignment assignment)
    {
        await _context.Assignments.AddAsync(assignment);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<int> CountAsync()
    {
        return await _context.Assignments.CountAsync();
    }

    public async Task<int> CountCompletedAsync()
    {
        return await _context.Assignments.CountAsync(
            a => a.Status == AssignmentStatus.Completed);
    }

    public async Task<int> CountInProgressAsync()
    {
        return await _context.Assignments.CountAsync(
            a => a.Status == AssignmentStatus.InProgress);
    }
}
