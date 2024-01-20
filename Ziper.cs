using System.IO;
using System.IO.Compression;
using static OwlStudio.MainWindow;

namespace OwlStudio
{
    public static class Ziper
    {

        public static async Task UnzipAsync(string zipFilePath, string extractPath, ProgressBarUpdtaeHandler progressBarUpdtaeHandler, CancellationToken token)
        {
            using (FileStream zipToOpen = new FileStream(zipFilePath, FileMode.Open))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
                {
                    int totalEntries = archive.Entries.Count;
                    int processedEntries = 0;

                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        string fullEntryPath = Path.Combine(extractPath, entry.FullName);
                        string directory = Path.GetDirectoryName(fullEntryPath);

                        if (!Directory.Exists(directory))
                            Directory.CreateDirectory(directory);

                        //Если это директория, игрнорируем ее в записи
                        if (entry.FullName.EndsWith("/"))
                        {
							processedEntries++;
                            continue;
						}

                        string filePath = Path.Combine(directory, Path.GetFileName(fullEntryPath));

                        using (Stream entryStream = entry.Open())
                        using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                        {
                            await entryStream.CopyToAsync(fileStream);
                        }

                        processedEntries++;
                        var e = new UpdateData()
                        {
                            BytesReceived = processedEntries,
                            TotalBytesToReceive = totalEntries,
                        };
                        progressBarUpdtaeHandler?.Invoke(null, e);
                    }
                }
            }
        }
    }
}
