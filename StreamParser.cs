using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;

namespace Pixel_IRC
{
    public partial class MainWindow
    {
        int buildingList = 0;
        /// <summary>
        /// Will take the converted stream data and route as necessary once parsed.
        /// </summary>
        /// <param name="streamData"></param>
        /// <returns></returns>
        private void StreamParser(string streamData)
        {
            if (streamData.Contains("\r\n"))
            {
                string[] parsed = streamData.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < parsed.Length - 1; i++)
                {
                    if (parsed[i].StartsWith(":")) { parsed[i] = parsed[i].Substring(1); }  //Remove the starting colon to clean-up parsing.
                    if (parsed[i].Contains(":"))
                    {
                        string[] scanner = parsed[i].Split(new string[] { ":" }, StringSplitOptions.None);
                        string[] serverInfo = scanner[0].Split(new string[] { " " }, StringSplitOptions.None);

                        if (serverInfo.Length > 1)
                        {
                            // If input is a PRIVMSG.
                            if (serverInfo[1] == "PRIVMSG")
                            {
                                string txtLine = "";
                                // Recompile string in case a colon was used.
                                for (int s = 1; s < scanner.Length; s++)
                                {
                                    if (s > 1)
                                    {
                                        txtLine += ":";
                                    }
                                    txtLine += scanner[s];
                                }
                                parseMessage(serverInfo[2], txtLine, serverInfo[0]);
                            }
                            // If server returns NAME list.
                            if (serverInfo[1] == "353")
                            {
                                string nameList = scanner[1];
                                string chan = serverInfo[4];
                                parseNames(chan, nameList, buildingList);
                                buildingList = 1;   // If names list is too long, server will send on multiple lines, need to append to previous.
                            }
                            if (serverInfo[1] == "366") // End of names list, turn off appending.
                            {
                                buildingList = 0;
                            }
                            else
                            {

                                readData = parsed[i];
                                msg();
                            }
                        }
                    }
                    else
                    {
                        readData = parsed[i];
                        msg();
                    }
                    if (readData.ToLower().Contains("end of /motd"))
                    {
                        readData = "SUCCESS: Connected to network!";
                        usrmsg();
                    }
                        

                }
            }
            if (streamData.ToLower().Contains("checking ident"))
            {
                negotiateConnection();
            }
        }
    }
}
