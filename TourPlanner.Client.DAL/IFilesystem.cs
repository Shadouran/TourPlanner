using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Client.DAL
{
    public interface IFilesystem
    {
        Guid SaveImage(byte[] bytes);
    }
}
