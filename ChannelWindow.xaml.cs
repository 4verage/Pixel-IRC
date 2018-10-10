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

namespace Pixel_IRC
{
    /// <summary>
    /// Interaction logic for ChannelWindow.xaml
    /// </summary>
    public partial class ChannelWindow : Window
    {

        public string channel = "";
        private SolidColorBrush _backupBrush = null;

        /// <summary>
        /// Constructor for Window component.
        /// </summary>
        public ChannelWindow(StackPanel dock)
        {
            InitializeComponent();
            Settings getSettings = new Settings();
            if (getSettings.settingsFileExists()) //If the settings file is valid, load settings.
            {
                txtUserInput.Foreground = setColor("chanUserInput");
            }
            getSettings.Close();
            this.Padding = new Thickness(0);

            this.StateChanged += delegate (object sender, EventArgs e)
            {
                if (this.WindowState == WindowState.Minimized)
                {
                    this._backupBrush = (SolidColorBrush)chanBody.Background;
                    Grid bounds = new Grid();

                    Grid display = new Grid();
                    RowDefinition oneRow = new RowDefinition();
                    RowDefinition twoRow = new RowDefinition();
                    display.RowDefinitions.Add(oneRow);
                    display.RowDefinitions.Add(twoRow);
                    display.Margin = new Thickness(5, 5, 5, 5);

                    Viewbox shrinker = new Viewbox();

                    this.Content = null;
                    this.ShowInTaskbar = false;

                    chanBody.Background = new SolidColorBrush(Color.FromArgb(90, 180, 180, 180));
                    chanBody.HorizontalAlignment = HorizontalAlignment.Stretch;
                    chanBody.VerticalAlignment = VerticalAlignment.Stretch;
                    shrinker.Child = chanBody;

                    Label chanName = new Label();
                    chanName.Content = this.channel;
                    chanName.Foreground = new SolidColorBrush(Color.FromArgb(220, 255, 255, 255));
                    chanName.Margin = new Thickness(0);
                    chanName.Padding = new Thickness(0);
                    chanName.VerticalAlignment = VerticalAlignment.Top;
                    chanName.VerticalContentAlignment = VerticalAlignment.Center;
                    chanName.SetValue(Grid.RowProperty, 1);
                    chanName.SetValue(Grid.ColumnProperty, 0);

                    int newWidth = 205;
                    double aspectRatio = newWidth / this.Width;
                    int newHeight = Convert.ToInt32(this.Height * aspectRatio);

                    chanBody.Height = 400;
                    chanBody.Width = 700;

                    shrinker.Height = newHeight;
                    shrinker.Width = newWidth;

                    shrinker.Margin = new Thickness(0);

                    shrinker.TranslatePoint(new Point(0, 0), dock);

                    shrinker.HorizontalAlignment = HorizontalAlignment.Left;
                    shrinker.VerticalAlignment = VerticalAlignment.Top;

                    shrinker.SetValue(Grid.RowProperty, 0);
                    shrinker.SetValue(Grid.ColumnProperty, 0);

                    Rectangle clickCover = new Rectangle();
                    clickCover.TranslatePoint(new Point(0, 0), display);
                    clickCover.Margin = new Thickness(5,5,5,5);
                    clickCover.HorizontalAlignment = HorizontalAlignment.Stretch;
                    clickCover.VerticalAlignment = VerticalAlignment.Stretch;
                    clickCover.Fill = new SolidColorBrush(Color.FromArgb(120, 0, 0, 0));
                    clickCover.Cursor = Cursors.Hand;
                    clickCover.PreviewMouseDown += delegate (object source, MouseButtonEventArgs m)
                    {
                        dock.Children.Remove(bounds);
                        shrinker.Child = null;
                        Window newWindow = this;
                        newWindow.Height = 321.485;
                        newWindow.Width = 525.2;
                        chanBody.Background = this._backupBrush;
                        chanBody.Height = double.NaN;
                        chanBody.Width = double.NaN;
                        chanBody.Margin = new Thickness(0);
                        newWindow.Content = chanBody;
                        newWindow.Show();
                        newWindow.WindowState = WindowState.Normal;
                        newWindow.ShowInTaskbar = true;
                    };
                    clickCover.MouseEnter += delegate (object source, MouseEventArgs m)
                    {
                        lightenContainer(source);
                    };
                    clickCover.MouseLeave += delegate (object source, MouseEventArgs m)
                    {
                        darkenContainer(source);
                    };

                    string dockOrientation = File_Tools.readSettings("channel_dock_position").Trim();
                    if (dockOrientation == "T")
                    {
                        bounds.Margin = new Thickness(5, 5, 0, 10);
                        clickCover.Margin = new Thickness(0);
                        chanName.SetValue(Grid.RowProperty, 0);
                        shrinker.SetValue(Grid.RowProperty, 1);
                        twoRow.Height = new GridLength(shrinker.Height);
                    }
                    if (dockOrientation == "B")
                    {
                        bounds.Margin = new Thickness(5, 5, 0, 10);
                        clickCover.Margin = new Thickness(0);
                        oneRow.Height = new GridLength(shrinker.Height);
                    }

                    display.Children.Add(shrinker);
                    display.Children.Add(chanName);

                    bounds.Children.Add(display);
                    bounds.Children.Add(clickCover);

                    dock.Children.Add(bounds);

                    this.Hide();

                }
            };
        }

