using System.ComponentModel.DataAnnotations;

namespace GolfProgressTracker.Core.ViewModels
{
    public class HoleViewModel
    {
        private int _id = 0;
        private int _number = 0;
        private int? _par = null;
        private int? _shots = null;

        public int Id
        {
            get => _id;
            set => _id = value < 1 ? 0 : value;
        }

        [Required(ErrorMessage = "Hole number is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Hole number must be more than 0")]
        public int Number
        {
            get => _number;
            set => _number = value < 1 ? 0 : value;
        }

        [Required(ErrorMessage = "Hole par is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Hole par must be more than 0")]
        public int? Par
        {
            get => _par;
            set => _par = value < 1 ? 0 : value;
        }

        [Required(ErrorMessage = "Hole shots is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Hole shots must be more than 0")]
        public int? Shots
        {
            get => _shots;
            set => _shots = value < 1 ? 0 : value;
        }
    }
}
