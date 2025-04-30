using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GolfProgressTracker.Core.Models
{
    [Table("Round")]
    public class Round
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string DatePlayed { get; set; } = null!;

        public ICollection<Hole> Holes { get; set; } = [];
    }
}
