using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace AmbianceController
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Software rendering can stabilize transparency on certain GPUs
            RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;

            this.Loaded += (s, e) => EnableBlur();
            this.MouseLeftButtonDown += Window_MouseLeftButtonDown;
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                this.DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int value, int attrSize);

        private void EnableBlur()
        {
            var hwnd = new WindowInteropHelper(this).Handle;

            // Attribute 38: Sets the Backdrop type
            // Value 3: Acrylic (The blur from your video)
            int glassValue = 3;
            DwmSetWindowAttribute(hwnd, 38, ref glassValue, sizeof(int));

            // Attribute 20: Forces Dark Mode on the window elements
            // Value 1: True
            int darkMode = 1;
            DwmSetWindowAttribute(hwnd, 20, ref darkMode, sizeof(int));
        }
    }
}