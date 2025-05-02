using System.ComponentModel.DataAnnotations;

namespace GolfProgressTracker.Core.ViewModels
{
    public class RoundAndHolesViewModel
    {
        public RoundViewModel Round { get; set; } = null!;

        public List<HoleViewModel> Holes { get; set; } = [];
    }
}
