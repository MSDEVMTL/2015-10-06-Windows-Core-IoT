
using System;
using Windows.Devices.Gpio;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace Blinky
{
	/// <summary>
	/// Demo #1
	/// MSDEVMTL - October 6 2015
	/// http://www.meetup.com/fr/msdevmtl/events/223839787/
	/// 
	/// 
	/// This UWP project purpose is to demonstrate how to use the Windows 10 GPIO namespace to blink the red LED
	/// on a FEZ Hat board attached to a Rapsberry Pi 2.
	/// 
	/// https://www.ghielectronics.com/catalog/product/500
	/// 
	/// </summary>
	public sealed partial class MainPage : Page
    {
        private const int LED_PIN = 24; //Red LED
        private GpioPin pin;
        private GpioPinValue pinValue;
        private DispatcherTimer timer;
        private SolidColorBrush redBrush = new SolidColorBrush(Windows.UI.Colors.Red);
        private SolidColorBrush grayBrush = new SolidColorBrush(Windows.UI.Colors.LightGray);

        public MainPage()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
            InitGPIO();

            if (pin != null)
            {
                timer.Start();
            }        
        }

        private void InitGPIO()
        {
            var gpio = GpioController.GetDefault();

            // Show an error if there is no GPIO controller
            if (gpio == null)
            {
                pin = null;
                GpioStatus.Text = "There is no GPIO controller on this device.";
                return;
            }

            pin = gpio.OpenPin(LED_PIN);
            pinValue = GpioPinValue.High;
            pin.Write(pinValue);
            pin.SetDriveMode(GpioPinDriveMode.Output);

            GpioStatus.Text = "GPIO pin initialized correctly.";

        }

   
		/// <summary>
		/// Change the red LED value every 1/2 second
		/// </summary>
        private void Timer_Tick(object sender, object e)
        {
            if (pinValue == GpioPinValue.High)
            {
                pinValue = GpioPinValue.Low;
                pin.Write(pinValue);
                LED.Fill = redBrush;  //UI
            }
            else
            {
                pinValue = GpioPinValue.High;
                pin.Write(pinValue);
                LED.Fill = grayBrush;  //UI
            }
        }
             

    }
}
