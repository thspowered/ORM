using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseProject.Models
{
    public class TrainingDietPlan
    {
        [Key]
        public int PlanId { get; set; }

        [Required]
        public DateTime StartingDate { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        [MaxLength(255)]
        public string GoalDescription { get; set; }

        [Required]
        [MaxLength(1)]
        public string Type { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int TrainerId { get; set; }

        [ForeignKey("UserId, TrainerId")]
        public User User { get; set; }
    }
}
