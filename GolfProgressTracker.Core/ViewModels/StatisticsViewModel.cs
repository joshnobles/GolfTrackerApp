using System.Globalization;

namespace GolfProgressTracker.Core.ViewModels
{
    public class StatisticsViewModel
    {
        public List<RoundAndHolesViewModel> RoundsAndHoles { get; set; }

        public StatisticsViewModel(List<RoundAndHolesViewModel> roundsAndHoles)
        {
            RoundsAndHoles = roundsAndHoles;
        }

        public string FormatAsString(double score)
        {
            string prefix = "";

            if (score > 0)
                prefix = "+";
            else if (score < 0)
                prefix = "-";

            return prefix + score;
        }

        public double Handicap
        {
            get
            {
                var roundScores = 0;
                var totalRounds = 0;
                int score;

                foreach (var round in RoundsAndHoles)
                {
                    score = 0;
                    foreach (var hole in round.Holes)
                    {
                        if (!hole.Shots.HasValue || !hole.Par.HasValue)
                            continue;

                        score += (int)hole.Shots - (int)hole.Par;
                    }

                    roundScores += score;
                    totalRounds++;
                }

                return Math.Round((double) roundScores / totalRounds, 2);
            }
        }
    
        public double AverageParThreeScore
        {
            get => GetAverage(h => h.Par != 3);
        }

        public double AverageParFourScore
        {
            get => GetAverage(h => h.Par != 4);
        }

        public double AverageParFiveScore
        {
            get => GetAverage(h => h.Par != 5);
        }

        public RoundScoresGraphViewModel RoundGraphData
        {
            get
            {
                var result = new RoundScoresGraphViewModel();
                int score;

                foreach (var round in RoundsAndHoles.OrderBy(r => r.Round.DatePlayed))
                {
                    result.TitlesAndDates.Add($"{round.Round.Title} | {round.Round.DatePlayed:d}");

                    score = 0;

                    foreach (var hole in round.Holes)
                    {
                        if (!hole.Shots.HasValue || !hole.Par.HasValue)
                            continue;

                        score += (int)hole.Shots - (int)hole.Par;
                    }

                    result.Scores.Add(score);
                }

                return result;
            }
        }

        private double GetAverage(Func<HoleViewModel, bool> continueCondition)
        {
            var totalScore = 0;
            var totalHoles = 0;

            foreach (var round in RoundsAndHoles)
            {
                foreach (var hole in round.Holes)
                {
                    if (!hole.Shots.HasValue || !hole.Par.HasValue || continueCondition(hole))
                        continue;

                    totalScore += (int)hole.Shots - (int)hole.Par;
                    totalHoles++;
                }
            }

            return Math.Round((double)totalScore / totalHoles, 2);
        }
    }
}
