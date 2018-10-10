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
        private void getMessage()
        {

            while (connClose == false)
            {
                try
                {
                    stream = client.GetStream();
                    int buffSize = 0;
                    byte[] inStream = new byte[client.ReceiveBufferSize + 10];
                    buffSize = client.ReceiveBufferSize;
                    stream.Read(inStream, 0, buffSize);
                    string returnData = System.Text.Encoding.UTF8.GetString(inStream);
                    StreamParser(returnData);                 
                }
                catch (IOException)
                {
                    //Makes sure it's all clear.
                    connClose = true;
                }
            }

        }

        private void negotiateConnection()
        {
            if (this.Dispatcher.CheckAccess() == false)
            {
                this.Dispatcher.Invoke(new Action(negotiateConnection));
            }
            else
            {
                sendOut("CAP LS 302");
                sendOut("NICK PixelTestClient");
                sendOut("USER tickledberries 0 * WeirdOne");
                sendOut("CAP END");
            }
        }

        private void confirmConnection()
        {
            
        }
    }
}
