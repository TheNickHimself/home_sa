using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace home_sa.Models
{
    public class JobOpportunity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int jobId { get; set; }

        [Required]
        public Guid employerId { get; set; }

        [Required]
        [Display(Name = "Job Title")]
        public string jobTitle { get; set; }

        [Required]
        [Display(Name = "Job Description")]
        public string jobDescription { get; set; }
    }
}
