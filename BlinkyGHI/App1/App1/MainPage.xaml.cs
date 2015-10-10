using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GHIElectronics.UWP.Shields;


namespace App1
{
	/// <summary>
	/// Demo #2
	/// MSDEVMTL - October 6 2015
	/// http://www.meetup.com/fr/msdevmtl/events/223839787/
	/// 
	/// 
	/// This UWP project purpose is to demonstrate how to use the Windows 10 GPIO namespace to blink the red LED
	/// on a FEZ Hat board attached to a Rapsberry Pi 2 this time, using GHI's driver code.
	/// 
	/// https://www.ghielectronics.com/catalog/product/500
	/// 
	/// </summary>
	public sealed partial class MainPage : Page
    {
		
		private FEZHAT hat;
		private DispatcherTimer timer;
		private bool next;
		private int i;

		public MainPage()
        {
            InitializeComponent();
			Setup();
        }



		private async void Setup()
		{
			hat = await FEZHAT.CreateAsync();

			hat.S1.SetLimits(500, 2400, 0, 180);
			hat.S2.SetLimits(500, 2400, 0, 180);

			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromMilliseconds(100);
			timer.Tick += OnTick;
			
		}

		/// <summary>
		/// Start/Stop the timer
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Button_Click(object sender, RoutedEventArgs e)
		{

			if (timer.IsEnabled)
			{
				hat.DIO24On = false;
				hat.D2.Color = FEZHAT.Color.Black;
				hat.D3.Color = FEZHAT.Color.Black;
				timer.Stop();
				btnStartStop.Content = "Start";
			}
			else
			{
				timer.Start();
				btnStartStop.Content = "Stop";
			}

		}

		/// <summary>
		/// Tick every 100 milliseconds to retrieve the button's state
		/// but blink only every 1/2 second.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnTick(object sender, object e)
		{
			double x, y, z;

			hat.GetAcceleration(out x, out y, out z);

			LightTextBox.Text = hat.GetLightLevel().ToString("P2");
			TempTextBox.Text = hat.GetTemperature().ToString("N2");
			AccelTextBox.Text = $"({x:N2}, {y:N2}, {z:N2})";
			Button18TextBox.Text = hat.IsDIO18Pressed().ToString();
			Button22TextBox.Text = hat.IsDIO22Pressed().ToString();

			if ((i++ % 5) == 0)
			{
				LedsTextBox.Text = next.ToString();

				hat.DIO24On = next;
				hat.D2.Color = next ? FEZHAT.Color.White : FEZHAT.Color.Black;
				hat.D3.Color = next ? FEZHAT.Color.White : FEZHAT.Color.Black;

				next = !next;
			}

		}


	}
}
