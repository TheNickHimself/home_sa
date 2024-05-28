
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace home_sa.Models
{
    public class RollManager
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
        public string SelectedRole { get; set; }
        public IEnumerable<string> AvailableRoles { get; set; }
    }
}
