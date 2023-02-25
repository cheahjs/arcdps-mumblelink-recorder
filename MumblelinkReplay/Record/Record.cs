using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Hardstuck.GuildWars2.MumbleLink;
using MumblelinkReplay.Record.Extension;

namespace MumblelinkReplay.Record
{
    internal class Record
    {
        private BinaryReader reader;
        private ulong logStartUnixTimeMicros;
        private ulong logStartMonotonicMicros;
        private LinkedMem lastLinkedMem;

        public Record(string path)
        {
            var file = File.OpenRead(path);
            reader = new BinaryReader(file, Encoding.Unicode);

            var magicBytes = reader.ReadBytes(4);
            if (magicBytes[0] != 'M' || magicBytes[1] != 'L' || magicBytes[2] != 'R' || magicBytes[3] != 'C')
            {
                throw new ArgumentException($"{path} does not contain a valid header");
            }

            logStartUnixTimeMicros = reader.ReadUInt64();
            logStartMonotonicMicros = reader.ReadUInt64();
            lastLinkedMem = new LinkedMem
            {
                UIVersion = reader.ReadUInt32(),
                UITick = reader.ReadUInt32(),
                AvatarPosition = reader.ReadVector3D(),
                AvatarFront =
                    reader.ReadVector3D(),
                AvatarTop = reader.ReadVector3D(),
                Name = new string(reader.ReadChars(256)),
                CameraPosition = reader.ReadVector3D(),
                CameraFront =
                    reader.ReadVector3D(),
                CameraTop = reader.ReadVector3D(),
                IdentityString = new string(reader.ReadChars(256)),
                ContextLength = reader.ReadUInt32(),
                Context = new Context
                {
                    ServerAddress = reader.ReadBytes(28),
                    MapID = reader.ReadUInt32(),
                    MapType = (MapType)reader.ReadUInt32(),
                    ShardID = reader.ReadUInt32(),
                    Instance = reader.ReadUInt32(),
                    BuildID = reader.ReadUInt32(),
                    UIState = (UIState)reader.ReadUInt32(),
                    CompassWidth = reader.ReadUInt16(),
                    CompassHeight = reader.ReadUInt16(),
                    CompassRotation = reader.ReadSingle(),
                    PlayerCoordinates = reader.ReadVector2D(),
                    MapCenterCoordinates = reader.ReadVector2D(),
                    MapScale = reader.ReadSingle(),
                    ProcessID = reader.ReadUInt32(),
                    MountIndex = (Mounts)reader.ReadByte(),
                },
                Description = new string(reader.ReadChars(2048)),
            };
        }

        public LinkedMem? NextUpdate()
        {
            if (reader.BaseStream.Position == reader.BaseStream.Length)
                return null;
            var uHead = reader.ReadMumbleUpdateHeader();
            var newMem = lastLinkedMem;
            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.UiTick))
            {
                newMem.UITick = reader.ReadUInt32();
            }
            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.AvatarPosition))
            {
                newMem.AvatarPosition = reader.ReadVector3D();
            }
            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.AvatarFront))
            {
                newMem.AvatarFront = reader.ReadVector3D();
            }
            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.AvatarTop))
            {
                newMem.AvatarTop = reader.ReadVector3D();
            }
            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.Name))
            {
                newMem.Name = new string(reader.ReadChars(256));
            }
            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.CameraPosition))
            {
                newMem.CameraPosition = reader.ReadVector3D();
            }
            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.CameraFront))
            {
                newMem.CameraFront = reader.ReadVector3D();
            }
            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.CameraTop))
            {
                newMem.CameraTop = reader.ReadVector3D();
            }
            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.Identity))
            {
                newMem.IdentityString = new string(reader.ReadChars(256));
            }
            var newContext = newMem.Context;
            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.ServerAddress))
            {
                newContext.ServerAddress = reader.ReadBytes(28);
            }
            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.MapId))
            {
                newContext.MapID = reader.ReadUInt32();
            }
            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.MapType))
            {
                newContext.MapType = (MapType)reader.ReadUInt32();
            }
            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.ShardId))
            {
                newContext.ShardID = reader.ReadUInt32();
            }
            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.Instance))
            {
                newContext.Instance = reader.ReadUInt32();
            }
            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.UiState))
            {
                newContext.UIState = (UIState)reader.ReadUInt32();
            }
            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.CompassWidth))
            {
                newContext.CompassWidth = reader.ReadUInt16();
            }
            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.CompassHeight))
            {
                newContext.CompassHeight = reader.ReadUInt16();
            }
            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.CompassRotation))
            {
                newContext.CompassRotation = reader.ReadSingle();
            }

            var newPlayerCoords = newContext.PlayerCoordinates;
            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.PlayerX))
            {
                newPlayerCoords.X = reader.ReadSingle();
            }
            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.PlayerY))
            {
                newPlayerCoords.Y = reader.ReadSingle();
            }
            newContext.PlayerCoordinates = newPlayerCoords;

            var newMapCenter = newContext.MapCenterCoordinates;
            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.MapCenterX))
            {
                newMapCenter.X = reader.ReadSingle();
            }
            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.MapCenterY))
            {
                newMapCenter.Y = reader.ReadSingle();
            }
            newContext.MapCenterCoordinates = newMapCenter;

            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.MapScale))
            {
                newContext.MapScale = reader.ReadSingle();
            }
            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.MountIndex))
            {
                newContext.MountIndex = (Mounts)reader.ReadByte();
            }

            newMem.Context = newContext;
            return null;
        }
    }
}