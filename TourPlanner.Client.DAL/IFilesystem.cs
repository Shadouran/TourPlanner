using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Client.DAL
{
    public interface IFilesystem
    {
        public string FilesystemPath { get; }
        Guid SaveImage(byte[] bytes);
        Image? LoadImage(Guid? id);
    }
}
