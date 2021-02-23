using System;
using System.Collections.Generic;
using UnityEngine;
using Environment = src.util.Environment;

namespace src.movement
{
    public class GridControl : MonoBehaviour
    {
        Dictionary<string, GridItem> gridItems = new Dictionary<string, GridItem>();
        
        void Awake()
        {
            var tiles = GameObject.FindGameObjectsWithTag(Environment.TAG_MOVEMENT);
            foreach (var tile in tiles)
            {
                gridItems.Add(tile.name, new GridItem(tile));
            }
        }
    }
}
