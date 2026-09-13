using Sabanishi.MebuMekaFarm.Stage;
using UnityEngine;

namespace Sabanishi.MebuMekaFarm.MainGame
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private StageGenerator stageGenerator;
        [SerializeField] private CameraPropInitializer cameraPropInitializer;
        
        public void Initialize(ChipType[,] typeMatrix)
        {
            stageGenerator.Generate(typeMatrix);
            
            int width = typeMatrix.GetLength(0);
            int height = typeMatrix.GetLength(1);
            cameraPropInitializer.Initialize(width, height);
        }
    }
}