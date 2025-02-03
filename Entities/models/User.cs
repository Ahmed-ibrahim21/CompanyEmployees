using Microsoft.AspNetCore.Identity;

namespace Entities.models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
