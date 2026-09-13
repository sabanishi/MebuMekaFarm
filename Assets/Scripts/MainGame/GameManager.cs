using Sabanishi.MebuMekaFarm.Stage;
using UnityEngine;

namespace Sabanishi.MebuMekaFarm.MainGame
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private StageGenerator stageGenerator;
        
        public void Initialize(ChipType[,] typeMatrix)
        {
            stageGenerator.Generate(typeMatrix);
        }
    }
}