using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Screen = System.Windows.Forms.Screen;
using Rect = System.Drawing.Rectangle;

namespace Pixel_IRC
{
    class dock : Window
    {
        //Dock position setting is: channel_dock_position
        //Monitor is: channel_dock_monitor
        //Always on top (Topmost) is: channel_dock_onTop

        //Initialize variables, assign default values.
        private string _position = "L";
        private bool _topmost = true;
        private string _monitor = "DISPLAY1";

        ScrollViewer scroll = new ScrollViewer();

        bool closedState = false;
        Screen getScreen = null;
        Rect screenArea = new Rect(); //Changed from resScreen to screenArea to test for multiple monitor support. All other code changed to accomodate.

        public dock()
        {
            drawDock();
        }

        internal void drawDock() { 

            ////////////////////////////////////////////////////////////////////////////////////////////
            // Load Dock, Set Window ///////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////////////
            Window dock = new Window();

            Screen[] drawScreen = Screen.AllScreens;

            //Load settings.
            this._position = File_Tools.readSettings("channel_dock_position").Trim();
            if (File_Tools.readSettings("channel_dock_onTop").Trim() == "1") { this._topmost = true; }
            if (File_Tools.readSettings("channel_dock_onTop").Trim() == "0") { this._topmost = false; }
            this._monitor = File_Tools.readSettings("channel_dock_monitor").Trim();

            bool scnExists = false;
            //Filter screens if screen data has changed.
            foreach (Screen probe in drawScreen)
            {
                string runWitIt = probe.DeviceName.ToString().Replace("\\", "");
                runWitIt = runWitIt.Replace(".", "").Trim();
                if (runWitIt == this._monitor)
                {
                    scnExists = true;
                    getScreen = probe;
                }
            }
            if (!scnExists)
            {
                this._monitor = Screen.PrimaryScreen.DeviceName.Trim('\\', '.');
                getScreen = drawScreen[0];
            }

            this.WindowStyle = WindowStyle.None;

            this.WindowStartupLocation = WindowStartupLocation.Manual;
            screenArea = getScreen.WorkingArea;

            int scnHeight = screenArea.Height;
            int scnWidth = screenArea.Right - screenArea.Left;

            Grid body = new Grid();
            Grid clearArea = new Grid();
            StackPanel clicker = new StackPanel();

            StackPanel content = new StackPanel();

            ////////////////////////////////////////////////////////////////////////////////////////////
            // Right Side Orientation //////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////////////

            if (this._position == "R")
            {

                this.Width = 250;
                this.Height = scnHeight;
                this.Top = 0;
                this.AllowsTransparency = true;
                this.Left = scnWidth - this.Width + screenArea.Left;
                this.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                this.Topmost = this._topmost;

                

                body.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                RowDefinition oneRow = new RowDefinition();
                body.RowDefinitions.Add(oneRow);
                ColumnDefinition slideColumn = new ColumnDefinition();
                slideColumn.Width = new GridLength(25);
                ColumnDefinition contentColumn = new ColumnDefinition();
                body.ColumnDefinitions.Add(slideColumn);
                body.ColumnDefinitions.Add(contentColumn);

                clearArea.Height = this.Height;
                clearArea.Width = 25;
                clearArea.Margin = new Thickness(0, 0, 0, 0);
                clearArea.HorizontalAlignment = HorizontalAlignment.Left;
                clearArea.VerticalAlignment = VerticalAlignment.Top;
                clearArea.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                clearArea.SetValue(Grid.RowProperty, 0);
                clearArea.SetValue(Grid.ColumnProperty, 0);

                clicker.Width = 25;
                clicker.Height = 100;
                clicker.Margin = new Thickness(0, (clearArea.Height / 2) - (clicker.Height / 2), 0, 0);
                clicker.TranslatePoint(new Point(0, 0), clearArea);
                clicker.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                clicker.HorizontalAlignment = HorizontalAlignment.Left;
                clicker.VerticalAlignment = VerticalAlignment.Top;
                clicker.Cursor = Cursors.Hand;

                StackPanel arrows = new StackPanel();
                arrows.IsHitTestVisible = false;
                Label leftArrow = new Label();
                leftArrow.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/cour.ttf");
                leftArrow.Foreground = new SolidColorBrush(Color.FromRgb(166, 166, 166));
                leftArrow.FontSize = 20;
                leftArrow.FontStretch = FontStretches.Condensed;
                leftArrow.Content = "\u25C4";
                leftArrow.IsHitTestVisible = false;
                leftArrow.VerticalContentAlignment = VerticalAlignment.Center;
                leftArrow.HorizontalContentAlignment = HorizontalAlignment.Center;
                leftArrow.Height = 40;

                Label arrowDivider = new Label();
                arrowDivider.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/cour.ttf");
                arrowDivider.Foreground = new SolidColorBrush(Color.FromRgb(166, 166, 166));
                arrowDivider.FontSize = 20;
                arrowDivider.FontStretch = FontStretches.Condensed;
                arrowDivider.Content = "\u25AC";
                arrowDivider.IsHitTestVisible = false;
                arrowDivider.VerticalContentAlignment = VerticalAlignment.Center;
                arrowDivider.HorizontalContentAlignment = HorizontalAlignment.Center;
                arrowDivider.Height = 10;

                Label rightArrow = new Label();
                rightArrow.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/cour.ttf");
                rightArrow.Foreground = new SolidColorBrush(Color.FromRgb(166, 166, 166));
                rightArrow.FontSize = 20;
                rightArrow.FontStretch = FontStretches.Condensed;
                rightArrow.Content = "\u25BA";
                rightArrow.IsHitTestVisible = false;
                rightArrow.VerticalContentAlignment = VerticalAlignment.Center;
                rightArrow.HorizontalContentAlignment = HorizontalAlignment.Center;
                rightArrow.Height = 40;
                

                arrows.Children.Add(leftArrow);
                arrows.Children.Add(arrowDivider);
                arrows.Children.Add(rightArrow);
                clicker.Children.Add(arrows);
                

                Point _currMouse = new Point();
                double _currTop = 0;
                bool isMoving = false;

                clicker.PreviewMouseLeftButtonDown += delegate (object sender, MouseButtonEventArgs e)
                {
                    _currMouse = e.GetPosition(clearArea);
                    _currTop = clicker.Margin.Top;

                };

                clicker.MouseLeftButtonUp += delegate (object sender, MouseButtonEventArgs e)
                {
                    if (!isMoving)
                    {
                        slideWindowR();
                    }
                    isMoving = false;
                };

                clicker.PreviewMouseMove += delegate (object sender, MouseEventArgs e)
                {
                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        isMoving = true;

                        Point newMouse = e.GetPosition(clearArea);
                        double intOldY = _currMouse.Y;
                        double intNewY = newMouse.Y;

                        double moveY = intOldY - intNewY;

                        clicker.Margin = new Thickness(0, _currTop - moveY, 0, 0);
                    }
                };

                scroll.Background = new SolidColorBrush(Color.FromArgb(200, 0, 0, 0));
                scroll.SetValue(Grid.RowProperty, 0);
                scroll.SetValue(Grid.ColumnProperty, 1);
                scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

            }

