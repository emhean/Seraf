using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Seraf.XNA.Tiled
{
    [Serializable]
    [XmlRoot("map")]
    public class TTileMap : ITiledProperties
    {
        //public enum MapRenderOrders { None, Right_Down }
        //public enum MapOrientations { None, Orthogonal }
        //public MapOrientations MapOrientation { get; set; }
        //public MapRenderOrders MapRenderOrder { get; set; }
        //string GetMapOrientationString(MapOrientations mapOrientation)
        //{
        //    if (mapOrientation == MapOrientations.None)
        //        return "none";
        //    if (mapOrientation == MapOrientations.Orthogonal)
        //        return "orthogonal";

        //    throw new Exception("MapOrientation not found!");
        //}
        //string GetMapRenderOrderString(MapRenderOrders mapRenderOrder)
        //{
        //    if (mapRenderOrder == MapRenderOrders.None)
        //        return "none";
        //    if (mapRenderOrder == MapRenderOrders.Right_Down)
        //        return "right-down";

        //    throw new Exception("MapRenderOrder not found!");
        //}

        [XmlAttribute("orientation")]
        public string orientation;

        [XmlAttribute("renderorder")]
        public string renderorder;

        [XmlElement("property")]
        public TProperties Properties { get; set; }

        [XmlElement("")]
        public string mapVersion;

        [XmlAttribute("tiledversion")]
        public string tiledVersion;

        [XmlAttribute("width")]
        public int mapWidth;

        [XmlAttribute("height")]
        public int mapHeight;

        [XmlAttribute("tilewidth")]
        public int tileWidth;

        [XmlAttribute("tileheight")]
        public int tileHeight;

        [XmlAttribute("infinite")]
        public int infinite; // bool?

        [XmlAttribute("nextlayerid")]
        /// <summary>
        /// Stores the next available ID for new layers. 
        /// <para>This number is stored to prevent reuse of the same ID after layers have been removed. (since 1.2)</para>
        /// </summary>
        public int nextlayerid;

        [XmlAttribute("nextobjectid")]
        /// <summary>
        /// nextobjectid: Stores the next available ID for new objects.
        /// <para>This number is stored to prevent reuse of the same ID after objects have been removed. (since 0.11)</para>
        /// </summary>
        public int nextobjectid;


        public Color backgroundColor;

        [XmlElement("objectgroup")]
        public List<TObjectGroup> objectGroups;

        [XmlElement("layer")]
        public List<TTileLayer> tileLayers;

        [XmlElement("tileset")]
        public List<TTileSet> tileSets;

        [XmlElement("imagelayer")]
        public List<TImageLayer> imageLayers;


        /// <summary>
        /// Get the properties XML elements and return an instance containing the properties their values.
        /// </summary>
        private TProperties LoadProperties(XmlElement xmlElement)
        {
            TProperties tprops = new TProperties();
            //if (!xmlElement.GetElementsByTagName("properties"))
            //    return tprops;

            foreach (XmlElement props in xmlElement.GetElementsByTagName("properties"))
            {
                foreach (XmlElement p in props.GetElementsByTagName("property"))
                {
                    if (p.HasAttribute("type"))
                        tprops.Add(p.GetAttribute("name"), p.GetAttribute("value"), p.GetAttribute("type"));
                    else tprops.Add(p.GetAttribute("name"), p.GetAttribute("value"));
                }
            }

            return tprops;
        }

        private void AddAttribute(XmlDocument doc, XmlElement element, string attributeName, string attributeValue)
        {
            var att = doc.CreateAttribute(attributeName);
            att.Value = attributeValue;
            element.Attributes.Append(att);
        }

        //private XmlElement AddPropertiesToXmlObject(XmlDocument doc, ITiledProperties tiledProperties)
        //{
        //    if (tiledProperties.Properties != null && tiledProperties.Properties.GetPropertyCount() != 0)
        //    {
        //        XmlElement element_properties = doc.CreateElement("properties");

        //        foreach (var p in YieldProperties(doc, tiledProperties))
        //        {
        //            p.AppendChild(p);
        //        }

        //        return element_properties;
        //    }

        //    return null;
        //}

        ///// <summary>
        ///// Enumerates the properties, adds the properties attributes and yields the xml element.
        ///// </summary>
        //private IEnumerable<XmlElement> YieldProperties(XmlDocument doc, ITiledProperties tiledProperties)
        //{
        //    //if (tiledProperties.Properties == null || tiledProperties.Properties.GetPropertyCount() == 0)


        //    TProperty p;
        //    for (int i = 0; i < Properties.GetPropertyCount(); ++i)
        //    {
        //        p = Properties.GetProperty(i);
        //        sb.AppendLine(string.Concat("key: ", p.Name, ", value:", p.Value));
        //    }

        //    foreach (var p in tiledProperties.Properties)
        //    {
        //        XmlElement element_property = doc.CreateElement("property");
        //        AddAttribute(doc, element_property, "name", p.Name);
        //        AddAttribute(doc, element_property, "type", p.ValueType);
        //        AddAttribute(doc, element_property, "value", p.Value);
        //        yield return element_property;
        //    }
        //}


        #region Old code
        ///// <summary>
        ///// Loads a .tmx file.
        ///// </summary>
        //public void Load(string fileName)
        //{
        //    Console.WriteLine("### Loading Map ###");

        //    var doc = new XmlDocument();
        //    doc.Load(string.Concat("Content/", folderPath, fileName));


        //    #region Map attributes
        //    this.mapVersion = doc.DocumentElement.GetAttribute("version");
        //    this.tiledVersion = doc.DocumentElement.GetAttribute("tiledversion");
        //    this.MapOrientation = MapOrientations.Orthogonal;
        //    this.MapRenderOrder = MapRenderOrders.Right_Down;
        //    this.mapWidth = int.Parse(doc.DocumentElement.GetAttribute("width"));
        //    this.mapHeight = int.Parse(doc.DocumentElement.GetAttribute("height"));
        //    this.tileWidth = int.Parse(doc.DocumentElement.GetAttribute("tilewidth"));
        //    this.tileHeight = int.Parse(doc.DocumentElement.GetAttribute("tileheight"));
        //    this.infinite = int.Parse(doc.DocumentElement.GetAttribute("infinite"));

        //    if (doc.DocumentElement.HasAttribute("backgroundcolor"))
        //    {
        //        string color = doc.DocumentElement.GetAttribute("backgroundcolor");

        //        // For some reason we have to store the concat in a string before passing it as a parameter in the hex parse function.
        //        string str_r = string.Concat(color[1].ToString() + color[2].ToString());
        //        string str_g = string.Concat(color[3].ToString() + color[4].ToString());
        //        string str_b = string.Concat(color[5].ToString() + color[6].ToString());
        //        int r = int.Parse(str_r, System.Globalization.NumberStyles.HexNumber);
        //        int g = int.Parse(str_g, System.Globalization.NumberStyles.HexNumber);
        //        int b = int.Parse(str_b, System.Globalization.NumberStyles.HexNumber);

        //        this.backgroundColor = new Color(r, g, b);
        //    }
        //    else
        //        this.backgroundColor = Color.Black;

        //    this.nextlayerid = int.Parse(doc.DocumentElement.GetAttribute("nextlayerid"));
        //    this.nextobjectid = int.Parse(doc.DocumentElement.GetAttribute("nextobjectid"));
        //    #endregion


        //    #region TileSets
        //    this.tileSets = new List<TTileSet>();
        //    foreach (XmlElement tileSet_inXml in doc.DocumentElement.GetElementsByTagName("tileset"))
        //    {
        //        var tsx = new XmlDocument();
        //        int firstgid = int.Parse(tileSet_inXml.GetAttribute("firstgid"));
        //        string source = tileSet_inXml.GetAttribute("source");
        //        tsx.Load(string.Concat("Content/", folderPath, source));

        //        // Inside the .tsx file (Tileset file format)
        //        TTileSet tset = new TTileSet(source, firstgid,
        //            tsx.DocumentElement.GetAttribute("name"),
        //            int.Parse(tsx.DocumentElement.GetAttribute("tilewidth")),
        //            int.Parse(tsx.DocumentElement.GetAttribute("tileheight")),
        //            int.Parse(tsx.DocumentElement.GetAttribute("tilecount")),
        //            int.Parse(tsx.DocumentElement.GetAttribute("columns")));

        //        //  THIS WILL CAUSE A BUG IN THE FUTURE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //        foreach (XmlElement img in tsx.DocumentElement.GetElementsByTagName("image"))
        //        {
        //            string s = img.GetAttribute("source");
        //            s = s.Remove(s.Length - 4, 4); // ??????????????????????????????????????
        //            var tex = ContentPipeline.Instance.Load<Texture2D>(folderPath + s);
        //            tset.SetImage(new TImage(tex, img.GetAttribute("source")));
        //        }


        //        // Add Air
        //        tset.tile_data.Add(new TTileData(0, "air", new Rectangle[] { Rectangle.Empty }, new Rectangle[] { Rectangle.Empty }, null, null));

        //        foreach (XmlElement tile in tsx.DocumentElement.GetElementsByTagName("tile"))
        //        {
        //            TTileData tileData;

        //            int tile_id = int.Parse(tile.GetAttribute("id"));
        //            string tile_type = tile.GetAttribute("type");
        //            TProperties tile_props = LoadProperties(tile);

        //            // Tile Collision Data
        //            List<Rectangle> bounds = new List<Rectangle>();
        //            foreach (XmlElement groups in tile.GetElementsByTagName("objectgroup"))
        //            {
        //                foreach (XmlElement o in groups.GetElementsByTagName("object"))
        //                {
        //                    // In the future: Add Property support 
        //                    int bounds_id = int.Parse(o.GetAttribute("id"));
        //                    int bounds_x = int.Parse(o.GetAttribute("x"));
        //                    int bounds_y = int.Parse(o.GetAttribute("y"));
        //                    int bounds_w = int.Parse(o.GetAttribute("width"));
        //                    int bounds_h = int.Parse(o.GetAttribute("height"));
        //                    bounds.Add(new Rectangle(bounds_x, bounds_y, bounds_w, bounds_h));
        //                }
        //            }

        //            // Tile Animation data
        //            List<TAnimation> animations = new List<TAnimation>();
        //            foreach (XmlElement anim in tile.GetElementsByTagName("animation"))
        //            {
        //                // In the future: Add Property support 
        //                foreach (XmlElement frame in anim.GetElementsByTagName("frame"))
        //                {
        //                    animations.Add(new TAnimation(
        //                        int.Parse(frame.GetAttribute("tileid")),
        //                        int.Parse(frame.GetAttribute("duration"))));
        //                }
        //            }

        //            tileData = new TTileData(
        //                tile_id,
        //                tile_type,
        //                bounds.ToArray(),
        //                new Rectangle[] { Rectangle.Empty }, // We will use the CreateClips later to create this.
        //                animations.ToArray(),
        //                tile_props);

        //            tset.tile_data.Add(tileData);
        //        }

        //        tset.CreateClips();
        //        this.tileSets.Add(tset);

        //    }
        //    #endregion


        //    #region Image Layers
        //    /* <imagelayer id="7" name="Image Layer 1" offsetx="-64" offsety="-8">
        //       <image source="../../../../../../Pictures/logo_300px_300dpi.bmp" width="300" height="142"/>
        //       </imagelayer>
        //    */
        //    this.imageLayers = new List<TImageLayer>();
        //    foreach (XmlElement imagelayer in doc.DocumentElement.GetElementsByTagName("imagelayer"))
        //    {
        //        //<imagelayer id="9" name="Image Layer 1" offsetx="-72" offsety="-16">
        //        //<image source="../sprites/logo_300px_300dpi.png" width="300" height="142"/>
        //        //</imagelayer>

        //        TImageLayer imglayer;
        //        int id = int.Parse(imagelayer.GetAttribute("id"));
        //        string name = imagelayer.GetAttribute("name");
        //        Vector2 offset = new Vector2(float.Parse(imagelayer.GetAttribute("offsetx")), float.Parse(imagelayer.GetAttribute("offsety")));

        //        TImage img = null;
        //        // Loop because maybe more images in the future? Why is it called ImageLayer?
        //        foreach (XmlElement img_inXml in imagelayer.GetElementsByTagName("image"))
        //        {
        //            string source = img_inXml.GetAttribute("source");
        //            //source = string.Concat(folderPath + source);
        //            //string source = "sprites/elab";

        //            img = new TImage(ContentPipeline.Instance.SafeLoad<Texture2D>(source), source);
        //        }

        //        if (imagelayer.HasAttribute("opacity"))
        //            imglayer = new TImageLayer(id, name, img, offset, float.Parse(imagelayer.GetAttribute("opacity")));
        //        else imglayer = new TImageLayer(id, name, img, offset, 1.0f);

        //        this.imageLayers.Add(imglayer);

        //    }
        //    #endregion


        //    #region Tile Layers
        //    tileLayers = new List<TTileLayer>();

        //    char[] char_sep = new char[] { ',' };
        //    foreach (XmlElement layer_inXml in doc.DocumentElement.GetElementsByTagName("layer"))
        //    {
        //        TTileLayer tileLayer;

        //        int id = int.Parse(layer_inXml.GetAttribute("id"));
        //        string name = layer_inXml.GetAttribute("name");
        //        int width = int.Parse(layer_inXml.GetAttribute("width"));
        //        int height = int.Parse(layer_inXml.GetAttribute("height"));



        //        // Create the instance
        //        if (layer_inXml.HasAttribute("opacity"))
        //            tileLayer = new TTileLayer(id, name, "csv", width, height, float.Parse(layer_inXml.GetAttribute("opacity")));
        //        else
        //            tileLayer = new TTileLayer(id, name, "csv", width, height);

        //        // Get the properties
        //        var foo = LoadProperties(layer_inXml);
        //        tileLayer.Properties = foo;

        //        foreach (XmlElement layer_data in layer_inXml.GetElementsByTagName("data"))
        //        {
        //            string encoding = layer_data.GetAttribute("encoding");

        //            string[] str = layer_data.InnerXml.Split(char_sep, width * height);
        //            TTile[,] tiles = new TTile[width, height];

        //            for (int row = 0; row < height; ++row)
        //            {
        //                for (int col = 0; col < width; ++col)
        //                {
        //                    // DON'T TOUCH!!!!!!!!!!!!!!!!! THIS TOOK ME TWO HOURS
        //                    var rect = new Rectangle(tileWidth * col, tileHeight * row, tileWidth, tileHeight);
        //                    int tileid = int.Parse(str[(row * width) + col]); // Get ID directly from the string. ID is based of the tile set it belongs to.
        //                    tiles[row, col] = new TTile(tileid, rect);
        //                    //Console.Write(tileid);
        //                }
        //                //Console.WriteLine();
        //            }

        //            tileLayer.tiles = tiles;

        //            this.tileLayers.Add(tileLayer);
        //        }


        //    }
        //    #endregion


        //    #region Object groups
        //    this.objectGroups = new List<TObjectGroup>();

        //    foreach (XmlElement ogroups in doc.DocumentElement.GetElementsByTagName("objectgroup"))
        //    {
        //        TObjectGroup objectGroup = new TObjectGroup(int.Parse(ogroups.GetAttribute("id")), ogroups.GetAttribute("name"));

        //        foreach (XmlElement tobjects_inXml in ogroups.GetElementsByTagName("object"))
        //        {
        //            int id = int.Parse(tobjects_inXml.GetAttribute("id"));
        //            string name = tobjects_inXml.GetAttribute("name");
        //            string type = tobjects_inXml.GetAttribute("type");
        //            Vector2 pos = new Vector2(float.Parse(tobjects_inXml.GetAttribute("x")), float.Parse(tobjects_inXml.GetAttribute("y")));

        //            Vector2 size = Vector2.Zero;
        //            if (tobjects_inXml.HasAttribute("width") && tobjects_inXml.HasAttribute("height"))
        //            {
        //                size = new Vector2(float.Parse(tobjects_inXml.GetAttribute("width")), float.Parse(tobjects_inXml.GetAttribute("height")));
        //            }


        //            TProperties props = LoadProperties(tobjects_inXml);

        //            //foreach (XmlElement props_tobject in tobjects_inXml.GetElementsByTagName("properties"))
        //            //{
        //            //    foreach (XmlElement prop_inProps in props_tobject.GetElementsByTagName("property"))
        //            //    {
        //            //        props.AddProperty(prop_inProps.GetAttribute("name"), prop_inProps.GetAttribute("value"));
        //            //    }
        //            //}

        //            objectGroup.objects.Add(new TObject(id, name, type, pos, size, props));
        //        }

        //        this.objectGroups.Add(objectGroup);
        //    }
        //    #endregion


        //    Console.WriteLine("### Done. ###");
        //}
        ///// <summary>
        ///// Saves the map.
        ///// </summary>
        //public void Save(string fileName)
        //{
        //    Console.WriteLine("### Saving Map ###");

        //    XmlDocument doc = new XmlDocument();
        //    doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", null));

        //    // This element is the root element of the whole document.
        //    XmlElement element_map = doc.CreateElement("map");


        //    #region Map Attributes
        //    var att_version = doc.CreateAttribute("version");
        //    att_version.Value = this.mapVersion;
        //    var att_tiledVersion = doc.CreateAttribute("tiledversion");
        //    att_tiledVersion.Value = this.tiledVersion;
        //    var att_orientation = doc.CreateAttribute("orientation");
        //    att_orientation.Value = this.GetMapOrientationString(this.MapOrientation);
        //    var att_renderorder = doc.CreateAttribute("renderorder");
        //    att_renderorder.Value = this.GetMapRenderOrderString(this.MapRenderOrder);
        //    var att_mapWidth = doc.CreateAttribute("width");
        //    att_mapWidth.Value = this.mapWidth.ToString();
        //    var att_mapHeight = doc.CreateAttribute("height");
        //    att_mapHeight.Value = this.mapHeight.ToString();
        //    var att_tileWidth = doc.CreateAttribute("tilewidth");
        //    att_tileWidth.Value = this.tileWidth.ToString();
        //    var att_tileHeight = doc.CreateAttribute("tileheight");
        //    att_tileHeight.Value = this.tileHeight.ToString();
        //    var att_infinite = doc.CreateAttribute("infinite");
        //    att_infinite.Value = this.infinite.ToString();
        //    var att_nextlayerid = doc.CreateAttribute("nextlayerid");
        //    att_nextlayerid.Value = this.nextlayerid.ToString();
        //    var att_nextobjectid = doc.CreateAttribute("nextobjectid");
        //    att_nextobjectid.Value = this.nextobjectid.ToString();
        //    element_map.Attributes.Append(att_version);
        //    element_map.Attributes.Append(att_tiledVersion);
        //    element_map.Attributes.Append(att_orientation);
        //    element_map.Attributes.Append(att_renderorder);
        //    element_map.Attributes.Append(att_mapWidth);
        //    element_map.Attributes.Append(att_mapHeight);
        //    element_map.Attributes.Append(att_tileWidth);
        //    element_map.Attributes.Append(att_tileHeight);
        //    element_map.Attributes.Append(att_infinite);
        //    element_map.Attributes.Append(att_nextlayerid);
        //    element_map.Attributes.Append(att_nextobjectid);

        //    // Add root element to document.
        //    doc.AppendChild(element_map); 
        //    #endregion


        //    #region TileSets
        //    foreach (var ts in this.tileSets)
        //    {
        //        XmlElement element_tileSet = doc.CreateElement("tileset");
        //        AddAttribute(doc, element_tileSet, "firstgid", ts.firstgid.ToString());
        //        AddAttribute(doc, element_tileSet, "source", ts.source);
        //        element_map.AppendChild(element_tileSet);
        //    }
        //    #endregion


        //    #region Image Layers
        //    foreach (var imgLayer in this.imageLayers)
        //    {
        //        XmlElement element_layer = doc.CreateElement("imagelayer");
        //        AddAttribute(doc, element_layer, "id", imgLayer.id.ToString());
        //        AddAttribute(doc, element_layer, "name", imgLayer.name.ToString());
        //        AddAttribute(doc, element_layer, "offsetx", imgLayer.offset.X.ToString());
        //        AddAttribute(doc, element_layer, "offsety", imgLayer.offset.Y.ToString());

        //        #region Image Element
        //        XmlElement element_image = doc.CreateElement("image");
        //        AddAttribute(doc, element_image, "source", imgLayer.image.image_source);
        //        AddAttribute(doc, element_image, "width", imgLayer.image.image_width.ToString());
        //        AddAttribute(doc, element_image, "height", imgLayer.image.image_height.ToString());
        //        element_layer.AppendChild(element_image); // Add image element to layer element
        //        #endregion

        //        // Add imagelayer element to document
        //        element_map.AppendChild(element_layer);
        //    }
        //    #endregion


        //    #region Tile Layers
        //    foreach(var tileLayer in this.tileLayers)
        //    {
        //        XmlElement element_layer = doc.CreateElement("layer");
        //        AddAttribute(doc, element_layer, "id", tileLayer.id.ToString());
        //        AddAttribute(doc, element_layer, "name", tileLayer.name);
        //        AddAttribute(doc, element_layer, "width", tileLayer.width.ToString());
        //        AddAttribute(doc, element_layer, "height", tileLayer.height.ToString());

        //        #region Properties
        //        XmlElement element_properties = doc.CreateElement("properties");
        //        foreach (var p in tileLayer.Properties)
        //        {
        //            XmlElement element_property = doc.CreateElement("property");
        //            AddAttribute(doc, element_property, "name", p.Name);
        //            AddAttribute(doc, element_property, "type", p.ValueType);
        //            AddAttribute(doc, element_property, "value", p.Value);
        //            element_properties.AppendChild(element_property);
        //        }
        //        element_layer.AppendChild(element_properties); // Add properties element to layer
        //        #endregion


        //        #region Data
        //        XmlElement element_data = doc.CreateElement("data");
        //        AddAttribute(doc, element_data, "encoding", tileLayer.encoding);
        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("\n"); // To make it more human readable, if read.
        //        int count = 0; // Lazy variable used for iteration count (tile count) in loop below.
        //        int current_row = 1; // Lazy variable for current row. Count is not zero-based like index.
        //        int rows = tileLayer.height; // Lazy variable used for row count in loop below.
        //        foreach(var tileid in tileLayer.tiles)
        //        {
        //            sb.Append(tileid.id);
        //            count += 1;

        //            if (count == tileLayer.width)
        //            {
        //                count = 0;

        //                if (current_row != rows)
        //                {
        //                    sb.Append(",");
        //                    current_row += 1;
        //                }

        //                sb.Append("\n");
        //            }
        //            else
        //            {
        //                sb.Append(",");
        //            }

        //            element_data.InnerText += sb.ToString();
        //            sb.Clear(); // Clear for use in next iteration
        //        }
        //        // UPDATE: Fixed problem in the loop above. 
        //        // This code fixes a crash in the Tiled Editor.
        //        // If the tile data ends with a "," then Tiled will complain about corrupt data.
        //        // DON'T TOUCH THIS!!!!! OTHERWISE MAPS WILL BECOME CORRUPT!!!
        //        //if(element_data.InnerText.EndsWith(",\n"))
        //        //{
        //        //    // Removes the last "," character.
        //        //    element_data.InnerText = element_data.InnerText.Remove(element_data.InnerText[element_data.InnerText.Length - 3], 3);
        //        //}
        //        element_layer.AppendChild(element_data);
        //        #endregion


        //        // Add imagelayer element to document
        //        element_map.AppendChild(element_layer);
        //    }
        //    #endregion


        //    #region Object Groups
        //    foreach (var objectGroup in this.objectGroups)
        //    {
        //        XmlElement element_group = doc.CreateElement("objectgroup");
        //        AddAttribute(doc, element_group, "id", objectGroup.id.ToString());
        //        AddAttribute(doc, element_group, "name", objectGroup.name);

        //        foreach (var obj in objectGroup.objects)
        //        {
        //            XmlElement element_object = doc.CreateElement("object");

        //            AddAttribute(doc, element_object, "id", obj.id.ToString());
        //            AddAttribute(doc, element_object, "type", obj.type);
        //            AddAttribute(doc, element_object, "x", obj.pos.X.ToString());
        //            AddAttribute(doc, element_object, "y", obj.pos.Y.ToString());
        //            AddAttribute(doc, element_object, "width", obj.size.X.ToString());
        //            AddAttribute(doc, element_object, "height", obj.size.Y.ToString());

        //            var e = AddPropertiesToXmlObject(doc, obj);
        //            if (e != null)
        //            {
        //                element_object.AppendChild(e);
        //            }

        //            element_group.AppendChild(element_object);
        //        }

        //        element_map.AppendChild(element_group); // Add objectgroup (aka object layer)
        //    }
        //    #endregion


        //    doc.Save(string.Concat("Content/", folderPath, fileName));
        //    Console.WriteLine("### Done. ###");
        //}
        #endregion
    }
}
