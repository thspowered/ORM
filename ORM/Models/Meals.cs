using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseProject.Models
{
    public class Meal
    {
        [Key]
        public int MealId { get; set; }

        [Required]
        [MaxLength(50)]
        public string MealName { get; set; }

        [Required]
        [MaxLength(255)]
        public string MealDescription { get; set; }

        [Required]
        public int MealCalories { get; set; }

        [Required]
        public double MealProtein { get; set; }

        [Required]
        public double MealCarbs { get; set; }

        [Required]
        public double MealFat { get; set; }

        [Required]
        public int TrainerId { get; set; }

        [ForeignKey("TrainerId")]
        public Trainer Trainer { get; set; }
    }
}
