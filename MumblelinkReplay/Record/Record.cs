using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Hardstuck.GuildWars2.MumbleLink;
using MumblelinkReplay.Record.Extension;
using Context = Hardstuck.GuildWars2.MumbleLink.Context;

namespace MumblelinkReplay.Record
{
    internal class Record
    {
        private BinaryReader reader;
        public ulong LogStartUnixTimeMicros;
        public ulong LogStartMonotonicMicros;
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

            LogStartUnixTimeMicros = reader.ReadUInt64();
            LogStartMonotonicMicros = reader.ReadUInt64();
            lastLinkedMem = new LinkedMem
            {
                UIVersion = reader.ReadUInt32(),
                UITick = reader.ReadUInt32(),
                AvatarPosition = reader.ReadVector3D(),
                AvatarFront = reader.ReadVector3D(),
                AvatarTop = reader.ReadVector3D(),
                Name = new string(reader.ReadChars(256)),
                CameraPosition = reader.ReadVector3D(),
                CameraFront = reader.ReadVector3D(),
                CameraTop = reader.ReadVector3D(),
                IdentityString = new string(reader.ReadChars(256)).Split('\0')[0],
                ContextLength = reader.ReadUInt32(),
            };
            var contextReader = new BinaryReader(new MemoryStream(reader.ReadBytes(256)), Encoding.Unicode);
            var context = new Context
            {
                ServerAddress = contextReader.ReadBytes(28),
                MapID = contextReader.ReadUInt32(),
                MapType = (MapType)contextReader.ReadUInt32(),
                ShardID = contextReader.ReadUInt32(),
                Instance = contextReader.ReadUInt32(),
                BuildID = contextReader.ReadUInt32(),
                UIState = (UIState)contextReader.ReadUInt32(),
                CompassWidth = contextReader.ReadUInt16(),
                CompassHeight = contextReader.ReadUInt16(),
                CompassRotation = contextReader.ReadSingle(),
                PlayerCoordinates = contextReader.ReadVector2D(),
                MapCenterCoordinates = contextReader.ReadVector2D(),
                MapScale = contextReader.ReadSingle(),
                ProcessID = contextReader.ReadUInt32(),
                MountIndex = (Mounts)contextReader.ReadByte(),
            };
            context.ProcessID = (uint)Process.GetCurrentProcess().Id;
            lastLinkedMem.Context = context;
            lastLinkedMem.Description = new string(reader.ReadChars(2048));
            Debug.WriteLine($"Position after initial read: {reader.BaseStream.Position}");
        }

        public LinkedMem GetPrevLinkedMem()
        {
            return lastLinkedMem;
        }

        public struct Update
        {
            public ulong TimestampMicro;
            public ulong TimeSinceStartMicro;
            public LinkedMem LinkedMem;
        }

