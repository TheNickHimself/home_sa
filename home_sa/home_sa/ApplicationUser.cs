using Microsoft.AspNetCore.Identity;
using home_sa.Models;

namespace home_sa
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? Address { get; set; }

        public DateTime LastLoggedIn { get; set; }

        public virtual ICollection<JobOpportunity> JobOportuneties { get; set; }

    }
}
