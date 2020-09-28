using System.Windows;
using System.Windows.Media;

namespace SoftwareRenderer3D.Graphics
{
    /// <summary>
    /// The drawing context exposes methods and properties to draw pixels on a screen.
    /// </summary>
    public interface IDrawingContext
    {
        void DrawPixel(Color color, int x, int y);

        void DrawLine(Color color, Point start, Point end);

        void FillRect(Color color, Int32Rect rect);

        void ClearScreen();
    }
}
