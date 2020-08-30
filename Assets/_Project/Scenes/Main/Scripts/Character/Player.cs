using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// プレイヤーが操作するキャラクター
/// </summary>
public class Player : ICharacter
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="boardInput"> ボードの入力を取得するインタフェース </param>
    public Player(string name, IBoardInput boardInput)
    {
        Name = name;
        BoardInput = boardInput;
    }


    /// <summary>
    /// ボードの入力を取得するインタフェース
    /// </summary>
    private IBoardInput BoardInput { get; set; }


    /// <summary>
    /// キャラクターの名前
    /// </summary>
    public string Name { get; set; }


    /// <summary>
    /// 石を置く場所を取得します。
    /// </summary>
    /// <param name="board"> 盤の情報取得インタフェース </param>
    /// <param name="isBlack"> 置く石の色 </param>
    /// <returns> 石を置く場所 </returns>
    public async UniTask<Vector2Int> GetDiskPutPositionAsync(IBoardReader board, bool isBlack)
    {
        while (true) {
            // 盤のクリックを待機。
            var clickPosition = await BoardInput.WaitToClickAsync();
            // 既に石があったら再度待機。
            if (board.GetSquareState(clickPosition) != SquareState.Empty) { continue; }

            // クリックしたところに石を置いた時の、ひっくり返る石を取得。
            var diskPositions = ReversiUtility.GetTurnDisks(board, new DiskInformation(isBlack, clickPosition));
            // ひっくり返らなかったら再度待機。
            var turnDisksExist = diskPositions.Count() != 0;
            if (!turnDisksExist) { continue; }

            // ひっくり返ったらその座標を返す。
            return clickPosition;
        }
    }
}
