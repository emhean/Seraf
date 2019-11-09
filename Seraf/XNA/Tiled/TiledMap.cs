using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Seraf.XNA.Tiled
{
    // Map usually looks like:
    //<map version="1.2" tiledversion="1.2.5" orientation="orthogonal" renderorder="right-down" 
    // width="20" height="20" tilewidth="8" tileheight="8" infinite="0" nextlayerid="4" nextobjectid="2">
    // ..... map data
    //</map>
    public class TiledMap : ITiledProperties
    {
        public enum MapRenderOrders { None, Right_Down }
        public enum MapOrientations { None, Orthogonal }
        public MapOrientations MapOrientation { get; set; }
        public MapRenderOrders MapRenderOrder { get; set; }
        public TProperties Properties { get; }

        public string mapVersion;
        public string tiledVersion;
        readonly string folderPath;

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
        public List<TTileLayer> tileLayers;
        public List<TTileSet> tileSets;
        public List<TImageLayer> imageLayers;



        /// <summary>
        /// Creates a new map instance at target folder.
        /// </summary>
        /// <param name="folderPath">The path to the folder.</param>
        public TiledMap(string folderPath)
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
            this.tileSets = new List<TTileSet>();
            foreach (XmlElement tileSet_inXml in xml.DocumentElement.GetElementsByTagName("tileset"))
            {
                var tsx = new XmlDocument();
                int firstgid = int.Parse(tileSet_inXml.GetAttribute("firstgid"));
                string source = tileSet_inXml.GetAttribute("source");
                tsx.Load(string.Concat("Content/", folderPath, source));

                // Inside the .tsx file (Tileset file format)
                TTileSet tset = new TTileSet(source, firstgid,
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
                    var tex = ContentPipeline.Instance.Load<Texture2D>(folderPath + s);
                    tset.SetImage(new TImage(tex, img.GetAttribute("source")));
                }

                List<TTileData> tile_datas = new List<TTileData>();
                foreach (XmlElement tile in tsx.DocumentElement.GetElementsByTagName("tile"))
                {
                    TTileData tileData;

                    int tile_id = int.Parse(tile.GetAttribute("id"));
                    string tile_type = tile.GetAttribute("type");
                    TProperties tile_props = LoadProperties(tile);

                    // Tile Collision Data
                    List<Rectangle> bounds = new List<Rectangle>();
                    foreach (XmlElement groups in tile.GetElementsByTagName("objectgroup"))
                    {
                        foreach (XmlElement o in groups.GetElementsByTagName("object"))
                        {
                            // In the future: Add Property support 
                            int bounds_id = int.Parse(o.GetAttribute("id"));
                            int bounds_x = int.Parse(o.GetAttribute("x"));
                            int bounds_y = int.Parse(o.GetAttribute("y"));
                            int bounds_w = int.Parse(o.GetAttribute("width"));
                            int bounds_h = int.Parse(o.GetAttribute("height"));
                            bounds.Add(new Rectangle(bounds_x, bounds_y, bounds_w, bounds_h));
                        }
                    }

                    // Tile Animation data
                    List<TTileDataAnim> animations = new List<TTileDataAnim>();
                    foreach (XmlElement anim in tile.GetElementsByTagName("animation"))
                    {
                        // In the future: Add Property support 
                        foreach (XmlElement frame in anim.GetElementsByTagName("frame"))
                        {
                            animations.Add(new TTileDataAnim(
                                int.Parse(frame.GetAttribute("tileid")), 
                                int.Parse(frame.GetAttribute("ms"))));
                        }
                    }

                    tileData = new TTileData(tile_id, tile_type, bounds.ToArray(), null, animations.ToArray(), tile_props);
                }

                tset.CreateClips();
                this.tileSets.Add(tset);
                
            }
            #endregion


            #region Image Layers
            /* <imagelayer id="7" name="Image Layer 1" offsetx="-64" offsety="-8">
               <image source="../../../../../../Pictures/logo_300px_300dpi.bmp" width="300" height="142"/>
               </imagelayer>
            */
            this.imageLayers = new List<TImageLayer>();
            foreach (XmlElement imagelayer in xml.DocumentElement.GetElementsByTagName("imagelayer"))
            {
                //<imagelayer id="9" name="Image Layer 1" offsetx="-72" offsety="-16">
                //<image source="../sprites/logo_300px_300dpi.png" width="300" height="142"/>
                //</imagelayer>

                TImageLayer imglayer;
                int id = int.Parse(imagelayer.GetAttribute("id"));
                string name = imagelayer.GetAttribute("name");
                Vector2 offset = new Vector2(float.Parse(imagelayer.GetAttribute("offsetx")), float.Parse(imagelayer.GetAttribute("offsety")));

                TImage img = null;
                // Loop because maybe more images in the future? Why is it called ImageLayer?
                foreach (XmlElement img_inXml in imagelayer.GetElementsByTagName("image"))
                {
                    string source = img_inXml.GetAttribute("source");
                    //source = string.Concat(folderPath + source);
                    //string source = "sprites/elab";

                    img = new TImage(ContentPipeline.Instance.SafeLoad<Texture2D>(source), source);
                }

                if (imagelayer.HasAttribute("opacity"))
                    imglayer = new TImageLayer(id, img, offset, float.Parse(imagelayer.GetAttribute("opacity")));
                else imglayer = new TImageLayer(id, img, offset, 1.0f);

                this.imageLayers.Add(imglayer);

            }
            #endregion


            #region Tile Layers
            tileLayers = new List<TTileLayer>();

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
                    TTileLayer tileLayer;

                    if (layer_data.HasAttribute("opacity"))
                    {
                        tileLayer = new TTileLayer(id, name, encoding, width, height, float.Parse(layer_data.GetAttribute("opacity")));
                    }
                    else tileLayer = new TTileLayer(id, name, encoding, width, height);

                    string[] str = layer_data.InnerXml.Split(char_sep, width * height);
                    TTile[,] tiles = new TTile[width, height];

                    for (int row = 0; row < height; ++row)
                    {
                        for (int col = 0; col < width; ++col)
                        {
                            // DON'T TOUCH!!!!!!!!!!!!!!!!! THIS TOOK ME TWO HOURS
                            var rect = new Rectangle(tileWidth * col, tileHeight * row, tileWidth, tileHeight);
                            int tileid = int.Parse(str[(row * width) + col]); // Get ID directly from the string. ID is based of the tile set it belongs to.
                            tiles[row, col] = new TTile(tileid, rect);
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


                    TProperties props = LoadProperties(tobjects_inXml);

                    //foreach (XmlElement props_tobject in tobjects_inXml.GetElementsByTagName("properties"))
                    //{
                    //    foreach (XmlElement prop_inProps in props_tobject.GetElementsByTagName("property"))
                    //    {
                    //        props.AddProperty(prop_inProps.GetAttribute("name"), prop_inProps.GetAttribute("value"));
                    //    }
                    //}

                    objectGroup.objects.Add(new TObject(id, name, type, pos, size, props));
                }

                this.objectGroups.Add(objectGroup);
            }
            #endregion


            Console.WriteLine("### Done. ###");
        }

        private TProperties LoadProperties(XmlElement xmlElement)
        {
            TProperties tprops = new TProperties();

            foreach (XmlElement props in xmlElement.GetElementsByTagName("properties"))
            {
                foreach (XmlElement p in props.GetElementsByTagName("property"))
                {
                    tprops.AddProperty(props.GetAttribute("name"), props.GetAttribute("value"));
                }
            }

            return tprops;
        }

        public void Unload() { }
    }
}
