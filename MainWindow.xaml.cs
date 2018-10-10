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
using static Pixel_IRC.File_Tools;

namespace Pixel_IRC
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// String that contains logged-in user's Nick.
        /// </summary>
        public string myNick = "PixelTestClient";

        TcpClient client = new TcpClient();
        NetworkStream stream = default(NetworkStream);
        string readData = null;

        static bool connClose = false;

        /// <summary>
        /// Primary Initialization of main program.
        /// </summary>
        public MainWindow()
        {
            this.WindowStyle = WindowStyle.None;

            InitializeComponent();
            this.Title = "Status Window";

            // If settings file doesn't exist, create it.
            bool chkSettingsFile = File_Tools.FileExists("sysconf.esf", "configs");
            if (chkSettingsFile == false)
            {
                File_Tools.file_CreateSettings();
            }

            // Get Window Colors
            updateColors();

            outputWindow.Document.Blocks.Clear();
            txtEntry.Text = "";
            dock mainDock = new dock();
            this.RegisterName("dock", mainDock);

            // !!!!!!!!!!!!!!!!
            // Testing - Remove when done!
            // !!!!!!!!!!!!!!!!
            //StackPanel panelRef = (StackPanel)mainDock.FindName("content");
            //for (int i = 0; i < 4; i++)
            //{
            //    ChannelWindow pewpChan = new ChannelWindow(panelRef);
            //    pewpChan.Name = "channel" + i;
            //    pewpChan.channel = "channel" + i;
            //    pewpChan.Show();
            //}
            // !!!!!!!!!!!!!!!!!
            // END TESTING AREA
            // !!!!!!!!!!!!!!!!!

            this.Loaded += delegate
            {
                WindowBorders createBorder = new WindowBorders();
                createBorder.MinClose(this);
            };

            this.Closed += new EventHandler(MainWindow_Closed);

        }

        private void msg()
        {
            if (this.Dispatcher.CheckAccess() == false)
            {
                this.Dispatcher.Invoke(new Action(msg));
            }
            else
            {
                Paragraph insertion = new Paragraph(new Run(readData));
                insertion.Foreground = Brushes.Red;
                outputWindow.Document.Blocks.Add(insertion);
                outputWindow.ScrollToEnd();
            }
        }

        private void usrmsg()
        {
            if (this.Dispatcher.CheckAccess() == false)
            {
                this.Dispatcher.Invoke(new Action(usrmsg));
            }
            else
            {
                Paragraph insertion = new Paragraph(new Run(readData));
                insertion.Foreground = Brushes.Cyan;
                outputWindow.Document.Blocks.Add(insertion);
                outputWindow.ScrollToEnd();
            }
        }

        private void txtEntry_KeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                sendOut(txtEntry.Text);
                txtEntry.Clear();
            }
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.isOpen == false)
            {
                Settings settingsWindow = new Settings();
                settingsWindow.Name = "SettingsWindow";

                this.RegisterName("SettingsWindow", settingsWindow);
                settingsWindow.Show();
            }
            else
            {
                Settings settingsWindow = (Settings)this.FindName("SettingsWindow");
                settingsWindow.Focus();
            }
        }

        /// <summary>
        /// Handles all functions that occur when closing the program.
        /// </summary>
        private void MainWindow_Closed(object sender, System.EventArgs e)
        {
            foreach (Window window in Application.Current.Windows) // Closes all instanced windows.
            {
                window.Close();
            }
        }

        internal void updateColors()
        {
            byte[] bgcolor = File_Tools.getColor("color_System_Window");
            byte[] textColor = File_Tools.getColor("color_System_Font");
            byte[] statWindow = File_Tools.getColor("color_System_StatusWindow");
            byte[] statFont = File_Tools.getColor("color_System_StatusFont");

            gridWindow.Background = new SolidColorBrush(Color.FromRgb(bgcolor[0], bgcolor[1], bgcolor[2]));
            outputWindow.Background = new SolidColorBrush(Color.FromRgb(statWindow[0], statWindow[1], statWindow[2]));
            outputWindow.Foreground = new SolidColorBrush(Color.FromRgb(statFont[0], statFont[1], statFont[2]));
        }
    }
}