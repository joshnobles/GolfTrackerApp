using System.ComponentModel.DataAnnotations;

namespace GolfProgressTracker.Core.ViewModels
{
    public class HoleViewModel
    {
        [Required]
        public int Number { get; set; }

        [Required]
        public int? Par { get; set; } = null;

        [Required]
        public int? Shots { get; set; } = null;
    }
}
