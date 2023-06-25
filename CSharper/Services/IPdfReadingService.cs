using CSharper.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharper.Services
{
    public interface IPdfReadingService
    {
        public async Task<bool> IsAccomplitAsync(Guid userId, Guid pdfMateriallId)
        {
            return false;
        }

        public async Task<bool> AccomplitAsync(Guid userId, Guid pdfMateriallId)
        {
            return false;
        }

        public async Task<bool> CancelAccomplitAsync(Guid userId, Guid pdfMateriallIdId)
        {
            return false;
        }
    }
}
