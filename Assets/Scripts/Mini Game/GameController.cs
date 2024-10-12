using UnityEngine;
using UnityEngine.Events;


public class GameController : Singleton<GameController> {
    [Header("游戏数据")]
    public GameH2A_SO gameData;
    public GameH2A_SO[] gameDataArray;
    public GameObject LineParent;
    public LineRenderer linePrefab;
    public Ball ballPrefab;

    public UnityEvent OnFinish;
    public Transform[] holderTransforms;

    private void OnEnable() {
        MusicManager.Instance.PlayOpenRoad();
        EventHandler.CheckGameStateEvent += OnCheckGameStateEvent;
    }

    private void OnDisable() {
        MusicManager.Instance.PlayPaperWings();

        EventHandler.CheckGameStateEvent -= OnCheckGameStateEvent;
    }

    private void OnCheckGameStateEvent() {
        foreach (var ball in FindObjectsOfType<Ball>()) {
            if (!ball.isMatch)
                return;
        }
        Debug.Log("小游戏已完成");
        foreach (var holder in holderTransforms) {
            holder.GetComponent<Collider2D>().enabled = false;
        }
        EventHandler.CallGamePassEvent(gameData.gameName);
        OnFinish?.Invoke();
    }

    public void Resetgame() {

        foreach (var holder in holderTransforms) {
            if (holder.childCount > 0)
                Destroy(holder.GetChild(0).gameObject);
        }
        CreateBall();
    }

    public void DrawLine() {
        foreach (var connections in gameData.lineConnections) {
            var line = Instantiate(linePrefab, LineParent.transform);
            line.SetPosition(0, holderTransforms[connections.from].position);
            line.SetPosition(1, holderTransforms[connections.to].position);
            //创建每个Holder的连接关系
            holderTransforms[connections.from].GetComponent<Holder>().LinkHolders.Add(holderTransforms[connections.to].GetComponent<Holder>());
            holderTransforms[connections.to].GetComponent<Holder>().LinkHolders.Add(holderTransforms[connections.from].GetComponent<Holder>());
        }
    }

    public void CreateBall() {
        for (int i = 0; i < gameData.startBallOrder.Count; i++) {
            if (gameData.startBallOrder[i] == BallName.None) {
                holderTransforms[i].GetComponent<Holder>().isEmpty = true;
                continue;
            }
            Ball ball = Instantiate(ballPrefab, holderTransforms[i]);

            holderTransforms[i].GetComponent<Holder>().CheckBall(ball);
            holderTransforms[i].GetComponent<Holder>().isEmpty = false;
            ball.SetupBall(gameData.GetBallDetails(gameData.startBallOrder[i]));
        }
    }

    public void SetGameWeekData(int week) {
        gameData = gameDataArray[week];
        DrawLine();
        CreateBall();
    }
}
