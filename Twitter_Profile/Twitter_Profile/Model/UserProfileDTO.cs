using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Twitter_Profile.Model
{
    public class UserProfileDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string PictureURL { get; set; }
        public string Email { get; set; }
        public DateTime JoinDate { get; set; }
    }
}
