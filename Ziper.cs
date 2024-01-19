using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OwlStudio
{
	public static class Ziper
	{
		public static async Task UnzipFile(string zipPath, string pathToFile)
		{
			if (!File.Exists(zipPath))
			{
				MessageBox.Show("Не обноружен исходный файл");
				return;
			}
			if(!Directory.Exists(pathToFile)) Directory.CreateDirectory(pathToFile);
			
			await Task.Run(() => { ZipFile.ExtractToDirectory(zipPath, pathToFile); });
		}
	}
}
