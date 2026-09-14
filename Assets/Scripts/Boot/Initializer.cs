using System.Linq;
using Cysharp.Threading.Tasks;
using Sabanishi.MebuMekaFarm.Sound;
using Sabanishi.ScreenSystem;
using TypeReferences;
using UnityEngine;
using Screen = Sabanishi.ScreenSystem.Screen;

namespace Sabanishi.MebuMekaFarm
{
    public class Initializer : MonoBehaviour
    {
        [SerializeField] private bool isAutoStart;

        [Inherits(typeof(IScreen))] [SerializeField]
        private TypeReference startScreenType;

        [SerializeField] private SoundDatabase<GeneralType> soundDatabase;

        private ScreenTransitioner _transitioner;

        private void Start()
        {
            if (!isAutoStart) return;
            Boot();
        }

        private void Boot()
        {
            SoundLocator.Instance.SoundPlayer.Setup(soundDatabase);

            _transitioner = new ScreenTransitioner(null, null);

            IScreen toScreen = SearchScreenFromHierarchy() ?? ScreenGenerator.Generate(startScreenType.Type);
            _transitioner.Jump<IScreen>(toScreen).Forget();
        }

        private Screen SearchScreenFromHierarchy()
        {
            // ヒエラルキー内のオブジェクトを走査してScreen継承クラスを返す
            return FindObjectsByType<Screen>(FindObjectsSortMode.None).FirstOrDefault();
        }
    }
}