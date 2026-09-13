using Sabanishi.MebuMekaFarm.Stage;
using UnityEngine;

namespace Sabanishi.MebuMekaFarm.MainGame
{
    public interface IStageGenerator
    {
        /// <summary>
        /// ステージを構築する
        /// </summary>
        public GameObject[,] Generate(ChipType[,] typeMatrix);
    }
}