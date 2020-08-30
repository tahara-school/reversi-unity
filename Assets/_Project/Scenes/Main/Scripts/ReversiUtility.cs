using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// オセロロジックの便利関数群
/// </summary>
public static class ReversiUtility
{
    /// <summary>
    /// 盤上の任意の場所に石を置いた時の、任意の方向のひっくり返る石のシーケンスを取得します。
    /// </summary>
    /// <param name="board"> 盤情報インタフェース </param>
    /// <param name="isBlack"> 置く石の色は黒か </param>
    /// <param name="putPosition"> 石を置く盤上の座標 </param>
    /// <param name="checkDirection"> ひっくり返る石を調べたい方向 </param>
    /// <returns> ひっくり返る石のシーケンス </returns>
    private static IEnumerable<Vector2Int> GetTurnDisks(IBoardReader board, bool isBlack, Vector2Int putPosition, Vector2Int checkDirection)
    {
        var result = new List<Vector2Int>();
        var checkPosition = putPosition;

        while (true) {
            checkPosition += checkDirection;

            // 枠外まで対が無かった。
            var isInRange = board.GetIsInRange(checkPosition);
            if (!isInRange) { return Enumerable.Empty<Vector2Int>(); }

            var state = board.GetSquareState(checkPosition);
            // 空白まで対が無かった。
            if (state == SquareState.Empty) { return Enumerable.Empty<Vector2Int>(); }

            // 対が見つかった。
            if (isBlack) {
                if (state == SquareState.Black) {
                    return result;
                }
            }
            else {
                if (state == SquareState.White) {
                    return result;
                }
            }

            // 挟みたい石があったので保持。
            result.Add(checkPosition);
        }
    }


    /// <summary>
    /// 盤上の任意の場所に石を置いた時の、ひっくり返る石のシーケンスを取得します。
    /// </summary>
    /// <param name="board"> 盤情報インタフェース </param>
    /// <param name="isBlack"> 置く石の色は黒か </param>
    /// <param name="putPosition"> 石を置く盤上の座標 </param>
    /// <returns> ひっくり返る石のシーケンス </returns>
    public static IEnumerable<Vector2Int> GetTurnDisks(IBoardReader board, bool isBlack, Vector2Int putPosition)
    {
        // ひっくり返る可能性のある、全方向ベクトル。
        IEnumerable<Vector2Int> directions = new[] {
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(0, 1),
            new Vector2Int(-1, -1),
            new Vector2Int(-1, 1),
            new Vector2Int(1, -1),
            new Vector2Int(1, 1),
        };
        // 全方向のひっくり返る石を集めて返す。
        return directions.SelectMany(d => GetTurnDisks(board, isBlack, putPosition, d));
    }
}
