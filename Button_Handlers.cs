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
        bool threadRunning = false;

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //Let's make a prompt before closing.

            //Close Streams

            if (threadRunning)
            {
                connClose = true;
                stream.Close();
                client.Close();
            }

            //Close Program
            Close();
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            //Insert IRC server connection code here. Test server should be irc.freenode.net.
            readData = "Connected to Freenode...";
            msg();
            client.Connect("irc.freenode.net", 6667);
            stream = client.GetStream();

            //byte[] outStream = System.Text.Encoding.ASCII.GetBytes("PixelatedOne$");
            //stream.Write(outStream, 0, outStream.Length);
            //stream.Flush();

            Thread ctThread = new Thread(getMessage);
            ctThread.Start();
            threadRunning = true;

            txtEntry.IsEnabled = true;
        }
    }
}
