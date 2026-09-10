namespace Sabanishi.MebuMekaFarm.Sound
{
    public interface ISoundDatabase
    {
        public void Setup();
        public void Cleanup();

        /// <summary>
        /// AudioClipの参照が外れているAudioDataのtag名を取得
        /// </summary>
        public string[] LookupReferenceMissingClipNames();
    }
}