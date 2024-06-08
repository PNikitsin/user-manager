using Microsoft.AspNetCore.Identity;

namespace UserManager.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsBlocked { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LoginedAt { get; set; }
    } 
}