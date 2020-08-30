/// <summary>
/// オセロの盤の状態列挙型
/// </summary>
public enum BoardState
{
    /// <summary>
    /// 石が置ける
    /// </summary>
    Continue,

    /// <summary>
    /// 石が置けない
    /// </summary>
    Pass,

    /// <summary>
    /// 全てのマスが埋まって試合終了
    /// </summary>
    Finish,
}
