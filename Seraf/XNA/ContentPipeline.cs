using Microsoft.Xna.Framework.Content;
using System.Text;

namespace Seraf.XNA
{
    /// <summary>
    /// (Singleton) Content pipeline that loads content.
    /// </summary>
    public class ContentPipeline
    {
        ContentManager content;
        public static ContentPipeline Instance { get; private set; }


        ContentPipeline(ContentManager content)
        {
            this.content = content;
        }

        public static void CreateInstance(ContentManager content)
        {
            Instance = new ContentPipeline(content);
        }
        

        public T Load<T>(string asset)
        {
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


    }
}
