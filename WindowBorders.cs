using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Rect = System.Drawing.Rectangle;

namespace Pixel_IRC
{
    class WindowBorders
    {
        /// <summary>
        /// Creates a Window border around an existing Window object that allows minimizing and closing.
        /// </summary>
        /// <param name="sender">Should be the Window object sending the border request (normally: this)</param>
        public void MinClose(Window sender)
        {
            string titleText = "Pixel-IRC - " + sender.Title;
            titleText = stringSpacer(titleText);
            sender.Title = titleText;

            #region BorderBuilder

            Frame testing = new Frame();
            testing.Content = sender.Content;
            testing.Height = sender.Height;
            testing.Width = sender.Width;
            testing.SetValue(Grid.ColumnProperty, 1);
            testing.SetValue(Grid.RowProperty, 1);

            sender.WindowStyle = WindowStyle.None;
            sender.Height = sender.Height + 20;
            sender.Width = sender.Width + 20;

            GridLength borderSize = new GridLength(10);

            Grid layout = new Grid();

            RowDefinition topBorder = new RowDefinition();
            topBorder.Height = borderSize;
            RowDefinition midRow = new RowDefinition();
            midRow.Height = new GridLength(testing.Height);
            RowDefinition botBorder = new RowDefinition();
            botBorder.Height = borderSize;

            ColumnDefinition leftBorder = new ColumnDefinition();
            leftBorder.Width = borderSize;
            ColumnDefinition midCol = new ColumnDefinition();
            midCol.Width = new GridLength(testing.Width);
            ColumnDefinition rightBorder = new ColumnDefinition();
            rightBorder.Width = borderSize;

            layout.RowDefinitions.Add(topBorder);
            layout.RowDefinitions.Add(midRow);
            layout.RowDefinitions.Add(botBorder);

            layout.ColumnDefinitions.Add(leftBorder);
            layout.ColumnDefinitions.Add(midCol);
            layout.ColumnDefinitions.Add(rightBorder);

            StackPanel borderTop = winBorder(Orientation.Horizontal, 0, 0, 3);

            StackPanel borderLeft = winBorder(Orientation.Vertical, 1, 0);

            StackPanel borderRight = winBorder(Orientation.Vertical, 1, 2);

            StackPanel borderBottom = winBorder(Orientation.Horizontal, 2, 0, 3);
            #endregion

            // Add some graphics...

            #region BorderTitle
            double headWidth = 0;

            Image titleLeftCap = new Image();
            titleLeftCap.Source = new BitmapImage(new Uri("pack://application:,,,/skins/default/top_left_l_title_cap.png", UriKind.Absolute));
            borderTop.Children.Add(titleLeftCap);
            titleLeftCap.Loaded += delegate (object s, RoutedEventArgs e)
            {
                headWidth += titleLeftCap.ActualWidth;
            };

            if (titleText != "")
            {
                Label titleTextGraphic = new Label();
                titleTextGraphic.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/skins/default/top_title_bg.png", UriKind.Absolute)));
                titleTextGraphic.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/NIRMALA.TTF");
                titleTextGraphic.FontSize = 7;
                titleTextGraphic.VerticalContentAlignment = VerticalAlignment.Top;
                titleTextGraphic.Padding = new Thickness(5,0,2,0);
                titleTextGraphic.Margin = new Thickness(0);
                titleTextGraphic.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                titleTextGraphic.FontStyle = FontStyles.Italic;
                titleTextGraphic.Content = titleText;
                borderTop.Children.Add(titleTextGraphic);
                titleTextGraphic.Loaded += delegate (object s, RoutedEventArgs e)
                {
                    headWidth += titleTextGraphic.ActualWidth;
                };
            }
            else
            {
                Image titleFiller = new Image();
                titleFiller.Source = new BitmapImage(new Uri("pack://application:,,,/skins/default/top_title_bg.png", UriKind.Absolute));
                borderTop.Children.Add(titleFiller);
                titleFiller.Loaded += delegate (object s, RoutedEventArgs e)
                {
                    headWidth += titleFiller.ActualWidth;
                };
            }

            Image titleRightCap = new Image();
            titleRightCap.Source = new BitmapImage(new Uri("pack://application:,,,/skins/default/top_r_title_cap.png", UriKind.Absolute));
            titleRightCap.Height = 10;
            titleRightCap.Width = 38;
            borderTop.Children.Add(titleRightCap);
            titleRightCap.Loaded += delegate (object s, RoutedEventArgs e)
            {
                headWidth += titleRightCap.ActualWidth;
            };

            #endregion

            #region BorderControlsButtons
            Grid buttonSet = new Grid();
            RowDefinition topBorderRow = new RowDefinition();
            ColumnDefinition topBorderFill = new ColumnDefinition();
            Rectangle emptyFill = new Rectangle();
            emptyFill.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/skins/default/top_bg.png", UriKind.Absolute)));
            emptyFill.SetValue(Grid.RowProperty, 0);
            emptyFill.SetValue(Grid.ColumnProperty, 0);
            ColumnDefinition topBorderButtons = new ColumnDefinition();
            buttonSet.Loaded += delegate (object s, RoutedEventArgs e)
            {
                buttonSet.Width = sender.Width - headWidth;
            };
            Image buttonPanel = new Image();
            buttonPanel.Source = new BitmapImage(new Uri("pack://application:,,,/skins/default/top_right_button_panel.png", UriKind.Absolute));
            buttonPanel.HorizontalAlignment = HorizontalAlignment.Right;
            buttonPanel.SetValue(Grid.RowProperty, 0);
            buttonPanel.SetValue(Grid.ColumnProperty, 1);
            buttonPanel.Loaded += delegate (object s, RoutedEventArgs e)
            {
                topBorderButtons.Width = new GridLength(buttonPanel.ActualWidth);
            };

            buttonSet.RowDefinitions.Add(topBorderRow);
            buttonSet.ColumnDefinitions.Add(topBorderFill);
            buttonSet.ColumnDefinitions.Add(topBorderButtons);

            Image closeButton = new Image();
            closeButton.HorizontalAlignment = HorizontalAlignment.Right;
            closeButton.Margin = new Thickness(0, 0, 6, 0);
            closeButton.Source = new BitmapImage(new Uri("pack://application:,,,/skins/default/close_button_off.png", UriKind.Absolute));
            closeButton.MouseEnter += delegate (object s, MouseEventArgs m)
            {
                closeButton.Source = new BitmapImage(new Uri("pack://application:,,,/skins/default/close_button_on.png", UriKind.Absolute));
            };
            closeButton.MouseLeave += delegate (object s, MouseEventArgs m)
            {
                closeButton.Source = new BitmapImage(new Uri("pack://application:,,,/skins/default/close_button_off.png", UriKind.Absolute));
            };
            closeButton.Cursor = Cursors.Hand;
            closeButton.MouseDown += delegate (object s, MouseButtonEventArgs m)
            {
                closeWindow(sender);
            };
            closeButton.SetValue(Grid.RowProperty, 0);
            closeButton.SetValue(Grid.ColumnProperty, 1);

            Image minButton = new Image();
            minButton.HorizontalAlignment = HorizontalAlignment.Right;
            minButton.Margin = new Thickness(0, 0, 20, 0);
            minButton.Source = new BitmapImage(new Uri("pack://application:,,,/skins/default/min_button_off.png", UriKind.Absolute));
            minButton.MouseEnter += delegate (object s, MouseEventArgs m)
            {
                minButton.Source = new BitmapImage(new Uri("pack://application:,,,/skins/default/min_button_on.png", UriKind.Absolute));
            };
            minButton.MouseLeave += delegate (object s, MouseEventArgs m)
            {
                minButton.Source = new BitmapImage(new Uri("pack://application:,,,/skins/default/min_button_off.png", UriKind.Absolute));
            };
            minButton.Cursor = Cursors.Hand;
            minButton.MouseDown += delegate (object s, MouseButtonEventArgs m)
            {
                sender.WindowState = WindowState.Minimized;
            };
            minButton.SetValue(Grid.RowProperty, 0);
            minButton.SetValue(Grid.ColumnProperty, 1);

            buttonSet.Children.Add(emptyFill);
            buttonSet.Children.Add(buttonPanel);
            buttonSet.Children.Add(closeButton);
            buttonSet.Children.Add(minButton);
            #endregion

            borderTop.Children.Add(buttonSet);

            layout.Children.Add(borderTop);
            layout.Children.Add(borderLeft);
            layout.Children.Add(testing);
            layout.Children.Add(borderRight);
            layout.Children.Add(borderBottom);

            #region WindowDragRules

            Point startMouse = new Point(0, 0);
            Point transMouse = new Point(0, 0);
            bool clickedTop = false;
            double oldWindowTop = 0;
            double oldWindowLeft = 0;

            borderTop.PreviewMouseDown += delegate (object s, MouseButtonEventArgs m)
            {
                Rect playground = new Rect(System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.X, System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Y, System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width, System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height);

                startMouse = m.GetPosition(sender);
                oldWindowTop = sender.Top;
                oldWindowLeft = sender.Left;

                clickedTop = true;
            };

            borderTop.MouseMove += delegate (object s, MouseEventArgs m)
            {
                if (m.LeftButton == MouseButtonState.Pressed)
                {
                    
                    transMouse = m.GetPosition(sender);
                    double moveX = startMouse.X - transMouse.X;
                    double moveY = startMouse.Y - transMouse.Y;

                    sender.Top = oldWindowTop - moveY;
                    sender.Left = oldWindowLeft - moveX;

                    oldWindowTop = sender.Top;
                    oldWindowLeft = sender.Left;

                    borderTop.CaptureMouse();
                    
                };
            };

            borderTop.MouseUp += delegate (object s, MouseButtonEventArgs m)
            {
                clickedTop = false;

                borderTop.ReleaseMouseCapture();
            };

            #endregion

            sender.Content = layout;
        }

        

