using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharper.Models
{
    public class Subject
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Complexity Complexity { get; set; }

        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
        public ICollection<Book> Books { get; set; } = new List<Book>();
        public ICollection<Video> Videos { get; set; } = new List<Video>();
        public ICollection<Article> Articles { get; set; } = new List<Article>();
        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            if (obj is Subject subject)
            {
                if (this.Id == subject.Id) { return true; }
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
