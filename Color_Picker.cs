using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.IO;
using System.Dynamic;
using System.Threading;

namespace Pixel_IRC
{
    class Color_Picker
    {
        Grid container = new Grid();
        Border bounds = new Border();
        TextBox redUpDown = new TextBox();
        TextBox greenUpDown = new TextBox();
        TextBox blueUpDown = new TextBox();
        Rectangle colorBox = new Rectangle();
        StackPanel lineup = new StackPanel();
        bool isCovered = false;
        string _key = "";

        public Grid Draw(object sender, string widgetName, string openRed, string openGreen, string openBlue, string searchKey)
        //public Color_Picker(object sender, string widgetName)
        {
            this._key = searchKey;

            // Create Container Object
            container.Margin = new Thickness(15);
            container.Name = "colorPicker";

            // Reference Creator Window
            Window dataObject = (Window)sender;

            // Create Border for Window
            bounds.BorderBrush = new SolidColorBrush(Color.FromRgb(166, 166, 166));
            bounds.BorderThickness = new Thickness(1);
            bounds.Margin = new Thickness(0);

            // Create Stackpanel to add Items
            StackPanel colorPopup = new StackPanel();
            colorPopup.Background = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            colorPopup.Margin = new Thickness(0);

            // Window Header / Titlebar
            TextBlock heading = new TextBlock();
            heading.Foreground = new SolidColorBrush(Color.FromRgb(166, 166, 166));
            heading.Padding = new Thickness(0, 3, 0, 3);
            heading.TextAlignment = TextAlignment.Center;
            heading.FontWeight = FontWeights.Bold;
            heading.Text = "Select Color for: " + widgetName;

            // Grid contains color selector, darkness slider, RGB inputs, and some swatch presets. (4 Columns)
            Grid columns = new Grid();
            columns.Background = new SolidColorBrush(Color.FromRgb(25, 25, 25));

            ColumnDefinition swatchColumn = new ColumnDefinition();
            swatchColumn.Width = new GridLength(150);
            ColumnDefinition brightDragger = new ColumnDefinition();
            ColumnDefinition rgbInputs = new ColumnDefinition();
            rgbInputs.Width = new GridLength(95);
            ColumnDefinition presetSwatches = new ColumnDefinition();
            columns.ColumnDefinitions.Add(swatchColumn);
            columns.ColumnDefinitions.Add(brightDragger);
            columns.ColumnDefinitions.Add(rgbInputs);
            columns.ColumnDefinitions.Add(presetSwatches);

            RowDefinition colorRow = new RowDefinition();
            colorRow.Height = new GridLength(120);
            columns.RowDefinitions.Add(colorRow);

            // Column 2/3 - RGB Values
            StackPanel rgbFields = new StackPanel();
            rgbFields.Margin = new Thickness(0);
            rgbFields.SetValue(Grid.RowProperty, 0);
            rgbFields.SetValue(Grid.ColumnProperty, 2);

            Grid labels = new Grid();
            ColumnDefinition col1 = new ColumnDefinition();
            col1.Width = new GridLength(45);
            ColumnDefinition col2 = new ColumnDefinition();
            col2.Width = new GridLength(50);
            labels.Margin = new Thickness(0, 20, 0, 20);
            labels.ColumnDefinitions.Add(col1);
            labels.ColumnDefinitions.Add(col2);

            RowDefinition row1 = new RowDefinition();
            row1.Height = new GridLength(29);
            RowDefinition row2 = new RowDefinition();
            row2.Height = new GridLength(29);
            RowDefinition row3 = new RowDefinition();
            row3.Height = new GridLength(29);
            labels.RowDefinitions.Add(row1);
            labels.RowDefinitions.Add(row2);
            labels.RowDefinitions.Add(row3);

            Label redTitle = new Label();
            redTitle.Content = "Red:";
            redTitle.Foreground = new SolidColorBrush(Color.FromRgb(166, 166, 166));
            redTitle.HorizontalContentAlignment = HorizontalAlignment.Right;
            redTitle.SetValue(Grid.RowProperty, 0);
            redTitle.SetValue(Grid.ColumnProperty, 0);
            redTitle.BorderBrush = new SolidColorBrush(Color.FromRgb(166, 166, 166));
            redTitle.BorderThickness = new Thickness(1);
            labels.Children.Add(redTitle);

            redUpDown.Text = "0";
            redUpDown.HorizontalContentAlignment = HorizontalAlignment.Right;
            redUpDown.VerticalContentAlignment = VerticalAlignment.Center;
            redUpDown.Margin = new Thickness(0, 0, 20, 0);
            redUpDown.SetValue(Grid.RowProperty, 0);
            redUpDown.SetValue(Grid.ColumnProperty, 1);
            redUpDown.TextChanged += delegate (object source, TextChangedEventArgs e)
            {
                int farter;
                bool testint = Int32.TryParse(redUpDown.Text, out farter);
                if (testint)
                {
                    if (farter < 0) { redUpDown.Text = "0"; }
                    if (farter > 255) { redUpDown.Text = "255"; }
                }
                else
                {
                    redUpDown.Text = "0";
                }
            };
            labels.Children.Add(redUpDown);

            Button redUpBtn = new Button();
            redUpBtn.Content = "\u25B2";
            redUpBtn.HorizontalContentAlignment = HorizontalAlignment.Center;
            redUpBtn.VerticalContentAlignment = VerticalAlignment.Center;
            redUpBtn.Padding = new Thickness(0);
            redUpBtn.Margin = new Thickness(30, 0, 0, 15);
            redUpBtn.SetValue(Grid.RowProperty, 0);
            redUpBtn.SetValue(Grid.ColumnProperty, 1);
            redUpBtn.Click += delegate (object source, RoutedEventArgs e)
            {
                int calcul8 = Convert.ToInt32(redUpDown.Text) + 1;
                if (calcul8 > 255) { redUpDown.Text = "0"; }
                else { redUpDown.Text = calcul8.ToString(); }
            };
            labels.Children.Add(redUpBtn);

            Button redDownBtn = new Button();
            redDownBtn.Content = "\u25BC";
            redDownBtn.HorizontalContentAlignment = HorizontalAlignment.Center;
            redDownBtn.VerticalContentAlignment = VerticalAlignment.Center;
            redDownBtn.Padding = new Thickness(0);
            redDownBtn.Margin = new Thickness(30, 14, 0, 0);
            redDownBtn.SetValue(Grid.RowProperty, 0);
            redDownBtn.SetValue(Grid.ColumnProperty, 1);
            redDownBtn.Click += delegate (object source, RoutedEventArgs e)
            {
                int calcul8 = Convert.ToInt32(redUpDown.Text) - 1;
                if (calcul8 < 0) { redUpDown.Text = "255"; }
                else { redUpDown.Text = calcul8.ToString(); }
            };
            labels.Children.Add(redDownBtn);

            Label greenTitle = new Label();
            greenTitle.Content = "Green:";
            greenTitle.Foreground = new SolidColorBrush(Color.FromRgb(166, 166, 166));
            greenTitle.HorizontalContentAlignment = HorizontalAlignment.Right;
            greenTitle.SetValue(Grid.RowProperty, 1);
            greenTitle.SetValue(Grid.ColumnProperty, 0);
            greenTitle.BorderBrush = new SolidColorBrush(Color.FromRgb(166, 166, 166));
            greenTitle.BorderThickness = new Thickness(1);
            labels.Children.Add(greenTitle);

            greenUpDown.Text = "0";
            greenUpDown.HorizontalContentAlignment = HorizontalAlignment.Right;
            greenUpDown.VerticalContentAlignment = VerticalAlignment.Center;
            greenUpDown.Margin = new Thickness(0, 0, 20, 0);
            greenUpDown.SetValue(Grid.RowProperty, 1);
            greenUpDown.SetValue(Grid.ColumnProperty, 1);
            greenUpDown.TextChanged += delegate (object source, TextChangedEventArgs e)
            {
                int floatValue;
                bool testint = Int32.TryParse(greenUpDown.Text, out floatValue);
                if (testint)
                {
                    if (floatValue < 0) { greenUpDown.Text = "0"; }
                    if (floatValue > 255) { greenUpDown.Text = "255"; }
                }
                else
                {
                    greenUpDown.Text = "0";
                }
            };
            labels.Children.Add(greenUpDown);

            Button greenUpBtn = new Button();
            greenUpBtn.Content = "\u25B2";
            greenUpBtn.HorizontalContentAlignment = HorizontalAlignment.Center;
            greenUpBtn.VerticalContentAlignment = VerticalAlignment.Center;
            greenUpBtn.Padding = new Thickness(0);
            greenUpBtn.Margin = new Thickness(30, 0, 0, 15);
            greenUpBtn.SetValue(Grid.RowProperty, 1);
            greenUpBtn.SetValue(Grid.ColumnProperty, 1);
            greenUpBtn.Click += delegate (object source, RoutedEventArgs e)
            {
                int calcul8 = Convert.ToInt32(greenUpDown.Text) + 1;
                if (calcul8 > 255) { greenUpDown.Text = "0"; }
                else { greenUpDown.Text = calcul8.ToString(); }
            };
            labels.Children.Add(greenUpBtn);

            Button greenDownBtn = new Button();
            greenDownBtn.Content = "\u25BC";
            greenDownBtn.HorizontalContentAlignment = HorizontalAlignment.Center;
            greenDownBtn.VerticalContentAlignment = VerticalAlignment.Center;
            greenDownBtn.Padding = new Thickness(0);
            greenDownBtn.Margin = new Thickness(30, 14, 0, 0);
            greenDownBtn.SetValue(Grid.RowProperty, 1);
            greenDownBtn.SetValue(Grid.ColumnProperty, 1);
            greenDownBtn.Click += delegate (object source, RoutedEventArgs e)
            {
                int calcul8 = Convert.ToInt32(greenUpDown.Text) - 1;
                if (calcul8 < 0) { greenUpDown.Text = "255"; }
                else { greenUpDown.Text = calcul8.ToString(); }
            };
            labels.Children.Add(greenDownBtn);

            Label blueTitle = new Label();
            blueTitle.Content = "Blue:";
            blueTitle.Foreground = new SolidColorBrush(Color.FromRgb(166, 166, 166));
            blueTitle.HorizontalContentAlignment = HorizontalAlignment.Right;
            blueTitle.SetValue(Grid.RowProperty, 2);
            blueTitle.SetValue(Grid.ColumnProperty, 0);
            blueTitle.BorderBrush = new SolidColorBrush(Color.FromRgb(166, 166, 166));
            blueTitle.BorderThickness = new Thickness(1);
            labels.Children.Add(blueTitle);

            blueUpDown.Text = "0";
            blueUpDown.HorizontalContentAlignment = HorizontalAlignment.Right;
            blueUpDown.VerticalContentAlignment = VerticalAlignment.Center;
            blueUpDown.Margin = new Thickness(0, 0, 20, 0);
            blueUpDown.SetValue(Grid.RowProperty, 2);
            blueUpDown.SetValue(Grid.ColumnProperty, 1);
            blueUpDown.TextChanged += delegate (object source, TextChangedEventArgs e)
            {
                int farter;
                bool testint = Int32.TryParse(blueUpDown.Text, out farter);
                if (testint)
                {
                    if (farter < 0) { blueUpDown.Text = "0"; }
                    if (farter > 255) { blueUpDown.Text = "255"; }
                }
                else
                {
                    blueUpDown.Text = "0";
                }
            };
            labels.Children.Add(blueUpDown);

            Button blueUpBtn = new Button();
            blueUpBtn.Content = "\u25B2";
            blueUpBtn.HorizontalContentAlignment = HorizontalAlignment.Center;
            blueUpBtn.VerticalContentAlignment = VerticalAlignment.Center;
            blueUpBtn.Padding = new Thickness(0);
            blueUpBtn.Margin = new Thickness(30, 0, 0, 15);
            blueUpBtn.SetValue(Grid.RowProperty, 2);
            blueUpBtn.SetValue(Grid.ColumnProperty, 1);
            blueUpBtn.Click += delegate (object source, RoutedEventArgs e)
            {
                int calcul8 = Convert.ToInt32(blueUpDown.Text) + 1;
                if (calcul8 > 255) { blueUpDown.Text = "0"; }
                else { blueUpDown.Text = calcul8.ToString(); }
            };
            labels.Children.Add(blueUpBtn);

            Button blueDownBtn = new Button();
            blueDownBtn.Content = "\u25BC";
            blueDownBtn.HorizontalContentAlignment = HorizontalAlignment.Center;
            blueDownBtn.VerticalContentAlignment = VerticalAlignment.Center;
            blueDownBtn.Padding = new Thickness(0);
            blueDownBtn.Margin = new Thickness(30, 14, 0, 0);
            blueDownBtn.SetValue(Grid.RowProperty, 2);
            blueDownBtn.SetValue(Grid.ColumnProperty, 1);
            blueDownBtn.Click += delegate (object source, RoutedEventArgs e)
            {
                int calcul8 = Convert.ToInt32(blueUpDown.Text) - 1;
                if (calcul8 < 0) { blueUpDown.Text = "255"; }
                else { blueUpDown.Text = calcul8.ToString(); }
            };
            labels.Children.Add(blueDownBtn);

            rgbFields.Children.Add(labels);

            // Column 0/3 - Swatch Image
            Image swatcher = new Image();
            swatcher.Source = new BitmapImage(new Uri("/Icons/physicswheel.png", UriKind.Relative));
            swatcher.Stretch = Stretch.Uniform;
            swatcher.Height = 100;
            swatcher.Width = 100;
            swatcher.Margin = new Thickness(5, 5, 5, 5);
            swatcher.SetValue(Grid.RowProperty, 0);
            swatcher.SetValue(Grid.ColumnProperty, 0);
            swatcher.HorizontalAlignment = HorizontalAlignment.Center;
            columns.Children.Add(swatcher);

            Ellipse bumper = new Ellipse();
            bumper.Height = 95;
            bumper.Width = 95;
            bumper.SetValue(Grid.RowProperty, 0);
            bumper.SetValue(Grid.ColumnProperty, 0);
            bumper.Stroke = new SolidColorBrush(Color.FromArgb(0, 12, 12, 255));
            bumper.StrokeThickness = 17;
            bumper.MouseEnter += delegate (object source, MouseEventArgs e)
            {
                    //Change Mouse Cursor to custom dropper.
                    Uri curDropper = new Uri("/Cursors/eyedropper.cur", UriKind.Relative);
                bumper.Cursor = new Cursor(App.GetResourceStream(curDropper).Stream);
            };
            bumper.MouseDown += delegate (object source, MouseButtonEventArgs e)
            {
                    //Get Mouse position and then grab pixel data.
                    int xPos = Convert.ToInt32(e.GetPosition(swatcher).X);
                int yPos = Convert.ToInt32(e.GetPosition(swatcher).Y);

                CroppedBitmap dropper = new CroppedBitmap(swatcher.Source as BitmapSource, new Int32Rect(xPos, yPos, 1, 1));

                byte[] pixel = new byte[4];
                dropper.CopyPixels(pixel, 4, 0);

                    //Change Swatch Preview and RGB fields.

                    redUpDown.Text = pixel[2].ToString();
                greenUpDown.Text = pixel[1].ToString();
                blueUpDown.Text = pixel[0].ToString();

            };
            columns.Children.Add(bumper);

            // Column 1/3 - Brightness Slider

            Slider brightness = new Slider();
            brightness.Maximum = 100;
            brightness.Minimum = 0;
            brightness.Orientation = Orientation.Vertical;
            brightness.Height = 100;
            brightness.Value = 100;
            brightness.SetValue(Grid.RowProperty, 0);
            brightness.SetValue(Grid.ColumnProperty, 1);
            columns.Children.Add(brightness);

            Rectangle brightLevels = new Rectangle();
            brightLevels.Height = 100;
            brightLevels.Width = 5;
            brightLevels.Stroke = new SolidColorBrush(Color.FromRgb(166, 166, 166));
            brightLevels.StrokeThickness = 1;
            brightLevels.Fill = new LinearGradientBrush(Color.FromRgb(255, 255, 255), Color.FromRgb(0, 0, 0), 90.0);
            brightLevels.SetValue(Grid.RowProperty, 0);
            brightLevels.SetValue(Grid.ColumnProperty, 1);
            columns.Children.Add(brightLevels);

            columns.Children.Add(rgbFields);
            // Column 3/3 - Swatches
            StackPanel Sampler = new StackPanel();
            Sampler.VerticalAlignment = VerticalAlignment.Center;
            Sampler.HorizontalAlignment = HorizontalAlignment.Center;
            Sampler.SetValue(Grid.RowProperty, 0);
            Sampler.SetValue(Grid.ColumnProperty, 3);

            colorBox.Width = 50;
            colorBox.Height = 50;
            colorBox.Stroke = new SolidColorBrush(Color.FromRgb(166, 166, 166));
            colorBox.StrokeThickness = 1;
            colorBox.Fill = new SolidColorBrush(Color.FromRgb(Convert.ToByte(Convert.ToInt32(redUpDown.Text)), Convert.ToByte(Convert.ToInt32(greenUpDown.Text)), Convert.ToByte(Convert.ToInt32(blueUpDown.Text))));
            Sampler.Children.Add(colorBox);

            brightness.ValueChanged += delegate (object source, RoutedPropertyChangedEventArgs<double> e)
            {
                colorBox.Fill = new SolidColorBrush(Color.FromRgb(Convert.ToByte(Convert.ToInt32(redUpDown.Text) * (brightness.Value * .01)), Convert.ToByte(Convert.ToInt32(greenUpDown.Text) * (brightness.Value * .01)), Convert.ToByte(Convert.ToInt32(blueUpDown.Text) * (brightness.Value * .01))));
            };

            redUpDown.TextChanged += delegate (object source, TextChangedEventArgs e)
            {
                SolidColorBrush currentBrush = (SolidColorBrush)colorBox.Fill;
                colorBox.Fill = new SolidColorBrush(Color.FromRgb(Convert.ToByte(Convert.ToInt32(redUpDown.Text) * (brightness.Value * .01)), Convert.ToByte(Convert.ToDouble(currentBrush.Color.G) * (brightness.Value * .01)), Convert.ToByte(Convert.ToDouble(currentBrush.Color.B) * (brightness.Value * .01))));
            };

            greenUpDown.TextChanged += delegate (object source, TextChangedEventArgs e)
            {
                SolidColorBrush currentBrush = (SolidColorBrush)colorBox.Fill;
                colorBox.Fill = new SolidColorBrush(Color.FromRgb(Convert.ToByte(Convert.ToDouble(currentBrush.Color.R) * (brightness.Value * .01)), Convert.ToByte(Convert.ToInt32(greenUpDown.Text) * (brightness.Value * .01)), Convert.ToByte(Convert.ToDouble(currentBrush.Color.B) * (brightness.Value * .01))));
            };

            blueUpDown.TextChanged += delegate (object source, TextChangedEventArgs e)
            {
                SolidColorBrush currentBrush = (SolidColorBrush)colorBox.Fill;
                colorBox.Fill = new SolidColorBrush(Color.FromRgb(Convert.ToByte(Convert.ToDouble(currentBrush.Color.R) * (brightness.Value * .01)), Convert.ToByte(Convert.ToDouble(currentBrush.Color.G) * (brightness.Value * .01)), Convert.ToByte(Convert.ToInt32(blueUpDown.Text) * (brightness.Value * .01))));
            };

            Button btn_addSwatch = new Button();
            btn_addSwatch.Content = "Add";
            btn_addSwatch.Width = 50;
            btn_addSwatch.Height = 25;
            Sampler.Children.Add(btn_addSwatch);

            btn_addSwatch.Click += delegate (object source, RoutedEventArgs e)
            {
                disableWindow();
                newColorDialog(redUpDown.Text, greenUpDown.Text, blueUpDown.Text);
            };

            columns.Children.Add(Sampler);

            // Swatch Preset Options
            Grid swatchSlider = new Grid();

            RowDefinition mainRow = new RowDefinition();
            mainRow.Height = new GridLength(40);
            swatchSlider.RowDefinitions.Add(mainRow);

            ColumnDefinition colLeftArrow = new ColumnDefinition();
            colLeftArrow.Width = new GridLength(30);
            ColumnDefinition colSwatches = new ColumnDefinition();
            ColumnDefinition colRightArrow = new ColumnDefinition();
            colRightArrow.Width = new GridLength(30);
            swatchSlider.ColumnDefinitions.Add(colLeftArrow);
            swatchSlider.ColumnDefinitions.Add(colSwatches);
            swatchSlider.ColumnDefinitions.Add(colRightArrow);

            Button swatchSlider_leftArrow = new Button();
            swatchSlider_leftArrow.Content = "\u25C4";
            swatchSlider_leftArrow.Height = 40;
            swatchSlider_leftArrow.Width = 30;
            swatchSlider_leftArrow.VerticalContentAlignment = VerticalAlignment.Center;
            swatchSlider_leftArrow.HorizontalAlignment = HorizontalAlignment.Center;
            swatchSlider_leftArrow.Padding = new Thickness(0);
            swatchSlider_leftArrow.FontSize = 25;
            swatchSlider_leftArrow.FontStretch = FontStretches.Expanded;
            swatchSlider_leftArrow.SetValue(Grid.RowProperty, 0);
            swatchSlider_leftArrow.SetValue(Grid.ColumnProperty, 0);
            swatchSlider.Children.Add(swatchSlider_leftArrow);

            Button swatchSlider_rightArrow = new Button();
            swatchSlider_rightArrow.Content = "\u25BA";
            swatchSlider_rightArrow.Height = 40;
            swatchSlider_rightArrow.Width = 30;
            swatchSlider_rightArrow.VerticalContentAlignment = VerticalAlignment.Center;
            swatchSlider_rightArrow.HorizontalAlignment = HorizontalAlignment.Center;
            swatchSlider_rightArrow.Padding = new Thickness(0);
            swatchSlider_rightArrow.FontSize = 25;
            swatchSlider_rightArrow.FontStretch = FontStretches.Expanded;
            swatchSlider_rightArrow.SetValue(Grid.RowProperty, 0);
            swatchSlider_rightArrow.SetValue(Grid.ColumnProperty, 2);
            swatchSlider.Children.Add(swatchSlider_rightArrow);

            ScrollViewer testWindow = new ScrollViewer();
            testWindow.Margin = new Thickness(0);
            testWindow.SetValue(Grid.RowProperty, 0);
            testWindow.SetValue(Grid.ColumnProperty, 1);
            testWindow.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            testWindow.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            swatchSlider.Children.Add(testWindow);

            StackPanel testTextLine = swatchSelector();
            testWindow.Content = testTextLine;

            swatchSlider_rightArrow.MouseDown += delegate (object source, MouseButtonEventArgs e)
            {
                testWindow.ScrollToHorizontalOffset(testWindow.HorizontalOffset + 10);
            };

            swatchSlider_leftArrow.PreviewMouseDown += delegate (object source, MouseButtonEventArgs e)
            {
                testWindow.ScrollToHorizontalOffset(testWindow.HorizontalOffset - 10);
            };

            Grid buttons = new Grid();
            RowDefinition oneRow = new RowDefinition();
            oneRow.Height = new GridLength(40);
            buttons.RowDefinitions.Add(oneRow);
            ColumnDefinition oneCol = new ColumnDefinition();
            ColumnDefinition twoCol = new ColumnDefinition();
            ColumnDefinition threeCol = new ColumnDefinition();
            ColumnDefinition fourCol = new ColumnDefinition();
            buttons.ColumnDefinitions.Add(oneCol);
            buttons.ColumnDefinitions.Add(twoCol);
            buttons.ColumnDefinitions.Add(threeCol);
            buttons.ColumnDefinitions.Add(fourCol);

            Button colorOKbtn = new Button();
            colorOKbtn.Content = "OK";
            colorOKbtn.Width = 80;
            colorOKbtn.Height = 20;
            colorOKbtn.VerticalAlignment = VerticalAlignment.Center;
            colorOKbtn.HorizontalAlignment = HorizontalAlignment.Center;
            colorOKbtn.Padding = new Thickness(0);
            colorOKbtn.SetValue(Grid.RowProperty, 0);
            colorOKbtn.SetValue(Grid.ColumnProperty, 1);
            buttons.Children.Add(colorOKbtn);

            Button colorCancelBtn = new Button();
            colorCancelBtn.Content = "Cancel";
            colorCancelBtn.Width = 80;
            colorCancelBtn.Height = 20;
            colorCancelBtn.VerticalAlignment = VerticalAlignment.Center;
            colorCancelBtn.HorizontalAlignment = HorizontalAlignment.Center;
            colorCancelBtn.Padding = new Thickness(0);
            colorCancelBtn.SetValue(Grid.RowProperty, 0);
            colorCancelBtn.SetValue(Grid.ColumnProperty, 2);
            buttons.Children.Add(colorCancelBtn);

            colorOKbtn.Click += delegate (object source, RoutedEventArgs e)
            {
                SolidColorBrush color = (SolidColorBrush)colorBox.Fill;
                string colours =  Convert.ToInt32(color.Color.R).ToString() + ", " + Convert.ToInt32(color.Color.G).ToString() + ", " + Convert.ToInt32(color.Color.B).ToString();
                File_Tools.writeSetting(this._key, colours);

                MainWindow mw = (MainWindow)Application.Current.MainWindow;
                Settings sw = (Settings)mw.FindName("SettingsWindow");
                sw.loadColors();

                closeColorPicker(container);
            };

            colorCancelBtn.Click += delegate (object source, RoutedEventArgs e)
            {
                closeColorPicker(container);
            };

            container.Children.Add(bounds);
            bounds.Child = colorPopup;

            colorPopup.Children.Add(heading);
            colorPopup.Children.Add(columns);
            colorPopup.Children.Add(swatchSlider);
            colorPopup.Children.Add(buttons);

            redUpDown.Text = openRed;
            greenUpDown.Text = openGreen;
            blueUpDown.Text = openBlue;

            return container;

        }

