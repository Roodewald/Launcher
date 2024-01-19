using System.ComponentModel;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OwlStudio
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		DownloadProgressChangedEventHandler progressChangedHandle;
		AsyncCompletedEventHandler completedHandler;

		public MainWindow()
		{
			InitializeComponent();
			progressChangedHandle = (sender, e) =>
			{
				DownloadBar.Value = e.ProgressPercentage;
			};
		}


		private async void Button_Click(object sender, RoutedEventArgs e)
		{
			await Downloader.DownloadFromUrlAsync(progressChangedHandle,completedHandler);
		}
	}
}