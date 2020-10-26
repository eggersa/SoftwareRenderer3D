using Sr3D.Graphics;
using Sr3D.Models;
using Sr3D.SrMath;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace SoftwareRenderer
{
    public partial class MainWindow : Window
    {
        private readonly Renderer3D renderer;
        private readonly DispatcherTimer animationTimer;
        private readonly Scene scene;

        public MainWindow()
        {
            InitializeComponent();

            scene = new Scene();
            scene.Cube = new Cube(0.6f);

            renderer = new Renderer3D(display, scene);
            renderer.Resize((int)Width, (int)Height);
            Loaded += MainWindow_Loaded;
            display.MouseWheel += Display_MouseWheel;

            animationTimer = new DispatcherTimer();
            animationTimer.Interval = TimeSpan.FromMilliseconds(33);
            animationTimer.Tick += AnimationTimer_Tick;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            animationTimer.Start();
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

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            float angle = 0.06047f;
            float s = (float)Math.Sin(angle);
            float c = (float)Math.Cos(angle);

            Matrix4x4 rot = new Matrix4x4();
            rot.M11 = c;
            rot.M12 = -s;
            rot.M21 = s;
            rot.M22 = c;
            rot.M33 = 1;
            rot.M44 = 1;

            scene.Transform = rot * scene.Transform;

            renderer.Render();
        }
    }
}