            ////////////////////////////////////////////////////////////////////////////////////////////
            // Left Side Orientation ///////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////////////

            if (this._position == "L")
            {
                this.Width = 250;
                this.Height = scnHeight;
                this.Top = 0;
                this.AllowsTransparency = true;
                this.Left = screenArea.Left;
                this.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                this.Topmost = this._topmost;

                body.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                RowDefinition oneRow = new RowDefinition();
                body.RowDefinitions.Add(oneRow);
                ColumnDefinition slideColumn = new ColumnDefinition();
                slideColumn.Width = new GridLength(25);
                ColumnDefinition contentColumn = new ColumnDefinition();
                body.ColumnDefinitions.Add(contentColumn);
                body.ColumnDefinitions.Add(slideColumn);

                clearArea.Height = this.Height;
                clearArea.Width = 25;
                clearArea.Margin = new Thickness(0, 0, 0, 0);
                clearArea.HorizontalAlignment = HorizontalAlignment.Left;
                clearArea.VerticalAlignment = VerticalAlignment.Top;
                clearArea.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                clearArea.SetValue(Grid.RowProperty, 0);
                clearArea.SetValue(Grid.ColumnProperty, 1);

                clicker.Width = 25;
                clicker.Height = 100;
                clicker.Margin = new Thickness(0, (clearArea.Height / 2) - (clicker.Height / 2), 0, 0);
                clicker.TranslatePoint(new Point(0, 0), clearArea);
                clicker.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                clicker.HorizontalAlignment = HorizontalAlignment.Left;
                clicker.VerticalAlignment = VerticalAlignment.Top;
                clicker.Cursor = Cursors.Hand;

                Point _currMouse = new Point();
                double _currTop = 0;
                bool isMoving = false;

                clicker.PreviewMouseLeftButtonDown += delegate (object sender, MouseButtonEventArgs e)
                {
                    _currMouse = e.GetPosition(clearArea);
                    _currTop = clicker.Margin.Top;

                };

                clicker.MouseLeftButtonUp += delegate (object sender, MouseButtonEventArgs e)
                {
                    if (!isMoving)
                    {
                        slideWindowL();
                    }
                    isMoving = false;
                };

                clicker.PreviewMouseMove += delegate (object sender, MouseEventArgs e)
                {
                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        isMoving = true;

                        Point newMouse = e.GetPosition(clearArea);
                        double intOldY = _currMouse.Y;
                        double intNewY = newMouse.Y;

                        double moveY = intOldY - intNewY;

                        clicker.Margin = new Thickness(0, _currTop - moveY, 0, 0);
                    }
                };

