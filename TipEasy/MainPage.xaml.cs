using System.Text.RegularExpressions;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
        public string zipCode;
        public double total;
        public double tipAmount, taxAmount;
        public Windows.Storage.ApplicationDataContainer localSettings;

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
            
            localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            object zipObj = localSettings.Values["zipcode"];

            if (zipObj == null) {
                taxRate = 0.08875;
            }
            else
            {
                zipCode = zipObj.ToString();
                System.Diagnostics.Debug.WriteLine("%s", zipCode);
                taxRate = 0.0000;
            }

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
                amount = 0;
            }
            else
            {
                amount = double.Parse(text);
            }
            recalculate();
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

            total = amount + taxAmount + tipAmount;
        }

        private void refreshUI()
        {
            uitaxRate.Text = taxAmount.ToString("N");

            uiTipAmount.Text = "$" + tipAmount.ToString("N");
            uiTotalAmount.Text = "$" + total.ToString("N");
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Amount.Focus(FocusState.Pointer);
        }

        private void uiZipcode_LostFocus(object sender, RoutedEventArgs e)
        {
            var zip = uiZipcode.Text;
            System.Diagnostics.Debug.WriteLine(zip);
            if (this.validateZipcode(zip))
            {
                localSettings.Values["zipcode"] = zip;
                zipCode = zip;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Invalid zip code: " + zipCode);
            }
        }

        private bool validateZipcode(string code)
        {
            return Regex.IsMatch(code, @"\d\d\d\d\d");
        }
    }
}
