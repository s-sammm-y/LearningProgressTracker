
using LearningTracker.Data;
using LearningTracker.Models.Entities;
using LearningTracker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LearningTracker.Repositories.Implementations;

public class ActivityRepository : IActivityRepository
{
    private readonly ApplicationDbContext _context;

    public ActivityRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Activity?> GetByIdAsync(Guid id)
    {
        return await _context.Activities
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<Activity>> GetAllAsync()
    {
        return await _context.Activities
            .OrderBy(a => a.Title)
            .ToListAsync();
    }

    public async Task AddAsync(Activity activity)
    {
        await _context.Activities.AddAsync(activity);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<int> CountAsync()
    {
        return await _context.Activities.CountAsync();
    }

    public async Task AddRangeAsync(IEnumerable<Activity> activities)
    {
        await _context.Activities.AddRangeAsync(activities);
    }
}
