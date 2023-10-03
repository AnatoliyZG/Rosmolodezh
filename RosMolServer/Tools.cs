using RosMolExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RosMolServer
{
    internal static class Tools
    {

        public static void SaveSquarePhoto(string userId, byte[] photo, string folder, int maxResolution)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            Image img = Image.Load(photo);

            int resolution = Math.Min(img.Width, img.Height);

            img.Mutate(i =>
            {
                i.Resize(new ResizeOptions()
                {
                    Size = new Size(resolution, resolution),
                    Mode = ResizeMode.Crop,
                });
                if (resolution > maxResolution)
                {
                    i.Resize(new ResizeOptions()
                    {
                        Size = new Size(maxResolution, maxResolution),
                        Mode = ResizeMode.Stretch,
                    });
                }
            });
            img.SaveAsJpeg($"{folder}/{userId}.jpg");

            img.Dispose();
        }

        public static SimpleResponse<string[]> GetPhotos(string key)
        {
            return new SimpleResponse<string[]>(new DirectoryInfo(key).GetFiles().Select((a)=>a.Name).ToArray());
        }
    }
}
