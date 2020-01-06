using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Xml;

namespace Seraf.XNA.NSECS.Components
{
    [ComponentBlueprint("inventory")]
    public class Inventory : Component
    {
        List<Collectible> collectibles = new List<Collectible>();

        public string[] slots;
        public int slots_c = 20;

        public Inventory(Entity entity) : base(entity)
        {
            slots = new string[slots_c];

            for (int i = 0; i < 20; ++i)
                slots[i] = string.Empty;
        }

        public override void Initialize(XmlElement e)
        {
        }


        public void AddToInventory(Collectible collectible)
        {
            collectibles.Add(collectible);
            collectible.Entity.Expire();
            //collectible.Expire();

            for (int i = 0; i < 20; ++i)
            {
                if (slots[i].Equals(string.Empty))
                {
                    slots[i] = collectible.ItemID;
                }
            }

            System.Console.WriteLine("Collected! " + collectible.ToString());
        }

        public override void Render(Scene scene)
        {
        }
    }
}
