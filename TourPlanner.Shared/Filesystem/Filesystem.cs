using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Shared.Filesystem
{
    public class Filesystem : IFilesystem
    {
        private readonly string _path;
        public string FilesystemPath => _path;

        public Filesystem(IConfiguration configuration)
        {
            _path = configuration["ImageDirectory"];
        }

        public bool ImageInCache(Guid? id)
        {
            var path = Path.Combine(_path, id.ToString());
            path = Path.ChangeExtension(path, "jpeg");
            if (!File.Exists(path))
                return false;
            return true;
        }

        public byte[]? LoadImage(Guid? id)
        {
            if (id == null)
                return null;
            var path = Path.Combine(_path, id.ToString());
            path = Path.ChangeExtension(path, "jpeg");
            if (!File.Exists(path))
                return null;

            using var image = Image.FromFile(path);
            using var ms = new MemoryStream();
            image.Save(ms, ImageFormat.Jpeg);
            var array = ms.ToArray();
            return array;
        }

        public Guid SaveImage(byte[] bytes, Guid? imageId = null)
        {
            using var memoryStream = new MemoryStream(bytes);
            var image = Image.FromStream(memoryStream);
            var id = imageId ?? Guid.NewGuid();

            if(!Directory.Exists(_path))
                Directory.CreateDirectory(_path);
            var path = Path.Combine(_path, id.ToString());

            image.Save(Path.ChangeExtension(path, "jpeg"), ImageFormat.Jpeg);
            return id;
        }

        public async Task ClearDirectoryAsync()
        {
            await Task.Run(() =>
            {
                var files = Directory.EnumerateFiles(_path);
                foreach (var file in files)
                {
                    File.Delete(file);
                }
            });
        }

        public void DeleteImage(Guid id)
        {
            var path = Path.Combine(_path, id.ToString());
            path = Path.ChangeExtension(path, "jpeg");
            File.Delete(path);
        }
    }
}
