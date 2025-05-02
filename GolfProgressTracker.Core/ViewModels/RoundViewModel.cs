using System.ComponentModel.DataAnnotations;

namespace GolfProgressTracker.Core.ViewModels
{
    public class RoundViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Round title is required")]
        [StringLength(200, ErrorMessage = "Title cannot be more than 200 characters")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Date Played is required")]
        public DateOnly DatePlayed { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }
}
