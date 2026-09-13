using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using Sabanishi.MebuMekaFarm.Common;
using UnityEngine;
using Screen = Sabanishi.ScreenSystem.Screen;

namespace Sabanishi.MebuMekaFarm.MainGame
{
    public class MainGameScreen : Screen
    {
        [SerializeField] private ButtonComponent goBackButton;
        
        [SerializeField] private TilemapParser tilemapParser;
        [SerializeField] private GameManager gameManager;

        protected override UniTask InitializeInternal(CancellationToken initializeToken)
        {
            var chipTypeMatrix = tilemapParser.ParseToChipTypeMatrix();
            gameManager.Initialize(chipTypeMatrix);
            return base.InitializeInternal(initializeToken);
        }

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