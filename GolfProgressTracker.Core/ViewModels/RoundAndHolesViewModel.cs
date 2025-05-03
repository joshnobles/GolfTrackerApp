namespace GolfProgressTracker.Core.ViewModels
{
    public class RoundAndHolesViewModel
    {
        public RoundViewModel Round { get; set; } = new();

        public List<HoleViewModel> Holes { get; set; } = [];
    }
}
