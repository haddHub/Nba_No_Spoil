using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nba.Model
{
    public class RootObject
    {
        public string resource { get; set; }
        public Parameter parameters { get; set; }
        public List<ResultSet> resultSets { get; set; }
    }
}
