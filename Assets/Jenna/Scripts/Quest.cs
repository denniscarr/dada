using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// making the Quest class for the Quest system
// commenting thoroughly for learning purposes
// seems similar to CSS
// will be useful later, hopefully -J

[System.Serializable]
public class Quest {

	// an enum is a number of integers that we can give a name, apparently
	// the curly brackets are possible states that a quest "progress" can be in.
	// this can be changed/added onto later; right now this is for testing purposes.
	// in fact, it looks like making this its own class in the game is giving it a lot of
	// flexibility for us!
	public enum QuestProgress {NOT_AVAILABLE, AVAILABLE, ACCEPTED, COMPLETE, DONE}

	// self-explanatory
	public string title;

	// id of the quest, which gives us apparently all the info for the quest
	public int id;

	// tracks the progress state and compare it with any other state, as well as
	// set states. This gives us access to the enum above!
	public QuestProgress progress;

	// quest description -- I gather that this will store text that will be accessible
	// to the player, what appears on screen and such the like
	public string description;

	// I'm not sure if we'll want this (lol it seems not relevant to our game)
	// but it's good to make just in case
	public string hint;

	// text that lets us know we've "completed" the quest. Again, maybe not relevant,
	// but probably worth making space for, just in case.
	public string congratulation;

	// same idea
	public string summary;

	// it'll be fun and probably not too hard to figure out how to randomize this
	// i love a challenge!
	// particularly useful for chained quests apparently
	public int nextQuest;

	// apparently this can also be an int, depending on how you want to classify your
	// quests. We might want to change this to an int later. Can also be used to remove
	// items if you have an inventory system? Which we might be doing?
	public string questObjective; 

	// in case it's a countable quest...instead of a countably infinite one lolol
	public int questObjectiveCount;

	// required amount of quest objectives/objects from the above few
	public int questObjectiveRequirement;

	// lol this definitely won't be relevant at all, but we could maybe replace the idea
	// of an expReward with, instead, a tween/lerp/perlin about BLEED WHOOOOOOOOOOOOOOA
	// i'm a genius
	// maybe we should call this "bleedReward"
	public int expReward;

	// lol we can rename this too
	public int goldReward;

	// this one we can maybe keep since we do have an inventory system eventually i think
	public int itemReward;
}
