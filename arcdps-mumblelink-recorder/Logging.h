#pragma once

#include <format>
#include <string>

#include "extension/arcdps_structs.h"

namespace logging
{
    /* log to arcdps.log, thread/async safe */
    inline void File(const char *str)
    {
        if (ARC_LOG_FILE)
            ARC_LOG_FILE(str);
    }

    /* log to extensions tab in arcdps log window, thread/async safe */
    inline void Arc(const char *str)
    {
        if (ARC_LOG)
            ARC_LOG(str);
    }

    inline void Log(char *str)
    {
#if _DEBUG
        Arc(std::format("mumblelink_recorder: {}", str).c_str());
#endif
        File(std::format("mumblelink_recorder: {}", str).c_str());
    }

    inline void Log(const char *str)
    {
        Log(const_cast<char *>(str));
    }

    inline void Log(std::string str)
    {
        Log(const_cast<char *>(str.c_str()));
    }

    template <typename... Args>
    void Log(const char *format, Args... args)
    {
        Log(std::format(format, args...));
    }

    inline void Debug(char *str)
    {
#if _DEBUG
        Log(std::format("DEBUG: {}", str).c_str());
#endif
    }

    inline void Debug(const char *str)
    {
#if _DEBUG
        Debug(const_cast<char *>(str));
#endif
    }

    inline void Debug(const std::string &str)
    {
#if _DEBUG
        Debug(const_cast<char *>(str.c_str()));
#endif
    }
} // namespace logging
