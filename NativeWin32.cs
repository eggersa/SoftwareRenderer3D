using System;

namespace SoftwareRenderer3D
{
    public static class NativeWin32
    {
        /// <summary>
        /// The RtlZeroMemory routine fills a block of memory with zeros, given a pointer to the block and the length, in bytes, to be filled.
        /// </summary>
        /// <param name="dst">Datatype: void*. A pointer to the memory block to be filled with zeros.</param>
        /// <param name="length">Datatype: size_t. The number of bytes to fill with zeros.</param>
        /// <remarks>https://docs.microsoft.com/en-us/windows-hardware/drivers/ddi/wdm/nf-wdm-rtlzeromemory</remarks>
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern void RtlZeroMemory(IntPtr dst, int length);
    }
}
