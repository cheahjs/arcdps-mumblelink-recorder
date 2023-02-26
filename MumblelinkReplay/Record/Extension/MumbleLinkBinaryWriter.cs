using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardstuck.GuildWars2.MumbleLink;

namespace MumblelinkReplay.Record.Extension
{
    static class MumbleLinkBinaryWriter
    {
        public static void Write(this BinaryWriter writer, Vector3D vec)
        {
            writer.Write(vec.X);
            writer.Write(vec.Y);
            writer.Write(vec.Z);
        }

        public static void Write(this BinaryWriter writer, Vector2D vec)
        {
            writer.Write(vec.X);
            writer.Write(vec.Y);
        }
    }
}
