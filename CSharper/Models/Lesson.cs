﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CSharper.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Experience { get; set; }
        public string LocalLink { get; set; }
        public Uri Url { get; set; }
        public Complexity Complexity { get; set; }

        public Subject Subject { get; set; }
        public ICollection<User> Users { get; set; }

    }
}
