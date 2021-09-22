using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace LecturesApp.Models
{
    public class User : IdentityUser
    {
        // (Inherited from Identity)
        //
        // Id
        // UserName
        // Email
        // PasswordHash

        [DataType("Date")]
        public DateTime DateOfBirth { get; set; }
        [NotMapped]
        public int Age
        {
            get
            {
                int age = Convert.ToInt32(DateTime.Now.Date - DateOfBirth.Date);
                return age;
            }
        }

        // Navigation
        public ICollection<Lecture> HostedLectures { get; set; }
        public ICollection<UserLecture> RegisteredOnLecturesLink { get; set; }
    }
}
