using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Environment = src.util.Environment;

public class DebugScreen : MonoBehaviour
{
    int wave;
    int kills;
    int scrap;
    int availableCards;
    int wavesBeat;
    int deaths;
    int droppedCards;

    int lastRemaining;

    Text   text;
    string debugString;
    
    void Awake()
    {
        text        = GetComponent<Text>();
        debugString = text.text;

        Environment.WaveService.NextWave += w =>
        {
            wave      = w.waveNumber;
            wavesBeat = Environment.WaveService.WavesBeat;
            Refresh();
        };
        
        Environment.LootService.DroppedCard += c =>
        {
            droppedCards++;
            Refresh();
        };
        Environment.LootService.DroppedScrap += s =>
        {
            scrap += s;
            Refresh();
        };

        Environment.SwarmService.Remaining += r =>
        {
            if (r < lastRemaining) kills++;
            lastRemaining = r;
            Refresh();
        };
        
        Refresh();
    }

    void Refresh()
    {
        text.text = string.Format(
            debugString, 
            wave, 
            kills, 
            scrap, 
            availableCards, 
            wavesBeat, 
            deaths, 
            droppedCards);
    }
}
