using UnityEngine;
using Cysharp.Threading.Tasks;

public class GameController : MonoBehaviour
{
    [SerializeField, Tooltip("使用するオセロ盤ビュー")]
    private BoardView boardView = default;


    /// <summary>
    /// 先手のキャラクター
    /// </summary>
    private ICharacter Player { get; set; }

    /// <summary>
    /// 後手のキャラクター
    /// </summary>
    private ICharacter Opponent { get; set; }


    /// <summary>
    /// 今は黒のターンか
    /// </summary>
    private bool IsBlackTurn { get; set; } = true;


    private void Start()
    {
        // キャラクター両者を初期化。
        Player = new Player("TestPlayer", boardView);
        Opponent = new AI();

        // ゲームを開始。
        PlayReversiAsync().Forget();
    }

    /// <summary>
    /// オセロを開始します。
    /// </summary>
    private async UniTaskVoid PlayReversiAsync()
    {
        while (true) {
            // このターンのキャラクターを取得。
            var turnPlayer = IsBlackTurn ? Player : Opponent;

            // 盤の状態
            var boardState = ReversiUtility.GetBoardState(boardView, IsBlackTurn);

            switch (boardState) {
                // 石を置けるのでターン続行。
                case BoardState.Continue:
                    Debug.Log($"{turnPlayer.Name}の番");
                    var putDiskPosition = await turnPlayer.GetDiskPutPositionAsync(boardView, IsBlackTurn);
                    var putDiskInfo = new DiskInformation(IsBlackTurn, putDiskPosition);

                    // 石配置。
                    boardView.PutDisk(putDiskInfo);

                    // 石をひっくり返す。
                    var diskPositions = ReversiUtility.GetTurnDisks(boardView, putDiskInfo);
                    foreach (var diskPosition in diskPositions) {
                        await UniTask.Delay(System.TimeSpan.FromSeconds(0.1f));
                        boardView.TurnDisk(diskPosition);
                    }
                    IsBlackTurn = !IsBlackTurn;
                    break;

                // 石を置け無かったらパス。
                case BoardState.Pass:
                    Debug.Log($"{turnPlayer.Name}の番はパス");
                    IsBlackTurn = !IsBlackTurn;
                    await UniTask.Delay(System.TimeSpan.FromSeconds(3f));
                    break;

                // 盤が埋まってたら終了。
                case BoardState.Finish:
                    Debug.Log("ゲーム終了");
                    return;
            }
        }
    }
}
