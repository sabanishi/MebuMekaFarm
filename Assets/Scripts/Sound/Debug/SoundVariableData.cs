namespace Sabanishi.MebuMekaFarm.Sound
{
    /// <summary>
    /// Audioに関する雑多な変数の値を監視するための構造体
    /// </summary>
    public class VariableData
    {
        public string name;
        public string value;
        
        public VariableData() { }

        public void Set(string name, string value)
        {
            this.name = name;
            this.value = value;
        }
    }
}