using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// オセロをプレイするキャラクターインタフェース(プレイヤー・通信相手・AIだったり)
/// </summary>
public interface ICharacter
{
    /// <summary>
    /// 石を置く場所を取得します。
    /// </summary>
    /// <param name="board"> 盤の情報取得インタフェース </param>
    /// <param name="isBlack"> 置く石の色 </param>
    /// <returns> 石を置く場所 </returns>
    UniTask<Vector2Int> GetDiskPutPositionAsync(IBoardReader board, bool isBlack);
}
