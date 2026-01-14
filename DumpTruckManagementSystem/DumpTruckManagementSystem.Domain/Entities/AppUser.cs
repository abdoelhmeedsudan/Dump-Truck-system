using Microsoft.AspNetCore.Identity;

namespace DumpTruckManagementSystem.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
