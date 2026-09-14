namespace Sabanishi.MebuMekaFarm.Stage
{
    /// <summary>
    /// マップ上のEntityの種類を識別するための列挙型
    /// </summary>
    public enum ChipType
    {
        None = 0,
        Mebuki,
        Mebuka,
        Goal,
        Ground,
        PlainEntity,
        DeliveryEntity,
        DamageEntity,
        JumpEntity,
        WarpEntity,
    }

    public static class ChipTypeExtensions
    {
        public static ChipType ToChipType(this string chipName)
        {
            switch (chipName)
            {
                case "Mebuki":
                    return ChipType.Mebuki;
                case "Mebuka":
                    return ChipType.Mebuka;
                case "GoalEntity":
                    return ChipType.Goal;
                case "Ground":
                    return ChipType.Ground;
                case "PlainEntity":
                    return ChipType.PlainEntity;
                case "DeliveryEntity":
                    return ChipType.DeliveryEntity;
                case "DamageEntity":
                    return ChipType.DamageEntity;
                case "JumpEntity":
                    return ChipType.JumpEntity;
                case "WarpEntity":
                    return ChipType.WarpEntity;
                default:
                    return ChipType.None;
            }
        }
    }
}