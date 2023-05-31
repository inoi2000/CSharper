using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharper.Models
{
    public class Subject
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Complexity? Complexity { get; set; }

        public ICollection<Lesson>? Lessons { get; set; }
        public ICollection<Book>? Books { get; set; }
        public ICollection<Video>? Videos { get; set; }
        public ICollection<Article>? Articles { get; set; }
        public ICollection<Assignment>? Assignment { get; set; }
    }
}
