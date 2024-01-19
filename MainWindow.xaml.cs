using System.ComponentModel;
using System.Net;
using System.Windows;
using System.IO;

namespace OwlStudio
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		DownloadProgressChangedEventHandler progressChangedHandle;
		AsyncCompletedEventHandler completedHandler;
		private string downloadPath = Path.Combine(Environment.CurrentDirectory, "Download");
		private string archiveName = "ULTRAKILLNewDemoPatch1B.zip";
		private string gamePath = Path.Combine(Environment.CurrentDirectory, "Game");
		private string downloadFileName;
		
		public MainWindow()
		{
			InitializeComponent();
			progressChangedHandle = (sender, e) =>
			{
				B1.Content = "Отменить";
				DownloadBar.Value = e.ProgressPercentage;
				L1.Content = $"Скачано: {e.BytesReceived / (1024 * 1024):F2} MB / {e.TotalBytesToReceive / (1024 * 1024):F2} MB";
			};
		}

		private async void Button_Click(object sender, RoutedEventArgs e)
		{
			downloadFileName = Path.Combine(downloadPath, archiveName);
			await Downloader.DownloadFromUrlAsync(downloadPath, downloadFileName, progressChangedHandle, completedHandler);
			await Ziper.UnzipFile(downloadFileName, gamePath);
		}

	}
}