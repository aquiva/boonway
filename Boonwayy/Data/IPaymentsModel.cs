using System.Data.Entity;

namespace Boonwayy.Data
{
    public interface IPaymentsModel
    {
        DbSet<Feature> Features { get; set; }
        DbSet<Plan> Plans { get; set; }
    }
}