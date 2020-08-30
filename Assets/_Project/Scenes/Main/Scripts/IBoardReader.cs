using UnityEngine;

/// <summary>
/// オセロの盤の情報読み取りインタフェース
/// </summary>
public interface IBoardReader
{
    /// <summary>
    /// X軸Y軸それぞれのマスの数
    /// </summary>
    Vector2Int SquaresNumbers { get; }

    /// <summary>
    /// 盤の任意のマスの状態を取得します。
    /// </summary>
    /// <param name="boardPosition"> 盤上の座標 </param>
    /// <returns> 盤の任意のマスの状態 </returns>
    SquareState GetSquareState(Vector2Int boardPosition);
}
