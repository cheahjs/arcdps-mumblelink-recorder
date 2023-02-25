#include "Mumble.h"

#include "Logging.h"

simple::file_ostream<std::true_type> &operator<<(simple::file_ostream<std::true_type> &out, const LinkedMem &mem)
{
    out << mem.uiVersion << mem.uiTick;
    out.write(reinterpret_cast<const char *>(mem.fAvatarPosition), sizeof mem.fAvatarPosition);
    out.write(reinterpret_cast<const char *>(mem.fAvatarFront), sizeof mem.fAvatarFront);
    out.write(reinterpret_cast<const char *>(mem.fAvatarFront), sizeof mem.fAvatarFront);
    out.write(reinterpret_cast<const char *>(mem.name), sizeof mem.name);
    out.write(reinterpret_cast<const char *>(mem.fCameraPosition), sizeof mem.fCameraPosition);
    out.write(reinterpret_cast<const char *>(mem.fCameraFront), sizeof mem.fCameraFront);
    out.write(reinterpret_cast<const char *>(mem.fCameraTop), sizeof mem.fCameraTop);
    out.write(reinterpret_cast<const char *>(mem.identity), sizeof mem.identity);
    out << mem.context_len;
    out.write(reinterpret_cast<const char *>(mem.context), sizeof mem.context);
    out.write(reinterpret_cast<const char *>(mem.description), sizeof mem.description);
    return out;
}

#define COMPARE_PRIMITIVE(left, right, field, flag) \
    if (left.field != right.field) { \
        update_fields |= UpdateFieldFlags::flag; \
        update_stream << right.field; \
    }
#define COMPARE_MEMORY(left, right, field, flag) \
    if (memcmp(left.field, right.field, sizeof left.field) != 0) { \
        update_fields |= UpdateFieldFlags::flag; \
        update_stream.write(reinterpret_cast<const char *>(right.field), sizeof right.field); \
    }

bool DiffAndWriteMumbleLink(const LinkedMem &left, const LinkedMem &right,
                            simple::file_ostream<std::true_type> &out_stream, uint64_t time)
{
    if (memcmp(&left, &right, sizeof LinkedMem) == 0)
    {
        return false;
    }
    uint32_t update_fields = 0;
    simple::mem_ostream<std::true_type> update_stream{};
    COMPARE_PRIMITIVE(left, right, uiTick, uiTick);
    COMPARE_MEMORY(left, right, fAvatarPosition, avatarPosition)
    COMPARE_MEMORY(left, right, fAvatarFront, avatarFront)
    COMPARE_MEMORY(left, right, fAvatarTop, avatarTop)
    COMPARE_MEMORY(left, right, name, name)
    COMPARE_MEMORY(left, right, fCameraPosition, cameraPosition)
    COMPARE_MEMORY(left, right, fCameraFront, cameraFront)
    COMPARE_MEMORY(left, right, fCameraTop, cameraTop)
    COMPARE_MEMORY(left, right, identity, identity)
    const auto left_context = *reinterpret_cast<const MumbleContext *>(left.context);
    const auto right_context = *reinterpret_cast<const MumbleContext *>(right.context);
    COMPARE_MEMORY(left_context, right_context, serverAddress, serverAddress)
    COMPARE_PRIMITIVE(left_context, right_context, mapId, mapId)
    COMPARE_PRIMITIVE(left_context, right_context, mapType, mapType)
    COMPARE_PRIMITIVE(left_context, right_context, shardId, shardId)
    COMPARE_PRIMITIVE(left_context, right_context, instance, instance)
    COMPARE_PRIMITIVE(left_context, right_context, uiState, uiState)
    COMPARE_PRIMITIVE(left_context, right_context, compassWidth, compassWidth)
    COMPARE_PRIMITIVE(left_context, right_context, compassHeight, compassHeight)
    COMPARE_PRIMITIVE(left_context, right_context, compassRotation, compassRotation)
    COMPARE_PRIMITIVE(left_context, right_context, playerX, playerX)
    COMPARE_PRIMITIVE(left_context, right_context, playerY, playerY)
    COMPARE_PRIMITIVE(left_context, right_context, mapCenterX, mapCenterX)
    COMPARE_PRIMITIVE(left_context, right_context, mapScale, mapScale)
    COMPARE_PRIMITIVE(left_context, right_context, mountIndex, mountIndex)
    const MumbleUpdate update{time, update_fields};
    out_stream << update;
    out_stream << update_stream.get_internal_vec();
    update_stream.close();
    return true;
}

simple::file_ostream<std::true_type> &operator<<(simple::file_ostream<std::true_type> &out,
                                                 const RecordingHeader &header)
{
    out.write(RecordingHeader::magicBytes, sizeof RecordingHeader::magicBytes);
    out << header.startTimeUnixMicro << header.startTimeMonotonicMicro << header.initialState;
    return out;
}

simple::file_ostream<std::true_type> &operator<<(simple::file_ostream<std::true_type> &out,
                                                 const MumbleUpdate &update)
{
    out << update.timeUnixMicro << update.updatedFields;
    return out;
}

MumbleLink::MumbleLink()
{
    fileMappingName_ = get_mumble_name();

    fileMapping_ =
        CreateFileMappingW(INVALID_HANDLE_VALUE, NULL, PAGE_READWRITE, 0,
                           sizeof(LinkedMem), fileMappingName_.c_str());
    if (!fileMapping_)
    {
        logging::Log("Could not find MumbleLink map!");
        return;
    }

    linkedMemory_ = static_cast<LinkedMem *>(
        MapViewOfFile(fileMapping_, FILE_MAP_READ, 0, 0, sizeof(LinkedMem)));
    if (!linkedMemory_)
    {
        logging::Log("Could not find MumbleLink map!");

        CloseHandle(fileMapping_);
        fileMapping_ = nullptr;
    }
}

MumbleLink::~MumbleLink()
{
    if (linkedMemory_)
    {
        UnmapViewOfFile(linkedMemory_);
        linkedMemory_ = nullptr;
    }
    if (fileMapping_)
    {
        CloseHandle(fileMapping_);
        fileMapping_ = nullptr;
    }
}

std::wstring MumbleLink::get_mumble_name()
{
    static std::wstring const command = L"-mumble";
    std::wstring commandLine = GetCommandLine();

    size_t index = commandLine.find(command, 0);

    if (index != std::wstring::npos)
    {
        if (index + command.length() < commandLine.length())
        {
            auto const start = index + command.length() + 1;
            auto const end = commandLine.find(' ', start);
            std::wstring mumble = commandLine.substr(
                start,
                (end != std::wstring::npos ? end : commandLine.length()) -
                start);

            return mumble;
        }
    }

    return L"MumbleLink";
}

const LinkedMem *MumbleLink::linked_mem() const
{
    if (!linkedMemory_)
        return nullptr;

    return linkedMemory_;
}
