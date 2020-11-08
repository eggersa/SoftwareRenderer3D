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
            scene.Cube = new Cube(0.5f);

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

        float radians = 0.0f;
        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            radians = (radians + 0.16047f) % (2 * (float)Math.PI);
            scene.Transform = Matrix4x4.CreateTranslation(0, 0, 2) * Matrix4x4.CreateRotationX(radians);

            renderer.Render();
        }
    }
}
