using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace TipEasy
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private double amount;
        public double[] tipRates;
        public double tipRate;
        public double taxRate;
        public double total;
        public double tipAmount, taxAmount;

        public MainPage()
        {
            //defaults
            tipRates = new double[] { 0.15, 0.18, 0.20 };
            this.InitializeComponent();
            TipRate1.Content = tipRates[0].ToString("P");
            TipRate2.Content = tipRates[1].ToString("P");
            TipRate2.IsChecked = true;
            TipRate3.Content = tipRates[2].ToString("P");
            tipRate = tipRates[1];
            taxRate = 0.08875;



            this.NavigationCacheMode = NavigationCacheMode.Required;

        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void Amount_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = ((TextBox)sender).Text;
            if (text.Length == 0)
            {
                uitaxRate.Text = uitaxRate.PlaceholderText;
                return;
            }
            amount = double.Parse(text);
            recalculate();



            total = amount * (1 + taxRate + tipRate);

            refreshUI();

        }

        private void TipRate1_Checked(object sender, RoutedEventArgs e)
        {
            tipRate = tipRates[0];
            recalculate();
            refreshUI();
        }

        private void TipRate3_Checked(object sender, RoutedEventArgs e)
        {
            tipRate = tipRates[2]; 
            recalculate();
            refreshUI();
        }

        private void TipRate2_Checked(object sender, RoutedEventArgs e)
        {
            tipRate = tipRates[1];
            recalculate();
            refreshUI();
        }

        private void recalculate()
        {
            taxAmount = amount * taxRate;

            tipAmount = amount * tipRate;

            total = amount * (1 + taxRate + tipRate);
        }

        private void refreshUI()
        {
            uitaxRate.Text = taxAmount.ToString("N");

            uiTipAmount.Text = "$" + tipAmount.ToString("N");
        }

    }
}
