using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharper.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Level { get; set; }
        public double Experience { get; set; }


        public ICollection<Lesson> Lessons { get; set; }
        public ICollection<Book> Books { get; set; }
        public ICollection<Video> Videos { get; set; }
        public ICollection<Article> Articles { get; set; }
        public ICollection<Assignment> Assignment { get; set; }
    }
}
