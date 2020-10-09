using Plugin.SewooXamarinSDK.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App2.Model
{
    public class PrintSerwis
    {
        CPCLConst cpclConst;
        int IResult;
        bool CanPrint;

        private static SemaphoreSlim printSemaphore = new SemaphoreSlim(1, 1);




    //    public PrintSerwis()
    //    {

    //    }


    //    int koniecLinii;
    //    int polozenie;
    //    bool printSukces;


    //    public async Task<bool> PrintCommand(string ile = null)
    //    {
    //        int drukSzt; 

    //        if (ile != null)
    //            drukSzt = Convert.ToInt16(ile);
    //        else
    //            drukSzt = 1;


    //        await printSemaphore.WaitAsync();
    //        try
    //        {
    //            if (SettingsPage.CzyCenaPierwsza)
    //                _akcja.TwrCena = _akcja.TwrCena1;

    //            var cenaZl = _akcja.TwrCena.Substring(0, _akcja.TwrCena.IndexOf(".", 0));
    //            var cenaGr = _akcja.TwrCena.Substring(_akcja.TwrCena.IndexOf(".", 0) + 1, 2);


    //            switch (cenaZl.Length)
    //            {
    //                case 1:
    //                    polozenie = 125;

    //                    break;

    //                case 2:
    //                    polozenie = 110;

    //                    break;

    //                case 3:
    //                    polozenie = 85;

    //                    break;

    //                default:
    //                    polozenie = 100;
    //                    break;
    //            }


    //            switch (_akcja.TwrCena1.Length)
    //            {
    //                case 4:

    //                    koniecLinii = 130;
    //                    break;

    //                case 5:

    //                    koniecLinii = 150;
    //                    break;

    //                case 6:

    //                    koniecLinii = 170;
    //                    break;

    //                default:
    //                    polozenie = 150;
    //                    break;
    //            }
    //            //= (_akcja.TwrCena1.Length <= 5) ? 155 : 165;


    //            string twr_nazwa = (_akcja.TwrNazwa.Length > 20 ? _akcja.TwrNazwa.Substring(0, 20) : _akcja.TwrNazwa);

    //            string procent = Convert.ToInt16((Double.Parse(_akcja.TwrCena.Replace(".", ",")) / Double.Parse(_akcja.TwrCena1.Replace(".", ",")) - 1) * 100).ToString();

    //            if (List_AkcjeView.TypAkcji.Contains("Zmiana"))
    //                await SettingsPage._cpclPrinter.setForm(0, 200, 200, 270, 350, drukSzt);
    //            else
    //                await SettingsPage._cpclPrinter.setForm(0, 200, 200, 370, 350, drukSzt);

    //            await SettingsPage._cpclPrinter.setBarCodeText(7, 0, 0);
    //            await SettingsPage._cpclPrinter.setMedia(cpclConst.LK_MEDIA_LABEL);

    //            // Set the code page of font number(7) 
    //            await SettingsPage._cpclPrinter.setCodePage((int)CodePages.LK_CODEPAGE_ISO_8859_2);


    //            await SettingsPage._cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_7, 0, 50, 5, _akcja.TwrKod, 0);
    //            await SettingsPage._cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_7, 0, 50, 30, twr_nazwa, 0);
    //            await SettingsPage._cpclPrinter.print1dBarCode(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_BCS_EAN13, 1,
    //cpclConst.LK_CPCL_BCS_0RATIO, 40, 80, 55, _akcja.TwrEan, 0);

    //            if (!List_AkcjeView.TypAkcji.Contains("Zmiana"))
    //            {
    //                await SettingsPage._cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_4, 0, 50, 115, _akcja.TwrCena1, 0);
    //                await SettingsPage._cpclPrinter.printLine(40, 160, koniecLinii, 125, 10);
    //                if (Convert.ToInt16(procent) < -15)
    //                    await SettingsPage._cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_7, 0, 50, 245, $"{procent}%", 0);

    //            }
    //            int YpolozenieCeny;
    //            //await _cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_4, 1, polozenie, 160, cenaZl, 0);
    //            //await _cpclPrinter.printText(cpclConst.LK_CPCL_0_ROTATION, cpclConst.LK_CPCL_FONT_4, 0, 158, 160, cenaGr, 0);
    //            //190
    //            if (List_AkcjeView.TypAkcji.Contains("Zmiana"))
    //                YpolozenieCeny = 115;
    //            else
    //                YpolozenieCeny = 160;
    //            await SettingsPage._cpclPrinter.setConcat(cpclConst.LK_CPCL_CONCAT, polozenie, YpolozenieCeny);
    //            await SettingsPage._cpclPrinter.concatText(cpclConst.LK_CPCL_FONT_4, 3, 0, cenaZl);
    //            await SettingsPage._cpclPrinter.concatText(cpclConst.LK_CPCL_FONT_4, 0, 0, cenaGr);//góra grosze
    //                                                                                               //await SettingsPage._cpclPrinter.concatText(cpclConst.LK_CPCL_FONT_4, 0, 45, cenaGr); dół
    //            await SettingsPage._cpclPrinter.concatText(cpclConst.LK_CPCL_FONT_7, 0, 60, "zł");
    //            await SettingsPage._cpclPrinter.resetConcat();



    //            await SettingsPage._cpclPrinter.printForm();



    //            iResult = await SettingsPage._cpclPrinter.printResults();
    //            switch (Device.RuntimePlatform)
    //            {
    //                case Device.iOS:
    //                    iResult = await SettingsPage._cpclPrinter.printStatus();
    //                    //Debug.WriteLine("PrinterStatus = " + iResult);
    //                    break;
    //                default:
    //                    //Debug.WriteLine("PrinterResults = " + iResult);
    //                    break;
    //            }
    //            if (iResult != cpclConst.LK_SUCCESS)
    //            {
    //                ErrorStatusDisp("Wydruk nieudany", iResult);
    //                printSukces = false;
    //            }
    //            else
    //            {
    //                //await DisplayAlert("Printing Result", "Printing success", "OK");

    //                printSukces = true;

    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            e.StackTrace.ToString();
    //            //await DisplayAlert("Exception", e.Message.ToString(), "OK");
    //        }
    //        finally
    //        {
    //            printSemaphore.Release();
    //        }

    //        return printSukces;


    //    }

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
