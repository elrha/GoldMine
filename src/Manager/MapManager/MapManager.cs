using PlayerInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mines.Manager.MapManager
{
    class MapManager
    {
        private static MapManager instance = new MapManager();

        public static MapManager Instance { get { return MapManager.instance; } }

        private static readonly string mapFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Map");

        private Random mapIndexGenerator = new Random(DateTime.Now.Second);
        private string selectedMapName;
        private Dictionary<string, BlockType[]> customMapList;
        
        public void Initialize()
        {
            this.customMapList = new Dictionary<string, BlockType[]>();
            this.customMapList["RANDOM"] = null;
            this.customMapList["HeximalPattern1"] = null;
            this.customMapList["RectPattern1"] = null;
            this.customMapList["RoadPattern1"] = null;
            this.selectedMapName = "RANDOM";

            foreach (var mapKV in ExcelMapLoader.LoadExcelMaps(MapManager.mapFolderPath))
                this.customMapList[mapKV.Key] = mapKV.Value;
        }
        
        public IEnumerable<string> GetMapNameList()
        {
            return customMapList.Keys;
        }

        public string GetSelectedMapName()
        {
            return this.selectedMapName;
        }

        public void SelectMap(string mapName)
        {
            this.selectedMapName = mapName;
        }

        public BlockType[] CreateField(int col, int row, List<int> startPosition)
        {
            var result = this.SelectField(col, row);
            foreach (var pos in startPosition)
                result[pos] = BlockType.NONE;
            return result;
        }

        private BlockType[] SelectField(int col, int row)
        {
            if (!this.customMapList.ContainsKey(this.selectedMapName))
                return DefaultMapGenerator.HeximalDevision(col, row);

            var targetMapName = this.selectedMapName;
            if (string.Compare(targetMapName, "RANDOM") == 0)
                targetMapName = this.customMapList.Keys.ElementAt((this.mapIndexGenerator.Next(this.customMapList.Count - 1) + 1));
            
            switch (targetMapName)
            {
                case "HeximalPattern1":
                    return DefaultMapGenerator.HeximalDevision(col, row);
                case "RectPattern1":
                    return DefaultMapGenerator.RectPattern1(col, row);
                case "RoadPattern1":
                    return DefaultMapGenerator.RoadPattern1(col, row);
                default:
                    return this.customMapList[targetMapName].ToArray();
            }
        }
    }
}
