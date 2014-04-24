using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace Miner.Extensions
{
	public static class ContentManagerExtensions
	{
		public static Dictionary<String, T> LoadContent<T>(this ContentManager contentManager, String contentFolder)
        {
            // Load directory info, abort if none
            var dir = new DirectoryInfo(contentManager.RootDirectory + "\\" + contentFolder);
            if (!dir.Exists)
                throw new DirectoryNotFoundException();
 
            // Init the resulting list
            var result = new Dictionary<String, T>();
 
            // Load all files that matches the file filter
            FileInfo[] files = dir.GetFiles("*.*");
            foreach (FileInfo file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file.Name);
 
                result[key] = contentManager.Load<T>(contentFolder + "/" + key);
            }
 
            // Return the result
            return result;
        }
    }
}
