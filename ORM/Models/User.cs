using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseProject.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        [MaxLength(20)]
        public string Surname { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }

        [Required]
        [MaxLength(12)]
        public string Phone { get; set; }

        [Required]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public double Height { get; set; }

        [Required]
        public double Weight { get; set; }

        [Required]
        [MaxLength(6)]
        public string Sex { get; set; }

        [Required]
        [MaxLength(1)]
        public string Role { get; set; }

        [MaxLength(255)]
        public string Allergies { get; set; }

        [Required]
        public int TrainerId { get; set; }

        [ForeignKey("TrainerId")]
        public Trainer Trainer { get; set; }
    }
}
