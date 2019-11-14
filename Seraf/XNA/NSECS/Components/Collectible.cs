using System.Collections.Generic;
using System.Xml;

namespace Seraf.XNA.NSECS.Components
{
    [ComponentBlueprint("inventory")]
    public class Inventory : Component
    {
        public Inventory(Entity entity) : base(entity)
        {
        }

        public override void Initialize(XmlElement e)
        {
        }

        List<Collectible> collectibles = new List<Collectible>();
        public void AddToInventory(Collectible collectible)
        {
            collectibles.Add(collectible);
            collectible.Entity.Expire();
            //collectible.Expire();

            System.Console.WriteLine("Collected! " + collectible.ToString());
        }
    }

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
