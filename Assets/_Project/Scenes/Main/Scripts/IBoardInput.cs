using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// 盤の入力を取得するインタフェース
/// </summary>
public interface IBoardInput
{
    /// <summary>
    /// 盤のクリックを待機します。
    /// </summary>
    /// <returns> クリックされた盤上の座標 </returns>
    UniTask<Vector2Int> WaitToClickAsync();
}
