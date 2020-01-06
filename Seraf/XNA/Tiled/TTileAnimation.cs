using System.Collections.Generic;
using System.Xml.Serialization;

namespace Seraf.XNA.Tiled
{
    [XmlType("animation")]
    public class TTileAnimation
    {
        public int t;
        public int current_frame;

        [XmlElement("frame")]
        public List<TTileAnimationFrame> frames;

        public TTileAnimation()
        {
            t = 0;
            current_frame = 0;
            frames = new List<TTileAnimationFrame>();
        }

        public void Update(float delta)
        {
            t += (int)(delta * 1000);
            if (t >= frames[current_frame].duration)
            {
                current_frame += 1;
                if (current_frame == frames.Count)
                    current_frame = 0;
            }
        }
    }
}
