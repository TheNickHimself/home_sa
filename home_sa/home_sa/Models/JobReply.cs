using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace home_sa.Models
{
    public class JobReply
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid replyId { get; set; }

        [Required]
        public Guid userId { get; set; }

        [NotMapped]
        [FileExtension(".docx,.pdf")]
        public IFormFile UploadedFile { get; set; }

        public string? FilePath { get; set; }


        public string? EncryptedSymmetricKey { get; set; }


        public string? IV { get; set; }

        [NotMapped]
        public string? signature { get; set; }

        [Required]
        [ForeignKey("JobOpportunity")]
        public int JobOpportunityId { get; set; } // Unique foreign key name

        [NotMapped]
        public int jobId
        {
            get => JobOpportunityId;
            set => JobOpportunityId = value;
        }

        public JobOpportunity? JobOpportunity { get; set; } // Navigation property
    }
}
