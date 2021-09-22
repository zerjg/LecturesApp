using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LecturesApp.Models
{
    public class UserLecture
    {
        public string UserID { get; set; }
        public User User { get; set; }

        public int LectureID { get; set; }
        public Lecture Lecture { get; set; }
    }
}
