using System.Collections.Generic;

namespace Seraf.XNA.NSECS
{
    public class UUID
    {
        int uuid_size = 1000;
        Dictionary<int, bool> uuid_dict;

        public UUID()
        {
            uuid_dict = new Dictionary<int, bool>();
            for (int i = 0; i < uuid_size; ++i)
                uuid_dict.Add(i, false);
        }

        /// <summary>
        /// Get next free UUID.
        /// </summary>
        public int GetFreeUUID()
        {
            for (int i = 0; i < uuid_size; ++i)
                if (uuid_dict[i] == false)
                    return i;
            // Should never happen :^)
            throw new System.Exception("No free UUID's left!");
        }

        /// <summary>
        /// Check if UUID is free. Throws exception if taken.
        /// </summary>
        public int GetUUID(int uuid)
        {
            // If false, UUID is free. (Because start value of bool is false!).
            if (!uuid_dict[uuid])
            {
                uuid_dict[uuid] = true; // Set it to true in order to take it.
                return uuid;
            }

            // Should never happen! User's fault! :^) 
            throw new System.Exception("UUID is already taken!");
        }

        public void FreeUUID(int uuid)
        {
            if (uuid_dict[uuid])
                uuid_dict[uuid] = false; // Set it to false to free it.
            else
            {
                // Should never happen! User's fault! :^) 
                throw new System.Exception("UUID is already free!");
            }
        }
    }
}
