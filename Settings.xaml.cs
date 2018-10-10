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
using System.Windows.Shapes;
using System.IO;
using static Pixel_IRC.ToolTips;
using Screen = System.Windows.Forms.Screen;

namespace Pixel_IRC
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {

        private bool showingPassword = false;
        private SortedList<int, IRC_Server> serverList = new SortedList<int, IRC_Server>();
        private bool comboOn = true;

        private string ircServerName;
        private string ircServer;
        private int ircServerPort;

        private Screen _defaultScreen;

        /// <summary>
        /// Static string used to track whether this window has already been instanced and is open.
        /// </summary>
        public static bool isOpen = false;

        /// <summary>
        /// Constructor for the Settings window.
        /// </summary>
        public Settings()
        {
            InitializeComponent();
            isOpen = true;
            loadServers();

            // Set ToolTips
            imgNameHelp.ToolTip = createTT("Friendly Name", "Provide a nice, friendly name, to describe your new IRC server entry.", "EXAMPLE: Pixelchat IRC");
            imgServerHelp.ToolTip = createTT("Server", "Place the DNS name or IP address of the IRC server you wish to connect to in this entry field.", "EXAMPLES: irc.pixelchat.com, 192.168.12.2");
            imgPortHelp.ToolTip = createTT("Port", "The numbered port for the IRC server. [Not required. Defaults to: 6667]", "EXAMPE: 6849");

            chkIChelpImg.ToolTip = createTT("Independent Channels", "Selecting this checkbox turns on the ability to set unique colors for server-specific channels.");

            // Load current saved settings into variables and then fill fields.
            ircServerName = File_Tools.readSettings("IRC_Server_Name").Trim();
            ircServer = File_Tools.readSettings("IRC_Server").Trim();
            ircServerPort = Convert.ToInt32(File_Tools.readSettings("IRC_Server_Port").Trim());

            int serverSelectThis;
            foreach (KeyValuePair<int, string> serverItem in serverDropDown.Items) {
                if (serverItem.Value.ToString().Equals(ircServerName))
                {
                    serverSelectThis = serverItem.Key;
                    serverDropDown.SelectedIndex = serverSelectThis;
                }
            }

            makeScreens();

        }

        /// <summary>
        /// Provides the actions to take when this window is closed.
        /// </summary>
        /// <param name="sender">Generic parameter for the object that sent the command.</param>
        /// <param name="e">Generic identifier for the Events actionable as 'Closed'.</param>
        private void Settings_Closed(object sender, EventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if ( window.Name == "consoleWindow")
                {
                    MainWindow superCommands = (MainWindow)window;
                    if (superCommands.windowExists("SettingsWindow"))
                    {
                        superCommands.UnregisterName("SettingsWindow");
                    }
                }
            }
            isOpen = false;
        }

        /// <summary>
        /// Check to see if the file exists.
        /// </summary>
        /// <returns>Returns true if the file exists.</returns>
        public bool settingsFileExists()
        {
            string workingDir = System.AppDomain.CurrentDomain.BaseDirectory;
            string configDir = "configs\\";
            string settingsFile = "sysconf.esf";

            if (File.Exists(workingDir + configDir + settingsFile))
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        /// <summary>
        /// Writes to our custom settings file.
        /// </summary>
        /// <param name="setting">The data to write to the file.</param>
        /// <param name="value">The value of the field that needs to be altered (I.E. winColor)</param>
        /// <returns>Returns true if write was successful.</returns>
        public bool writeSettings(string setting, string value)
        {
            throw (new NotImplementedException());
        }

        /// <summary>
        /// Contains all of the default settings for the program. Can re-write them to the settings file to restore default settings.
        /// </summary>
        public static void resetToDefaults()
        {

            var defSetting = new Dictionary<string, string>();
            defSetting["chanUserInput"] = "255, 255, 255";
            defSetting["chanJoin"] = "0, 0, 255";
            defSetting["chanPart"] = "0, 0, 255";
            defSetting["IRC_Server_Name"] = "Freenode";
            defSetting["IRC_Server"] = "irc.freenode.net";
            defSetting["IRC_Server_Port"] = "6667";

            string workingDir = System.AppDomain.CurrentDomain.BaseDirectory + "configs\\";
            string filename = "sysconf.esf";

            string file = workingDir + filename;
            var output = new List<string>();
            for (int i = 0; i < defSetting.Count; i++)
            {
                output.Add("[" + defSetting.ElementAt(i).Key + "] " + defSetting.ElementAt(i).Value);
            }
            string[] toFile = output.ToArray();

            File.AppendAllLines(file, toFile);
            
        }

        /// <summary>
        /// Creates the settings file (normally not found by method settingsFileExists) and fills the data with method resetToDefaults.
        /// </summary>
        public static void createSettingsFile()
        {
            string workingDir = System.AppDomain.CurrentDomain.BaseDirectory;
            string settingsDir = "configs\\";
            string settingsFilename = "sysconf.esf"; //System Configuration - External Settings File

            if (Directory.Exists(workingDir + settingsDir) == false)
            {
                Directory.CreateDirectory(workingDir + settingsDir);
            }

            FileStream filecreator = null;

            filecreator = File.Create(workingDir + settingsDir + settingsFilename);
            if (filecreator != null)
            {
                filecreator.Close();
            }

            resetToDefaults();
            
        }

        /// <summary>
        /// Fills the server comboBox for server selection.
        /// </summary>
        public void loadServers()
        {
            string workingDir = System.AppDomain.CurrentDomain.BaseDirectory;
            string settingsDir = "configs\\";
            string filename = "servers.esf";

            FileStream filecreator = null;

            if (Directory.Exists(workingDir + settingsDir) == false)
            {
                Directory.CreateDirectory(workingDir + settingsDir);
            }

            if (File.Exists(workingDir + settingsDir + filename) == false)
            {
                filecreator = File.Create(workingDir + settingsDir + filename);
                if (filecreator != null) { filecreator.Close(); }
            }

            string file = workingDir + settingsDir + filename;
            string[] lineReader = File.ReadAllLines(file);

            if (lineReader.Length > 0)
            {
                int line = 0;
                foreach (string serverInfo in lineReader)
                {
                    dynamic[] breakdown = serverInfo.Split(new string[] { "," }, StringSplitOptions.None);
                    if (breakdown.Length > 0)
                    {
                        string tmp1 = Convert.ToString(breakdown[0]);
                        string tmp2 = Convert.ToString(breakdown[1]);
                        int tmp3 = Convert.ToInt32(breakdown[2]);
                        IRC_Server newServer = new IRC_Server(tmp1, tmp2, tmp3);
                        serverList.Add(line, newServer);
                        line += 1;
                    }
                }

                SortedList<int, string> comboList = new SortedList<int, string>();
                line = 0;
                foreach (KeyValuePair<int, IRC_Server> node in serverList)
                {
                    IRC_Server grabber = node.Value;
                    comboList[line] = grabber.Name;
                    line += 1;
                }

                serverDropDown.ItemsSource = comboList;
                serverDropDown.DisplayMemberPath = "Value";
                serverDropDown.SelectedValuePath = "Key";

            }
            else { serverDropDown.Items.Add("Click Add ->");
                comboOn = false;
                serverDropDown.SelectedIndex = 0;
                serverDropDown.IsEnabled = false;
                serverDropDown.IsEnabled = false;
                btnServerDel.IsEnabled = false;
                btnServerEdit.IsEnabled = false;
            }
        }

        private void updateSelection(object sender, RoutedEventArgs e)
        {
            if (comboOn)
            {
                int grabPort = Convert.ToInt32(serverDropDown.SelectedValue);
                txtPort.Text = Convert.ToString(serverList[grabPort].getPort());
            }
        }

        private void treeChange(object sender, RoutedEventArgs e)
        {
            if (tvGeneral.IsSelected)
            {
                showGeneral();
            }
            if (tvServerSettings.IsSelected)
            {
                showServerSettings();
            }
            if (tvColors.IsSelected)
            {
                showColorSettings();
            }
            if (tvScreen.IsSelected)
            {
                showScreenSelection();
            }
        }

        private void showScreenSelection()
        {
            hideGrids();
            scrollWindowScreen.Visibility = Visibility.Visible;
        }

        private void showColorSettings()
        {
            hideGrids();
            scrollWindowColors.Visibility = Visibility.Visible;
            loadColors();
        }

        private void showGeneral()
        {
            hideGrids();
            gridGeneralSettings.Visibility = Visibility.Visible;
        }

        private void showServerSettings()
        {
            hideGrids();
            scrollWindowSettings.Visibility = Visibility.Visible;
        }

        private void hideGrids()
        {
            gridGeneralSettings.Visibility = Visibility.Hidden;
            scrollWindowSettings.Visibility = Visibility.Hidden;
            scrollWindowColors.Visibility = Visibility.Hidden;
            scrollWindowScreen.Visibility = Visibility.Hidden;
        }

        private void passCheck(object sender, RoutedEventArgs e)
        {
            if (chkPassword.IsChecked == true)
            {
                authenticateOn();
            }
            else
            {
                authenticateOff();
            }
        }

        private void authenticateOn()
        {
            lblPassword.IsEnabled = true;
            lblPassword.FontStyle = FontStyles.Normal;
            lblPassword.Foreground = new SolidColorBrush(Color.FromArgb(100, 166, 166, 166));
            txtPassword.IsEnabled = true;

            lblAuthNick.IsEnabled = true;
            lblAuthNick.FontStyle = FontStyles.Normal;
            lblAuthNick.Foreground = new SolidColorBrush(Color.FromArgb(100, 166, 166, 166));
            txtAuthNick.IsEnabled = true;
        }

        private void authenticateOff()
        {
            if (showingPassword == true)
            {
                hidePassword();
            }
            
            lblPassword.FontStyle = FontStyles.Italic;
            lblPassword.Foreground = new SolidColorBrush(Color.FromArgb(100, 80, 80, 80));
            lblPassword.IsEnabled = false;
            txtPassword.Clear(); txtPassword.IsEnabled = false;

            lblAuthNick.FontStyle = FontStyles.Italic;
            lblAuthNick.Foreground = new SolidColorBrush(Color.FromArgb(100, 80, 80, 80));
            lblAuthNick.IsEnabled = false;
            txtAuthNick.Clear(); txtAuthNick.IsEnabled = false;
        }

        private void showPass(object sender, MouseEventArgs m)
        {
            if (chkPassword.IsChecked == true)
            {
                if (showingPassword == false)
                {
                    showPassword();
                }
                else
                {
                    hidePassword();
                }
            }
        }

        private void showPassword()
        {
            TextBox txtPasswordUnhidden = new TextBox();
            txtPasswordUnhidden.Height = txtPassword.Height;
            txtPasswordUnhidden.Width = txtPassword.Width;
            txtPasswordUnhidden.Margin = txtPassword.Margin;
            txtPasswordUnhidden.HorizontalAlignment = txtPassword.HorizontalAlignment;
            txtPasswordUnhidden.VerticalAlignment = txtPassword.VerticalAlignment;
            txtPasswordUnhidden.Text = txtPassword.Password;
            txtPassword.Visibility = Visibility.Hidden;
            txtPasswordUnhidden.Visibility = Visibility.Visible;
            this.RegisterName("txtPasswordUnhidden", txtPasswordUnhidden);
            gridGeneralSettings.Children.Add(txtPasswordUnhidden);

            showingPassword = true;
            imgEyeBall.Source = new BitmapImage(new Uri("/Icons/421-200.png", UriKind.Relative));
        }

        private void hidePassword()
        {
            TextBox removeBox = (TextBox)this.FindName("txtPasswordUnhidden");
            txtPassword.Password = removeBox.Text;
            this.UnregisterName("txtPasswordUnhidden");
            gridGeneralSettings.Children.Remove(removeBox);
            txtPassword.Visibility = Visibility.Visible;

            showingPassword = false;
            imgEyeBall.Source = new BitmapImage(new Uri("/Icons/421-201.png", UriKind.Relative));
        }

        private void addBtnClicked(object sender, RoutedEventArgs m)
        {
            addServerPanel.Visibility = Visibility.Visible;
        }

        // -------------------------------------------------------------------------------
        // = Server Input Window Stuff ===================================================
        // -------------------------------------------------------------------------------

        private void siCancelClicked(object sender, RoutedEventArgs m)
        {
            siResetFields();
            addServerPanel.Visibility = Visibility.Hidden;
        }

        private void siNameTestInput(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.OemComma)
            {
                siTxtName.Text = siTxtName.Text.Substring(0, siTxtName.Text.Length - 1);
                siTxtName.SelectionStart = siTxtName.Text.Length;
                siTxtName.SelectionLength = 0;
            }
        }

        private void siServerTestInput(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Space) || (e.Key == Key.OemComma))
            {
                siTxtServer.Text = siTxtServer.Text.Substring(0, siTxtServer.Text.Length - 1);
                siTxtServer.SelectionStart = siTxtServer.Text.Length;
                siTxtServer.SelectionLength = 0;
            }
        }

        private void siPortTestInput(object sender, KeyEventArgs e)
        {
            if (!(e.Key > Key.D0 && e.Key <= Key.D9) && !(e.Key > Key.NumPad0 && e.Key <= Key.NumPad9) && !(e.Key == Key.Tab) && !(e.Key == Key.Enter))
            {
                if (siTxtPort.Text.Length >= 1)
                {
                    siTxtPort.Text = siTxtPort.Text.Substring(0, siTxtPort.Text.Length - 1);
                    siTxtPort.SelectionStart = siTxtPort.Text.Length;
                    siTxtPort.SelectionLength = 0;
                }
            }
        }

        private void siResetFields()
        {
            siTxtName.Clear();
            siTxtServer.Clear();
            siTxtPort.Clear();
        }

        private void siOK_Clicked(object sender, RoutedEventArgs e)
        {
            bool canClose = siCheckFields();
            if (canClose)
            {
                siWriteFields();
                siResetFields();
                addServerPanel.Visibility = Visibility.Hidden;
            }
        }

        private bool siCheckFields()
        {
            if (siTxtName.Text.Equals(""))
            {
                MessageBox.Show("Please enter a valid friendly server name in order to save this entry!");
                siTxtName.Focus();
                return false;
            }

            if (siTxtServer.Text.Equals(""))
            {
                MessageBox.Show("An IRC server entry is required to save this entry.");
                siTxtServer.Focus();
                return false;
            }

            if (siTxtPort.Text.Length > 0)
            {
                int port;
                bool numTest = Int32.TryParse(siTxtPort.Text, out port);
                if (numTest == false)
                {
                    MessageBox.Show("Port field can accept numbers only, please try again or leave blank to continue.");
                    siTxtPort.Focus();
                    return false;
                }
            }
            return true;
        }

        private void siWriteFields()
        {
            return;
        }

        // - Server Input Stuff END ------------------------------------------------------

        // -------------------------------------------------------------------------------
        // = Color Settings Window =======================================================
        // -------------------------------------------------------------------------------

            /// <summary>
            /// Loads colors from settings file and fills the color settings window.
            /// </summary>
        public void loadColors()
        {
            colorChatWindow.Fill = getColor("color_CS_ChatWindow");
            colorUserMessages.Fill = getColor("color_CS_UserMessages");
            colorOwnMessages.Fill = getColor("color_CS_OwnMessages");
            colorInputField.Fill = getColor("color_CS_InputField");
            colorInputText.Fill = getColor("color_CS_InputText");

            colorNickBackground.Fill = getColor("color_NL_Background");
            colorNicknames.Fill = getColor("color_NL_Nicknames");
            colorNickOP.Fill = getColor("color_NL_OPNicks");
            colorNickVoice.Fill = getColor("color_NL_VNicks");

            colorActionJoin.Fill = getColor("color_Action_Join");
            colorActionPart.Fill = getColor("color_Action_Part");
            colorActionKick.Fill = getColor("color_Action_Kick");
            colorActionBanned.Fill = getColor("color_Action_Ban");
            colorAction.Fill = getColor("color_Action_Default");
            colorActionInvite.Fill = getColor("color_Action_Invite");
            colorActionTopic.Fill = getColor("color_Action_Topic");

            colorSystemWindow.Fill = getColor("color_System_Window");
            colorSystemFont.Fill = getColor("color_System_Font");
            colorSystemStatWindow.Fill = getColor("color_System_StatusWindow");
            colorSystemStatFont.Fill = getColor("color_System_StatusFont");
        }

        private SolidColorBrush getColor(string keyname)
        {
            byte[] colorgrab = new byte[3];
            string[] splitter = new string[3];
            SolidColorBrush paintcolor = new SolidColorBrush();

            splitter = File_Tools.readSettings(keyname).Replace(" ", "").Split(new string[] { "," }, StringSplitOptions.None);
            colorgrab = new byte[] { Convert.ToByte(splitter[0]), Convert.ToByte(splitter[1]), Convert.ToByte(splitter[2]) };
            paintcolor = new SolidColorBrush(Color.FromRgb(colorgrab[0], colorgrab[1], colorgrab[2]));

            return paintcolor;
        }

        private void IC_Checked(object sender, RoutedEventArgs e)
        {
            comboICServer.IsEnabled = true;
            comboICChan.IsEnabled = true;

            lblICServer.FontStyle = FontStyles.Normal;
            lblICServer.Foreground = new SolidColorBrush(Color.FromRgb(166, 166, 166));

            lblICChan.FontStyle = FontStyles.Normal;
            lblICChan.Foreground = new SolidColorBrush(Color.FromRgb(166, 166, 166));
        }

        private void IC_UnChecked(object sender, RoutedEventArgs e)
        {
            comboICServer.IsEnabled = false;
            comboICChan.IsEnabled = false;

            lblICServer.FontStyle = FontStyles.Italic;
            lblICServer.Foreground = new SolidColorBrush(Color.FromRgb(120, 120, 120));

            lblICChan.FontStyle = FontStyles.Italic;
            lblICChan.Foreground = new SolidColorBrush(Color.FromRgb(120, 120, 120));
        }

        private void clicked_color(object sender, MouseButtonEventArgs e)
        {
            Rectangle colorGet = (Rectangle)sender;
            SolidColorBrush colorInfo = (SolidColorBrush)colorGet.Fill;

            //Breakdown tag: [0] Field Name, [1] Settings File Key
            string[] tags = colorGet.Tag.ToString().Replace(" ", "").Split(new string[] { "," }, StringSplitOptions.None);

            Color_Picker colorGrab = new Color_Picker();
            Grid colorWindow = colorGrab.Draw(this, tags[0], colorInfo.Color.R.ToString(), colorInfo.Color.G.ToString(), colorInfo.Color.B.ToString(), tags[1]);

            this.gridMain.Children.Add(colorWindow);
        }

        // - Color Settings Window END ---------------------------------------------------

        private void settings_Apply(object sender, RoutedEventArgs e)
        {
            saveSettings();
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            mw.updateColors();
        }

        private void settings_ApplyNClose(object sender, RoutedEventArgs e)
        {
            saveSettings();
            this.Close();
        }

        private void settings_Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void saveSettings()
        {
            KeyValuePair<int, string> testSelection = (KeyValuePair<int, string>)serverDropDown.SelectedItem;
            if (ircServerName != testSelection.Value.ToString())
            {
                File_Tools.writeSetting("IRC_Server_Name", testSelection.Value.ToString());
                File_Tools.writeSetting("IRC_Server", serverList[testSelection.Key].getServer());
            }
            if (!(ircServerPort.Equals(txtPort.Text))) { File_Tools.writeSetting("IRC_Server_Port", txtPort.Text); }
        }

        // ----------------------------------------------------------------------------------------
        // === SCREEN WINDOW ======================================================================
        // ----------------------------------------------------------------------------------------

        Grid screenSetup = new Grid();
        bool mouseGotClicked = false;

        private void makeScreens()
        {
            this._defaultScreen = Screen.FromHandle(new System.Windows.Interop.WindowInteropHelper(this).Handle);

            RowDefinition oneRow = new RowDefinition();
            screenSetup.RowDefinitions.Add(oneRow);

            int r = 0;
            int c = 0;
            bool makeColumns = true;
            int m = 1;

            foreach (var screen in Screen.AllScreens)
            {

                if (c == 3)
                {
                    RowDefinition newRow = new RowDefinition();
                    screenSetup.RowDefinitions.Add(newRow);
                    r = r + 1;

                    c = 0;
                    makeColumns = false;
                }

                if ((c < 3) && (makeColumns))
                {
                    ColumnDefinition newCol = new ColumnDefinition();
                    screenSetup.ColumnDefinitions.Add(newCol);
                }

                Grid screenContain = new Grid();
                screenContain.Height = 50;
                screenContain.Width = 50;
                screenContain.SetValue(Grid.RowProperty, r);
                screenContain.SetValue(Grid.ColumnProperty, c);

                // Draw the monitors
                Image monitor = new Image();
                monitor.Source = new BitmapImage(new Uri(@"Icons/simple-monitor.png", UriKind.Relative));
                monitor.Stretch = Stretch.Uniform;

                // Draw numbers in middle of the monitors
                Label screenNumber = new Label();
                screenNumber.Content = m.ToString();
                screenNumber.Foreground = new SolidColorBrush(Color.FromRgb(166, 166, 166));
                screenNumber.TranslatePoint(new Point(0, 0), monitor);
                screenNumber.VerticalContentAlignment = VerticalAlignment.Center;
                screenNumber.HorizontalContentAlignment = HorizontalAlignment.Center;
                screenNumber.FontSize = 16;
                screenNumber.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#Seven Segment");
                screenNumber.Margin = new Thickness(3, 9, 3, 15);

                Rectangle mouseOver = new Rectangle();
                mouseOver.Name = screen.DeviceName.ToString().TrimStart(new char[] { '\\', '.' });
                mouseOver.Margin = new Thickness(-6, 0, -6, 0);
                mouseOver.RadiusX = 8;
                mouseOver.RadiusY = 8;
                mouseOver.MouseEnter += delegate (object source, MouseEventArgs e)
                {
                    monitorFadeIn(source);
                };
                mouseOver.MouseLeave += delegate (object source, MouseEventArgs e)
                {
                    monitorFadeOut(source);
                };
                mouseOver.PreviewMouseDown += delegate (object source, MouseButtonEventArgs e)
                {
                    monitorChangeDefault(source);
                    mouseGotClicked = true;
                };
                if (screen.ToString() == this._defaultScreen.ToString())
                {
                    mouseOver.Fill = new SolidColorBrush(Color.FromArgb(50, 0, 120, 166));
                    mouseOver.Tag = "Selected";
                }
                else
                {
                    mouseOver.Fill = new SolidColorBrush(Color.FromArgb(0, 166, 166, 166));
                }
                mouseOver.Cursor = Cursors.Hand;
                
                screenContain.Children.Add(monitor);
                screenContain.Children.Add(screenNumber);
                screenContain.Children.Add(mouseOver);
                screenSetup.Children.Add(screenContain);

                c = c + 1;
                m = m + 1;
            }

            monitorPanel.Children.Add(screenSetup);
        }

        private async void monitorFadeIn(object sender)
        {
            Rectangle element = (Rectangle)sender;
            SolidColorBrush currentFill = (SolidColorBrush)element.Fill;
            for (int i = Convert.ToInt32(currentFill.Color.A); i <= Convert.ToInt32(currentFill.Color.A) + 50; i++)
            {
                element.Fill = new SolidColorBrush(Color.FromArgb(Convert.ToByte(i), currentFill.Color.R, currentFill.Color.G, currentFill.Color.B));
                await Task.Delay(5);
                if (element.IsMouseOver == false)
                {
                    monitorFadeOut(sender);
                    break;
                }
                if (mouseGotClicked)
                {
                    mouseGotClicked = false;
                    break;
                }
            }
        }

        private async void monitorFadeOut(object sender)
        {
            Rectangle element = (Rectangle)sender;
            SolidColorBrush currentFill = (SolidColorBrush)element.Fill;
            int stahp = 0;
            try
            {
                if (element.Tag.ToString() == "Selected") { stahp = 50; }
            }
            catch
            {

            }
            for (int i = Convert.ToInt32(currentFill.Color.A); i >= stahp; i--)
            {
                element.Fill = new SolidColorBrush(Color.FromArgb(Convert.ToByte(i), currentFill.Color.R, currentFill.Color.G, currentFill.Color.B));
                await Task.Delay(5);
                if (element.IsMouseOver)
                {
                    monitorFadeIn(sender);
                    break;
                }
                if (mouseGotClicked)
                {
                    mouseGotClicked = false;
                    break;
                }
            }
        }

        private void monitorChangeDefault(object sender)
        {
            clearMonitorSelection();
            Rectangle catcher = (Rectangle)sender;
            catcher.Fill = new SolidColorBrush(Color.FromArgb(50, 0, 120, 166));
            catcher.Tag = "Selected";

            File_Tools.writeSetting("channel_dock_monitor", catcher.Name.ToString());
        }

        private void clearMonitorSelection()
        {

            foreach (Grid pod in screenSetup.Children.OfType<Grid>())
            {
                foreach (Rectangle selection in pod.Children.OfType<Rectangle>())
                {
                    selection.Fill = new SolidColorBrush(Color.FromArgb(0, 166, 166, 166));
                    selection.Tag = "";
                }
            }
        }

    }
}
