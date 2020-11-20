using CSharpRenderer.Core;
using Sr3D.Core;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Sr3D.Utils
{
    /// <summary>
    /// The <see href="https://de.wikipedia.org/wiki/Bresenham-Algorithmus">Bresenham line-drawing algorithm</see> is a fast
    /// algorithm for drawing a line between two points. The algorithm can be implemented using only integer arithmetic which
    /// makes it very fast. The BresenhamIterator class allows to enumerate over each point on a line.
    /// </summary>
    /// <remarks>Probably slow as f***.</remarks>
    public sealed class BresenhamEnumerator : IEnumerator<Int32Point>, IEnumerable<Int32Point>
    {
        private Int32Point current;
        private int dx, dy, xinc, yinc, error;
        private Int32Point start, end;
        private bool reset;

        public BresenhamEnumerator(Int32Point start, Int32Point end)
        {
            this.start = start;
            this.end = end;

            dx = this.end.X - this.start.X;
            dy = this.end.Y - this.start.Y;
            xinc = Math.Sign(dx);
            yinc = Math.Sign(dy);
            dx = Math.Abs(dx);
            dy = Math.Abs(dy);

            Reset();
        }

        public Int32Point Current => current;

        object IEnumerator.Current => current;

        public bool MoveNext()
        {
            // Some nasty hack to include start point
            if (reset)
            {
                reset = false;
                return true;
            }

            bool hasNext = false;

            // horizontal line (slope is 0)
            if (dy == 0 && current.X != end.X)
            {
                hasNext = current.X != end.X;
                current.X += xinc;
            }
            // vertical line
            else if (dx == 0 && current.Y != end.Y)
            {
                hasNext = current.Y != end.Y;
                current.Y += yinc;
            }
            // slope is between -1 and 1 except 0
            else if (dy <= dx)
            {
                if (current.X != end.X)
                {
                    error += dy;
                    if (error >= 0)
                    {
                        current.Y += yinc;
                        error -= dx;
                    }
                    hasNext = current.X != end.X;
                    current.X += xinc;
                }
            }
            // slope is greator than 1 or smaller than -1
            else
            {
                if (current.Y != end.Y)
                {
                    error += dx;
                    if (error >= 0)
                    {
                        current.X += xinc;
                        error -= dy;
                    }
                    hasNext = current.Y != end.Y;
                    current.Y += yinc;
                }
            }

            return hasNext;
        }

        public void Reset()
        {
            current.X = start.X;
            current.Y = start.Y;
            error = -(dy <= dx ? dx : dy) / 2;
            reset = true;
        }

        public void Dispose()
        {
            // nothing to dispose
        }

        public bool MoveNextY()
        {
            int y = current.Y;
            while(MoveNext())
            {
                if(y != current.Y)
                {
                    return current.Y != end.Y;
                }
            }

            return false;
        }

        #region IEnumerable<Int32Point> interface
        public IEnumerator<Int32Point> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }
        #endregion IEnumerable<Int32Point> interface
    }
}
