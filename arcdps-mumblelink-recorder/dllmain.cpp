#include <chrono>
#include <Windows.h>
#include <stdio.h>

#include <cstdint>
#include <string>

#include "Globals.h"
#include "Logging.h"
#include "MiniBinStream.h"
#include "Mumble.h"
#include "extension/KeyBindHandler.h"
#include "extension/KeyInput.h"
#include "extension/Singleton.h"
#include "extension/Widgets.h"
#include "extension/Windows/PositioningComponent.h"
#include "extension/arcdps_structs.h"
#include "imgui/imgui.h"

const std::string kMumbleLinkRecorderPluginName = "MumbleLink Recorder";

/* proto/globals */
arcdps_exports arc_exports = {};
char *arc_version;

/* arcdps exports */
arc_export_func_u64 ARC_EXPORT_E6;
arc_export_func_u64 ARC_EXPORT_E7;
e3_func_ptr ARC_LOG_FILE;
e3_func_ptr ARC_LOG;

bool init_failed = false;

/* dll attach -- from winapi */
void dll_init(HMODULE hModule) { return; }

/* dll detach -- from winapi */
void dll_exit() { return; }

/* dll main -- winapi */
BOOL APIENTRY DllMain(HMODULE hModule, DWORD ulReasonForCall,
                      LPVOID lpReserved)
{
    switch (ulReasonForCall)
    {
    case DLL_PROCESS_ATTACH:
        dll_init(hModule);
        break;
    case DLL_PROCESS_DETACH:
        dll_exit();
        break;

    case DLL_THREAD_ATTACH:
        break;
    case DLL_THREAD_DETACH:
        break;
    }
    return 1;
}

/* window callback -- return is assigned to umsg (return zero to not be
 * processed by arcdps or game) */
uintptr_t mod_wnd(HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
    try
    {
        if (ImGuiEx::KeyCodeInputWndHandle(hWnd, uMsg, wParam, lParam))
        {
            return 0;
        }

        if (KeyBindHandler::instance().Wnd(hWnd, uMsg, wParam, lParam))
        {
            return 0;
        }

        auto const io = &ImGui::GetIO();

        switch (uMsg)
        {
        case WM_KEYUP:
        case WM_SYSKEYUP:
        {
            const int vkey = (int)wParam;
            io->KeysDown[vkey] = false;
            if (vkey == VK_CONTROL)
            {
                io->KeyCtrl = false;
            }
            else if (vkey == VK_MENU)
            {
                io->KeyAlt = false;
            }
            else if (vkey == VK_SHIFT)
            {
                io->KeyShift = false;
            }
            break;
        }
        case WM_KEYDOWN:
        case WM_SYSKEYDOWN:
        {
            const int vkey = (int)wParam;
            if (vkey == VK_CONTROL)
            {
                io->KeyCtrl = true;
            }
            else if (vkey == VK_MENU)
            {
                io->KeyAlt = true;
            }
            else if (vkey == VK_SHIFT)
            {
                io->KeyShift = true;
            }
            io->KeysDown[vkey] = true;
            break;
        }
        case WM_ACTIVATEAPP:
        {
            globals::UpdateArcExports();
            if (!wParam)
            {
                io->KeysDown[globals::arc_global_mod1] = false;
                io->KeysDown[globals::arc_global_mod2] = false;
            }
            break;
        }
        default:
            break;
        }
    }
    catch (const std::exception &e)
    {
        ARC_LOG_FILE("exception in mod_wnd");
        ARC_LOG_FILE(e.what());
        throw e;
    }
    return uMsg;
}

uintptr_t mod_windows(const char *windowname)
{
    if (!windowname)
    {
    }
    return 0;
}

uint64_t start_time_unix;
uint64_t num_of_events;
simple::file_ostream<std::true_type> record_stream;
LinkedMem prev_link;

