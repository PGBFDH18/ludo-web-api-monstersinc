using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LudoWebAPI.Models
{
    public interface IStatsContainter
    {
        List<Winner> StatsLoader();
        void AddWinner(string name);
    }
}
