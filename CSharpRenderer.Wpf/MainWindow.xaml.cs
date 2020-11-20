using CSharpRenderer.Data;
using CSharpRenderer.Graphics;
using CSharpRenderer.SrMath;
using Sr3D.SrMath;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SoftwareRenderer
{
    public partial class MainWindow : Window
    {
        private readonly Scene scene;
        private readonly Stopwatch stopWatch = new Stopwatch();
        
        private WriteableBitmap bitmap;

        private int fpsCounter;

        public MainWindow()
        {
            InitializeComponent();

            scene = new Scene();

            Loaded += MainWindow_Loaded;
            display.MouseWheel += Display_MouseWheel;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadModel(@"Models\cube.obj");

            CreateBitmap();

            // https://docs.microsoft.com/en-us/dotnet/desktop/wpf/graphics-multimedia/how-to-render-on-a-per-frame-interval-using-compositiontarget?view=netframeworkdesktop-4.8
            CompositionTarget.Rendering += CompositionTarget_Rendering;
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

        private void MenuItemCube_Click(object sender, RoutedEventArgs e)
        {
            LoadModel(@"Models\cube.obj");
        }

        private void MenuItemGazebo_Click(object sender, RoutedEventArgs e)
        {
            LoadModel(@"Models\gazebo.obj");
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.M)
            {
                //renderer.Mode = renderer.Mode == RenderMode.Solid ? RenderMode.Wireframe : RenderMode.Solid;
            }

            if (e.Key == Key.I)
            {
                info.Visibility = info.IsVisible ? Visibility.Hidden : Visibility.Visible;
            }
        }

        private void UpdateState(float dt)
        {
            float rotX = scene.RotX;
            float rotY = scene.RotY;

            if (Keyboard.IsKeyDown(Key.Up))
            {
                scene.RotX = SrMathUtils.SanitizeAngle(scene.RotX - Scene.RotInc);
            }
            else if (Keyboard.IsKeyDown(Key.Down))
            {
                scene.RotX = SrMathUtils.SanitizeAngle(scene.RotX + Scene.RotInc);
            }

            if (Keyboard.IsKeyDown(Key.Right))
            {
                scene.RotY = SrMathUtils.SanitizeAngle(scene.RotY - Scene.RotInc);
            }
            else if (Keyboard.IsKeyDown(Key.Left))
            {
                scene.RotY = SrMathUtils.SanitizeAngle(scene.RotY + Scene.RotInc);
            }

            scene.Transform = Matrix4x4.CreateTranslation(0, 0, 1.5f) *
                              Matrix4x4.CreateRotationX(scene.RotX) *
                              Matrix4x4.CreateRotationY(scene.RotY);
        }

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            UpdateState(0);

            if (bitmap == null)
            {
                return;
            }

            if (fpsCounter++ == 0)
            {
                stopWatch.Restart();
            }

            try
            {
                bitmap.Lock();

                var surface = new Surface(bitmap.BackBuffer, bitmap.BackBufferStride, 4,
                    bitmap.PixelWidth, bitmap.PixelHeight);

                var renderer = new Renderer(surface);
                renderer.Render(scene);

                bitmap.AddDirtyRect(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));
            }
            finally
            {
                bitmap.Unlock();
            }

            if (stopWatch.ElapsedMilliseconds > 999)
            {
                info.Text = $"{fpsCounter} fps";
                fpsCounter = 0;
            }
        }

        private void LoadModel(string model)
        {
            scene.RotX = 0;
            scene.RotY = 0;

            scene.Model = ObjLoader.Load(model);
        }

        private void CreateBitmap()
        {
            bitmap = new WriteableBitmap((int)canvas.ActualWidth, (int)canvas.ActualHeight, 96, 96, PixelFormats.Bgr32, null);
            display.Source = bitmap;
        }

        private void Display_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CreateBitmap();
        }
    }
}
