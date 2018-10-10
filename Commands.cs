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

namespace Pixel_IRC
{
    public partial class MainWindow
    {
        /// <summary>
        /// Checks user input and decides what to do with it.
        /// </summary>
        /// <param name="userInput">Passed string</param>
        /// <param name="sender">The object that requested the function call.</param>
        /// <returns></returns>
        public string CommandProcessor(string userInput, object sender)
        {
            string userOutput = "";

            //Determine if this is intended as a user command.
            if (userInput[0] == '/')
            {
                string[] breakdown = userInput.Split(new string[] { " " }, StringSplitOptions.None);
                string command = breakdown[0].ToUpper();
                command = command.Substring(1);
                userOutput = command;

                for (int i = 1; i < breakdown.Length; i++)
                {
                    userOutput += " " + breakdown[i];
                }
            }
            else
            {
                ChannelWindow hrmph = sender as ChannelWindow;
                string chanName = hrmph.Name.Substring(7);
                chanName = "#" + chanName;
                userOutput = "PRIVMSG " + chanName + " :" + userInput;
            }

            return userOutput;
        }
    }
}
