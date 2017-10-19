using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Nba.Model
{
    public class Match : INotifyPropertyChanged
    {
        public int GameID { get; set; }

        public Team HomeTeam { get; set; }
        public Team VisitorTeam { get; set; }

        private bool spoiler;

        public bool Spoiler
        {
            get { return spoiler; }
            set
            {
                if (spoiler == value)
                    return;

                spoiler = value;
                OnPropertyChanged();
            }
        }


        public Match()
        {
            HomeTeam = new Team();
            VisitorTeam = new Team();
            Spoiler = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
