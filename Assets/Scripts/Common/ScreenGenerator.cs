using System;
using System.Collections.Generic;
using Sabanishi.MebuMekaFarm.MainGame;
using Sabanishi.MebuMekaFarm.StageSelect;
using Sabanishi.MebuMekaFarm.Title;
using Sabanishi.MebuMekaFarm.Ugc;
using Sabanishi.ScreenSystem;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sabanishi.MebuMekaFarm
{
    public static class ScreenGenerator
    {
        private static readonly Dictionary<Type, string> ScreenPrefabPathMap = new Dictionary<Type, string>()
        {
            { typeof(TitleScreen), "Screen/TitleScreen" },
            { typeof(StageSelectScreen), "Screen/StageSelectScreen" },
            { typeof(MainGameScreen), "Screen/MainGameScreen" },
            { typeof(UgcScreen), "Screen/UgcScreen" },
        };

        /// <summary>
        /// Typeに対応するScreenを生成する
        /// </summary>
        public static IScreen Generate(Type type)
        {
            if (!ScreenPrefabPathMap.TryGetValue(type, out var prefabPath))
            {
                Debug.LogError($"[ScreenGenerator]Screenに対応するパスが存在しません Type:{type}");
                return null;
            }

            var prefab = Resources.Load<GameObject>(prefabPath);
            if (prefab == null)
            {
                Debug.LogError($"[ScreenGenerator]Screenに対応するプレハブがResourcesフォルダに存在しません Path:{prefabPath}");
                return null;
            }

            var screenObject = Object.Instantiate(prefab);
            if (!screenObject.TryGetComponent(type, out var screen))
            {
                Debug.LogError($"[ScreenGenerator]Screenに対応するコンポーネントがアタッチされていません:{type}");
                return null;
            }

            return screen as IScreen;
        }

        /// <summary>
        /// Typeに対応するScreenを生成する
        /// </summary>
        public static T Generate<T>() where T : IScreen
        {
            return (T)Generate(typeof(T));
        }
    }
}