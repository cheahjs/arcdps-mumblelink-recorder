using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MumblelinkReplay.Record
{
    [Flags]
    public enum UpdateFieldMask : uint
    {
        NoUpdate = 0,
        UiTick = 1,
        AvatarPosition = 1 << 1,
        AvatarFront = 1 << 2,
        AvatarTop = 1 << 3,
        Name = 1 << 4,
        CameraPosition = 1 << 5,
        CameraFront = 1 << 6,
        CameraTop = 1 << 7,
        Identity = 1 << 8,
        ServerAddress = 1 << 9,
        MapId = 1 << 10,
        MapType = 1 << 11,
        ShardId = 1 << 12,
        Instance = 1 << 13,
        UiState = 1 << 14,
        CompassWidth = 1 << 15,
        CompassHeight = 1 << 16,
        CompassRotation = 1 << 17,
        PlayerX = 1 << 18,
        PlayerY = 1 << 19,
        MapCenterX = 1 << 20,
        MapCenterY = 1 << 21,
        MapScale = 1 << 22,
        MountIndex = 1 << 23,
    }

    struct MumbleUpdate
    {
        public ulong TimestampMicro;
        public UpdateFieldMask UpdateFieldMask;
    }

    internal class DeltaUpdate
    {
    }
}
