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
		/// <summary>
		/// Ładuje wszystkie obiekty typu T z folderu contentFolder
		/// </summary>
		/// <typeparam name="T">Typ obiektu - np. Texture2d</typeparam>
		/// <param name="contentManager">Manager zasobów</param>
		/// <param name="contentFolder">Folder, z którego ładowane będą zasoby</param>
		/// <returns>Słownik zasobów</returns>
		public static Dictionary<String, T> LoadContent<T>(this ContentManager contentManager, String contentFolder)
        {
            var dir = new DirectoryInfo(contentManager.RootDirectory + "\\" + contentFolder);
            if (!dir.Exists)
                throw new DirectoryNotFoundException();
 
            var result = new Dictionary<String, T>();
 
            FileInfo[] files = dir.GetFiles("*.*");
            foreach (FileInfo file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file.Name);
 
                result[key] = contentManager.Load<T>(contentFolder + "/" + key);
            }
 
            return result;
        }
    }
}