uintptr_t mod_options()
{
    ImGui::Text(std::format("Start time: {}", start_time_unix).c_str());
    ImGui::Text(std::format("Events written: {}", num_of_events).c_str());

    bool disable_start = start_time_unix;
    bool disable_end = !disable_start;

    if (disable_start)
    {
        ImGui::PushItemFlag(ImGuiItemFlags_Disabled, true);
        ImGui::PushStyleVar(ImGuiStyleVar_Alpha, ImGui::GetStyle().Alpha * 0.5f);
    }
    if (ImGui::Button("Start recording") && !start_time_unix)
    {
        const auto cur_system = std::chrono::system_clock::now();
        const auto local_system = std::chrono::current_zone()->to_local(cur_system);
        const auto cur_monotonic = std::chrono::steady_clock::now();
        CreateDirectoryW(L"addons/mumblerecordings", nullptr);
        record_stream.open(std::format("addons/mumblerecordings/{:%F-%H-%M-%S}.mlrec", local_system).c_str());
        start_time_unix = std::chrono::duration_cast<std::chrono::microseconds>(cur_system.time_since_epoch()).count();
        const auto start_time_mono =
            std::chrono::duration_cast<std::chrono::microseconds>(cur_monotonic.time_since_epoch()).count();
        const auto mumble = MumbleLink::instance();
        prev_link = *mumble.linked_mem();
        const auto header = RecordingHeader(start_time_unix, start_time_mono, &prev_link);
        record_stream << header;
    }
    if (disable_start)
    {
        ImGui::PopItemFlag();
        ImGui::PopStyleVar();
    }
    if (disable_end)
    {
        ImGui::PushItemFlag(ImGuiItemFlags_Disabled, true);
        ImGui::PushStyleVar(ImGuiStyleVar_Alpha, ImGui::GetStyle().Alpha * 0.5f);
    }
    if (ImGui::Button("End recording") && start_time_unix)
    {
        start_time_unix = 0;
        num_of_events = 0;
        record_stream.close();
    }
    if (disable_end)
    {
        ImGui::PopItemFlag();
        ImGui::PopStyleVar();
    }
    return 0;
}

uintptr_t mod_imgui(uint32_t not_charsel_or_loading)
{
    if (start_time_unix == 0)
    {
        return 0;
    }
    // Check Mumblelink
    const auto &mumble = MumbleLink::instance();
    const LinkedMem state = *mumble.linked_mem();
    if (DiffAndWriteMumbleLink(prev_link, state, record_stream,
                               std::chrono::duration_cast<std::chrono::microseconds>(
                                   std::chrono::steady_clock::now().time_since_epoch()).count()))
    {
        num_of_events++;
        prev_link = state;
    }
    return 0;
}

/* initialize mod -- return table that arcdps will use for callbacks. exports
 * struct and strings are copied to arcdps memory only once at init */
arcdps_exports *mod_init()
{
    memset(&arc_exports, 0, sizeof(arcdps_exports));
    arc_exports.imguivers = IMGUI_VERSION_NUM;
    arc_exports.out_name = kMumbleLinkRecorderPluginName.c_str();
    arc_exports.out_build = "1.0.0.1";

    arc_exports.sig = 0xFBCA5641;
    arc_exports.size = sizeof(arcdps_exports);
    arc_exports.wnd_nofilter = mod_wnd;
    arc_exports.imgui = mod_imgui;
    arc_exports.options_end = mod_options;
    arc_exports.options_windows = mod_windows;

    logging::Debug("done mod_init");
    return &arc_exports;
}

/* release mod -- return ignored */
uintptr_t mod_release()
{
    g_singletonManagerInstance.Shutdown();
    return 0;
}

/* export -- arcdps looks for this exported function and calls the address it
 * returns on client load */
extern "C" __declspec(dllexport) void *get_init_addr(
    char *arcversion, ImGuiContext *imguicontext, void *id3dptr, HMODULE arcdll,
    void *mallocfn, void *freefn, uint32_t d3dversion)
{
    // id3dptr is IDirect3D9* if d3dversion==9, or IDXGISwapChain* if
    // d3dversion==11
    arc_version = arcversion;
    ImGui::SetCurrentContext(imguicontext);
    ImGui::SetAllocatorFunctions(static_cast<void* (*)(size_t, void *)>(mallocfn),
                                 static_cast<void (*)(void *, void *)>(freefn));

    ARC_EXPORT_E6 =
        reinterpret_cast<arc_export_func_u64>(GetProcAddress(arcdll, "e6"));
    ARC_EXPORT_E7 =
        reinterpret_cast<arc_export_func_u64>(GetProcAddress(arcdll, "e7"));
    ARC_LOG_FILE = reinterpret_cast<e3_func_ptr>(GetProcAddress(arcdll, "e3"));
    ARC_LOG = reinterpret_cast<e3_func_ptr>(GetProcAddress(arcdll, "e8"));
    return mod_init;
}

/* export -- arcdps looks for this exported function and calls the address it
 * returns on client exit */
extern "C" __declspec(dllexport) void *get_release_addr()
{
    arc_version = nullptr;
    return mod_release;
}
