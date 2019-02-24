using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MiscFunctions : MonoBehaviour {

	// Finds a game object, searching only for children of a particular game object (and the game object itself)
	public static GameObject FindGameObjectInRoot(Transform root, string searchName) {
		
		// Add this game object and all child objects to selfAndChildren
		List<Transform> rootAndChildren = new List<Transform>();
		rootAndChildren.Add(root);
		for (int i = 0; i < root.childCount; i++) {
			rootAndChildren.Add(root.GetChild(i));
		}

		// Go through selfAndChildren game objects and add various randomizer scripts based on what components are currently attached to those objects.
		foreach(Transform t in rootAndChildren)
		{
			if (t.name == searchName) {
				return t.gameObject;
			}
		}

		Debug.LogError("Could not find a game object named "+searchName+" in "+root.name+" or its children.");
		return null;
	}


	// Maps a value between a new range of two numbers.
	public static float Map(float value, float inputMin, float inputMax, float outputMin, float outputMax) {
		return (value - inputMin) * (outputMax - outputMin) / (inputMax - inputMin) + outputMin;
	}
}
