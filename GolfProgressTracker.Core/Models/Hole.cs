using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GolfProgressTracker.Core.Models
{
    [Table("Hole")]
    public class Hole
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Round))]
        public int RoundId { get; set; }

        public int Number { get; set; }

        public int Par { get; set; }

        public int Shots { get; set; }


        public Round Round { get; set; } = null!;
    }
}
