using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Client.DAL
{
    public class Filesystem : IFilesystem
    {
        private readonly string _path;

        public Filesystem(IConfiguration configuration)
        {
            _path = configuration["imageDirectory"];
        }
        public Guid SaveImage(byte[] bytes)
        {
            using var memoryStream = new MemoryStream(bytes);
            var image = Image.FromStream(memoryStream);
            var id = Guid.NewGuid();

            if(!Directory.Exists(_path))
                Directory.CreateDirectory(_path);
            var path = Path.Combine(_path, id.ToString());

            image.Save(Path.ChangeExtension(path, "jpeg"), ImageFormat.Jpeg);
            return id;
        }
    }
}
