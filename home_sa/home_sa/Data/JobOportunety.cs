using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace home_sa.Data
{
    public class JobOportunety
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string jobId { get; set; }

        public string employerId { get; set; }

        public string jobTitle { get; set; }

        public string jobDescription { get; set; }

    }
}
