using App2.View;
using Plugin.SewooXamarinSDK;
using Plugin.SewooXamarinSDK.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App2.Model
{
    public class PrintSerwis
    {

        CPCLConst cpclConst;
        int iResult;
        bool CanPrint;
        int koniecLinii;
        int polozenie;
        bool printSukces;


        private static SemaphoreSlim printSemaphore = new SemaphoreSlim(1, 1);


        public PrintSerwis()
        {
            cpclConst = new CPCLConst();

        }


        public async Task<bool> ConnToPrinter()
        {
            bool connected =false;
            SettingsPage._cpclPrinter = CrossSewooXamarinSDK.Current.createCpclService((int)CodePages.LK_CODEPAGE_ISO_8859_2);

 
            try
            {
                if (!SettingsPage.CzyDrukarkaOn)
                {
                    string adresDrukarki = string.IsNullOrEmpty(SettingsPage.editAddress.Text) ? "00:00:00:00" : SettingsPage.editAddress.Text;
                    iResult = await SettingsPage._cpclPrinter.connect(adresDrukarki);
                    if (iResult == cpclConst.LK_SUCCESS)
                    {
                        SettingsPage.CzyDrukarkaOn = true;
                        CanPrint = true;
                        connected = true;
                    }
                    else
                    {
                        CanPrint = false; 

                        ErrorStatusDisp("Błąd  drukarki", iResult);
                        SettingsPage.CzyDrukarkaOn = false;

                    }

                }
                else
                {
                    //CanPrint = true;
                    //View.SettingsPage.CzyDrukarkaOn = true;
                    connected = true;

                }
            }
            catch (Exception)
            {
                connected = false;
            }

            return connected;

        }



        public async Task<bool> PrintCommand(TwrKarty _akcja,string kolor, string ile = null)
        {
            int drukSzt;

            if (!string.IsNullOrEmpty(ile))
                drukSzt = Convert.ToInt16(ile);
            else
                drukSzt = 1;


            if(string.IsNullOrEmpty(_akcja.twr_cena1))
            {
                string Webquery = "cdn.pc_pobierztwr '" + _akcja.twr_ean + "'";
                var dane = await App.TodoManager.PobierzTwrAsync(Webquery);
                _akcja.twr_cena1 = dane[0].cena1;
            }

            await printSemaphore.WaitAsync();
            try
            {
                if (SettingsPage.CzyCenaPierwsza)
                    _akcja.twr_cena = _akcja.twr_cena1;

                var cenaZl = _akcja.twr_cena.Substring(0, _akcja.twr_cena.IndexOf(".", 0));
                var cenaGr = _akcja.twr_cena.Substring(_akcja.twr_cena.IndexOf(".", 0) + 1, 2);


                switch (cenaZl.Length)
                {
                    case 1:
                        polozenie = 125;

                        break;

                    case 2:
                        polozenie = 110;

                        break;

                    case 3:
                        polozenie = 85;

                        break;

                    default:
                        polozenie = 100;
                        break;
                }


                switch (_akcja.twr_cena1.Length)
                {
                    case 4:

                        koniecLinii = 130;
                        break;

                    case 5:

                        koniecLinii = 150;
                        break;

                    case 6:

                        koniecLinii = 170;
                        break;

                    default:
                        polozenie = 150;
                        break;
                }
                //= (_akcja.TwrCena1.Length <= 5) ? 155 : 165;


                string twr_nazwa = (_akcja.twr_nazwa.Length > 20 ? _akcja.twr_nazwa.Substring(0, 20) : _akcja.twr_nazwa);

                string procent = Convert.ToInt16((Double.Parse(_akcja.twr_cena.Replace(".", ",")) / Double.Parse(_akcja.twr_cena1.Replace(".", ",")) - 1) * 100).ToString();

                if (kolor=="biały")
                    await SettingsPage._cpclPrinter.setForm(0, 200, 200, 270, 350, drukSzt);
                else
                    await SettingsPage._cpclPrinter.setForm(0, 200, 200, 370, 350, drukSzt);

                await SettingsPage._cpclPrinter.setBarCodeText(7, 0, 0);
                await SettingsPage._cpclPrinter.setMedia(cpclConst.LK_MEDIA_LABEL);

            
                await SettingsPage._cpclPrinter.setCodePage((int)CodePages.LK_CODEPAGE_ISO_8859_2);


                await SettingsPage._cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_7, 0, 50, 5, _akcja.twr_kod, 0);
                await SettingsPage._cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_7, 0, 50, 30, twr_nazwa, 0);
                await SettingsPage._cpclPrinter.print1dBarCode(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_BCS_EAN13, 1,
                        cpclConst.LK_CPCL_BCS_0RATIO, 40, 80, 55, _akcja.twr_ean, 0);

                if (kolor!="biały")
                {
                    await SettingsPage._cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_4, 0, 50, 115, _akcja.twr_cena1, 0);
                    await SettingsPage._cpclPrinter.printLine(40, 160, koniecLinii, 125, 10);
                    if (Convert.ToInt16(procent) < -15)
                        await SettingsPage._cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_7, 0, 50, 245, $"{procent}%", 0);

                }
                int YpolozenieCeny;
               
                if (kolor == "biały")
                    YpolozenieCeny = 115;
                else
                    YpolozenieCeny = 160;
                await SettingsPage._cpclPrinter.setConcat(cpclConst.LK_CPCL_CONCAT, polozenie, YpolozenieCeny);
                await SettingsPage._cpclPrinter.concatText(cpclConst.LK_CPCL_FONT_4, 3, 0, cenaZl);
                await SettingsPage._cpclPrinter.concatText(cpclConst.LK_CPCL_FONT_4, 0, 0, cenaGr); 
                                                                                                   
                await SettingsPage._cpclPrinter.concatText(cpclConst.LK_CPCL_FONT_7, 0, 60, "zł");
                await SettingsPage._cpclPrinter.resetConcat();



                await SettingsPage._cpclPrinter.printForm();



                iResult = await SettingsPage._cpclPrinter.printResults();
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        iResult = await SettingsPage._cpclPrinter.printStatus();
                        //Debug.WriteLine("PrinterStatus = " + iResult);
                        break;
                    default:
                        //Debug.WriteLine("PrinterResults = " + iResult);
                        break;
                }
                if (iResult != cpclConst.LK_SUCCESS)
                {
                    ErrorStatusDisp("Wydruk nieudany", iResult);
                    printSukces = false;
                }
                else
                {
                    //await DisplayAlert("Printing Result", "Printing success", "OK");

                    printSukces = true;

                }
            }
            catch (Exception e)
            {
                e.StackTrace.ToString();
                //await DisplayAlert("Exception", e.Message.ToString(), "OK");
            }
            finally
            {
                printSemaphore.Release();
            }

            return printSukces;


        }

        string ErrorStatusDisp(string strStatus, int errCode)
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


                 
            }
            else
            {
                errMsg += "Nie znaleziono drukarki\n";
            }
            return errMsg;
        }

    }
}
