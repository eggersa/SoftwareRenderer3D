using SoftwareRenderer3D.Data;
using SoftwareRenderer3D.SrMath;
using Sr3D.Graphics;
using Sr3D.SrMath;
using System;
using System.Windows;
using System.Windows.Input;
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
            scene.Model = ObjLoader.Load("cube.obj");
            animationTimer.Start();
        }

        private void Display_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            //Matrix m = display.RenderTransform.Value;

            //if (e.Delta > 0)
            //{
            //    m.ScaleAt(1.5, 1.5, e.GetPosition(display).X, e.GetPosition(display).Y);
            //}
            //else
            //{
            //    m.ScaleAt(1.0 / 1.5, 1.0 / 1.5, e.GetPosition(display).X, e.GetPosition(display).Y);
            //}

            //display.RenderTransform = new MatrixTransform(m);
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.M)
            {
                renderer.Wireframe = !renderer.Wireframe;
            }
        }

        private float rotX = 0.0f;
        private float rotY = 0.0f;
        private float rotInc = 0.1047f;

        private void UpdateState()
        {
            if (Keyboard.IsKeyDown(Key.Up))
            {
                rotX = SrMathUtils.SanitizeAngle(rotX - rotInc);
            }
            else if (Keyboard.IsKeyDown(Key.Down))
            {
                rotX = SrMathUtils.SanitizeAngle(rotX + rotInc);
            }

            if (Keyboard.IsKeyDown(Key.Right))
            {
                rotY = SrMathUtils.SanitizeAngle(rotY - rotInc);
            }
            else if (Keyboard.IsKeyDown(Key.Left))
            {
                rotY = SrMathUtils.SanitizeAngle(rotY + rotInc);
            }
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            UpdateState();

            scene.Transform = Matrix4x4.CreateTranslation(0, 0, 2) *
                   Matrix4x4.CreateRotationX(rotX) *
                   Matrix4x4.CreateRotationY(rotY);

            renderer.Render();
        }
    }
}
