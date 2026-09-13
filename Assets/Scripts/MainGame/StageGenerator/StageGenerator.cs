using System;
using Sabanishi.MebuMekaFarm.Common;
using Sabanishi.MebuMekaFarm.Stage;
using UnityEngine;

namespace Sabanishi.MebuMekaFarm.MainGame
{
    public class StageGenerator : MonoBehaviour, IStageGenerator
    {
        [SerializeField] private ChipDict prefabDict;
        [SerializeField] private Transform chipsParent;
        
        public GameObject[,] Generate(ChipType[,] typeMatrix)
        {
            float chipSize = Constants.ChipSize;
            int width = typeMatrix.GetLength(0);
            int height = typeMatrix.GetLength(1);
            GameObject[,] result = new GameObject[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    ChipType type = typeMatrix[x, y];
                    if(type == ChipType.None)continue;
                    GameObject instance = Instantiate(prefabDict[type], chipsParent, true);
                    instance.name = type.ToString();
                    instance.transform.localPosition = new Vector3(x, y, 0);
                    result[x, y] = instance;
                }
            }
            
            return result;
        }

        [Serializable]
        public class ChipDict : SerializableDictionary<ChipType, GameObject>
        {
        }

        [Serializable]
        public class ChipDictType : SerializableKeyValuePair<ChipType, GameObject>
        {
            public ChipDictType(ChipType key, GameObject value) : base(key, value)
            {
            }
        }
    }
}