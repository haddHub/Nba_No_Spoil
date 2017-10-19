using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nba.Model
{
    public class Team
    {
        public int TeamID { get; set; }
        public string TeamName { get; set; }
        public int Score { get; set; }
        public bool Winned { get; set; } = false;
        public string TeamImageUri => String.Format($"Nba.Images.Teams_Icons.{TeamID}.png");
    }
}
