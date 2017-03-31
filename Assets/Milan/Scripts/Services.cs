using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Services
{
    public static GameObject Player { get; set; }
    public static LevelManager LevelGen { get; set; }
    public static PrefabManager Prefabs { get; set; }
    public static QuestManager Quests { get; set; }
    public static CS_AudioManager AudioManager { get; set; }
    public static IncoherenceManager IncoherenceManager { get; set; }
	public enum TYPES{Emptiness = 0, NPCs = 1 , Pickups = 2, NonPickups = 3, Sprite = 4};
	public enum SPRITES{image = 0, tall = 1 , plinth = 3, gate = 2, foliage = 4, small = 5, fat = 6, squiggle = 7, tv = 8};
}