using Microsoft.Xna.Framework.Content;
using System.Text;

namespace Seraf.XNA
{
    /// <summary>
    /// Singleton instance of a Content pipeline that loads content.
    /// </summary>
    public class ContentPipeline
    {
        /// <summary>
        /// Instance to the ContentManager which is used for loading.
        /// </summary>
        ContentManager content;

        const string CONTENT_PATH = "Content/";

        public bool Debug { get; set; }
        public static ContentPipeline Instance { get; private set; }

        ContentPipeline(ContentManager content)
        {
            this.content = content;
        }
        public static void CreateInstance(ContentManager content) => Instance = new ContentPipeline(content);
        
        

        public T Load<T>(string asset)
        {
            if(Debug)
                System.Console.WriteLine(asset);
            return content.Load<T>(asset);
        }

        /// <summary>
        /// Checks if Content directory is mentioned, otherwise fixes it for you. Also fixes problems with file extensions.
        /// </summary>
        public T SafeLoad<T>(string asset)
        {
            if(!asset.StartsWith("Content/"))
            {
                var sb = new StringBuilder(); // Uses builder to avoid creating new strings
                sb.Append("Content/");

                // Check if asset ends with a file extension which in some cases causes trouble.
                if (asset[asset.Length - 4].Equals('.'))// 4 is length of ".foo" or whatever. 
                    sb.Append(asset.Remove(asset.IndexOf(".png"), 4)); 
                else
                    sb.Append(asset);

                return content.Load<T>(sb.ToString());
            }

            return content.Load<T>(asset);
        }


        //public SpriteAnim LoadAnim(string asset)
        //{
        //    var doc = new XmlDocument();
        //    doc.Load(string.Concat(CONTENT_PATH, asset));

        //    string name = doc.DocumentElement.GetAttribute("name");
        //    string tex_name = doc.DocumentElement.GetAttribute("texture");
        //    Texture2D tex = this.Load<Texture2D>(tex_name);

        //    var sprites = new List<Sprite>();
        //    foreach(XmlElement sprite in doc.DocumentElement.GetElementsByTagName("sprite"))
        //    {
        //        Rectangle clip = new Rectangle(
        //            int.Parse(sprite.GetAttribute("x")),
        //            int.Parse(sprite.GetAttribute("y")),
        //            int.Parse(sprite.GetAttribute("width")),
        //            int.Parse(sprite.GetAttribute("height")));

        //        SpriteData spriteData = new SpriteData

        //        //sprites.Add(new Sprite(tex, clip));
        //    }
        //    return null;
        //}
    }
}
