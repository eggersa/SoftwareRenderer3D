using BenchmarkDotNet.Attributes;
using CSharpRenderer.Core;
using CSharpRenderer.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace CSharpRenderer.Benchmark
{
    public class RasterizerBenchmark : IDisposable
    {
        private bool isDisposed;
        private const int Height = 768;
        private const int Width = 1024;
        
        private readonly Surface surface;
        private readonly EdgeRasterizer edgeRasterizer;
        private IntPtr buffer;
        private readonly Random rnd = new Random();

        public RasterizerBenchmark()
        {
            buffer = Marshal.AllocHGlobal(sizeof(int));
            surface = new Surface(buffer, 0, 0, Width, Height);
            edgeRasterizer = new EdgeRasterizer(surface);
        }

        public IEnumerable<object> Triangle()
        {
            yield return new Int32Point[] { new Int32Point(100, 70), new Int32Point(50, 50), new Int32Point(60, 100) };
        }

        [Benchmark]
        [ArgumentsSource(nameof(Triangle))]
        public void EdgeRasterizer(Int32Point[] triangle)
        {
            edgeRasterizer.FillTriangle(Color.White, triangle, triangle /* not used anyway */);
        }

        public void Dispose()
        {
            DisposeInternal();
            GC.SuppressFinalize(this);
        }

        protected virtual void DisposeInternal()
        {
            if (isDisposed)
            {
                return;
            }

            if (buffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(buffer);
                buffer = IntPtr.Zero;
            }

            isDisposed = true;
        }

        ~RasterizerBenchmark()
        {
            DisposeInternal();
        }
    }
}