        private async void lightenContainer(object sender)
        {
            Rectangle clickCover = (Rectangle)sender;
            SolidColorBrush colors = (SolidColorBrush)clickCover.Fill;
            for (int i = 120; i >= 0; i-=5)
            {
                clickCover.Fill = new SolidColorBrush(Color.FromArgb(Convert.ToByte(i), colors.Color.R, colors.Color.G, colors.Color.B));
                if (!clickCover.IsMouseOver)
                {
                    break;
                }
                await Task.Delay(5);
            }
        }

        private async void darkenContainer(object sender)
        {
            Rectangle clickCover = (Rectangle)sender;
            SolidColorBrush colors = (SolidColorBrush)clickCover.Fill;
            for (int i = 0; i <= 120; i+=5)
            {
                clickCover.Fill = new SolidColorBrush(Color.FromArgb(Convert.ToByte(i), colors.Color.R, colors.Color.G, colors.Color.B));
                if (clickCover.IsMouseOver)
                {
                    break;
                }
                await Task.Delay(5);
            }
        }

        /// <summary>
        /// Used to set the channel name of this window's hosted channel.
        /// </summary>
        public void setChannel()
        {
            this.channel = "#" + this.Name.Substring(7);
        }

        /// <summary>
        /// Used to get the name of this window's hosted IRC channel.
        /// </summary>
        /// <returns></returns>
        public string getChannel()
        {
            return this.channel;
        }

        /// <summary>
        /// Searches settings file and returns the provided color.
        /// </summary>
        /// <param name="getKey">The key to search settings file for.</param>
        /// <returns>Returns a SolidColorBrush object to paint the color with.</returns>
        private SolidColorBrush setColor(string getKey)
        {
            string[] colors = File_Tools.readSettings(getKey).Split(new string[] { ", " }, StringSplitOptions.None);
            byte[] color = new byte[] { Convert.ToByte(colors[0]), Convert.ToByte(colors[1]), Convert.ToByte(colors[2]) };
            return new SolidColorBrush(Color.FromRgb(color[0], color[1], color[2]));
        }

