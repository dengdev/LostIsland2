public interface Isaveable {
    void SaveableRegister() {
        SaveLoadManager.Instance.Register(this);
    }

    GameSaveData GeneratesaveData();
    void RestoreGameData(GameSaveData saveData);
}