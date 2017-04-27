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
	public enum TYPES{NPCs = 0 , Pickups = 1, NonPickups = 2, Sprite = 3};
	public enum SPRITES{image = 0, tall = 1 , plinth = 2, gate = 3, foliage = 4, small = 5, fat = 6, squiggle = 7, tv = 8};
}