﻿using CSharpRenderer.Core;
using Sr3D.Core;
using System.Drawing;

namespace CSharpRenderer.Graphics
{
    /// <summary>
    /// The drawing context exposes methods and properties to draw pixels on a screen.
    /// </summary>
    public interface IDrawingContext
    {
        void DrawPixel(Color color, int x, int y);

        void DrawPixel(int color, int x, int y);

        void DrawLine(Color color, Int32Point start, Int32Point end);

        void FillRect(Color color, Int32Rect rect);

        void ClearScreen();
    }
}
