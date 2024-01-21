using System.IO;
using System.Net.Http;

namespace OwlStudio
{
    public static class Downloader
    {
        public static async Task<int> DownloadFileAsync(string downloadPath, string destinationPath, ProgressBarUpdateHandler progressBarUpdtaeHandler, CancellationToken token)
        {
            if (!Directory.Exists(downloadPath))
            {
                Directory.CreateDirectory(downloadPath);
            }
            if (File.Exists(Path.Combine(destinationPath)))
            {
                return 0;
            }

            string url = "https://getfile.dokpub.com/yandex/get/https://disk.yandex.ru/d/8ASLEMHdPfB6jQ";
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            using (Stream contentStream = await response.Content.ReadAsStreamAsync(),
            stream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
            {
                if (!response.IsSuccessStatusCode)
                {
                    System.Windows.MessageBox.Show("Нет доступа к ресурсу");
                    return 1;
                }

                byte[] buffer = new byte[8192];
                int bytesRead;
                long totalBytesRead = 0;

                while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    if (token.IsCancellationRequested)
                    {
                        stream.Dispose();
                        File.Delete(destinationPath);
                        return 1;
                    }
                    await stream.WriteAsync(buffer, 0, bytesRead);

                    // Обновить статус загрузки
                    totalBytesRead += bytesRead;
                    UpdateData e = new()
                    {
                        ProcessedData = totalBytesRead,
                        TotalData = response.Content.Headers.ContentLength.GetValueOrDefault(),
                        Mode = 1
                    };
                    progressBarUpdtaeHandler?.Invoke(null, e);
                }
            }
            return 0;
        }
    }
}