        private void closeColorPicker(Grid container)
        {
            container.Children.Clear();
            Grid parent = (Grid)container.Parent;
            parent.Children.Remove(container);
        }

        private void newColorDialog(string red, string green, string blue)
        {
            Window getColorName = new Window();
            getColorName.Height = 100;
            getColorName.Width = 300;

            StackPanel body = new StackPanel();

            TextBlock header = new TextBlock();
            header.HorizontalAlignment = HorizontalAlignment.Stretch;
            header.Text = "Enter unique color name:";

            TextBox newName = new TextBox();
            newName.HorizontalAlignment = HorizontalAlignment.Stretch;
            newName.HorizontalContentAlignment = HorizontalAlignment.Center;
            newName.VerticalContentAlignment = VerticalAlignment.Center;

            Grid buttonContainer = new Grid();
            RowDefinition oneRow = new RowDefinition();
            buttonContainer.RowDefinitions.Add(oneRow);
            ColumnDefinition oneCol = new ColumnDefinition();
            ColumnDefinition twoCol = new ColumnDefinition();
            buttonContainer.ColumnDefinitions.Add(oneCol);
            buttonContainer.ColumnDefinitions.Add(twoCol);

            Button okBtn = new Button();
            okBtn.Content = "OK";
            okBtn.SetValue(Grid.RowProperty, 0);
            okBtn.SetValue(Grid.ColumnProperty, 0);
            buttonContainer.Children.Add(okBtn);

            Button cancelBtn = new Button();
            cancelBtn.Content = "Cancel";
            cancelBtn.SetValue(Grid.RowProperty, 0);
            cancelBtn.SetValue(Grid.ColumnProperty, 1);
            buttonContainer.Children.Add(cancelBtn);

            okBtn.Click += delegate (object source, RoutedEventArgs e)
            {
                if (newName.Text.Trim().Length > 0)
                {
                    newName.Text = newName.Text.Trim().Replace(",", "");
                    if (File_Tools.colorExists(newName.Text))
                    {
                        MessageBox.Show("A color with this name already exists! Try a new name.");
                        newName.Clear();
                        newName.Focus();
                    }
                    else
                    {
                        File_Tools.writeColour(redUpDown.Text, greenUpDown.Text, blueUpDown.Text, newName.Text);
                        MessageBox.Show("Successfully saved new color!");
                        fillSwatches();
                        getColorName.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid input! Try something different.");
                    newName.Clear();
                    newName.Focus();
                }
            };

            cancelBtn.Click += delegate (object source, RoutedEventArgs e)
            {
                getColorName.Close();
            };

            getColorName.Closed += delegate (object source, EventArgs e)
            {
                enableWindow();
            };

            body.Children.Add(header);
            body.Children.Add(newName);
            body.Children.Add(buttonContainer);

            getColorName.Content = body;

            getColorName.Show();
        }

        private StackPanel swatchSelector()
        {
            lineup.HorizontalAlignment = HorizontalAlignment.Left;
            lineup.Orientation = Orientation.Horizontal;

            fillSwatches();

            return lineup;
        }

        public void fillSwatches()
        {
            lineup.Children.Clear();

            Dictionary<string, byte[]> swatchColours = File_Tools.getColours();

            foreach (KeyValuePair<string, byte[]> readColor in swatchColours)
            {
                byte[] rgbs = readColor.Value;
                SwatchBox drawColor = new SwatchBox(rgbs[0], rgbs[1], rgbs[2], readColor.Key, container);
                Rectangle swatch = drawColor.Draw();
                swatch.PreviewMouseDown += delegate (object source, MouseButtonEventArgs e)
                {
                    redUpDown.Text = drawColor.getRed().ToString();
                    greenUpDown.Text = drawColor.getGreen().ToString();
                    blueUpDown.Text = drawColor.getBlue().ToString();
                };
                swatch.Margin = new Thickness(5, 2, 5, 2);
                lineup.Children.Add(swatch);
            }
        }

        public void disableWindow()
        {
            if (isCovered == false)
            {
                Rectangle cover = new Rectangle();
                cover.Name = "deranged";
                cover.Margin = new Thickness(0);
                cover.Fill = new SolidColorBrush(Color.FromArgb(230, 0, 0, 0));
                container.RegisterName("deranged", cover);
                container.Children.Add(cover);
                isCovered = true;
            }
        }

        public void enableWindow()
        {
            if (isCovered)
            {
                Rectangle cover = (Rectangle)container.FindName("deranged");
                container.Children.Remove(cover);
                container.UnregisterName("deranged");
                isCovered = false;
            }
        }
    }

