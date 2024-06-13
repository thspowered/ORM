using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseProject.Models
{
    public class DietPlanMeal
    {
        [Key]
        [Column(Order = 1)]
        public int PlanId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int MealId { get; set; }

        [ForeignKey("PlanId")]
        public TrainingDietPlan TrainingDietPlan { get; set; }

        [ForeignKey("MealId")]
        public Meal Meal { get; set; }
    }
}
