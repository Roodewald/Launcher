using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Windows;

namespace OwlStudio
{
	public static class Downloader
	{
		public static async Task DownloadFromUrlAsync(string downloadPath, string fileName, DownloadProgressChangedEventHandler progressChangedHandler, AsyncCompletedEventHandler completedHandler)
		{
			string url = "https://s494vla.storage.yandex.net/rdisk/2c4425b22fbf7f997a544b51e3c9877fe600d270d8fc3339dc23a766f012759b/65aa9700/g3_u9dEOHjd_Zc1L4wdQcO2z7PM44OZqRy1Dvu89s7tgPkcIeD0ldxbp4CNYxxfeKk9XhIJofoU3lJDuKAuN9w==?uid=574548681&filename=ULTRAKILLNewDemoPatch1B.zip&disposition=attachment&hash=&limit=0&content_type=application%2Fzip&owner_uid=574548681&fsize=136324427&hid=0b1aa6e540b401867b2967c2da381f7c&media_type=compressed&tknv=v2&etag=c4c9d5e8bdff460b4e8fdbd77918c324&rtoken=cOvgpFNA9SHo&force_default=yes&ycrid=na-f2c7b98f1b63cdcb57ba4c3fae35dfca-downloader21h&ts=60f4e3e93c000&s=e9713700fccabb6b46cc9edf7326db04867ab3068c85b482bb74b8fba4590146&pb=U2FsdGVkX1-t4cBh4U7LgwO0PFNegsijEy-p2XYMI3Rd_thVAH_mKbL8o9SINv_SPABEhTsd_8DFnF-3oGIBwOknAhXsfZ7yf_WQ9aAbWfE";
			
			if (File.Exists(fileName))
			{
				return;
			}
			if (!Directory.Exists(downloadPath))
			{
				Directory.CreateDirectory(downloadPath);
			}
			using (HttpClient client = new HttpClient())
			{
				HttpResponseMessage headResponse = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));
				if (!headResponse.IsSuccessStatusCode)
				{
					MessageBox.Show("Файл не доступен для скачивания\n" + headResponse.StatusCode);
					return;
				}
			}


			using (WebClient client = new WebClient())
			{
				try
				{
					client.DownloadProgressChanged += progressChangedHandler;
					client.DownloadFileCompleted += completedHandler;

					// Асинхронно скачиваем файл
					await client.DownloadFileTaskAsync(new Uri(url), fileName);

				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error: {ex.Message}");
				}
			}
		}
	}
}
