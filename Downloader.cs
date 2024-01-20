using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Windows;
using static OwlStudio.MainWindow;

namespace OwlStudio
{
    public static class Downloader
    {
        public static async Task DownloadFileAsync(string downloadPath, string destinationPath, ProgressBarUpdtaeHandler progressBarUpdtaeHandler, CancellationToken token)
        {
            string url = "https://s654sas.storage.yandex.net/rdisk/be66ff0df629bf2791fb763888c88209a658ca262a8d0db253a6ba377f4b33c0/65ab77a5/g3_u9dEOHjd_Zc1L4wdQcO2z7PM44OZqRy1Dvu89s7tgPkcIeD0ldxbp4CNYxxfeKk9XhIJofoU3lJDuKAuN9w==?uid=574548681&filename=ULTRAKILLNewDemoPatch1B.zip&disposition=attachment&hash=&limit=0&content_type=application%2Fzip&owner_uid=574548681&fsize=136324427&hid=0b1aa6e540b401867b2967c2da381f7c&media_type=compressed&tknv=v2&etag=c4c9d5e8bdff460b4e8fdbd77918c324&rtoken=1e9J6YaiudSw&force_default=yes&ycrid=na-d7385c69230d8a95dad3ed471564d1aa-downloader11f&ts=60f5ba2617340&s=4bcb707c86788d98f482bf8047e0ebc4765d6628f8648861655f184c04140387&pb=U2FsdGVkX1_qFUzo6cm0NjwGTloL0_7QXrg29ymg-GwF_XOxybp787RCcraFuEEHbVsPVvw_Ju00s7d10rXfZaEvvk7kH08dPDky6OKbluw";
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Нет доступа к ресурсу");
                }
                if (!Directory.Exists(downloadPath))
                {
                    Directory.CreateDirectory(downloadPath);
                }

                using (Stream contentStream = await response.Content.ReadAsStreamAsync(),
                              stream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                {
                    byte[] buffer = new byte[8192];
                    int bytesRead;
                    long totalBytesRead = 0;

                    while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        if (token.IsCancellationRequested)
                        {
                            stream.Dispose();
                            File.Delete(destinationPath);
                            return;
                        }
                        await stream.WriteAsync(buffer, 0, bytesRead);

                        // Обновить статус загрузки
                        totalBytesRead += bytesRead;
                        UpdateData e = new()
                        {
                            BytesReceived = totalBytesRead,
                            TotalBytesToReceive = response.Content.Headers.ContentLength.GetValueOrDefault()
                        };
                        progressBarUpdtaeHandler?.Invoke(null, e);
                    }
                }
            }
        }
    }
}
