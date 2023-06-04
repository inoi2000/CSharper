using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharper.Models
{
    public class Article
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public double Experience { get; set; }
        public string? LocalLink { get; set; }
        public Uri? Url { get; set; }
        public Complexity Complexity { get; set; }

        public Subject Subject { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            if (obj is Article article)
            {
                if (this.Id == article.Id) { return true; }
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
