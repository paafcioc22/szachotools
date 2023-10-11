using App2.Model.ApiModel;
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



        public async Task<bool> PrintCommand(TwrInfo _akcja,string kolor, sbyte drukSzt=1 )
        {
           // int drukSzt;
            string typCodeEan = "";
            int przesuniecieDlaCode128=0;

            //if (!string.IsNullOrEmpty(ile))
            //    drukSzt = Convert.ToInt16(ile);
            //else
            //    drukSzt = 1;


            //todo : po co skoro pobieram okno wześnieej
            //if(string.IsNullOrEmpty(_akcja.Twr_Cena1))
            //{
            //    string Webquery = "cdn.pc_pobierztwr '" + _akcja.Twr_Ean + "'";
            //    var dane = await App.TodoManager.PobierzTwrAsync(Webquery);
            //    _akcja.Twr_Cena1 = dane.Twr_Cena1.ToString();
            //}

            await printSemaphore.WaitAsync();
            try
            {
                if (SettingsPage.CzyCenaPierwsza)
                    _akcja.Twr_Cena = _akcja.Twr_Cena1;

                int cenaZl = (int)_akcja.Twr_Cena; // część całkowita
                int cenaGr = (int)Math.Round((_akcja.Twr_Cena - cenaZl) * 100);


                switch (cenaZl.ToString().Length)
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


                switch (_akcja.Twr_Cena1.ToString().Length)
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

                int polozeniePLN = polozenie + 52 * cenaZl.ToString().Length;
                string twr_nazwa = (_akcja.Twr_Nazwa.Length > 20 ? _akcja.Twr_Nazwa.Substring(0, 20) : _akcja.Twr_Nazwa);

                //var cena = Double.Parse(_akcja.Twr_Cena.Replace(".", ","));
                //var cena1 = Double.Parse(_akcja.twr_cena1.Replace(".", ","));

                var prr = double.IsInfinity(((double)_akcja.Twr_Cena / (double)_akcja.Twr_Cena1 - 1) * 100) ? 0 : (_akcja.Twr_Cena / _akcja.Twr_Cena1 - 1) * 100;

                //prevent from devide by 0, when cena1 dsnt exists


                string procent = Convert.ToInt16(prr).ToString();
                //string procent = Convert.ToInt16((Double.Parse(_akcja.twr_cena.Replace(".", ",")) / Double.Parse(_akcja.twr_cena1.Replace(".", ",")) - 1) * 100).ToString();

                if (kolor=="biały")
                    await SettingsPage._cpclPrinter.setForm(0, 200, 200, 270, 350, drukSzt);
                else
                    await SettingsPage._cpclPrinter.setForm(0, 200, 200, 370, 350, drukSzt);

                await SettingsPage._cpclPrinter.setBarCodeText(7, 0, 0);
                await SettingsPage._cpclPrinter.setMedia(cpclConst.LK_MEDIA_LABEL);

            
                await SettingsPage._cpclPrinter.setCodePage((int)CodePages.LK_CODEPAGE_ISO_8859_2);
                //LK_CPCL_BCS_EAN13


                if (SettingsPage.OnAlfaNumeric)
                {
                    typCodeEan = cpclConst.LK_CPCL_BCS_128;
                    przesuniecieDlaCode128 = 60;
                }
                else
                {
                    typCodeEan = cpclConst.LK_CPCL_BCS_EAN13;
                    przesuniecieDlaCode128 = 80;
                }

                await SettingsPage._cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_7, 0, 50, 5, _akcja.Twr_Kod, 0);
                await SettingsPage._cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_7, 0, 50, 30, twr_nazwa, 0);
                await SettingsPage._cpclPrinter.print1dBarCode(cpclConst.LK_CPCL_0_ROTATION, typCodeEan, 1,
                        cpclConst.LK_CPCL_BCS_0RATIO, 40, przesuniecieDlaCode128, 55, _akcja.Twr_Ean, 0);

                if (kolor!="biały")
                {
                    await SettingsPage._cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_4, 0, 50, 135, _akcja.Twr_Cena1.ToString(), 0);
                    await SettingsPage._cpclPrinter.printLine(40, 170, koniecLinii, 145, 10);
                    if (Convert.ToInt16(procent) < -15)
                    {                        
                        await SettingsPage._cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_7, 0, 45, 245, $"~{procent}%", 0);//245  165 wyzej
                    }

                }
                int YpolozenieCeny;
               
                if (kolor == "biały")
                    YpolozenieCeny = 115;
                else
                    YpolozenieCeny = 180;

                await SettingsPage._cpclPrinter.setConcat(cpclConst.LK_CPCL_CONCAT, polozenie, YpolozenieCeny);
                await SettingsPage._cpclPrinter.concatText(cpclConst.LK_CPCL_FONT_4, 3, 0, cenaZl.ToString());
                await SettingsPage._cpclPrinter.concatText(cpclConst.LK_CPCL_FONT_4, 0, 0, cenaGr.ToString());
                await SettingsPage._cpclPrinter.resetConcat();

                await SettingsPage._cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_7, 0, polozeniePLN, 240, "PLN", 0);


                await SettingsPage._cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_0, 0, 100, 120, "najnizsza cena z 30 dni", 0);
                await SettingsPage._cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_0, 0, 200, 135, "przed obnizka", 0);//old value 140

                if (_akcja.Twr_Cena30 > 0 && _akcja.Twr_Cena30 < _akcja.Twr_Cena1)
                    await SettingsPage._cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_7, 0, 200, 150, $"{_akcja.Twr_Cena30}pln", 0);



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