                scroll.Background = new SolidColorBrush(Color.FromArgb(200, 0, 0, 0));
                scroll.SetValue(Grid.RowProperty, 0);
                scroll.SetValue(Grid.ColumnProperty, 0);
                scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            }

            ////////////////////////////////////////////////////////////////////////////////////////////
            // Top Screen Orientation //////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////////////

            if (this._position == "T")
            {
                this.Width = scnWidth;
                this.Height = 200;
                this.Top = 0;
                this.AllowsTransparency = true;
                this.Left = screenArea.Left;
                this.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                this.Topmost = this._topmost;

                content.Orientation = Orientation.Horizontal;

                body.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                RowDefinition contentRow = new RowDefinition();
                RowDefinition slideRow = new RowDefinition();
                slideRow.Height = new GridLength(25);
                body.RowDefinitions.Add(contentRow);
                body.RowDefinitions.Add(slideRow);
                ColumnDefinition oneCol = new ColumnDefinition();
                body.ColumnDefinitions.Add(oneCol);

                clearArea.Height = 25;
                clearArea.Width = this.Width;
                clearArea.Margin = new Thickness(0, 0, 0, 0);
                clearArea.HorizontalAlignment = HorizontalAlignment.Left;
                clearArea.VerticalAlignment = VerticalAlignment.Top;
                clearArea.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                clearArea.SetValue(Grid.RowProperty, 1);
                clearArea.SetValue(Grid.ColumnProperty, 0);

                clicker.Width = 100;
                clicker.Height = 25;
                clicker.Margin = new Thickness((clearArea.Width / 2) - (clicker.Width /2), 0, 0, 0);
                clicker.TranslatePoint(new Point(0, 0), clearArea);
                clicker.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                clicker.HorizontalAlignment = HorizontalAlignment.Left;
                clicker.VerticalAlignment = VerticalAlignment.Top;
                clicker.Cursor = Cursors.Hand;

                Point _currMouse = new Point();
                double _currLeft = 0;
                bool isMoving = false;

                clicker.PreviewMouseLeftButtonDown += delegate (object sender, MouseButtonEventArgs e)
                {
                    _currMouse = e.GetPosition(clearArea);
                    _currLeft = clicker.Margin.Left;

                };

                clicker.MouseLeftButtonUp += delegate (object sender, MouseButtonEventArgs e)
                {
                    if (!isMoving)
                    {
                        slideWindowT();
                    }
                    isMoving = false;
                };

                clicker.PreviewMouseMove += delegate (object sender, MouseEventArgs e)
                {
                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        isMoving = true;

                        Point newMouse = e.GetPosition(clearArea);
                        double intOldX = _currMouse.X;
                        double intNewX = newMouse.X;

                        double moveX = intOldX - intNewX;

                        clicker.Margin = new Thickness(_currLeft - moveX, 0, 0, 0);
                    }
                };

