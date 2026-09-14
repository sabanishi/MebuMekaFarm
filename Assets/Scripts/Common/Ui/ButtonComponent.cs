using R3;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sabanishi.MebuMekaFarm
{
    public class ButtonComponent : UIBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private GameObject filter;

        public Observable<Unit> OnClickAsObservable => button.SafeOnClickAsObservable();

        /// <summary>
        /// ボタンの見た目が変わって押せなくなる（ボタンのカラーがButtonコンポーネントのDisable Colorになる）
        /// </summary>
        public void SetInteractable(bool isInteractable)
        {
            button.interactable = isInteractable;
            if (filter != null) filter.SetActive(!isInteractable);
        }

        /// <summary>
        /// ボタンの見た目は変わらないが押せなくなる
        /// </summary>
        public void SetEnabled(bool isEnabled)
        {
            button.enabled = isEnabled;
        }
    }

    public static class ButtonComponentExtensions
    {
        public static Observable<Unit> SafeOnClickAsObservable(this ButtonComponent source)
        {
            if (source == null)
            {
                Debug.LogError($"ButtonComponentの参照が外れています");
                return Observable.Empty<Unit>();
            }

            return source.OnClickAsObservable;
        }
    }
}