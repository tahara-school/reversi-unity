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
    /// <param name="putDisk"> 置く石の情報 </param>
    /// <param name="checkDirection"> ひっくり返る石を調べたい方向 </param>
    /// <returns> ひっくり返る石のシーケンス </returns>
    private static IEnumerable<Vector2Int> GetTurnDisks(IBoardReader board, DiskInformation putDisk, Vector2Int checkDirection)
    {
        var result = new List<Vector2Int>();
        var checkPosition = putDisk.Position;

        while (true) {
            checkPosition += checkDirection;

            // 空白まで対が無かった。
            var state = GetSquareStateOrEmpty(board, checkPosition);
            if (state == SquareState.Empty) { return Enumerable.Empty<Vector2Int>(); }

            // 対が見つかった。
            if (putDisk.IsBlack) {
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
    /// 盤の任意のマスの状態を取得します。盤外だった場合は<see cref="SquareState.Empty"/>を返します。
    /// </summary>
    /// <param name="board"> 盤情報インタフェース </param>
    /// <param name="boardPosition"> 盤上の座標 </param>
    /// <returns></returns>
    private static SquareState GetSquareStateOrEmpty(IBoardReader board, Vector2Int boardPosition)
    {
        // 枠外だったら、石が置かれていない扱いとする。
        var isInRange = GetIsInRange(board, boardPosition);
        if (!isInRange) { return SquareState.Empty; }

        return board.GetSquareState(boardPosition);
    }

    /// <summary>
    /// 任意の盤上の座標が範囲外かを取得します。
    /// </summary>
    /// <param name="board"> 盤情報インタフェース </param>
    /// <param name="boardPosition"> 盤上の座標 </param>
    /// <returns> 任意の盤上の座標が範囲外か </returns>
    public static bool GetIsInRange(IBoardReader board, Vector2Int boardPosition)
    {
        return MathUtility.IsInRange(boardPosition, Vector2Int.zero, GetMaxBoardPosition(board));
    }

    /// <summary>
    /// 盤の座標の最大値
    /// </summary>
    /// <param name="board"> 盤情報インタフェース </param>
    public static Vector2Int GetMaxBoardPosition(IBoardReader board)
    {
        return new Vector2Int(board.SquaresNumbers.x - 1, board.SquaresNumbers.y - 1);
    }

    /// <summary>
    /// 盤上の任意の場所に石を置いた時の、ひっくり返る石のシーケンスを取得します。
    /// </summary>
    /// <param name="board"> 盤情報インタフェース </param>
    /// <param name="putDisk"> 置く石の情報 </param>
    /// <returns> ひっくり返る石のシーケンス </returns>
    public static IEnumerable<Vector2Int> GetTurnDisks(IBoardReader board, DiskInformation putDisk)
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
        return directions.SelectMany(d => GetTurnDisks(board, putDisk, d));
    }

    /// <summary>
    /// 盤上に石を置くことが出来るかを取得します。
    /// </summary>
    /// <param name="board"> 盤情報インタフェース </param>
    /// <param name="isBlack"> 置く石は黒か </param>
    /// <returns> 盤上に石を置くことが出来るか </returns>
    public static bool GetCanPutDisk(IBoardReader board, bool isBlack)
    {
        // 全マスを走査。
        for (int x = 0; x < board.SquaresNumbers.x; x++) {
            for (int y = 0; y < board.SquaresNumbers.y; y++) {
                var p = new Vector2Int(x, y);

                // 石が既にあるマスは無視。
                var state = board.GetSquareState(p);
                if (state != SquareState.Empty) { continue; }

                // 石を置くことで石をひっくり返せるマスだったら、そのマスの座標を返す。
                var turnDisks = GetTurnDisks(board, new DiskInformation(isBlack, p));
                var turnDisksExist = turnDisks.Count() != 0;
                if (turnDisksExist) { return true; }
            }
        }
        return false;
    }
}
