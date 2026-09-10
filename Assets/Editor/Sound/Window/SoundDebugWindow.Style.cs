using UnityEngine;

namespace Sabanishi.MebuMekaFarm.Editor.Sound
{
    public partial class SoundDebugWindow
    {
        private const int CustomStylesCount = 20;
        private static readonly GUIStyle[] CustomStyles = new GUIStyle[CustomStylesCount];
        private static readonly Texture2D[] CustomStyleTex = new Texture2D[CustomStylesCount];

        private delegate void AdditionalStyleDelegate(GUIStyle style);

        private GUIStyle DisplayStyle => InitializeStyleWithBgColor(1, new Color(0.1f, 0.1f, 0.1f));

        private GUIStyle SelectionStyle
        {
            get
            {
                // 初期化
                InitializeStyleWithBgColor(6, new Color(0.3f, 0.3f, 0.3f, 1) );
                InitializeStyleWithBgColor(7, new Color(0.3f, 0.3f, 0.3f, 1) );
                InitializeStyleWithBgColor(8, new Color(0.3f, 0.3f, 0.3f, 1) );
                
                CustomStyleTex[6].SetPixel(1, 1, new Color(0.4f, 0.4f, 0.4f, 1));
                CustomStyleTex[6].Apply();
                CustomStyles[8].hover.background = CustomStyleTex[6];
                
                CustomStyleTex[7].SetPixel(1, 1, new Color(0.7f, 0.7f, 0.7f, 1));
                CustomStyleTex[7].Apply();
                CustomStyles[8].onActive.background = CustomStyleTex[7];

                CustomStyles[8].margin.top = 8;
                CustomStyles[8].margin.right = 2;
                CustomStyles[8].alignment = TextAnchor.MiddleCenter;

                return InitializeStyleWithBgColor(8, new Color(0.3f, 0.3f, 0.3f, 1) );
            }
        }

        private GUIStyle ActiveSelectionStyle
        {
            get
            {
                InitializeStyleWithBgColor(9, new Color(0.1f, 0.1f,0.1f, 1));
                CustomStyles[9].margin.right = 8;
                CustomStyles[9].margin.left = 8;
                CustomStyles[9].alignment = TextAnchor.MiddleCenter;
                return InitializeStyleWithTextColor(9, Color.white);
            }
        }

        private GUIStyle VolumeStyle => InitializeStyleWithBgColor(4, Color.yellow);

        private GUIStyle LoopStyle => InitializeStyleWithTextColor(5, Color.yellow);

        private GUIStyle PlayingStyle => InitializeStyleWithTextColor(3, Color.green);

        private GUIStyle StopStyle => InitializeStyleWithTextColor(3, Color.gray);

        private GUIStyle PlayingTimeStyle => InitializeStyleWithBgColor(2, Color.cyan);

        private GUIStyle InitializeStyleWithTextColor(int id, Color color, AdditionalStyleDelegate func = null)
        {
            if (CustomStyles[id] == null)
            {
                CustomStyles[id] = new GUIStyle();
                if (func != null)
                {
                    func(CustomStyles[id]);
                }
            }
            CustomStyles[id].normal.textColor = color;
            return CustomStyles[id];
        }
        
        private GUIStyle InitializeStyleWithBgColor(int id, Color color, AdditionalStyleDelegate func = null)
        {
            if (CustomStyleTex[id] == null)
            {
                CustomStyleTex[id] = new Texture2D(1, 1, TextureFormat.RGB24, false);
                CustomStyleTex[id].SetPixel(1, 1, color);
                CustomStyleTex[id].Apply();
            }

            if (CustomStyles[id] == null)
            {
                CustomStyles[id] = new GUIStyle();
                if (func != null)
                {
                    func(CustomStyles[id]);
                }
            }

            CustomStyles[id].normal.background = CustomStyleTex[id];
            return CustomStyles[id];
        }
    }
}