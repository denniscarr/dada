using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Services {

    public static GameObject Player { get; set; }
    public static LevelManager LevelGen { get; set; }
    public static PrefabManager Prefabs { get; set; }
    public static QuestManager Quests { get; set; }
    public static CS_AudioManager AudioManager { get; set; }
    public static IncoherenceManager IncoherenceManager { get; set; }
}