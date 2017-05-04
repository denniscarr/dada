using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncoherenceEvent : MonoBehaviour {

    public float threshold = 0.0f;  // The mininum globalIncoherence at which this event can occur.

    public bool instantaneous;  // Whether this event happens in one frame or over time.
    public bool active; // Whether this event is currently doing stuff.

	public virtual void Initiate()
    {

    }


    public virtual void Perform()
    {

    }
}
