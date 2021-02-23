using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Environment = src.util.Environment;

namespace src.movement
{
    public class GridItem
    {
        public readonly List<GridItem> neighbours = new List<GridItem>();
        
        public readonly int x;
        public readonly int y;

        public bool blocked = false;

        public int visited = -1;

        List<string> neighbourNames = new List<string>();

        Regex xRgx = new Regex("(?<=r)\\d+", RegexOptions.Compiled);
        Regex yRgx = new Regex("(?<=c)\\d+", RegexOptions.Compiled);

        public GridItem(GameObject tile)
        {
            var xMatch = xRgx.Match(tile.name);
            var yMatch = yRgx.Match(tile.name);
            x = int.Parse(xMatch.Value); 
            y = int.Parse(yMatch.Value);

            neighbourNames = new List<string>();

            if (y + 1 < 11)  neighbourNames.Add(string.Format(Environment.TILE_NAME_FORMAT, y + 1, x));
            if (y - 1 > -1)  neighbourNames.Add(string.Format(Environment.TILE_NAME_FORMAT, y - 1, x));
            if (x + 1 < 11)  neighbourNames.Add(string.Format(Environment.TILE_NAME_FORMAT, y, x + 1));
            if (x - 1 > -1)  neighbourNames.Add(string.Format(Environment.TILE_NAME_FORMAT, y, x - 1));
        }
    }
}