    class SwatchBox
    {
        private byte _R = 0;
        private byte _G = 0;
        private byte _B = 0;
        private string _name = "";
        private object owner = null;

        public SwatchBox(byte red, byte green, byte blue, string name, object sender)
        {
            this._R = red;
            this._G = green;
            this._B = blue;
            this._name = name;
            this.owner = sender;
        }

        public Rectangle Draw()
        {
            Rectangle dummy = new Rectangle();
            dummy.Width = 35;
            dummy.Height = 25;
            dummy.Stroke = new SolidColorBrush(Color.FromRgb(166, 166, 166));
            dummy.StrokeThickness = 1;
            dummy.Fill = new SolidColorBrush(Color.FromRgb(this._R, this._G, this._B));
            dummy.ToolTip = swatchTip(this._name);
            dummy.ContextMenu = swatchOptions();

            return dummy;
        }

        public ToolTip swatchTip(string colorInfo)
        {
            ToolTip swatchInfo = new ToolTip();
            swatchInfo.Padding = new Thickness(0);

            Grid si_container = new Grid();
            si_container.Height = 20;
            si_container.Width = 100;
            si_container.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));

            TextBlock si_ColorName = new TextBlock();
            si_ColorName.Foreground = new SolidColorBrush(Color.FromRgb(166, 166, 166));
            si_ColorName.Text = colorInfo;
            si_ColorName.TextAlignment = TextAlignment.Center;
            si_ColorName.LineHeight = 10;
            si_container.Children.Add(si_ColorName);

