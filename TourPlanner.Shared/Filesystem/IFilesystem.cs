using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Shared.Filesystem
{
    public interface IFilesystem
    {
        public string FilesystemPath { get; }
        public bool ImageInCache(Guid? id);
        Guid SaveImage(byte[] bytes, Guid? imageId = null);
        byte[]? LoadImage(Guid? id);
        Task ClearDirectoryAsync();
        void DeleteImage(Guid id);
    }
}
