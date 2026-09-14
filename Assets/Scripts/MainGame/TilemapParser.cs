using Sabanishi.MebuMekaFarm.Stage;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Sabanishi.MebuMekaFarm.MainGame
{
    public class TilemapParser : MonoBehaviour
    {
        [SerializeField] private Tilemap tmpTilemap;

        public ChipType[,] ParseToChipTypeMatrix()
        {
            tmpTilemap.CompressBounds();
            BoundsInt bounds = tmpTilemap.cellBounds;
            TileBase[] allBlocks = tmpTilemap.GetTilesBlock(bounds);
            
            int width = Constants.MapWidth;
            int height = Constants.MapHeight;
            ChipType[,] result = new ChipType[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    TileBase tileBase = allBlocks[x + y * width];
                    if(tileBase == null)continue;
                    var chipType = tileBase.name.ToChipType();
                    result[x, y] = chipType;
                }
            }
            
            return result;
        }
    }
}