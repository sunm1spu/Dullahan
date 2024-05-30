// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Collections;
using System.Resources;
using System.Reflection;
using ReaderB;
using System.IO.Ports;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Timers;


// API calls
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using Dullahan;

namespace Dullahan

{
    class RFIDClient
    {


        static void Main(string[] args)
        {


            int runStatus = 0;
            RFID client = new RFID();
            Console.Clear();
            Console.WriteLine("WELCOME TO THE ETC RASPBERRY PI CLIENT \n" +
                "* OPTIONS\n" +
                " 1. scan <URL> | s <URL> | s | 1 <URL> | 1\n" +
                " 2. reconnect | r | 2\n" +
                " 3. sethost <URL> | sh <URL> | 3 <URL>\n" +
                " 4. setrate <Scan rate for unique scans in ms> | sr <rate> | 4 <rate>\n" +
                " 5. close | c | 9\n");
            String input = "2";

            // runStatus > -1
            while (runStatus > -1)
            {
                if (runStatus == 1)
                {
                    input = Console.ReadLine();
                }
                String[] inputList = input.Split(' ');

                runStatus = fieldLeader(inputList, client);
            }

        }

        static public int fieldLeader(String[] inputList, RFID client)
        {
            int statusUpdate = 0;


            String input = inputList[0].ToUpper();
            switch (input)
            {
                case "SCAN":
                case "S":
                case "1":
                    statusUpdate = 1;
                    while (!Console.KeyAvailable) 
                    {
                        try
                        {
                            // if new url is specified
                            if (inputList.Length > 1) 
                            {
                                client.Read_6B(inputList[1]);
                            }
                            // if no new URL is specified
                            else
                            {
                                client.Read_6B();
                            }
                            
                        }

                        catch (Exception e)
                        {
                            Console.WriteLine("Error sending scan data!");
                        }
                    }
                    break;

                case "RECONNECT":
                case "R":
                case "2":
                    client.OpenCOMPort();
                    statusUpdate = 1;
                    break;

                case "SETHOST":
                case "SH":
                case "3":
                    client.Set_URL(inputList[1]);
                    statusUpdate = 1;
                    break;

                case "SETRATE":
                case "SR":
                case "4":
                    try
                    {
                        client.Set_Rate(Int32.Parse(inputList[1]));
                    }

                    catch (Exception e)
                    {
                        Console.WriteLine("Please enter a valid integer!");
                    }
                    
                    statusUpdate = 1;
                    break;

                case "CLOSE":
                case "C":
                case "9":
                    statusUpdate = -1;
                    break;

                default:
                    Console.WriteLine("Invalid Command!");
                    statusUpdate = 1;
                    break;

            }


            return statusUpdate;
        }



    }

}
