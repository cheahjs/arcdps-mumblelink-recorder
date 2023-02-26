using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardstuck.GuildWars2.MumbleLink;

namespace MumblelinkReplay.Record.Extension
{
    static class MumbleLinkBinaryReader
    {
        public static Vector3D ReadVector3D(this BinaryReader reader)
        {
            return new Vector3D { X = reader.ReadSingle(), Y = reader.ReadSingle(), Z = reader.ReadSingle() };
        }

        public static Vector2D ReadVector2D(this BinaryReader reader)
        {
            return new Vector2D { X = reader.ReadSingle(), Y = reader.ReadSingle() };
        }

        public static MumbleUpdate ReadMumbleUpdateHeader(this BinaryReader reader)
        {
            return new MumbleUpdate
            {
                TimestampMicro = reader.ReadUInt64(),
                UpdateFieldMask = (UpdateFieldMask)reader.ReadUInt32(),
            };
        }
    }
}
