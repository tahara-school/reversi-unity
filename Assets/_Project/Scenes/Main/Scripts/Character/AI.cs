using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// AIが操作するキャラクター
/// </summary>
public class AI : ICharacter
{
    /// <summary>
    /// キャラクターの名前
    /// </summary>
    public string Name { get; } = "CPU";


    /// <summary>
    /// 石を置く場所を取得します。
    /// </summary>
    /// <param name="board"> 盤の情報取得インタフェース </param>
    /// <param name="isBlack"> 置く石の色 </param>
    /// <returns> 石を置く場所 </returns>
    public async UniTask<Vector2Int> GetDiskPutPositionAsync(IBoardReader board, bool isBlack)
    {
        // 考えてるふりをする。
        await UniTask.Delay(System.TimeSpan.FromSeconds(1f));

        // 全マスを走査。
        for (int x = 0; x < board.SquaresNumbers.x; x++) {
            for (int y = 0; y < board.SquaresNumbers.y; y++) {
                var p = new Vector2Int(x, y);

                // 石が既にあるマスは無視。
                var state = board.GetSquareState(p);
                if (state != SquareState.Empty) { continue; }

                // 石を置くことで石をひっくり返せるマスだったら、そのマスの座標を返す。
                var turnDisks = ReversiUtility.GetTurnDisks(board, new DiskInformation(isBlack, p));
                var turnDisksExist = turnDisks.Count() != 0;
                if (turnDisksExist) { return p; }
            }
        }
        throw new System.Exception("盤に配置できる場所がありません。");
    }
}
