using ModernHttpClient;
using Nba.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Nba
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamesSchedule : ContentPage
    {
        private HttpClient _client = new HttpClient(new NativeMessageHandler());
        
        public GamesSchedule()
        {
            InitializeComponent();
            UpdateView();
        }

        private async void UpdateView()
        {
            MatchesView.ItemsSource = await LoadDataFromService(MatchsDatePicker.Date);
            MatchesView.EndRefresh();
        }

        private async Task<IEnumerable<Match>> LoadDataFromService(DateTime date)
        {
            ObservableCollection<Match> _matchs = new ObservableCollection<Match>();
            string urlNba = $"https://stats.nba.com/stats/scoreboardV2/?GameDate={date.Month}/{date.Day}/{date.Year}&LeagueID=00&DayOffset=0";

            _client.DefaultRequestHeaders.Add("accept-encoding", "Accepflate, sdch");
            _client.DefaultRequestHeaders.Add("Accept-Language", "en");
            _client.DefaultRequestHeaders.Add("origin", "http://stats.nba.com");
            _client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.133 Safari/537.36");
            var response = await _client.GetAsync(urlNba);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (content != null)
                {
                    var matchs = JsonConvert.DeserializeObject<RootObject>(content);
                    if (matchs != null)
                    {
                        var gameHeaders = matchs.resultSets.Where(rs => rs.name == "GameHeader").First();
                        var lineScores = matchs.resultSets.Where(rs => rs.name == "LineScore").First();

                        foreach (var rowSet in gameHeaders.rowSet)
                        {
                            Match m = new Match();
                            m.GameID = Int32.Parse(rowSet[2].ToString());
                            m.HomeTeam.TeamID = Int32.Parse(rowSet[6].ToString());
                            m.VisitorTeam.TeamID = Int32.Parse(rowSet[7].ToString());

                            var scores = lineScores.rowSet.Where(rs => Int32.Parse(rs[2].ToString()) == m.GameID);
                            var lineScoreHomeTeam = scores.Where(sc => Int32.Parse(sc[3].ToString()) == m.HomeTeam.TeamID).First();
                            var lineScoreVisitorTeam = scores.Where(sc => Int32.Parse(sc[3].ToString()) == m.VisitorTeam.TeamID).First();

                            m.HomeTeam.TeamName = lineScoreHomeTeam[6].ToString();
                            if (lineScoreHomeTeam.Count >= 23 && lineScoreHomeTeam[22] != null)
                                m.HomeTeam.Score = Int32.Parse(lineScoreHomeTeam[22].ToString());

                            m.VisitorTeam.TeamName = lineScoreVisitorTeam[6].ToString();
                            if (lineScoreVisitorTeam.Count >= 23 && lineScoreVisitorTeam[22] != null)
                                m.VisitorTeam.Score = Int32.Parse(lineScoreVisitorTeam[22].ToString());

                            // Set the winning team
                            if (m.VisitorTeam.Score > 0 && m.HomeTeam.Score > 0)
                            {
                                if (m.VisitorTeam.Score > m.HomeTeam.Score)
                                    m.VisitorTeam.Winned = true;
                                else if (m.VisitorTeam.Score == m.HomeTeam.Score)
                                {
                                    m.VisitorTeam.Winned = true;
                                    m.HomeTeam.Winned = true;
                                }
                                else
                                {
                                    m.HomeTeam.Winned = true;
                                }
                            }

                            _matchs.Add(m);
                        } 
                    }
                }
            }
            return _matchs;
        }

        private void MatchesView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            if (e.SelectedItem is Match)
            {
                Match m = e.SelectedItem as Match;
                m.Spoiler = !m.Spoiler;
            }

            MatchesView.SelectedItem = null;
        }

        private void MatchsDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            MatchesView.BeginRefresh();
        }

        private void MatchesView_Refreshing(object sender, EventArgs e)
        {
            UpdateView();
        }
    }
}