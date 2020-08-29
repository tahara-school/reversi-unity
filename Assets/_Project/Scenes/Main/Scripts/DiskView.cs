using UnityEngine;
using UniRx;

/// <summary>
/// オセロの石ビュー
/// </summary>
public class DiskView : MonoBehaviour
{
    [SerializeField, Tooltip("オセロの石を表す3Dモデル")]
    private Transform disk = default;


    /// <summary>
    /// 現在黒面か(変更検知用)
    /// </summary>
    private BoolReactiveProperty IsBlackReactive = new BoolReactiveProperty(true);


    /// <summary>
    /// ひっくり返ります。
    /// </summary>
    public void Turn()
    {
        IsBlackReactive.Value = !IsBlackReactive.Value;
    }

    /// <summary>
    /// 任意の面にひっくり返ります。
    /// </summary>
    /// <param name="isBlack"> 表面は黒か </param>
    public void Turn(bool isBlack)
    {
        IsBlackReactive.Value = isBlack;
    }


    private void Start()
    {
        // 黒か白かによって回転させ、実際にモデルをひっくり返す。
        IsBlackReactive.Subscribe(b => {
            if (b) {
                disk.rotation = Quaternion.identity;
            }
            else {
                disk.rotation = Quaternion.Euler(180f, 0f, 0f);
            }
        });
    }
}
