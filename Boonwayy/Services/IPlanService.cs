using System.Collections.Generic;
using Boonwayy.Data;

namespace Boonwayy.Services
{
    public interface IPlanService
    {
        Plan Find(int id);
        IList<Plan> List();
    }
}