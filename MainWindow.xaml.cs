using SoftwareRenderer3D;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SoftwareRenderer
{
    public partial class MainWindow : Window
    {
        private readonly Renderer3D renderer;

        public MainWindow()
        {
            InitializeComponent();
            
            renderer = new Renderer3D(display);
            renderer.Resize((int)Width, (int)Height);
            Loaded += MainWindow_Loaded;
            display.MouseWheel += Display_MouseWheel;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            renderer.Render();
        }

        private void Display_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Matrix m = display.RenderTransform.Value;

            if (e.Delta > 0)
            {
                m.ScaleAt(1.5, 1.5, e.GetPosition(display).X, e.GetPosition(display).Y);
            }
            else
            {
                m.ScaleAt(1.0 / 1.5, 1.0 / 1.5, e.GetPosition(display).X, e.GetPosition(display).Y);
            }

            display.RenderTransform = new MatrixTransform(m);
        }
    }
}
