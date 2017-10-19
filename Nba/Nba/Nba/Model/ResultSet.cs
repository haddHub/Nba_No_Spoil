using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nba.Model
{
    public class ResultSet
    {
        public string name { get; set; }
        public List<object> headers { get; set; }
        public List<List<object>> rowSet { get; set; }
    }
}
