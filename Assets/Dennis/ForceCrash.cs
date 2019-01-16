using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceCrash : MonoBehaviour
{
    const float CRASHTIME = 15f;
    const float crashFps = 1f;
    float crashTimer = 0f;

    private void Update() {
        float fps = 1f / Time.deltaTime;
        if (fps <= crashFps) {
            crashTimer += Time.deltaTime;
            if (crashTimer >= CRASHTIME) { Application.Quit(); }
        } else {
            crashTimer = 0f;
        }
    }
}