        private void txtUserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                foreach (Window window in Application.Current.Windows)
                {
                    if (window.Name == "consoleWindow")
                    {
                        MainWindow processor = (MainWindow)window;

                        string testingit = processor.CommandProcessor(txtUserInput.Text, this);
                        txtUserInput.Clear();
                        processor.sendOut(testingit);

                    }
                }
            }
        }

        /// <summary>
        /// Populate the nickList Listbox of channel's usernames.
        /// </summary>
        /// <param name="users">List of users passed in from stream reader.</param>
        /// <param name="append">Used to determine if this line is to be appended or begin the nick list. (Used for channels with lots of users).</param>
        public void updateNameList(string users, int append)
        {
            List<string> names = new List<string>();
            if (append == 0)
            {
                nickList.ItemsSource = null;
                nickList.Items.Clear();
                string[] breakdown = users.Split(new string[] { " " }, StringSplitOptions.None);
                for (int i = 0; i < breakdown.Length; i++)
                {
                    names.Add(breakdown[i]);
                }
            }
            else
            {
                string[] breakdown = users.Split(new string[] { " " }, StringSplitOptions.None);
                foreach (String item in nickList.Items)
                {
                    names.Add(item);
                }
                for (int i = 0; i < breakdown.Length; i++)
                {
                    names.Add(breakdown[i]);
                }
            }
            names.Sort();
            nickList.ItemsSource = names;
        }

        private void Channel_Closed(object sender, EventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.Name == "consoleWindow")
                {
                    MainWindow superCommands = (MainWindow)window;
                    if (superCommands.windowExists(this.Name))
                    {
                        superCommands.sendOut("PART " + getChannel() + " :Leaving...");
                        superCommands.UnregisterName(this.Name);
                    }
                }
            }
        }

        private void nickList_SubMenu(object sender, ContextMenuEventArgs m)
        {
            bool nickSelected = false;
            bool isNickOperator = false;
            bool isNickVoice = false;
            if (nickList.SelectedItem != null)
            {
                nickSelected = true;
            }
            if (nickSelected)
            {
                if (isOP(nickList.SelectedItem.ToString()))
                {
                    isNickOperator = true;
                }
                else if (isVoice(nickList.SelectedItem.ToString()))
                {
                    isNickVoice = true;
                }
            }

            MainWindow power = null;

            foreach (Window window in Application.Current.Windows)
            {
                if (window.Name == "consoleWindow")
                {
                    power = (MainWindow)window;
                }
            }
            string username = "";

            if (power != null)
            {
                foreach (string usr in nickList.Items)
                {
                    if (usr.Contains(power.getNick())) { username = usr; }
                }
            }

            ContextMenu disSubMenu = new ContextMenu();
            if (username != "")
            {
                if (isOP(username))
                {
                    disSubMenu.Items.Add("Operator Commands");
                    disSubMenu.Items.Add("-");
                    if (isNickOperator)
                    {
                        MenuItem OPrem = new MenuItem();
                        OPrem.Header = "Remove OP";
                        disSubMenu.Items.Add(OPrem);

                        MenuItem Vexch = new MenuItem();
                        Vexch.Header = "Make Voice";
                        Vexch.Click += OpToVoice;
                        disSubMenu.Items.Add(Vexch);
                    }
                    if (isNickVoice)
                    {
                        disSubMenu.Items.Add("Voiced User");
                        disSubMenu.Items.Add("-");
                    }

                    MenuItem menuKick = new MenuItem();
                    menuKick.Header = "Kick";
                    menuKick.Click += kickUser;
                    disSubMenu.Items.Add(menuKick);

                    MenuItem menuBan = new MenuItem();
                    menuBan.Header = "Ban";
                    menuBan.Click += banUser;
                    disSubMenu.Items.Add(menuBan);

                    MenuItem menuKB = new MenuItem();
                    menuKB.Header = "Kick / Ban ";
                    menuKB.Click += kbUser;
                    disSubMenu.Items.Add(menuKB);

                    MenuItem subIgnore = new MenuItem();
                    subIgnore.Header = "Ignore";
                    MenuItem menuIgnore = new MenuItem();
                    menuIgnore.Header = "Ignore";
                    menuIgnore.Click += userIgnore;
                    subIgnore.Items.Add(menuIgnore);
                    MenuItem menuUnignore = new MenuItem();
                    menuUnignore.Header = "Unignore";
                    menuUnignore.Click += userUnignore;
                    subIgnore.Items.Add(menuUnignore);
                    disSubMenu.Items.Add(subIgnore);
                }
                else
                {
                    disSubMenu.Items.Add("Basic Commands");
                }
                disSubMenu.Items.Add("-");

                // DCC Menu
                MenuItem menuDCC = new MenuItem();
                menuDCC.Header = "DCC";
                MenuItem dccChat = new MenuItem();
                dccChat.Header = "Chat";
                dccChat.Click += openDCCchat;
                menuDCC.Items.Add(dccChat);
                MenuItem dccSend = new MenuItem();
                dccSend.Header = "Send File";
                dccSend.Click += openDCCsend;
                menuDCC.Items.Add(dccSend);
                disSubMenu.Items.Add(menuDCC);

                // Customizable SLAP Menu
                MenuItem menuSlaps = new MenuItem();
                menuSlaps.Header = "Slaps";
                // Add code here to import user-made slaps and/or programmed slaps.
                disSubMenu.Items.Add(menuSlaps);

                MenuItem prvMsg = new MenuItem();
                prvMsg.Header = "Private Message";
                prvMsg.Click += openPrvMsg;
                disSubMenu.Items.Add(prvMsg);
            }

            nickList.ContextMenu = disSubMenu;

        }

        // Nick Status Checks
        private bool isOP(string nick)
        {
            if (nick.StartsWith("@"))
            {
                return true;
            }
            else { return false; }
        }

        private bool isVoice(string nick)
        {
            if (nick.StartsWith("+"))
            {
                return true;
            }
            else { return false; }
        }

        // Context Menu Functions
        private void OpToVoice(object sender, EventArgs e) 
        {
            string nick = nickList.SelectedItem.ToString().Substring(1);
           
            MainWindow parent = getConsole();
            if (parent != null)
            {
                parent.sendOut("MODE " + this.channel + " -o " + nick);
                parent.sendOut("MODE " + this.channel + " +v " + nick);
            }
            
        }

        private MainWindow getConsole()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.Name == "consoleWindow")
                {
                    return (MainWindow)window;
                }
            }
            return null;
        }

        private void openDCCchat(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void openDCCsend(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void openPrvMsg(object sender, EventArgs e)
        {
            string nick = nickList.SelectedItem.ToString();
            if (isOP(nick) || isVoice(nick))
            {
                nick = nick.Substring(1);
            }

            // Code to open private message window here!
        }

        private void kickUser(object sender, EventArgs e)
        {
            string nick = nickList.SelectedItem.ToString();
            if (isOP(nick) || isVoice(nick))
            {
                nick = nick.Substring(1);
            }

            string msg = "";

            MainWindow parent = getConsole();
            if (parent != null)
            {
                parent.sendOut("KICK " + this.channel + " " + nick + " :" + msg);
            }
        }

        private void banUser(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void kbUser(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void userIgnore(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void userUnignore(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
