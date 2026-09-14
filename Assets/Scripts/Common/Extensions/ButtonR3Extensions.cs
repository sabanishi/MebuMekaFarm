using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Sabanishi.MebuMekaFarm
{
    public static class ButtonR3Extensions
    {
        public static Observable<Unit> SafeOnClickAsObservable(this Button source)
        {
            if (source == null)
            {
                Debug.LogError($"Buttonの参照が外れています");
                return Observable.Empty<Unit>();
            }

            return source.OnClickAsObservable();
        }
    }
}