using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Seraf.XNA.Tiled
{
    public class TTileMapManager
    {
        string mapFolder;

        public TTileMapManager(string mapFolder)
        {
            this.mapFolder = mapFolder;
        }


        public void Load(string mapName)
        {
            var fileManager = new FileManager<TTileMap>();

            fileManager.ImportRaw(string.Concat(mapFolder, mapName));
        }

        public void Save(string mapName) { }
    }
}
