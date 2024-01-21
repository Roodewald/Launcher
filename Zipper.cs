using System.IO;
using System.IO.Compression;

namespace OwlStudio
{
    public static class Zipper
    {
        public static async Task<int> Unzip(string zipFilePath, string extractPath, ProgressBarUpdateHandler progressBarUpdtaeHandler, CancellationToken token)
        {
            using ZipArchive zip = ZipFile.Open(zipFilePath, ZipArchiveMode.Read);
            for (int i = 0; i < zip.Entries.Count; i++)
            {
                string fullEntryPath = Path.Combine(extractPath, zip.Entries[i].FullName);
                string directory = Path.GetDirectoryName(fullEntryPath);

                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                if (token.IsCancellationRequested)
                {
                    zip.Dispose();
                    Directory.Delete(extractPath,true);
                    return 1;
                }

                //Если это директория, игрнорируем ее в записи
                if (zip.Entries[i].FullName.EndsWith("/")) continue;

                string filePath = Path.Combine(directory, Path.GetFileName(fullEntryPath));

                int extractRes = await Task<int>.Run(() =>
                {
                    try
                    {
                        zip.Entries[i].ExtractToFile(filePath);
                    }
                    catch (IOException)
                    {
                        return 0;
                    }
                    catch (Exception)
                    {
                        return 2;
                    }
                    return -1;
                }, token);

                if (extractRes != -1) return extractRes;

                var e = new UpdateData()
                {
                    //Потому что счет с нуля
                    ProcessedData = i + 1,
                    TotalData = zip.Entries.Count,
                    Mode = 2
                };
                progressBarUpdtaeHandler?.Invoke(null, e);
            }
            return 0;
        }
    }
}