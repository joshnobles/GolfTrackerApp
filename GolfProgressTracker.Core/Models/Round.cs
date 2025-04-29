using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GolfProgressTracker.Core.Models
{
    [Table("Round")]
    class Round
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; } = null!;
    }
}
