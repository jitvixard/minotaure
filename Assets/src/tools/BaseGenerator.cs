using System;
using UnityEditor;
using UnityEngine;
using Environment = src.util.Environment;

namespace src.tools
{
    public class BaseGenerator
    {
        static string namePrefix = Environment.TILE_NAME_FORMAT;

        [MenuItem("Tools/Generate Grid")]
        public static void GenerateGrid()
        {
            
            var tileCount = 1;
            
            GameObject startTile = GameObject.FindGameObjectWithTag("StartTile");
            
            var tilesInColumn = PopulateColumn(startTile);
            var allRows = PopulateRows(tilesInColumn);
            
        }

        static GameObject[] PopulateColumn(GameObject startTile)
        {
            GameObject[] tilesInColumn = new GameObject[11];
            tilesInColumn[5] = startTile;
            startTile.name = String.Format(namePrefix, 5, 5); 
            
            int[] directions = {-1, 1};
            var startPos = startTile.transform.position;
            
            foreach (var dir in directions)
            {
                var index = 1 * dir;
                while (index < 6 && index > -6)
                {
                    var newTile = GameObject.Instantiate(startTile);
                    newTile.tag = "MovementTile";
                    newTile.name = String.Format(namePrefix, 5, index + 5);
                    
                    newTile.transform.position = new Vector3(
                        startPos.x,
                        startPos.y,
                        startPos.z + index);

                    tilesInColumn[index + 5] = newTile;
                    index += dir;
                }
            }

            return tilesInColumn;
        }

        static GameObject[][] PopulateRows(GameObject[] centreColumn) 
        {
            var rows = new GameObject[11][];
            int rowIndex = 0;

            foreach (var tile in centreColumn)
            {
                GameObject[] row = new GameObject[11];
                row[5] = tile;

                var startPos = tile.transform.position;
                
                int[] directions = {-1, 1};
                foreach (var dir in directions)
                {
                    var index = 1 * dir;
                    while (index < 6 && index > -6)
                    {
                        var newTile = GameObject.Instantiate(tile);
                        newTile.tag = "MovementTile";
                        newTile.name = String.Format(namePrefix, index + 5, rowIndex);
                    
                        newTile.transform.position = new Vector3(
                            startPos.x + index,
                            startPos.y,
                            startPos.z);

                        row[index + 5] = newTile;
                        index += dir;
                    }
                }

                rows[rowIndex++] = row;
            }

            return rows;
        }
    }   
}