                scroll.Background = new SolidColorBrush(Color.FromArgb(200, 0, 0, 0));
                scroll.SetValue(Grid.RowProperty, 0);
                scroll.SetValue(Grid.ColumnProperty, 0);
                scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            }

            ////////////////////////////////////////////////////////////////////////////////////////////
            // Bottom Screen Orientation ///////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////////////

            if (this._position == "B")
            {
                this.Width = scnWidth;
                this.Height = 200;
                this.Top = scnHeight - this.Height;
                this.AllowsTransparency = true;
                this.Left = screenArea.Left;
                this.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                this.Topmost = this._topmost;

                content.Orientation = Orientation.Horizontal;

                body.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                RowDefinition contentRow = new RowDefinition();
                RowDefinition slideRow = new RowDefinition();
                slideRow.Height = new GridLength(25);
                body.RowDefinitions.Add(slideRow);
                body.RowDefinitions.Add(contentRow);
                ColumnDefinition oneCol = new ColumnDefinition();
                body.ColumnDefinitions.Add(oneCol);

                clearArea.Height = 25;
                clearArea.Width = this.Width;
                clearArea.Margin = new Thickness(0, 0, 0, 0);
                clearArea.HorizontalAlignment = HorizontalAlignment.Left;
                clearArea.VerticalAlignment = VerticalAlignment.Top;
                clearArea.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                clearArea.SetValue(Grid.RowProperty, 0);
                clearArea.SetValue(Grid.ColumnProperty, 0);

                clicker.Width = 100;
                clicker.Height = 25;
                clicker.Margin = new Thickness((clearArea.Width / 2) - (clicker.Width / 2), 0, 0, 0);
                clicker.TranslatePoint(new Point(0, 0), clearArea);
                clicker.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                clicker.HorizontalAlignment = HorizontalAlignment.Left;
                clicker.VerticalAlignment = VerticalAlignment.Top;
                clicker.Cursor = Cursors.Hand;

                Point _currMouse = new Point();
                double _currLeft = 0;
                bool isMoving = false;

                clicker.PreviewMouseLeftButtonDown += delegate (object sender, MouseButtonEventArgs e)
                {
                    _currMouse = e.GetPosition(clearArea);
                    _currLeft = clicker.Margin.Left;

                };

                clicker.MouseLeftButtonUp += delegate (object sender, MouseButtonEventArgs e)
                {
                    if (!isMoving)
                    {
                        slideWindowB();
                    }
                    isMoving = false;
                };

                clicker.PreviewMouseMove += delegate (object sender, MouseEventArgs e)
                {
                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        isMoving = true;

                        Point newMouse = e.GetPosition(clearArea);
                        double intOldX = _currMouse.X;
                        double intNewX = newMouse.X;

                        double moveX = intOldX - intNewX;

                        clicker.Margin = new Thickness(_currLeft - moveX, 0, 0, 0);
                    }
                };

                scroll.Background = new SolidColorBrush(Color.FromArgb(200, 0, 0, 0));
                scroll.SetValue(Grid.RowProperty, 1);
                scroll.SetValue(Grid.ColumnProperty, 0);
                scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            }

            NameScope.SetNameScope(this, new NameScope());

            clearArea.Children.Add(clicker);

            this.RegisterName("content", content);

            scroll.Content = content;
            body.Children.Add(scroll);
            body.Children.Add(clearArea);
            this.Content = body;

            this.Show();

        }

        // -----------------------------------------
        // SLIDE ANIMATIONS
        // -----------------------------------------

        public async void slideWindowR()
        {
            double startLeft = this.Left;
            double maintainWidth = screenArea.Right;
            if (closedState)
            {
                for (int i = Convert.ToInt32(startLeft); i >= (Convert.ToInt32(screenArea.Width) + Convert.ToInt32(screenArea.Left)) - 250; i -= 5)
                {
                    this.Left = i;
                    this.Width = maintainWidth - this.Left;
                    await Task.Delay(1);
                }
                if (scroll.VerticalScrollBarVisibility == ScrollBarVisibility.Hidden)
                {
                    scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                }
                closedState = false;
            }
            else
            {
                if (scroll.VerticalScrollBarVisibility == ScrollBarVisibility.Auto)
                {
                    scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                }
                for (int i = Convert.ToInt32(startLeft); i <= (Convert.ToInt32(screenArea.Width) + Convert.ToInt32(screenArea.Left)) - 25; i += 5)
                {
                    this.Left = i;
                    this.Width = maintainWidth - this.Left;
                    await Task.Delay(1);
                }
                closedState = true;
            }
        }

        public async void slideWindowL()
        {
            double startWidth = this.Width;
            double maintainWidth = screenArea.Right;
            if (closedState)
            {
                for (int i = Convert.ToInt32(startWidth); i <= 250; i += 5)
                {
                    this.Width = i;
                    await Task.Delay(1);
                }
                if (scroll.VerticalScrollBarVisibility == ScrollBarVisibility.Hidden)
                {
                    scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                }
                closedState = false;
            }
            else
            {
                if (scroll.VerticalScrollBarVisibility == ScrollBarVisibility.Auto)
                {
                    scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                }
                for (int i = Convert.ToInt32(startWidth); i >= 25; i -= 5)
                {
                    this.Width = i;
                    await Task.Delay(1);
                }
                closedState = true;
            }
        }
        public async void slideWindowT()
        {
            double startHeight = this.Height;
            double maintainWidth = screenArea.Right;
            if (closedState)
            {
                for (int i = Convert.ToInt32(startHeight); i <= 200; i += 5)
                {
                    this.Height = i;
                    await Task.Delay(1);
                }
                if (scroll.HorizontalScrollBarVisibility == ScrollBarVisibility.Hidden)
                {
                    scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                }
                closedState = false;
            }
            else
            {
                if (scroll.HorizontalScrollBarVisibility == ScrollBarVisibility.Auto)
                {
                    scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                }
                for (int i = Convert.ToInt32(startHeight); i >= 25; i -= 5)
                {
                    this.Height = i;
                    await Task.Delay(1);
                }
                closedState = true;
            }
        }
        public async void slideWindowB()
        {
            double startTop = this.Top;
            double startHeight = this.Height;
            if (closedState)
            {
                for (int i = Convert.ToInt32(startTop); i >= screenArea.Height - 200; i -= 5)
                {
                    this.Top = i;
                    this.Height = screenArea.Height - this.Top;
                    await Task.Delay(1);
                }
                if (scroll.HorizontalScrollBarVisibility == ScrollBarVisibility.Hidden)
                {
                    scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                }
                closedState = false;
            }
            else
            {
                if (scroll.HorizontalScrollBarVisibility == ScrollBarVisibility.Auto)
                {
                    scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                }
                for (int i = Convert.ToInt32(startTop); i <= screenArea.Height - 25; i += 5)
                {
                    this.Top = i;
                    this.Height = screenArea.Height - this.Top;
                    await Task.Delay(1);
                }
                closedState = true;
            }
        }
    }
}
