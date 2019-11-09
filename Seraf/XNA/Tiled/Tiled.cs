using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NSECS.Tiled
{
    public class Map
    {
        public enum MapRenderOrders { None, Right_Down }
        public enum MapOrientations { None, Orthogonal }
        public MapOrientations MapOrientation { get; set; }
        public MapRenderOrders MapRenderOrder { get; set; }

        // Map usually looks like:
        //<map version="1.2" tiledversion="1.2.5" orientation="orthogonal" renderorder="right-down" 
        // width="20" height="20" tilewidth="8" tileheight="8" infinite="0" nextlayerid="4" nextobjectid="2">
        // ..... map data
        //</map>

        public string mapVersion, tiledVersion;

        public int mapWidth, mapHeight, tileWidth, tileHeight;
        public int infinite; // bool?

        /// <summary>
        /// Stores the next available ID for new layers. 
        /// <para>This number is stored to prevent reuse of the same ID after layers have been removed. (since 1.2)</para>
        /// </summary>
        public int nextlayerid;
        /// <summary>
        /// nextobjectid: Stores the next available ID for new objects.
        /// <para>This number is stored to prevent reuse of the same ID after objects have been removed. (since 0.11)</para>
        /// </summary>
        public int nextobjectid;


        public Color backgroundColor;


        public List<TObjectGroup> objectGroups;
        public List<TileLayer> tileLayers;
        public List<TileSet> tileSets;
        public List<ImageLayer> imageLayers;

        string folderPath;

        /// <summary>
        /// Creates a new map instance at target folder.
        /// </summary>
        /// <param name="folderPath">The path to the folder.</param>
        public Map(string folderPath)
        {
            this.folderPath = folderPath;
        }

        public void Load(string fileName)
        {
            Console.WriteLine("### Loading Map ###");

            var xml = new XmlDocument();
            xml.Load(string.Concat("Content/", folderPath, fileName));


            #region Map attributes
            this.mapVersion = xml.DocumentElement.GetAttribute("version");
            this.tiledVersion = xml.DocumentElement.GetAttribute("tiledversion");
            this.MapOrientation = MapOrientations.Orthogonal;
            this.MapRenderOrder = MapRenderOrders.Right_Down;
            this.mapWidth = int.Parse(xml.DocumentElement.GetAttribute("width"));
            this.mapHeight = int.Parse(xml.DocumentElement.GetAttribute("height"));
            this.tileWidth = int.Parse(xml.DocumentElement.GetAttribute("tilewidth"));
            this.tileHeight = int.Parse(xml.DocumentElement.GetAttribute("tileheight"));
            this.infinite = int.Parse(xml.DocumentElement.GetAttribute("infinite"));

            if (xml.DocumentElement.HasAttribute("backgroundcolor"))
            {
                string color = xml.DocumentElement.GetAttribute("backgroundcolor");

                // For some reason we have to store the concat in a string before passing it as a parameter in the hex parse function.
                string str_r = string.Concat(color[1].ToString() + color[2].ToString());
                string str_g = string.Concat(color[3].ToString() + color[4].ToString());
                string str_b = string.Concat(color[5].ToString() + color[6].ToString());
                int r = int.Parse(str_r, System.Globalization.NumberStyles.HexNumber);
                int g = int.Parse(str_g, System.Globalization.NumberStyles.HexNumber);
                int b = int.Parse(str_b, System.Globalization.NumberStyles.HexNumber);

                this.backgroundColor = new Color(r, g, b);
            }
            else
                this.backgroundColor = Color.Black;

            this.nextlayerid = int.Parse(xml.DocumentElement.GetAttribute("nextlayerid"));
            this.nextobjectid = int.Parse(xml.DocumentElement.GetAttribute("nextobjectid"));
            #endregion


            #region TileSets
            this.tileSets = new List<TileSet>();
            foreach (XmlElement tileSet_inXml in xml.DocumentElement.GetElementsByTagName("tileset"))
            {
                var tsx = new XmlDocument();
                int firstgid = int.Parse(tileSet_inXml.GetAttribute("firstgid"));
                string source = tileSet_inXml.GetAttribute("source");
                tsx.Load(string.Concat("Content/", folderPath, source));

                // Inside the .tsx file (Tileset file format)
                TileSet tset = new TileSet(source, firstgid,
                    tsx.DocumentElement.GetAttribute("name"),
                    int.Parse(tsx.DocumentElement.GetAttribute("tilewidth")),
                    int.Parse(tsx.DocumentElement.GetAttribute("tileheight")),
                    int.Parse(tsx.DocumentElement.GetAttribute("tilecount")),
                    int.Parse(tsx.DocumentElement.GetAttribute("columns")));

                //  THIS WILL CAUSE A BUG IN THE FUTURE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                foreach (XmlElement img in tsx.DocumentElement.GetElementsByTagName("image"))
                {
                    string s = img.GetAttribute("source");
                    s = s.Remove(s.Length - 4, 4); // ??????????????????????????????????????
                    var tex = Game2.Game1.ContentPipeline.Load<Texture2D>(folderPath + s);
                    tset.SetImage(new Image(tex, img.GetAttribute("source")));
                }

                int c = 0;
                foreach (XmlElement tile in tsx.DocumentElement.GetElementsByTagName("tile"))
                {
                    List<TileSet.TileCollision> colls = new List<TileSet.TileCollision>();

                    foreach (XmlElement groups in tile.GetElementsByTagName("objectgroup"))
                    {
                        // objects inside group
                        // one object is one collision rect for that tile.
                        foreach (XmlElement o in groups.GetElementsByTagName("object"))
                        {
                            int id = int.Parse(o.GetAttribute("id"));
                            int x = int.Parse(o.GetAttribute("x"));
                            int y = int.Parse(o.GetAttribute("y"));
                            int w = int.Parse(o.GetAttribute("width"));
                            int h = int.Parse(o.GetAttribute("height"));

                            colls.Add(new TileSet.TileCollision(id, new Rectangle(x, y, w, h)));
                        }

                        // Before iterating next tile, set tiles collisions to local scope list
                        tset.tile_collisions[c] = colls;
                    }
                    ++c;
                }

                this.tileSets.Add(tset);
            }
            #endregion


            #region Image Layers
            /* <imagelayer id="7" name="Image Layer 1" offsetx="-64" offsety="-8">
               <image source="../../../../../../Pictures/logo_300px_300dpi.bmp" width="300" height="142"/>
               </imagelayer>
            */
            this.imageLayers = new List<ImageLayer>();
            foreach (XmlElement imagelayer in xml.DocumentElement.GetElementsByTagName("imagelayer"))
            {
                //<imagelayer id="9" name="Image Layer 1" offsetx="-72" offsety="-16">
                //<image source="../sprites/logo_300px_300dpi.png" width="300" height="142"/>
                //</imagelayer>

                ImageLayer imglayer;
                int id = int.Parse(imagelayer.GetAttribute("id"));
                string name = imagelayer.GetAttribute("name");
                Vector2 offset = new Vector2(float.Parse(imagelayer.GetAttribute("offsetx")), float.Parse(imagelayer.GetAttribute("offsety")));

                Image img = null;
                // Loop because maybe more images in the future? Why is it called ImageLayer?
                foreach (XmlElement img_inXml in imagelayer.GetElementsByTagName("image"))
                {
                    string source = img_inXml.GetAttribute("source");
                    //source = string.Concat(folderPath + source);
                    //string source = "sprites/elab";

                    img = new Image(Game2.Game1.ContentPipeline.SafeLoad<Texture2D>(source), source);
                }

                if (imagelayer.HasAttribute("opacity"))
                    imglayer = new ImageLayer(id, img, offset, float.Parse(imagelayer.GetAttribute("opacity")));
                else imglayer = new ImageLayer(id, img, offset, 1.0f);

                this.imageLayers.Add(imglayer);

            }
            #endregion


            #region Tile Layers
            tileLayers = new List<TileLayer>();

            char[] char_sep = new char[] { ',' };
            foreach (XmlElement layers_inXml in xml.DocumentElement.GetElementsByTagName("layer"))
            {
                int id = int.Parse(layers_inXml.GetAttribute("id"));
                string name = layers_inXml.GetAttribute("name");
                int width = int.Parse(layers_inXml.GetAttribute("width"));
                int height = int.Parse(layers_inXml.GetAttribute("height"));

                foreach (XmlElement layer_data in layers_inXml.GetElementsByTagName("data"))
                {
                    string encoding = layer_data.GetAttribute("encoding");
                    TileLayer tileLayer;

                    if (layer_data.HasAttribute("opacity"))
                    {
                        tileLayer = new TileLayer(id, name, encoding, width, height, float.Parse(layer_data.GetAttribute("opacity")));
                    }
                    else tileLayer = new TileLayer(id, name, encoding, width, height);

                    string[] str = layer_data.InnerXml.Split(char_sep, width * height);
                    Tile[,] tiles = new Tile[width, height];

                    for (int row = 0; row < height; ++row)
                    {
                        for (int col = 0; col < width; ++col)
                        {
                            // DON'T TOUCH!!!!!!!!!!!!!!!!! THIS TOOK ME TWO HOURS
                            var rect = new Rectangle(tileWidth * col, tileHeight * row, tileWidth, tileHeight);
                            int tileid = int.Parse(str[(row * width) + col]); // Get ID directly from the string. ID is based of the tile set it belongs to.
                            tiles[row, col] = new Tile(tileid, rect);
                            //Console.Write(tileid);
                        }
                        //Console.WriteLine();
                    }

                    tileLayer.tiles = tiles;
                    this.tileLayers.Add(tileLayer);
                }


            }
            #endregion


            #region Object groups
            this.objectGroups = new List<TObjectGroup>();

            foreach (XmlElement ogroups in xml.DocumentElement.GetElementsByTagName("objectgroup"))
            {
                TObjectGroup objectGroup = new TObjectGroup(int.Parse(ogroups.GetAttribute("id")), ogroups.GetAttribute("name"));

                foreach (XmlElement tobjects_inXml in ogroups.GetElementsByTagName("object"))
                {
                    int id = int.Parse(tobjects_inXml.GetAttribute("id"));
                    string name = tobjects_inXml.GetAttribute("name");
                    string type = tobjects_inXml.GetAttribute("type");
                    Vector2 pos = new Vector2(float.Parse(tobjects_inXml.GetAttribute("x")), float.Parse(tobjects_inXml.GetAttribute("y")));
    
                    Vector2 size = Vector2.Zero;
                    if (tobjects_inXml.HasAttribute("width") && tobjects_inXml.HasAttribute("height"))
                    {
                        size = new Vector2(float.Parse(tobjects_inXml.GetAttribute("width")), float.Parse(tobjects_inXml.GetAttribute("height")));
                    }


                    Dictionary<string, string> props = new Dictionary<string, string>();
                    foreach (XmlElement props_tobject in tobjects_inXml.GetElementsByTagName("properties"))
                    {
                        foreach (XmlElement prop_inProps in props_tobject.GetElementsByTagName("property"))
                        {
                            props.Add(prop_inProps.GetAttribute("name"), prop_inProps.GetAttribute("value"));
                        }
                    }

                    objectGroup.objects.Add(new TObject(id, name, type, pos, size, props));
                }

                this.objectGroups.Add(objectGroup);
            }
            #endregion


            Console.WriteLine("### Done. ###");
        }

        public void Unload() { }
    }


    public class TObjectGroup
    {
        public int id;
        public string name;

        public List<TObject> objects;

        public TObjectGroup(int id, string name)
        {
            this.id = id;
            this.name = name;
            this.objects = new List<TObject>();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Concat("[Group] id:", id, ", name:", name, ", count:", objects.Count));
            foreach (var o in objects)
            {
                sb.AppendLine(string.Concat("in group: {\n", o.ToString(), "}"));
            }

            return sb.ToString();
        }
    }

    public class TObject
    {
        public int id;
        public string name, type;
        public Vector2 pos, size;
        public Dictionary<string, string> properties;

        public TObject(int id, string name, string type, Vector2 pos, Vector2 size, Dictionary<string, string> properties)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.pos = pos;
            this.size = size;
            this.properties = properties;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Concat("id: ", id, ", name:", name, ", type:", type, ". pos:", pos, ", size", size));

            sb.AppendLine("Properties:");
            foreach (var prop in properties)
                sb.AppendLine(string.Concat("key: ", prop.Key, ", value:", prop.Value));

            return sb.ToString();
        }
    }


    public struct Tile
    {
        public int id;

        /// <summary>
        /// Contains position and also used for rendering.
        /// </summary>
        public Rectangle rect;

        public Tile(int id, Rectangle rect)
        {
            this.id = id;
            this.rect = rect;
        }
    }

    public class TileLayer
    {
        // A tile layer usually looks like:
        //<layer id = "3" name="Tile Layer 1" width="20" height="20">
        //<data encoding = "csv" >
        // tile data ..........................
        //</data>
        //</layer>

        public int id;
        public string name;
        public string encoding = "csv"; // maybe more support for others in the future?
        public int width, height;

        float opacity; // Use property

        public Tile[,] tiles;

        public TileLayer(int id, string name, string encoding, int width, int height)
        {
            this.id = id;
            this.name = name;
            this.encoding = encoding;
            this.width = width;
            this.height = height;
            this.opacity = 1.0f;
        }

        public TileLayer(int id, string name, string encoding, int width, int height, float opacity)
        {
            this.id = id;
            this.name = name;
            this.encoding = encoding;
            this.width = width;
            this.height = height;
            this.Opacity = opacity;
        }

        public float Opacity
        {
            get => opacity;
            set
            {
                opacity = value;
                if (opacity < 0)
                    opacity = 0f;
                if (opacity > 1f)
                    opacity = 1f;
            }
        }
    }

    public class Image
    {
        public Texture2D texture;
        public string image_source;
        public int image_width => texture.Width;
        public int image_height => texture.Height;

        public Image(Texture2D texture, string image_source)
        {
            this.texture = texture;
            this.image_source = image_source;
        }
    }

    public class ImageLayer
    {
        public int id;
        public Image image;
        public float opacity;
        public Vector2 offset;

        public ImageLayer(int id, Image image, Vector2 offset, float opacity)
        {
            this.id = id;
            this.image = image;
            this.offset = offset;
            this.opacity = opacity;
        }

    }

    public class TileSet
    {
        /* A tileset usually looks like:
        <tileset version="1.2" tiledversion="1.2.5" name="tes_overworld" tilewidth="8" tileheight="8" tilecount="16" columns="4">
        <image source="tes_overworld.png" width="32" height="32"/>
        <tile id="0">
        <objectgroup draworder="index">
        <object id="1" x="0" y="0" width="8" height="8"/>
        </objectgroup>
        </tile>
        </tileset>
        */

        public string name;

        public string source;
        public int firstgid; // First grid id. id 1 == 0, so offset by 1.

        public int tileWidth;
        public int tileHeight;
        public int tileCount;
        public int columns;

        public Image image;

        public struct TileCollision
        {
            //<objectgroup draworder="index">
            //<object id="1" x="0" y="0" width="8" height="4"/>
            //</objectgroup>

            public int id; // usually same as index
            public Rectangle bounds;

            public TileCollision(int id, Rectangle bounds)
            {
                this.id = id;
                this.bounds = bounds;
            }

            public override string ToString() => string.Concat("id:", id, ", bounds:", bounds);
        }

        /// <summary>
        /// The collisions of this tileset.
        /// </summary>
        public List<TileCollision>[] tile_collisions;

        public List<Rectangle> tile_clips;

        public TileSet(string source, int firstgid, string name, int tileWidth, int tileHeight, int tileCount, int columns)
        {
            this.source = source;
            this.firstgid = firstgid;
            this.name = name;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            this.tileCount = tileCount;
            this.columns = columns;
            this.tile_collisions = new List<TileCollision>[tileCount];


            #region Tile clips
            this.tile_clips = new List<Rectangle>();

            int sq = (int)Math.Sqrt(tileCount);  // TO GET THE WIDTH AND SIZE CUZ ITS ALL SQUARED ANYWAYS
            // START WITH CANCEROUS FIRST LINE CUZ IT FUCKS THE INDEXES
            // DONT FUCKING TOUCH THIS
            for (int i = 0; i < sq; ++i)
            {
                this.tile_clips.Add(new Rectangle(tileWidth * i, 0, tileWidth, tileHeight));
            }
            // THE REST OF THE LINES CUZ IT WORKS
            for (int row = 1; row < sq; ++row)
            {
                for (int col = 0; col < sq; ++col)
                {
                    var rect = new Rectangle(tileWidth * col, tileHeight * row, tileWidth, tileHeight);
                    this.tile_clips.Add(rect);
                }
            }

            #endregion
        }

        public void SetImage(Image image)
        {
            this.image = image;
        }

        /// <summary>
        /// Checks if a Tile's ID is a part of this tileset. 
        /// </summary>
        public bool IsTileIDPartOfSet(int tileid)
        {
            //if (tileid == 0 && this.firstgid == 1)
            //    return true;

            return (tileid >= (this.firstgid - 1) && (tileid < this.firstgid + this.tileCount));
        }


        public override string ToString()
        {
            return string.Concat(
                "name:", name,
                ", source", source,
                ", firstgid:", firstgid,
                ", tileWidth:", tileWidth,
                ", tileHeight:", tileHeight,
                ", tileCount:", tileCount,
                ", columns:", columns,
                ", tileCollisions:", tile_collisions.Length);
        }
    }
}
