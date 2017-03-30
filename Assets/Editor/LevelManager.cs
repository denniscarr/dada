using UnityEditor;
using UnityEngine;
using System.Collections;

// Creates an instance of a primitive depending on the option selected by the user.

public enum PROPS { 
	NPC = 0, 
	SCENERY = 1, 
	SPRITE = 2
}

public class EditorGUILayoutEnumPopup : EditorWindow {
	public PROPS op;
	[MenuItem("Examples/Editor GUILayout Enum Popup usage")]
	static void Init() {
		UnityEditor.EditorWindow window = GetWindow(typeof(EditorGUILayoutEnumPopup));
		window.Show();
	}
	void OnGUI() {
		op = (PROPS) EditorGUILayout.EnumPopup("Primitive to create:", op);
		if (GUILayout.Button("Create"))
			InstantiatePrimitive(op);

	}
	void InstantiatePrimitive(PROPS op) {
		switch(op) {
		case PROPS.NPC:
			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.transform.position = Vector3.zero;
			break;
		case PROPS.SCENERY:
			GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			sphere.transform.position = Vector3.zero;
			break;
		case PROPS.SPRITE:
			GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
			plane.transform.position = Vector3.zero;
			break;
		default:
			Debug.LogError("Unrecognized Option");
			break;
		}
	}
}