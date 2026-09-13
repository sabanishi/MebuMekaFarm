using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using Sabanishi.MebuMekaFarm.Common;
using UnityEngine;
using Screen = Sabanishi.ScreenSystem.Screen;

namespace Sabanishi.MebuMekaFarm.Ugc
{
    public class UgcScreen : Screen
    {
        [SerializeField] private ButtonComponent goBackButton;

        protected override UniTask ActivateInternal(CancellationToken activeToken)
        {
            goBackButton.SafeOnClickAsObservable().Subscribe(_ =>
            {
                GoBack();
            }).AddTo(activeToken);
            return base.ActivateInternal(activeToken);
        }

        private void GoBack()
        {
            ParentTransitioner.Back().Forget();
        }
    }
}