using System.Xml;

namespace Seraf.XNA.NSECS.Components
{
    [ComponentBlueprint("collectible")]
    public class Collectible : Component
    {
        public Collectible(Entity entity) : base(entity)
        {
        }

        public string ItemID { get; set; }
        public int ItemValue { get; set; }
        public string ItemText { get; set; }

        public override void Initialize(XmlElement e)
        {
            ItemID = e.GetAttribute("id");
            ItemValue = int.Parse(e.GetAttribute("value"));
            ItemText = e.GetAttribute("text");
        }

        public override string ToString()
        {
            return "ItemID:" + ItemID + ", ItemValue:" + ItemValue + ", ItemText:" + ItemText;
        }
    }
}
