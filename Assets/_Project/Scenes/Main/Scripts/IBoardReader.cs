using UnityEngine;

/// <summary>
/// オセロの盤の情報読み取りインタフェース
/// </summary>
public interface IBoardReader
{
    /// <summary>
    /// 任意の盤上の座標が範囲外かを取得します。
    /// </summary>
    /// <param name="boardPosition"> 盤上の座標 </param>
    /// <returns> 任意の盤上の座標が範囲外か </returns>
    bool GetIsInRange(Vector2Int boardPosition);

    /// <summary>
    /// 盤の任意のマスの状態を取得します。
    /// </summary>
    /// <param name="boardPosition"> 盤上の座標 </param>
    /// <returns> 盤の任意のマスの状態 </returns>
    SquareState GetSquareState(Vector2Int boardPosition);
}
