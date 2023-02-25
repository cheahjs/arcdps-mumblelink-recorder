#pragma once

#include <Windows.h>

namespace globals
{
    // arc keyboard modifier
    extern DWORD arc_global_mod1;
    extern DWORD arc_global_mod2;
    extern DWORD arc_global_mod_multi;

    // Arc export Cache
    extern bool arc_hide_all;
    extern bool arc_panel_always_draw;
    extern bool arc_movelock_altui;
    extern bool arc_clicklock_altui;
    extern bool arc_window_fastclose;

    // Arc helper functions
    void UpdateArcExports();
    bool ModsPressed();
    bool CanMoveWindows();
} // namespace globals
