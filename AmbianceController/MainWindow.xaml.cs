using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace AmbianceController
{
    public partial class MainWindow : Window
    {
        // DWM API Constants
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int value, int attrSize);

        public MainWindow()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += Window_MouseLeftButtonDown;

            // Apply the gloss once the window handle is created
            this.Loaded += (s, e) => EnableAcrylic();
        }

        private void EnableAcrylic()
        {
            var hwnd = new WindowInteropHelper(this).Handle;

            // 1. Apply the Acrylic Backdrop
            int glassValue = 3;
            DwmSetWindowAttribute(hwnd, 38, ref glassValue, sizeof(int));

            // 2. NEW: Tell Windows to round the actual Window corners to match your UI
            // Value 2 = Forced Rounded Corners
            int cornerPreference = 2;
            DwmSetWindowAttribute(hwnd, 33, ref cornerPreference, sizeof(int));

            // 3. Force Dark Mode
            int darkMode = 1;
            DwmSetWindowAttribute(hwnd, 20, ref darkMode, sizeof(int));
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e) => this.Close();
        private void MinimizeButton_Click(object sender, RoutedEventArgs e) => this.WindowState = WindowState.Minimized;
    }
}