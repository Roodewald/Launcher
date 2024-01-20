using System.Windows;
using System.IO;

namespace OwlStudio
{
    public partial class MainWindow : Window
    {
		public delegate void ProgressBarUpdtaeHandler(object sender, UpdateData e);

        ProgressBarUpdtaeHandler progressBarUpdtaeHandler;
        private static string downloadPath = Path.Combine(Environment.CurrentDirectory, "Download");
        private static string archiveName = "ULTRAKILLNewDemoPatch1B.zip";
        private static string gamePath = Path.Combine(Environment.CurrentDirectory, "Game");
        private static string downloadFileName = Path.Combine(downloadPath, archiveName);

        public MainWindow()
        {
            InitializeComponent();
			progressBarUpdtaeHandler = (sender, e) =>
            {
                B1.Content = "Отменить";
                DownloadBar.Value = (float)e.BytesReceived / e.TotalBytesToReceive * 100;
                L1.Content = $"Скачано: {e.BytesReceived / (1024 * 1024):F2} MB / {e.TotalBytesToReceive / (1024 * 1024):F2} MB";

			};
        }
        bool pressed = false;
        CancellationTokenSource token;

		private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!pressed)
            {
				pressed = true;
					token = new CancellationTokenSource();
					await Downloader.DownloadFileAsync(downloadPath, downloadFileName, progressBarUpdtaeHandler,token.Token);
					await Ziper.UnzipAsync(downloadFileName,gamePath,progressBarUpdtaeHandler,token.Token);
			}
            else
            {
                DownloadBar.Value = 0;
				pressed = false;
                token?.Cancel();
            }
            
        }
		public class UpdateData : EventArgs
        {
            public long BytesReceived { get; set; }
            public long TotalBytesToReceive { get; set; }
        }

		private void OpenFolderDialogButton_Click(object sender, RoutedEventArgs e)
		{
			
			FolderBrowserDialog folderDialog = new FolderBrowserDialog();	
			DialogResult result = folderDialog.ShowDialog();

			
			if (result == System.Windows.Forms.DialogResult.OK)
			{
				string selectedFolder = folderDialog.SelectedPath;
				gamePath = Path.Combine(selectedFolder, "Game");
			}
		}


		/*  Stages:
         *  0 - Game is not downloaded of found
         *  1 - Archive is downloaded now;
         *  2 - Archive dowload complite
         *  3 - Unzip
         *  4 - GeadyToPlay
         */
	}
}