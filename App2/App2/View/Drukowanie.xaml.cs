using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.SewooXamarinSDK;
using Plugin.SewooXamarinSDK.Abstractions;
using System.Diagnostics;
using System.Threading;

namespace App2.View
{
    public partial class Drukowanie : ContentPage
    {
        private ISewooXamarinCPCL _cpclPrinter;
        CPCLConst cpclConst;
        private Picker mediaTypePicker;

        private Editor editAddress;
        private Button connectButton;
        private Button disconnectButton;
        private Button printText;
    
        private Model.AkcjeNagElem _akcja;
        private static SemaphoreSlim printSemaphore = new SemaphoreSlim(1, 1);

        public Drukowanie(Model.AkcjeNagElem akcje)
        {
            InitializeComponent();
            _akcja = akcje;
            // Initialize sample app UI.
            InitializeSampleUI();
        }

        private async void InitializeSampleUI()
        {
            //            _cpclPrinter = CrossSewooXamarinSDK.Current.createCpclService();
            _cpclPrinter = CrossSewooXamarinSDK.Current.createCpclService((int)CodePages.LK_CODEPAGE_ISO_8859_2);

            printText = this.FindByName<Button>("btnPrintText");
           
          
            printText.IsEnabled = false;
         

            connectButton = this.FindByName<Button>("btnConnect");
            disconnectButton = this.FindByName<Button>("btnDisconnect");
            editAddress = this.FindByName<Editor>("txtAddress");
            mediaTypePicker = this.FindByName<Picker>("cboMediaType");
            mediaTypePicker.SelectedIndex = 0; // Gap label default.
            connectButton.IsEnabled = true;
            disconnectButton.IsEnabled = false;
            var deviceList = await _cpclPrinter.connectableDevice();
            connectableDeviceView.ItemsSource = deviceList;
            if (deviceList.Count == 0)
            {
                editAddress.IsEnabled = true;
                editAddress.Text = "00:00:00:00:00:00";
            }
            else
            {
                editAddress.IsEnabled = false;
                editAddress.Text = deviceList.ElementAt(0).Address;
            }

            cpclConst = new CPCLConst();
        }

        async void btnConnectClicked(object sender, EventArgs args)
        {
            int iResult;
            await printSemaphore.WaitAsync();
            try
            {
                iResult = await _cpclPrinter.connect(editAddress.Text);
                Debug.WriteLine("connect = " + iResult);
                if (iResult == cpclConst.LK_SUCCESS)
                {
                    connectableDeviceView.IsEnabled = false;
                    connectButton.IsEnabled = false;
                    disconnectButton.IsEnabled = true;
                    printText.IsEnabled = true;
        
                }
                else
                {
                    await DisplayAlert("Connection", "Connection failed", "OK");
                }
            }
            catch (Exception e)
            {
                e.StackTrace.ToString();
                await DisplayAlert("Exception", e.Message.ToString(), "OK");
            }
            finally
            {
                printSemaphore.Release();
            }

        }

        async void btnDisconnectClicked(object sender, EventArgs args)
        {
            await printSemaphore.WaitAsync();
            try
            {
                await _cpclPrinter.disconnect();

                connectableDeviceView.IsEnabled = true;
                connectButton.IsEnabled = true;
                disconnectButton.IsEnabled = false;
                printText.IsEnabled = false;
                
            }
            catch (Exception e)
            {
                e.StackTrace.ToString();
                await DisplayAlert("Exception", e.Message.ToString(), "OK");
            }
            finally
            {
                printSemaphore.Release();
            }
        }

        async void ErrorStatusDisp(string strStatus, int errCode)
        {
            string errMsg = string.Empty;

            if (errCode > 0)
            {
                if ((errCode & cpclConst.LK_STS_CPCL_BATTERY_LOW) > 0)
                    errMsg += "Battery Low\n";
                if ((errCode & cpclConst.LK_STS_CPCL_COVER_OPEN) > 0)
                    errMsg += "Cover Open\n";
                if ((errCode & cpclConst.LK_STS_CPCL_PAPER_EMPTY) > 0)
                    errMsg += "Paper Empty\n";
                if ((errCode & cpclConst.LK_STS_CPCL_POWEROFF) > 0)
                    errMsg += "Power OFF\n";
                if ((errCode & cpclConst.LK_STS_CPCL_TIMEOUT) > 0)
                    errMsg += "Timeout\n";
                await DisplayAlert(strStatus, errMsg, "OK");
            }
            else
            {
                await DisplayAlert(strStatus, "Return value = " + errCode, "OK");
            }
        }