        /// <summary>
        /// Prepares a new StackPanel for the border element to trim code.
        /// </summary>
        /// <param name="direction">Direction StackPanel will add new items.</param>
        /// <param name="rowNum">Indicates row number in the required parent Grid.</param>
        /// <param name="colNum">Indicates column number in the required parent Grid.</param>
        /// <param name="spanNum">OPTIONAL: Number of columns a row can span.</param>
        /// <returns></returns>
        private StackPanel winBorder(Orientation direction, int rowNum, int colNum, int spanNum = 0)
        {
            StackPanel borderBuilder = new StackPanel();
            borderBuilder.Orientation = direction;
            borderBuilder.SetValue(Grid.RowProperty, rowNum);
            borderBuilder.SetValue(Grid.ColumnProperty, colNum);
            if (spanNum != 0)
            {
                borderBuilder.SetValue(Grid.ColumnSpanProperty, spanNum);
            }

            return borderBuilder;
        }

        /// <summary>
        /// Takes a string and uses the C# built-in string indexer to add spaces between each existing character. (Faux letter-spacing)
        /// </summary>
        /// <param name="title">The string passed for processing.</param>
        /// <returns></returns>
        private string stringSpacer(string title)
        {
            string newTitle = "";

            for (int i = 0; i < title.Length; i++)
            {
                newTitle = newTitle + title[i] + " ";
            }

            newTitle = newTitle.Substring(0, newTitle.Length - 1);

            return newTitle;
        }

        private void closeWindow(Window sender)
        {
            sender.Close();  
        }
    }
}
