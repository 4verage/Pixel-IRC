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
        /// <summary>
        /// Prepares a string [input] and sends it to the stream and then displays in the output window.
        /// </summary>
        /// <param name="input"> The string that should be sent out to the IRC server. </param>
        public void sendOut(string input)
        {
            Paragraph insertion = new Paragraph(new Run(input));
            insertion.Foreground = Brushes.Red;
            outputWindow.Document.Blocks.Add(insertion);
            outputWindow.ScrollToEnd();
            //Join Channel
            if (input.ToLower().Contains("join"))
            {
                string[] channeljoiner = input.Split(new string[] { " " }, StringSplitOptions.None);
                Regex rgx = new Regex("[^a-zA-Z0-9 -]");
                if (this.FindName("channel" + rgx.Replace(channeljoiner[1], "")) != null)
                {
                    ChannelWindow sameChannel = (ChannelWindow)this.FindName("channel" + rgx.Replace(channeljoiner[1], ""));
                    sameChannel.Show();
                }
                else
                {
                    dock refDock = (dock)this.FindName("dock");
                    StackPanel chanDock = (StackPanel)refDock.FindName("content");
                    ChannelWindow newChannel = new ChannelWindow(chanDock);
                    newChannel.Name = "channel" + rgx.Replace(channeljoiner[1], "");
                    newChannel.setChannel();
                    this.RegisterName(newChannel.Name, newChannel);
                    newChannel.Show();
                }
            }
            //Message Handling
            if (input.ToLower().Contains("privmsg"))
            {
                if (input.ToLower().Contains("#"))
                {
                    string[] actualText = input.Split(new string[] { ":" }, StringSplitOptions.None);
                    string[] getChan = actualText[0].Split(new string[] { " " }, StringSplitOptions.None);

                    string fullText = "";
                    for (int i = 1; i < actualText.Length; i++)
                    {
                        if (i == 1)
                        {
                            fullText = fullText + actualText[i];
                        }
                        else
                        {
                            fullText = fullText + ":" + actualText[i];
                        }
                    }

                    Regex rgx = new Regex("[^a-zA-Z0-9 -]");
                    string testString = "channel" + rgx.Replace(getChan[1], "");
                    sendToChannel(testString, "Me", fullText);
                }
            }
            byte[] outStream = System.Text.Encoding.UTF8.GetBytes(input + "\r\n");
            stream.Write(outStream, 0, outStream.Length);
            stream.Flush();
        }

        /// <summary>
        /// Used to check if a ChannelWindow exists or not.
        /// </summary>
        /// <param name="chanName">This should be the name of the ChannelWindow using our naming convention ('channel[channel name (no '#')]')</param>
        /// <returns></returns>
        private bool isChannel(string chanName)
        {
            if ((ChannelWindow)this.FindName(chanName) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Receives data from the StreamParser as a "PRIVMSG" and then determines where to send it.
        /// </summary>
        /// <param name="dest">Destination of private message.</param>
        /// <param name="content">Message to include once the destination is set.</param>
        /// <param name="sender">Name who sent message.</param>
        private void parseMessage(string dest, string content, string sender)
        {
            if (this.Dispatcher.CheckAccess() == false)
            {
                this.Dispatcher.Invoke(new Action(() => parseMessage(dest, content, sender)));
            }
            else
            {

                // If the destination is an IRC channel...
                if (dest.Contains("#"))
                {
                    // This is required for our ChannelWindow naming convention.
                    Regex rgx = new Regex("[^a-zA-Z0-9 -]");
                    string hashTrimmer = rgx.Replace(dest, "");
                    string chanName = "channel" + hashTrimmer;

                    string[] usr = sender.Split(new string[] { "!" }, StringSplitOptions.None);
                    string user = usr[0];

                    // See if channel exists.
                    if (isChannel(chanName))
                    {
                        sendToChannel(chanName, user, content);
                    }
                }
            }
        }

        private void parseNames(string dest, string nicks, int append)
        {
            if (this.Dispatcher.CheckAccess() == false)
            {
                this.Dispatcher.Invoke(new Action(() => parseNames(dest, nicks, append)));
            }
            else
            {
                // If the destination is an IRC channel...
                if (dest.Contains("#"))
                {
                    // This is required for our ChannelWindow naming convention.
                    Regex rgx = new Regex("[^a-zA-Z0-9 -]");
                    string hashTrimmer = rgx.Replace(dest, "");
                    string chanName = "channel" + hashTrimmer;

                    // See if channel exists.
                    if (isChannel(chanName))
                    {
                        ChannelWindow pinpoint = (ChannelWindow)this.FindName(chanName);
                        pinpoint.updateNameList(nicks, append);
                    }
                }
            }
        }

        /// <summary>
        /// Send messages to IRC channels.
        /// </summary>
        /// <param name="channel">IRC Channel name to send message to.</param>
        /// <param name="nick">The username that originally sent the message.</param>
        /// <param name="message">The content of the user's message.</param>
        private void sendToChannel(string channel, string nick, string message)
        {
            string sendString = "<" + nick + "> " + message;
            ChannelWindow channelWindow = (ChannelWindow)this.FindName(channel);
            Paragraph sentFromUser = new Paragraph(new Run(sendString));
            channelWindow.chanText.Document.Blocks.Add(sentFromUser);
            channelWindow.chanText.ScrollToEnd();
        }

        /// <summary>
        /// Quick check to determine if a window name is already registered.
        /// </summary>
        /// <param name="windowName">The name of the window to look for.</param>
        /// <returns>True/False</returns>
        public bool windowExists(string windowName)
        {
            if (this.FindName(windowName) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get currently set user Nick.
        /// </summary>
        /// <returns>Returns the nickname being used.</returns>
        public string getNick()
        {
            return myNick;
        }

        /// <summary>
        /// Sets user nickname.
        /// </summary>
        /// <param name="nick">The nickname wishing to be used.</param>
        public void setNick(string nick)
        {
            this.myNick = nick;
        }

    }
}
