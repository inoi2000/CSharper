using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharper.Models
{
    public interface IPdfReading
    {
        public Guid Id { get; init; }

        public string? LocalLink { get; set; }
    }
}