            swatchInfo.Content = si_container;

            return swatchInfo;
        }

        public ContextMenu swatchOptions()
        {
            ContextMenu swatchWatch = new ContextMenu();

            MenuItem Rename = new MenuItem();
            Rename.Header = "Rename";
            Rename.Click += new RoutedEventHandler(renSwatch);

            MenuItem Delete = new MenuItem();
            Delete.Header = "Delete";
            Delete.Click += new RoutedEventHandler(delSwatch);

            swatchWatch.Items.Add(Rename);
            swatchWatch.Items.Add(Delete);

            return swatchWatch;
        }

        public void delSwatch(object sender, RoutedEventArgs e)
        {
            File_Tools.removeColour(this._name);
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            Settings sw = (Settings)mw.FindName("SettingsWindow");
            Color_Picker sc = (Color_Picker)sw.FindName("colorPicker");
            sc.fillSwatches();
        }

        public void renSwatch(object sender, RoutedEventArgs e)
        {
            renameDialog getNewName = new renameDialog(this._name, this.owner);
        }

        public byte getRed()
        {
            return this._R;
        }

        public byte getGreen()
        {
            return this._G;
        }

        public byte getBlue()
        {
            return this._B;
        }
    }

    class renameDialog : Window
    {
        bool isAlive = false;
        Window rename = new Window();
        TextBox newName = new TextBox();
        public string oldName = "";

        public renameDialog(string name, object sender)
        {
            if (isAlive)
            {
                rename.Show();
            }
            else
            {
                isAlive = true;
                rename.Name = "renameWindow";
                Grid mainRoom = (Grid)sender;

                this.oldName = name;

                rename.Height = 150;
                rename.Width = 300;

                Point srcPoint = mainRoom.PointToScreen(new Point(0, 0));
                rename.Left = srcPoint.X;
                rename.Top = srcPoint.Y;

                StackPanel filler = new StackPanel();
                TextBlock header = new TextBlock();
                header.Padding = new Thickness(5);
                header.Text = "Please enter a new name for this color.";
                header.TextWrapping = TextWrapping.Wrap;

                newName.Text = this.oldName;
                newName.HorizontalAlignment = HorizontalAlignment.Stretch;
                newName.TextAlignment = TextAlignment.Center;

                Grid buttons = new Grid();
                RowDefinition oneRow = new RowDefinition();
                buttons.RowDefinitions.Add(oneRow);
                ColumnDefinition oneCol = new ColumnDefinition();
                ColumnDefinition twoCol = new ColumnDefinition();
                buttons.ColumnDefinitions.Add(oneCol);
                buttons.ColumnDefinitions.Add(twoCol);

                Button okBtn = new Button();
                okBtn.Content = "OK";
                okBtn.Click += new RoutedEventHandler(renameColor);
                okBtn.SetValue(Grid.RowProperty, 0);
                okBtn.SetValue(Grid.ColumnProperty, 0);
                buttons.Children.Add(okBtn);

                Button cancelBtn = new Button();
                cancelBtn.Content = "Cancel";
                cancelBtn.Click += new RoutedEventHandler(cancelRename);
                cancelBtn.SetValue(Grid.RowProperty, 0);
                cancelBtn.SetValue(Grid.ColumnProperty, 1);
                buttons.Children.Add(cancelBtn);

                filler.Children.Add(header);
                filler.Children.Add(newName);
                filler.Children.Add(buttons);

                rename.Content = filler;

                rename.Closed += delegate (object source, EventArgs e)
                {
                    closeRename(sender);
                };

                MainWindow mw = (MainWindow)Application.Current.MainWindow;
                Settings sw = (Settings)mw.FindName("SettingsWindow");
                Color_Picker sc = (Color_Picker)sw.FindName("colorPicker");
                sc.disableWindow();
                
                rename.Show();
            }
        }

        public void renameColor(object sender, RoutedEventArgs e)
        {
            File_Tools.renameColour(this.oldName, this.newName.Text);
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            Settings sw = (Settings)mw.FindName("SettingsWindow");
            Color_Picker sc = (Color_Picker)sw.FindName("colorPicker");
            sc.fillSwatches();
            rename.Close();
        }

        public void cancelRename(object sender, RoutedEventArgs e)
        {
            rename.Close();
        }

        public void closeRename(object sender)
        {
            isAlive = false;
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            Settings sw = (Settings)mw.FindName("SettingsWindow");
            Color_Picker sc = (Color_Picker)sw.FindName("colorPicker");
            sc.enableWindow();
        }

    }

}
