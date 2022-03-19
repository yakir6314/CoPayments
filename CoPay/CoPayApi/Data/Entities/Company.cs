using System.Collections.Generic;

namespace CoPayApi.Data.Entities
{
    public class Company
    {
        public string Name { get; set; }
        public int PrivateCompanyID { get; set; }
        public int Id { get; set; }
        public string customerCode; 
        public List<User> UsersInTheCompany { get; set; }
    }
}
