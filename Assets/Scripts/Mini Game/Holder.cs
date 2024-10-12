using System.Collections.Generic;

public class Holder : Interactive {
    public BallName matchBall;
    public HashSet<Holder> LinkHolders = new HashSet<Holder>();
    public bool isEmpty;

    private Ball currentBall;

    public void CheckBall(Ball ball) {
        currentBall = ball;

        if (ball.ballDetails.BallName == matchBall) {
            currentBall.isMatch = true;
            currentBall.SetRight();
        } else {
            currentBall.isMatch = false;
            currentBall.SetWrong();
        }
    }

    public override void EmptyClicked() {

        foreach (Holder holder in LinkHolders) {
            if (holder.isEmpty) {
                currentBall.transform.position = holder.transform.position;
                currentBall.transform.SetParent(holder.transform);
                holder.CheckBall(currentBall);
                this.currentBall = null;
                this.isEmpty = true;
                holder.isEmpty = false;
                EventHandler.CallCheckGameStateEvent();
            }
        }
    }
}
