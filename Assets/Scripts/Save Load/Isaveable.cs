public interface Isaveable {
    void SaveableRegister() {
        SaveLoadManager.Instance.Register(this);
    }

    GameSaveData GenerateSaveData();
    void RestoreSavedGameData(GameSaveData saveData);
}