using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LudoWebAPI.Models
{
    public class StatsContainer : IStatsContainter
    {
        public List<Winner> winners { get; private set; }

        public StatsContainer()
        {
            winners = new List<Winner>();
        }

        public List<Winner> StatsLoader()
        {
            return winners.OrderByDescending(w => w.GamesWon).ToList();
        }

        public void AddWinner(string name)
        {
            var winner = winners.Where(w => w.Name.ToLower() == name.ToLower()).FirstOrDefault();
            if (winner != null)
            {
                winner.GamesWon += 1;
            }
            else
            {
                winners.Add(new Winner { Name = name, GamesWon = 1 });
            }
        }
    }
}
