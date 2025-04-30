namespace GolfProgressTracker.Core.ViewModels
{
    public class RoundViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public DateOnly DatePlayed { get; set; }
    }
}