        async void btnPrintTextClicked(object sender, EventArgs args)
        {
            await printSemaphore.WaitAsync();
            try
            {
                int iResult;

                iResult = await _cpclPrinter.printerCheck();
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        iResult = await _cpclPrinter.printStatus();
                        Debug.WriteLine("PrinterStatus = " + iResult);
                        break;
                    default:
                        Debug.WriteLine("printerCheck = " + iResult);
                        break;
                }

                if (iResult != cpclConst.LK_SUCCESS)
                {
                    ErrorStatusDisp("Printer error", iResult);
                    return;
                }

                
                string twr_nazwa = (_akcja.TwrNazwa.Length > 20 ? _akcja.TwrNazwa.Substring(0, 20) : _akcja.TwrNazwa);
                await _cpclPrinter.setForm(0, 200, 200, 370, 350, 1);
                await _cpclPrinter.setBarCodeText(0, 1, 0);
                await _cpclPrinter.setMedia(cpclConst.LK_MEDIA_LABEL);

                // Set the code page of font number(7) 
                await _cpclPrinter.setCodePage((int)CodePages.LK_CODEPAGE_ISO_8859_2);

                await _cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_7, 0, 111, 0, "Łóżżko", 0);
                await _cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_7, 0, 45, 25, _akcja.TwrKod, 0);
                await _cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_7, 0, 45, 50, twr_nazwa, 0);
                await _cpclPrinter.print1dBarCode(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_BCS_EAN13, 1,
    cpclConst.LK_CPCL_BCS_0RATIO, 35, 70, 80, _akcja.TwrEan, 0);

                await _cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_4, 0, 50, 125, _akcja.TwrCena1, 0);
                //await _cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_4, 1, polozenie, 160, cenaZl, 0);
                //await _cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_4, 0, 158, 160, cenaGr, 0);

                // await _cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_4, 0, 220, 190, "pln", 0);
                await _cpclPrinter.printLine(40, 165, 180, 135, 10);

                await _cpclPrinter.printForm();



                iResult = await _cpclPrinter.printResults();
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        iResult = await _cpclPrinter.printStatus();
                        Debug.WriteLine("PrinterStatus = " + iResult);
                        break;
                    default:
                        Debug.WriteLine("PrinterResults = " + iResult);
                        break;
                }
                if (iResult != cpclConst.LK_SUCCESS)
                {
                    ErrorStatusDisp("Printing error", iResult);
                }
                else
                {
                    await DisplayAlert("Printing Result", "Printing success", "OK");
                }
            }
            catch (Exception e)
            {
                e.StackTrace.ToString();
                await DisplayAlert("Exception", e.Message.ToString(), "OK");
            }
            finally
            {
                printSemaphore.Release();
            }
        }

         
         
         

        async void OnConnectableDeviceSelection(object sender, SelectedItemChangedEventArgs e)
        {
            int iResult;

            if (e.SelectedItem == null)
                return;

            await printSemaphore.WaitAsync();
            try
            {
                var info = e.SelectedItem as connectableDeviceInfo;

                if (info != null)
                {
                    Debug.WriteLine("Selected device address = " + info.Address.ToString());
                    iResult = await _cpclPrinter.connect(info.Address.ToString());
                    editAddress.Text = info.Address.ToString();
                    if (iResult == cpclConst.LK_SUCCESS)
                    {
                        connectButton.IsEnabled = false;
                        disconnectButton.IsEnabled = true;

                        connectableDeviceView.IsEnabled = false;

                        printText.IsEnabled = true;
                    
                    }
                    else
                    {
                        await DisplayAlert("Connection", "Connection failed", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                ex.StackTrace.ToString();
                await DisplayAlert("Exception", ex.Message.ToString(), "OK");
            }
            finally
            {
                printSemaphore.Release();
            }
        }
    }
}
