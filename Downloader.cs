using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace OwlStudio
{
	public static class Downloader
	{


		public static async Task DownloadFromUrlAsync(DownloadProgressChangedEventHandler progressChangedHandler, AsyncCompletedEventHandler completedHandler)
		{
			string url = "https://s494vla.storage.yandex.net/rdisk/6e8c51b65d9da3f14029b16818183eca95a56f7ab6de5639dd4a3062cd666848/65aa22b7/g3_u9dEOHjd_Zc1L4wdQcO2z7PM44OZqRy1Dvu89s7tgPkcIeD0ldxbp4CNYxxfeKk9XhIJofoU3lJDuKAuN9w==?uid=574548681&filename=ULTRAKILLNewDemoPatch1B.zip&disposition=attachment&hash=&limit=0&content_type=application%2Fzip&owner_uid=574548681&fsize=136324427&hid=0b1aa6e540b401867b2967c2da381f7c&media_type=compressed&tknv=v2&etag=c4c9d5e8bdff460b4e8fdbd77918c324&rtoken=krpYVyUAqL4v&force_default=yes&ycrid=na-9c149e024a3b1d7da50c3a24040c34d1-downloader3f&ts=60f475034dbc0&s=c27745618ae90fd92892b54cb4269ba07d19657ffb0abcfd9f6fcd293f496797&pb=U2FsdGVkX1-FNhN2X5kSZljaXjHxnPvwUpHYwmiwQnjL4eUlREM5g13lREImg1GmVWhsM0o6hplXXIIWmel2zGJU1yJeUFwIkDeDDvp76Ws";
			string downloadPath = Path.Combine(Environment.CurrentDirectory, "Download");
			MessageBox.Show(downloadPath);
			if (!Directory.Exists(downloadPath))
			{
				Directory.CreateDirectory(downloadPath);
			}

			string fileName = Path.Combine(downloadPath, "ULTRAKILLNewDemoPatch1B.zip");

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


		private static void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			MessageBox.Show(e.ProgressPercentage.ToString());
		}
	}
}
