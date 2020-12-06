using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MatchGame
{
	using System.Windows.Threading;

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		DispatcherTimer timer = new DispatcherTimer();
		int tenthsOfSecondsElapsed;
		int bestScore = 0;
		int matchesFound;

		public MainWindow()
		{
			InitializeComponent();

			timer.Interval = TimeSpan.FromSeconds(0.1);
			timer.Tick += Timer_Tick;
			SetUpGame();
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			tenthsOfSecondsElapsed++;
			timeTextBlock.Text = (tenthsOfSecondsElapsed / 10.0).ToString("0.0s");
			if (matchesFound == 8)
			{
				timer.Stop();
				if ((bestScore == 0) || (tenthsOfSecondsElapsed < bestScore))
				{
					bestScore = tenthsOfSecondsElapsed;
					timeTextBlock.Foreground = Brushes.DarkGreen;
				}
				timeTextBlock.FontSize = 28;
				timeTextBlock.Text += " (Best " + (bestScore / 10.0).ToString("0.0s") + ") - Again?";
			}
		}

		private void SetUpGame()
		{
			List<string> animalEmoji = new List<string>()
			{
//				"🐙", "🐡", "🐘", "🐳", "🐪", "🦕", "🦘", "🦔",
				"🐙", "🐬", "🐘", "🐍", "🐪", "🦕", "🦘", "🦔",
				"🐙", "🐬", "🐘", "🐍", "🐪", "🦕", "🦘", "🦔",
			};

			Random random = new Random();

			foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
			{
				if (textBlock.Name != "timeTextBlock")
				{
					int index = random.Next(animalEmoji.Count());
					string nextEmoji = animalEmoji[index];
					textBlock.Text = nextEmoji;
					textBlock.Foreground = Brushes.Black;
					textBlock.Visibility = Visibility.Visible;
					animalEmoji.RemoveAt(index);
				}
			}

			timeTextBlock.FontSize = 36;
			timeTextBlock.Foreground = Brushes.Black;
			tenthsOfSecondsElapsed = 0;
			matchesFound = 0;
			timer.Start();
		}

		TextBlock lastTextBlockClicked = null;

		private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
		{
			TextBlock textBlock = sender as TextBlock;
			if (lastTextBlockClicked == null)
			{
				textBlock.Foreground = Brushes.Red;
				lastTextBlockClicked = textBlock;
			}
			else if ((textBlock.Text == lastTextBlockClicked.Text) && (textBlock != lastTextBlockClicked))
			{
				matchesFound++;
				lastTextBlockClicked.Visibility = Visibility.Hidden;
				textBlock.Visibility = Visibility.Hidden;
				lastTextBlockClicked = null;
			}
			else
			{
				lastTextBlockClicked.Foreground = Brushes.Black;
				lastTextBlockClicked = null;
			}
		}

		private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (matchesFound == 8)
			{
				SetUpGame();
			}
		}
	}
}
