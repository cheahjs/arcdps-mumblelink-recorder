using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Hardstuck.GuildWars2.MumbleLink;

namespace MumblelinkReplay.Record.Extension
{
    internal static class LinkedMemSerialization
    {
        public static byte[] GetBytes(this LinkedMem mem)
        {
            var buf = new byte[Marshal.SizeOf<LinkedMem>()];
            var ptr = IntPtr.Zero;
            try
            {
                ptr = Marshal.AllocHGlobal(buf.Length);
                Marshal.StructureToPtr(mem, ptr, true);
                Marshal.Copy(ptr, buf, 0, buf.Length);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }

            return buf;
        }
    }
}
