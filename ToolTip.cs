using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;


namespace Pixel_IRC
{
    class ToolTips
    {
        public static ToolTip createTT(string title, string message, string example = "")
        {
            ToolTip help_panel = new System.Windows.Controls.ToolTip();
            help_panel.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
            help_panel.Padding = new Thickness(0, 0, 0, 0);
            help_panel.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));

            StackPanel ttPanel = new StackPanel();
            ttPanel.Width = 250;

            TextBlock ttHeadText = new TextBlock();
            ttHeadText.Height = 20;
            ttHeadText.Width = ttPanel.Width;
            ttHeadText.TextAlignment = TextAlignment.Center;
            ttHeadText.Background = new SolidColorBrush(Color.FromArgb(230, 0, 0, 0));
            ttHeadText.Foreground = new SolidColorBrush(Color.FromArgb(255, 220, 220, 220));
            ttHeadText.HorizontalAlignment = HorizontalAlignment.Center;
            ttHeadText.VerticalAlignment = VerticalAlignment.Center;
            ttHeadText.FontWeight = FontWeights.Bold;
            ttHeadText.Text = title;

            TextBlock ttContentText = new TextBlock();
            ttContentText.Width = ttPanel.Width;
            ttContentText.MaxWidth = ttPanel.Width;
            ttContentText.TextWrapping = TextWrapping.Wrap;
            ttContentText.Height = Double.NaN;
            ttContentText.TextAlignment = TextAlignment.Justify;
            ttContentText.Background = new SolidColorBrush(Color.FromArgb(220, 30, 30, 30));
            ttContentText.Foreground = new SolidColorBrush(Color.FromArgb(255, 220, 220, 220));
            ttContentText.Padding = new Thickness(15, 15, 15, 15);
            ttContentText.HorizontalAlignment = HorizontalAlignment.Center;
            ttContentText.VerticalAlignment = VerticalAlignment.Center;
            ttContentText.Text = message;

            ttPanel.Children.Add(ttHeadText);
            ttPanel.Children.Add(ttContentText);

            if (example != "")
            {
                TextBlock ttExampleText = new TextBlock();
                ttExampleText.Width = ttPanel.Width;
                ttExampleText.Height = Double.NaN;
                ttExampleText.Padding = new Thickness(5, 0, 5, 0);
                ttExampleText.FontStyle = FontStyles.Italic;
                ttExampleText.Background = new SolidColorBrush(Color.FromArgb(230, 0, 0, 0));
                ttExampleText.Foreground = new SolidColorBrush(Color.FromArgb(255, 220, 220, 220));
                ttExampleText.Text = example;

                ttPanel.Children.Add(ttExampleText);
            }

            help_panel.Content = ttPanel;

            return help_panel;
        }
    }
}
