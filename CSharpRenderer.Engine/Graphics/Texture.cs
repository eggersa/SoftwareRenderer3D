using System;
using System.Drawing;

namespace CSharpRenderer.Graphics
{
    public class Texture : IDisposable
    {
        private readonly Image image;
        private bool isDisposed;

        public Texture(Image image)
        {
            this.image = image;
        }

        public static Texture FromFile(string fileName)
        {
            var image = Image.FromFile(fileName);
            return new Texture(image);
        }

        #region IDisposable
        public void Dispose()
        {
            // https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1063?view=vs-2019
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }

            if (disposing)
            {
                image.Dispose();
            }

            isDisposed = true;
        }
        #endregion IDisposable
    }
}
