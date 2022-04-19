using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.Helpres
{
    public class Helper
    {
        public static void DeleteFile(IWebHostEnvironment _env, string fileName,params string[] folders)
        {
            string path = _env.WebRootPath;
            foreach (string folder in folders)
            {
                path = Path.Combine(path, folder);
            }
            path = Path.Combine(path, fileName);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
