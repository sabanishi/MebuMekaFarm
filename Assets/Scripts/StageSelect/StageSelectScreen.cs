using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using Sabanishi.MebuMekaFarm.Common;
using Sabanishi.MebuMekaFarm.MainGame;
using Sabanishi.MebuMekaFarm.Ugc;
using UnityEngine;
using Screen = Sabanishi.ScreenSystem.Screen;

namespace Sabanishi.MebuMekaFarm.StageSelect
{
    public class StageSelectScreen : Screen
    {
        [SerializeField] private ButtonComponent goToMainGameButton;
        [SerializeField] private ButtonComponent goToUgcButton;

        protected override UniTask ActivateInternal(CancellationToken activeToken)
        {
            goToMainGameButton.SafeOnClickAsObservable().Subscribe(_ =>
            {
                GoToMainGameScreen();
            }).AddTo(activeToken);
            goToUgcButton.SafeOnClickAsObservable().Subscribe(_ =>
            {
                GoToUgcScreen();
            }).AddTo(activeToken);
            
            return base.ActivateInternal(activeToken);
        }

        private void GoToMainGameScreen()
        {
            var to = ScreenGenerator.Generate<MainGameScreen>();
            ParentTransitioner.Move<MainGameScreen>(to).Forget();
        }
        
        private void GoToUgcScreen()
        {
            var to = ScreenGenerator.Generate<UgcScreen>();
            ParentTransitioner.Move<UgcScreen>(to).Forget();
        }
    }
}