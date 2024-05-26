using Microsoft.AspNetCore.Identity;
using home_sa.Data;

namespace home_sa
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? Address { get; set; }

        public DateTime LastLoggedIn { get; set; }

        public virtual ICollection<JobOportunety> JobOportuneties { get; set; }

    }
}
