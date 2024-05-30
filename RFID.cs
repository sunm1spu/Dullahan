using ReaderB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

// API calls
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;

namespace Dullahan
{
    internal class RFID
    {
        // Dictionary<string, DateTime> scanPackage = new Dictionary<string, DateTime>();
        List<String[]> scanPackage = new List<String[]>();
        HashSet<String> uniqueIds = new HashSet<String>();

        WebCalls calls = new WebCalls();
        private static readonly HttpClient client = new HttpClient();
        // End of HTTP INIT

        const int BAUD_RATE = 576000;
        private byte fBaud;
        private byte fComAdr = 0xff;
        private int frmcomportindex = -1;
        private int fOpenComIndex = -1;
        private bool ComOpen = true;
        private int[] BaudOptions = { 57600 };
        private int ferrorcode = -1;
        public void OpenCOMPort()
        {
            // COM PORT RIGHT HERE
            int port = 0;
            int openresult, i;
            openresult = 30;
            string temp;
            int baud;
            int SELECTED_INDEX = 3;

            try
            {
                Console.WriteLine("Opening COM Port");
                fBaud = Convert.ToByte(SELECTED_INDEX);
                if (fBaud > 2)
                {
                    fBaud = Convert.ToByte(fBaud + 2);
                }
                openresult = StaticClassReaderB.AutoOpenComPort(ref port, ref fComAdr, fBaud, ref frmcomportindex);
                fOpenComIndex = frmcomportindex;
                if (openresult == 0)
                {
                    ComOpen = true;
                    // Button3_Click(sender, e); //自动执行读取写卡器信息
                    if (fBaud > 3)
                    {
                        baud = Convert.ToInt32(fBaud - 2);
                    }
                    else
                    {
                        baud = Convert.ToInt32(fBaud);
                    }
                    // Button3_Click(sender, e); //自动执行读取写卡器信息
                    /* if ((fCmdRet == 0x35) | (fCmdRet == 0x30))
                    {
                        Console.WriteLine("Serial Communication Error or Occupied", "Information");
                        StaticClassReaderB.CloseSpecComPort(frmcomportindex);
                        ComOpen = false;
                    }
                    */
                }
                Console.WriteLine("Port Opened");
            }
            catch (Exception e)
            {
                Console.WriteLine("Couldn't Open COMPort");
            }

        }
        private int fCmdRet = 30;

        private string configURL;

        public void Set_Rate(int newRate)
        { 
        }
        public void Set_URL(string newURL)
        {
            configURL = newURL;
        }

        public void Read_6B(string newURL)
        {
            Set_URL(newURL);
            Read_6B();
        }
        public void Read_6B()
        {
            string temp, temps;
            byte[] CardData = new byte[320];
            byte[] ID_6B = new byte[8];
            byte Num, StartAddress;
            // SETTINGS CHECK
            /* if (ComboBox_ID1_6B.Items.Count == 0)
                return;
            if (ComboBox_ID1_6B.SelectedItem == null)
                return;
            */
            // QUERY CHECK

            /*
            temp = ComboBox_ID1_6B.SelectedItem.ToString();
            if (temp == "")
                return;
            ID_6B = HexStringToByteArray(temp);
            if (Edit_StartAddress_6B.Text == "")
                return;
            StartAddress = Convert.ToByte(Edit_StartAddress_6B.Text, 16);
            if (Edit_Len_6B.Text == "")
                return;
            Num = Convert.ToByte(Edit_Len_6B.Text);
            */
            Num = Convert.ToByte("12");
            StartAddress = Convert.ToByte("00", 16);
            fCmdRet = StaticClassReaderB.ReadCard_6B(ref fComAdr, ID_6B, StartAddress, Num, CardData, ref ferrorcode, frmcomportindex);


            if (fCmdRet == 0)
            {

                byte[] data = new byte[Num];


                Array.Copy(CardData, data, Num);
                temps = ByteArrayToHexString(data);
                // Add to WinForms
                // listBox2.Items.Add(temps);
                // rightherex
                // hard tag 1: E20040D40000000000000000

                // Add new scan to scanPackage
                String concatTemps = temps.Substring(0, 4);
                Console.WriteLine("--- Concat Data: " + concatTemps);

                if (!uniqueIds.Contains(concatTemps))
                {
                    uniqueIds.Add(concatTemps);
                    String[] newKeyTime = { concatTemps, DateTime.Now.ToString() };


                    string s = string.Join(",", newKeyTime);
                    Console.WriteLine("uniqueID: " + s);
                    scanPackage.Add(newKeyTime);
                }


                // Dummy data 

                // POST test code

            }

            String[] firstPair = { };

            if (scanPackage.Any())
            {
                string s = string.Join(",", scanPackage);

                Console.WriteLine("CURRENT PACKAGE: " + s);
                firstPair = (String[])scanPackage[0];
                Console.WriteLine("FIRST PAIR: " + firstPair[0]);
                bool isTimeUp = calls.Debouncer(WebCalls.LOGSCAN, firstPair);


                if (isTimeUp)
                {
                    String[] uniqueIdArray = new String[uniqueIds.Count];
                    uniqueIds.CopyTo(uniqueIdArray);

                    Console.WriteLine("--- PACKAGE: " + uniqueIdArray);

                    Console.WriteLine("----- SENDING PACKAGE");


                    //calls.ScanCall(uniqueIdArray, client);
                    // String configURL = textBox6.Text;
                    ResponseChecker(uniqueIdArray, client, configURL);
                    scanPackage.Clear();
                    uniqueIds.Clear();

                }
            }
        }

        private byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }

        private string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
            return sb.ToString().ToUpper();

        }

        private async void ResponseChecker(String[] uniqueIdArray, HttpClient client, string URL)
        {
            String responseObject = await calls.ScanCall(uniqueIdArray, client, URL);
        }

    }


}
