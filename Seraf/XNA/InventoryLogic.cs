namespace Seraf.XNA
{
    //public struct Item
    //{
    //    public int stackSize;
    //    public int itemId;

    //    public Item(int itemId, int stackSize)
    //    {
    //        this.itemId = itemId;
    //        this.stackSize = stackSize;
    //    }
    //}

    //public enum ENUM_ITEMS
    //{
    //    None = 0,
    //    Coin = 1,
    //    Bomb = 9
    //}

    //public class InventoryTest
    //{
    //    InventoryLogic inventoryLogic;
    //    Item[] items;

    //    public InventoryTest()
    //    {
    //        inventoryLogic = new InventoryLogic(20);
    //        for (int i = 0; i < 20; ++i)
    //            items[i] = new Item(i, 0);
    //    }


    //    public ENUM_ITEMS GetSlotValue(int id)
    //    {
    //        int val = inventoryLogic.GetSlotValue(id);

    //        var arr = System.Enum.GetValues(typeof(ENUM_ITEMS));

    //    }
    //}

    public class InventoryLogic
    {
        readonly int slots_n;
        readonly int[] slots_arr;

        public InventoryLogic(int slots_n)
        {
            this.slots_n = slots_n;
            this.slots_arr = new int[slots_n];
        }

        /// <summary>
        /// Is id (index) out of bounds?? Good for crash handling.
        /// </summary>
        public bool IsIdOutOfIndex(int id) => (id >= slots_n || id < 0); // (id < 0) because int is signed.

        /// <summary>
        /// Gets the next available slot. Returns -1 if none are available.
        /// </summary>
        public int GetAvailable()
        {
            for(int i = 0; i < slots_n; ++i)
            {
                if (slots_arr[i] == 0)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Clear all slots, setting their values to 0.
        /// </summary>
        public void ClearAll()
        {
            for (int i = 0; i < slots_n; ++i)
                slots_arr[i] = 0;
        }

        /// <summary>
        /// Swap the value of two slots.
        /// </summary>
        public void SwapSlots(int id1, int id2)
        {
            int kept = slots_arr[id1]; // Keep 1
            slots_arr[id1] = slots_arr[id2]; // Set 1 to 2
            slots_arr[id2] = kept; // Set 2 to kept 1
        }

        /// <summary>
        /// Set slot value. Will override.
        /// </summary>
        public void SetSlotValue(int id, int value)
        {
            slots_arr[id] = value;
        }

        /// <summary>
        /// Get value of slot. 0 means empty.
        /// </summary>
        public int GetSlotValue(int id)
        {
            return slots_arr[id];
        }

        /// <summary>
        /// Get whether slot is free or not.
        /// </summary>
        public bool IsSlotFree(int id)
        {
            return (slots_arr[id] == 0); // Returns true if contained int is 0, which means empty.
        }
    }
}
