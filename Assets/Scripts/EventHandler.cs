using System;

public static class EventHandler {
    public static event Action<ItemDetails, int> UpdateUIEvent;
    public static void CallUpdateUIEvent(ItemDetails itemDetails, int index) {
        UpdateUIEvent?.Invoke(itemDetails, index);
    }

    public static event Action BeforeSceneUnloadEvent; // 当前场景卸载之前
    public static void CallBeforeSceneUnloadEvent() {
        BeforeSceneUnloadEvent?.Invoke();
    }

    public static event Action AfterSceneloadedEvent; // 当前场景卸载之后 
    public static void CallAfterSceneLoadedEvent() {
        AfterSceneloadedEvent?.Invoke();

    }

    public static event Action<ItemDetails, bool> ItemSelectedEvent;
    public static void CallItemSelectedEvent(ItemDetails itemDetails, bool isselected) {
        ItemSelectedEvent?.Invoke(itemDetails, isselected);
    }

    // 使用物品要触发的事件
    public static event Action<ItemName> UseItemEvent;
    public static void CallUseItemEvent(ItemName itemname) {
        UseItemEvent?.Invoke(itemname);
    }

    public static event Action<int> ChangeItemEvent;
    public static void CallChangeItemEvent(int index) {
        ChangeItemEvent?.Invoke(index);
    }

    public static event Action<string> ShowDialogueEvent;
    public static void CallShowDialogueEvent(string dialogue) {
        ShowDialogueEvent?.Invoke(dialogue);
    }

    public static event Action<GameState> GameStateChangeEvent;
    public static void CallGameStateChangeEvent(GameState gameState) {
        GameStateChangeEvent?.Invoke(gameState);
    }

    public static event Action CheckGameStateEvent;
    public static void CallCheckGameStateEvent() {
        CheckGameStateEvent?.Invoke();
    }

    public static event Action<string> GamePassEvent;
    public static void CallGamePassEvent(string gameName) {
        GamePassEvent?.Invoke(gameName);
    }

    public static event Action<int> StarNewGameEvent;
    public static void CallStarNewGameEvent(int gameWeek) {
        StarNewGameEvent?.Invoke(gameWeek);
    }
    public static event Action<ItemName> RetunItemEvent;
    public static void CallRetunItemEvent(ItemName itemname_useless) { RetunItemEvent?.Invoke(itemname_useless); }
}
