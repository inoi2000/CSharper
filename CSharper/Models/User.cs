using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CSharper.Models
{
    public class User
    {
        public Guid Id { get; init; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string? Salt { get; set; }
        public string? Level { get; set; }
        public double Experience { get; set; }


        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
        public ICollection<Book> Books { get; set; } = new List<Book>();
        public ICollection<Video> Videos { get; set; } = new List<Video>();
        public ICollection<Article> Articles { get; set; } = new List<Article>();
        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

        public override string ToString()
        {
            return Login;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            if (obj is User user)
            {
                if (this.Id == user.Id) { return true; }
                else { return false; }
            }
            else throw new ArgumentException(nameof(obj));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
