using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceCrash : MonoBehaviour
{
    const float CRASHTIME = 40f;
    const float crashFps = 3f;
    float crashTimer = 0f;

    private void Update() {

        float fps = 1f / Time.deltaTime;

        Debug.Log("Current fps: " + fps);
        
        if (fps <= crashFps) {
            crashTimer += Time.deltaTime;
            Debug.Log("Crash timer: " + crashTimer);
            if (crashTimer >= CRASHTIME) { Debug.Log("QUIUTTING"); Application.Quit(); }
        } else {
            crashTimer = 0f;
        }
    }
}
