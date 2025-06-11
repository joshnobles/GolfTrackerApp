namespace GolfProgressTracker.Core.ViewModels
{
    public class StatisticsViewModel(List<RoundAndHolesViewModel> roundsAndHoles)
    {
        private List<RoundAndHolesViewModel> RoundsAndHoles { get; set; } = roundsAndHoles;

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

        public string FormatAsString(string key)
        {
            var property = GetType()
                .GetProperties()
                .FirstOrDefault(p => p.Name.Equals(key))
                ?? throw new KeyNotFoundException($"Property not found: {key}");

            var scoreObj = property
                .GetValue(this)
                ?? throw new KeyNotFoundException($"Property not found: {key}");

            var score = Convert.ToDouble(scoreObj);

            if (score < 0)
                return "+" + Math.Abs(score);

            return Math.Abs(score) + "";
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
