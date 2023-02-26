using Hardstuck.GuildWars2.MumbleLink;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MumblelinkReplay.Record.Extension;

namespace MumblelinkReplay.Record
{
    internal class SharedMemory
    {
        private MemoryMappedFile _file;
        public SharedMemory()
        {
            _file = MemoryMappedFile.CreateOrOpen("MumbleLink", (long)Marshal.SizeOf<LinkedMem>());
        }

        public void Update(LinkedMem mem)
        {
            using (var ms = _file.CreateViewStream())
            using (var stream = new BinaryWriter(ms, Encoding.Unicode))
            {
                stream.Write(mem.GetBytes());
                // stream.Write(mem.UIVersion);
                // stream.Write(mem.UITick);
                // stream.Write(mem.AvatarPosition);
                // stream.Write(mem.AvatarFront);
                // stream.Write(mem.AvatarTop);
                // stream.Write(Encoding.Unicode.GetBytes(mem.Name.PadRight(256, '\0')));
                // stream.Write(mem.CameraPosition);
                // stream.Write(mem.CameraFront);
                // stream.Write(mem.CameraTop);
                // stream.Write(Encoding.Unicode.GetBytes(mem.IdentityString.PadRight(256, '\0')));
                // stream.Write(mem.ContextLength);
                // stream.Write(mem.Context.ServerAddress);
                // stream.Write(mem.Context.MapID);
                // stream.Write((uint)mem.Context.MapType);
                // stream.Write(mem.Context.ShardID);
                // stream.Write(mem.Context.Instance);
                // stream.Write(mem.Context.BuildID);
                // stream.Write((uint)mem.Context.UIState);
                // stream.Write(mem.Context.CompassWidth);
                // stream.Write(mem.Context.CompassHeight);
                // stream.Write(mem.Context.CompassRotation);
                // stream.Write(mem.Context.PlayerCoordinates);
                // stream.Write(mem.Context.MapCenterCoordinates);
                // stream.Write(mem.Context.MapScale);
                // stream.Write(mem.Context.ProcessID);
                // stream.Write((byte)mem.Context.MountIndex);
                // stream.Write(new byte[171]);
                // stream.Write(Encoding.Unicode.GetBytes(mem.Description.PadRight(2048, '\0')));
            }
        }
    }
}
