using Microsoft.AspNetCore.Identity;

namespace CoPayApi.Data.Entities
{
    public class User : IdentityUser
    {                                          
        public  string FirstName { get; set; }
        public string LastName { get; set; }
        public Company company { get; set; }
    }
}
