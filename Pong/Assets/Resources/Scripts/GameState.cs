using System.Collections;
using System.Collections.Generic;


public class GameState  {
    public enum State
    {
        TitleScreen,
        Playing,
        Paused,
        WinScreen
    }

    private static State state;

    // Using public methods instead of get/set, since this should produce more legible code
    // And allows us to change how state is set up later
    public static void SetState(State newState)
    {
        state = newState;
    }

    public static bool IsState(State queryState)
    {
        return state == queryState;
    }
}
