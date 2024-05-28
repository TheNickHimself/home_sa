using Microsoft.AspNetCore.Identity;
using home_sa.Models;

namespace home_sa
{
    public class ApplicationUser : IdentityUser
    {

        public DateTime LastLoggedIn { get; set; }

        public string PublicKey { get; set; }

        public string PrivateKey { get; set; }

        public virtual ICollection<JobOpportunity> JobOportuneties { get; set; }

    }
}
