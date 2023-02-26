#pragma once
#include <cstdint>

#include <Windows.h>

#include "MiniBinStream.h"
#include "extension/Singleton.h"

struct LinkedMem
{
    uint32_t uiVersion;
    uint32_t uiTick;
    float avatarPosition[3];
    float avatarFront[3];
    float avatarTop[3];
    wchar_t name[256];
    float cameraPosition[3];
    float cameraFront[3];
    float cameraTop[3];
    wchar_t identity[256];
    uint32_t context_len;
    unsigned char context[256];
    wchar_t description[2048];
};

simple::file_ostream<std::true_type> &operator<<(simple::file_ostream<std::true_type> &out, const LinkedMem &mem);

struct MumbleContext
{
    unsigned char serverAddress[28]; // contains sockaddr_in or sockaddr_in6
    uint32_t mapId;
    uint32_t mapType;
    uint32_t shardId;
    uint32_t instance;
    uint32_t buildId;
    // Additional data beyond the <context_len> bytes the game instructs
    // Mumble to use to distinguish between instances. Bitmask: Bit 1 =
    // IsMapOpen, Bit 2 = IsCompassTopRight, Bit 3 =
    // DoesCompassHaveRotationEnabled, Bit 4 = Game has focus, Bit 5 = Is in
    // Competitive game mode, Bit 6 = Textbox has focus, Bit 7 = Is in
    // Combat
    uint32_t uiState;
    uint16_t compassWidth; // pixels
    uint16_t compassHeight; // pixels
    float compassRotation; // radians
    float playerX; // continentCoords
    float playerY; // continentCoords
    float mapCenterX; // continentCoords
    float mapCenterY; // continentCoords
    float mapScale;
    uint32_t processId;
    uint8_t mountIndex;
};

enum UpdateFieldFlags : uint32_t
{
    uiTick = 1,
    avatarPosition = 1 << 1,
    avatarFront = 1 << 2,
    avatarTop = 1 << 3,
    name = 1 << 4,
    cameraPosition = 1 << 5,
    cameraFront = 1 << 6,
    cameraTop = 1 << 7,
    identity = 1 << 8,
    serverAddress = 1 << 9,
    mapId = 1 << 10,
    mapType = 1 << 11,
    shardId = 1 << 12,
    instance = 1 << 13,
    uiState = 1 << 14,
    compassWidth = 1 << 15,
    compassHeight = 1 << 16,
    compassRotation = 1 << 17,
    playerX = 1 << 18,
    playerY = 1 << 19,
    mapCenterX = 1 << 20,
    mapCenterY = 1 << 21,
    mapScale = 1 << 22,
    mountIndex = 1 << 23,
};

bool DiffAndWriteMumbleLink(const LinkedMem &left, const LinkedMem &right, simple::file_ostream<std::true_type> &out_stream, uint64_t time);

struct RecordingHeader
{
    inline static char magicBytes[4] = {'M', 'L', 'R', 'C'};
    uint64_t startTimeUnixMicro;
    uint64_t startTimeMonotonicMicro;
    LinkedMem initialState{};

    RecordingHeader(uint64_t start_time_unix_micro, uint64_t start_time_monotonic_micro, const LinkedMem *initial_state)
    {
        startTimeUnixMicro = start_time_unix_micro;
        startTimeMonotonicMicro = start_time_monotonic_micro;
        initialState = *initial_state;
    }
};

simple::file_ostream<std::true_type> &operator<<(simple::file_ostream<std::true_type> &out,
                                                   const RecordingHeader &header);

struct MumbleUpdate
{
    uint64_t timeUnixMicro;
    uint32_t updatedFields;
};

simple::file_ostream<std::true_type> &operator<<(simple::file_ostream<std::true_type> &out,
                                                   const MumbleUpdate &update);

class MumbleLink : public Singleton<MumbleLink>
{
public:
    MumbleLink();
    ~MumbleLink() override;
    [[nodiscard]] const LinkedMem *linked_mem() const;

protected:
    std::wstring fileMappingName_ = L"MumbleLink";

    HANDLE fileMapping_ = nullptr;
    LinkedMem *linkedMemory_ = nullptr;

private:
    static std::wstring get_mumble_name();
};
