using System.IO;
using System.Net.Http;
using System.Windows;
using static OwlStudio.MainWindow;

namespace OwlStudio
{
    public static class Downloader
    {
        public static async Task DownloadFileAsync(string downloadPath, string destinationPath, ProgressBarUpdtaeHandler progressBarUpdtaeHandler, CancellationToken token)
        {
            string url = "https://s654sas.storage.yandex.net/rdisk/64792642cc53e5764199bbb7bbc5816f3598aafcd6d0ad67bfdcae0ae6fc7b8d/65abd489/g3_u9dEOHjd_Zc1L4wdQcO2z7PM44OZqRy1Dvu89s7tgPkcIeD0ldxbp4CNYxxfeKk9XhIJofoU3lJDuKAuN9w==?uid=574548681&filename=ULTRAKILLNewDemoPatch1B.zip&disposition=attachment&hash=&limit=0&content_type=application%2Fzip&owner_uid=574548681&fsize=136324427&hid=0b1aa6e540b401867b2967c2da381f7c&media_type=compressed&tknv=v2&etag=c4c9d5e8bdff460b4e8fdbd77918c324&rtoken=iQW36k7hau4q&force_default=yes&ycrid=na-622ad028b34ebb00b8398d348e7f27f2-downloader6e&ts=60f612bc77440&s=018739daf41f07f0a0cd248f37bbffd6d3af183d07ab1b643a303f852c8ac11d&pb=U2FsdGVkX19JDxfAMKIJdq3Lk6-kjWlG8gxtzUvi9B3Nxnje83FQ1ZOlDrSMgfORySCgPDB6Q89y9dHLz-x4kUK2yEKRvpKd2YugXkNAk1g";
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                if (!response.IsSuccessStatusCode)
                {
                    System.Windows.MessageBox.Show("Нет доступа к ресурсу");
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
