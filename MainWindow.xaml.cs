using System.Windows;
using System.IO;
using System.Diagnostics;

namespace OwlStudio
{
    public partial class MainWindow : Window
    {
        ProgressBarUpdateHandler progressBarUpdtaeHandler;
        private static string downloadPath = Path.Combine(Environment.CurrentDirectory, "Download");
        private static string archiveName = "ULTRAKILLNewDemoPatch1B.zip";
        private static string gamePath = Path.Combine(Environment.CurrentDirectory, "Game");
        private static string downloadFileName = Path.Combine(downloadPath, archiveName);
        private static int gameReady = 0;
        bool pressed = false;
        CancellationTokenSource token;

        public MainWindow()
        {
            InitializeComponent();
            ResetBar();
            progressBarUpdtaeHandler += UpdateBar;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (gameReady == 1)
            {
                Process.Start(Path.Combine(gamePath, "ULTRAKILL.exe"));
            }
            else if (!pressed)
            {
                DownloadBar.Visibility = Visibility.Visible;
                pressed = true;
                token = new CancellationTokenSource();
                int res = await Downloader.DownloadFileAsync(downloadPath, downloadFileName, progressBarUpdtaeHandler, token.Token);
                if (res == 0) res = await Zipper.Unzip(downloadFileName, gamePath, progressBarUpdtaeHandler, token.Token);
                if (res == 0) GetReadyToGame();
            }
            else
            {
                pressed = false;
                token?.Cancel();
                ResetBar();
            }
        }

        private void UpdateBar(object sender, UpdateData e)
        {

            B1.Content = "Отменить";
            if (e.Mode == 1)
            {
                DownloadBar.Value = (float)e.ProcessedData / e.TotalData * 100;
                L1.Content = $"Скачано: {e.ProcessedData / (1024 * 1024):F2} MB / {e.TotalData / (1024 * 1024):F2} MB";
            }
            else if (e.Mode == 2)
            {
                DownloadBar.Value = (float)e.ProcessedData / e.TotalData * 100;
                L1.Content = $"Распакованно: {e.ProcessedData} / из: {e.TotalData}";
            }
        }
        private void ResetBar()
        {

            DownloadBar.Value = 0;
            DownloadBar.Visibility = Visibility.Collapsed;
            GameDirB.Visibility = Visibility.Collapsed;
            L1.Content = string.Empty;
            B1.Content = "Загрузить";
        }
        private void GetReadyToGame()
        {
            pressed = false;
            gameReady = 1;
            ResetBar();
            GameDirB.Visibility = Visibility.Visible;
            B1.Content = "Играть";
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
        private void OpenGameFolderButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", gamePath);
        }
    }
}