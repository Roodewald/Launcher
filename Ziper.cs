using System.IO;
using System.IO.Compression;
using static OwlStudio.MainWindow;

namespace OwlStudio
{
    public static class Ziper
    {

        public static async Task UnzipAsync(string zipFilePath, string extractPath, ProgressBarUpdtaeHandler progressBarUpdtaeHandler, CancellationToken token)
        {
            using ZipArchive zip = ZipFile.Open(zipFilePath, ZipArchiveMode.Read);
            for (int i = 0; i < zip.Entries.Count; i++)
            {
                string fullEntryPath = Path.Combine(extractPath, zip.Entries[i].FullName);
                string directory = Path.GetDirectoryName(fullEntryPath);

                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                //Если это директория, игрнорируем ее в записи
                if (zip.Entries[i].FullName.EndsWith("/")) continue;

                string filePath = Path.Combine(directory, Path.GetFileName(fullEntryPath));

                zip.Entries[i].ExtractToFile(filePath);
                var e = new UpdateData()
                {
                    ProcessedData = i,
                    TotalData = zip.Entries.Count,
                };
                progressBarUpdtaeHandler?.Invoke(null, e);
            }
        }
    }
}
