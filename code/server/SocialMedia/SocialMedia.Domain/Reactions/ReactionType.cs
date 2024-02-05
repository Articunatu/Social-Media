namespace SocialMedia.Domain.Reactions
{
    public class ReactionType
    {
        public int Value { get; }
        public string Name { get; }
        public Guid IconId { get; }

        private ReactionType(string name, int value, Guid iconId)
        {
            Name = name;
            Value = value;
            IconId = iconId;
        }
        private ReactionType() { }

        public static ReactionType Plant => new ReactionType("Plant", 0, Icons.PlantId);
        public static ReactionType Fire => new ReactionType("Fire", 1, Icons.FireId);
        public static ReactionType Water => new ReactionType("Water", 2, Icons.WaterId);
        public static ReactionType Lightning => new ReactionType("Lightning", 3, Icons.LightningId);
        public static ReactionType Cloud => new ReactionType("Cloud", 4, Icons.CloudPId);
        public static ReactionType Stone => new ReactionType("Stone", 5, Icons.StoneId);
        public static ReactionType Chemical => new ReactionType("Chemical", 6, Icons.ChemicalId);
        public static ReactionType Void => new ReactionType("Void", 7, Icons.VoidId);
        public static ReactionType Energy => new ReactionType("Energy", 8, Icons.EnergyId);
        public static ReactionType Ice => new ReactionType("Ice", 9, Icons.IceId);
    }
}
