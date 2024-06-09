using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace Persian_Date_Converter;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow {
	public MainWindow() {
		InitializeComponent();
	}

	private void ConvertPersianToGregorian(string value) {
		var pCal = new PersianCalendar();
		var pCul = new CultureInfo("fa-IR");
		var parts = value.Split("-/\\ ._*".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
		if (parts.Length == 3 && int.TryParse(parts[0], out var year) && int.TryParse(parts[1], out var month) && int.TryParse(parts[2], out var day)) {
			try {
				var date = new DateTime(year, month, day, pCal);
				Dispatcher.Invoke(() => {
					TxtYear.Text = year.ToString();
					TxtMonth.Text = month.ToString();
					TxtDay.Text = day.ToString();
					TxtBlkPersianDateString.Text = date.ToString("dddd، dd MMMM yyyy", pCul);
					DPGregorian.SelectedDate = date;
					TbGregorianDate.Text = date.ToString("yyyy-MM-dd");
					TbMessage.Text = "";
					if (ChkAutoCopy.IsChecked ?? false) SetClipboard(TbGregorianDate.Text);
				});
			} catch {
				Dispatcher.Invoke(() => {
					TbMessage.Text = "Invalid Persian Date";
				});
			}
		} else {
			Dispatcher.Invoke(() => {
				TbMessage.Text = "Invalid Persian Date";
			});
		}
	}

	private void TxtPersianDate_KeyUp(object sender, KeyEventArgs e) {
		if (e.Key == Key.Enter) {
			ConvertPersianToGregorian(TxtPersianDate.Text);
		}
	}

	private void BtnToday_Click(object sender, RoutedEventArgs e) {
		var pCul = new CultureInfo("fa-IR");
		var today = DateTime.Now;
		Dispatcher.Invoke(() => {
			TxtPersianDate.Text = today.ToString("yyyy-MM-dd", pCul);
		});
	}

	private void BtnConvertPersianToGregorian_Click(object sender, RoutedEventArgs e) {
		ConvertPersianToGregorian(TxtPersianDate.Text);
	}

	private void TbGregorianDate_OnMouseUp(object sender, MouseButtonEventArgs e) {
		if (e.ClickCount != 2) return;
		SetClipboard(TbGregorianDate.Text);
	}

	private void ChkAutoCopy_Checked(object sender, RoutedEventArgs e) {
		Settings.Default.AutoCopy = ChkAutoCopy.IsChecked ?? false;
		Settings.Default.Save();
	}


	private void ConvertGregorianToPersian() {
		if (DateTime.TryParse(DPGregorian.Text, out var gDate)) {
			var pCal = new PersianCalendar();
			var pCul = new CultureInfo("fa-IR");
			var year = pCal.GetYear(gDate);
			var month = pCal.GetMonth(gDate);
			var day = pCal.GetDayOfMonth(gDate);
			Dispatcher.Invoke(() => {
				TbGregorianDate.Text = gDate.ToString("yyyy-MM-dd");
				TxtYear.Text = year.ToString();
				TxtMonth.Text = month.ToString();
				TxtDay.Text = day.ToString();
				TxtBlkPersianDateString.Text = gDate.ToString("dddd، dd MMMM yyyy", pCul);
				TxtPersianDate.Text = gDate.ToString("yyyy-MM-dd", pCul);
				TbMessage.Text = "";
				if (ChkAutoCopy.IsChecked ?? false) SetClipboard(TxtPersianDate.Text);
			});
		} else {
			Dispatcher.Invoke(() => {
				TbMessage.Text = "Invalid Gregorian Date";
			});
		}
	}

	private void DPGregorian_KeyUp(object sender, KeyEventArgs e) {
		if (e.Key == Key.Enter) {
			ConvertGregorianToPersian();
		}
	}
	private void BtnConvertGregorianToPersian_Click(object sender, RoutedEventArgs e) {
		ConvertGregorianToPersian();
	}

	private static void SetClipboard(string value) {
		Clipboard.Clear();
		Clipboard.SetText(value);
	}
}