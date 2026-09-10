using System;
using TypeReferences;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Sabanishi.MebuMekaFarm.Sound
{
    /// <summary>
    /// SoundEnumCreatorで表示するためのScriptableObject <br />
    /// エディタ拡張のためだけに使われており、ゲーム中は使用されない
    /// </summary>
    public class DatabaseCreator : ScriptableObject
    {
#if UNITY_EDITOR
        [Inherits(typeof(SoundDatabase<>))]
        [SerializeField] private TypeReference type;
        [SerializeField] private string assetName="NewDatabase";
        [SerializeField] private DefaultAsset savePathDirectory;

        public Type Type => type.Type;
        public string AssetName => assetName;
        public DefaultAsset SavePathDirectory => savePathDirectory;
#endif
    }
}