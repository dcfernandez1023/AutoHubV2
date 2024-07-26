using AutoHub.Data;
using AutoHub.Models;
using AutoHub.Repositories.Abstractions;

namespace AutoHub.Repositories
{
    public class ScheduledServiceTypeRepository : AbstractRepository<ScheduledServiceType>, IScheduledServiceTypeRepository
    {
        public ScheduledServiceTypeRepository(AppDbContext dbContext) : base(dbContext) { }
    }
}
