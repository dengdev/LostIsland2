using System.Collections.Generic;

public class GameSaveData {
    public int gameWeek;
    public SceneName currentScene;
    public Dictionary<string, bool> miniGameStateDict;
    public Dictionary<ItemName, bool> itemAvailableDict;
    public Dictionary<string, bool> interactiveStateDict;
    public List<ItemName> itemList;
}
