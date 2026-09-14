using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using Sabanishi.MebuMekaFarm.StageSelect;
using UnityEngine;
using Screen = Sabanishi.ScreenSystem.Screen;

namespace Sabanishi.MebuMekaFarm.Title
{
    public class TitleScreen : Screen
    {
        [SerializeField] private ButtonComponent goToStageSelectButton;

        protected override UniTask ActivateInternal(CancellationToken activeToken)
        {
            goToStageSelectButton.SafeOnClickAsObservable().Subscribe(_ =>
            {
                GoToStageSelectScreen();
            }).AddTo(activeToken);
            return base.ActivateInternal(activeToken);
        }

        private void GoToStageSelectScreen()
        {
            var to = ScreenGenerator.Generate<StageSelectScreen>();
            ParentTransitioner.Jump<StageSelectScreen>(to).Forget();
        }
    }
}