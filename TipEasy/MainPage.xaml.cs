using System.Text.RegularExpressions;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System;
using System.IO;
using System.Collections.Generic;

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
        public Dictionary<string, double> taxTable;

        public MainPage()
        {
            //defaults
            tipRates = new double[] { 0.15, 0.18, 0.20 };
            taxTable = new Dictionary<string, double>();
            this.InitializeComponent();
            TipRate1.Content = tipRates[0].ToString("P");
            TipRate2.Content = tipRates[1].ToString("P");
            TipRate2.IsChecked = true;
            TipRate3.Content = tipRates[2].ToString("P");
            tipRate = tipRates[1];
            
            localSettings = ApplicationData.Current.LocalSettings;
            object zipObj = localSettings.Values["zipcode"];
            
            if (zipObj == null) {
                zipCode = "10001";
                taxRate = 0.08875;//New York
            }
            else
            {
                zipCode = zipObj.ToString();
                System.Diagnostics.Debug.WriteLine("Zipcode loaded: " + zipCode);
            }
            uiZipcode.Text = zipCode;

            //load tax rates
            loadTaxRateTable();
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
            if (taxAmount == 0.0)
            {
                uitaxRate.Text = taxRate.ToString("P");
            }
            else
            {
                uitaxRate.Text = taxAmount.ToString("N");
            }

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
                setTaxRateFromZipcode(zip);
                recalculate();
            }
            else
            {
                uiZipcode.Text = "Invalid zipcode";
            }
            refreshUI();
        }

        private bool validateZipcode(string code)
        {
            return Regex.IsMatch(code, @"\d\d\d\d\d");
        }

        private async System.Threading.Tasks.Task loadTaxRateTable()
        {
            StorageFolder storageFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            
            var taxFile = await storageFolder.GetFileAsync(@"Assets\zip_tax.csv");
            //System.Diagnostics.Debug.WriteLine("Loading tax table " + taxFile.ToString());
            var sr = new StreamReader(await taxFile.OpenStreamForReadAsync());
            // string text = await Windows.Storage.FileIO.ReadTextAsync(taxFile);
            string headerLine = sr.ReadLine();
            //System.Diagnostics.Debug.WriteLine(headerLine);
            string line;
            // read a line while we have a line to read
            while ((line = sr.ReadLine()) != null)
            {
                string[] dataString = line.Split(new char[] { ',' });
                //System.Diagnostics.Debug.WriteLine("DEBUG:" + line);
                if (dataString.Length < 2)
                {
                    continue;
                }
                //System.Diagnostics.Debug.WriteLine(dataString[0] + "Taxrate: " + dataString[1]);
                this.taxTable[dataString[0]] = double.Parse(dataString[1]);
            }

            setTaxRateFromZipcode(zipCode);
            
        }

        private void setTaxRateFromZipcode(string zip)
        {
            if (taxTable.ContainsKey(zip))
            {
                taxRate = taxTable[zip];
                // System.Diagnostics.Debug.WriteLine("DEBUG: Tax rate for zip code" + zipCode + " : " + taxTable[zipCode]);
            }
            else
            {
                uiZipcode.Text = "Invalid zipcode";
            }
            refreshUI();
        }
    }
}
