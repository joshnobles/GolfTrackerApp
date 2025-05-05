using System.ComponentModel.DataAnnotations;

namespace GolfProgressTracker.Core.ViewModels
{
    public class RoundViewModel
    {
        private int _id = 0;
        private string _title = null!;
        private DateOnly _datePlayed = DateOnly.FromDateTime(DateTime.Now);

        public int Id
        {
            get => _id;
            set => _id = value;
        }

        [Required(ErrorMessage = "Round title is required")]
        [StringLength(200, ErrorMessage = "Title cannot be more than 200 characters")]
        public string Title
        {
            get => _title;
            set => _title = string.IsNullOrWhiteSpace(value) ? null! : value.Trim();
        }

        [Required(ErrorMessage = "Date Played is required")]
        public DateOnly DatePlayed
        {
            get => _datePlayed;
            set => _datePlayed = value;
        }
    }
}