        public Update? NextUpdate()
        {
            if (reader.BaseStream.Position == reader.BaseStream.Length)
                return null;
            var uHead = reader.ReadMumbleUpdateHeader();
            Debug.WriteLine(
                $"Reading update at time {uHead.TimestampMicro} ({uHead.TimestampMicro - LogStartMonotonicMicros}us since start) with {uHead.UpdateFieldMask} updates");
            var newMem = lastLinkedMem;
            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.UiTick))
            {
                newMem.UITick = reader.ReadUInt32();
                Debug.WriteLine($"- Read UITick: {newMem.UITick}");
            }

            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.AvatarPosition))
            {
                newMem.AvatarPosition = reader.ReadVector3D();
                Debug.WriteLine($"- Read AvatarPosition: {newMem.AvatarPosition}");
            }

            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.AvatarFront))
            {
                newMem.AvatarFront = reader.ReadVector3D();
                Debug.WriteLine($"- Read AvatarFront: {newMem.AvatarFront}");
            }

            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.AvatarTop))
            {
                newMem.AvatarTop = reader.ReadVector3D();
                Debug.WriteLine($"- Read AvatarTop: {newMem.AvatarTop}");
            }

            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.Name))
            {
                newMem.Name = new string(reader.ReadChars(256));
                Debug.WriteLine($"- Read Name: {newMem.Name}");
            }

            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.CameraPosition))
            {
                newMem.CameraPosition = reader.ReadVector3D();
                Debug.WriteLine($"- Read CameraPosition: {newMem.CameraPosition}");
            }

            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.CameraFront))
            {
                newMem.CameraFront = reader.ReadVector3D();
                Debug.WriteLine($"- Read CameraFront: {newMem.CameraFront}");
            }

            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.CameraTop))
            {
                newMem.CameraTop = reader.ReadVector3D();
                Debug.WriteLine($"- Read CameraTop: {newMem.CameraTop}");
            }

            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.Identity))
            {
                newMem.IdentityString = new string(reader.ReadChars(256)).Split('\0')[0];
                Debug.WriteLine($"- Read Identity: {newMem.IdentityString}");
            }

            var newContext = newMem.Context;
            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.ServerAddress))
            {
                newContext.ServerAddress = reader.ReadBytes(28);
                Debug.WriteLine($"- Read ServerAddress: {newContext.ServerAddress}");
            }

            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.MapId))
            {
                newContext.MapID = reader.ReadUInt32();
                Debug.WriteLine($"- Read MapId: {newContext.MapID}");
            }

            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.MapType))
            {
                newContext.MapType = (MapType)reader.ReadUInt32();
                Debug.WriteLine($"- Read MapType: {newContext.MapType}");
            }

            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.ShardId))
            {
                newContext.ShardID = reader.ReadUInt32();
                Debug.WriteLine($"- Read ShardID: {newContext.ShardID}");
            }

            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.Instance))
            {
                newContext.Instance = reader.ReadUInt32();
                Debug.WriteLine($"- Read Instance: {newContext.Instance}");
            }

            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.UiState))
            {
                newContext.UIState = (UIState)reader.ReadUInt32();
                Debug.WriteLine($"- Read UIState: {newContext.UIState}");
            }

            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.CompassWidth))
            {
                newContext.CompassWidth = reader.ReadUInt16();
                Debug.WriteLine($"- Read CompassWidth: {newContext.CompassWidth}");
            }

            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.CompassHeight))
            {
                newContext.CompassHeight = reader.ReadUInt16();
                Debug.WriteLine($"- Read CompassHeight: {newContext.CompassHeight}");
            }

            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.CompassRotation))
            {
                newContext.CompassRotation = reader.ReadSingle();
                Debug.WriteLine($"- Read CompassRotation: {newContext.CompassRotation}");
            }

            var newPlayerCoords = newContext.PlayerCoordinates;
            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.PlayerX))
            {
                newPlayerCoords.X = reader.ReadSingle();
                Debug.WriteLine($"- Read PlayerX: {newPlayerCoords.X}");
            }

            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.PlayerY))
            {
                newPlayerCoords.Y = reader.ReadSingle();
                Debug.WriteLine($"- Read PlayerY: {newPlayerCoords.Y}");
            }

            newContext.PlayerCoordinates = newPlayerCoords;

            var newMapCenter = newContext.MapCenterCoordinates;
            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.MapCenterX))
            {
                newMapCenter.X = reader.ReadSingle();
                Debug.WriteLine($"- Read MapCenterX: {newMapCenter.X}");
            }

            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.MapCenterY))
            {
                newMapCenter.Y = reader.ReadSingle();
                Debug.WriteLine($"- Read MapCenterY: {newMapCenter.Y}");
            }

            newContext.MapCenterCoordinates = newMapCenter;

            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.MapScale))
            {
                newContext.MapScale = reader.ReadSingle();
                Debug.WriteLine($"- Read MapScale: {newContext.MapScale}");
            }

            if (uHead.UpdateFieldMask.HasFlag(UpdateFieldMask.MountIndex))
            {
                newContext.MountIndex = (Mounts)reader.ReadByte();
                Debug.WriteLine($"- Read MountIndex: {newContext.MountIndex}");
            }

            newMem.Context = newContext;
            return new Update()
            {
                LinkedMem = newMem,
                TimestampMicro = uHead.TimestampMicro,
                TimeSinceStartMicro = uHead.TimestampMicro - LogStartMonotonicMicros,
            };
        }
    }
}