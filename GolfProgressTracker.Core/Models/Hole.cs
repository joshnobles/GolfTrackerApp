using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GolfProgressTracker.Core.Models
{
    [Table("Hole")]
    class Hole
    {
        [Key]
        public int Id { get; set; }

        public int RoundId { get; set; }

        public int Par { get; set; }

        public int Shots { get; set; }
    }
}
