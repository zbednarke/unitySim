using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FPSCounter : MonoBehaviour
{
    public TMPro.TMP_Text fpsText;    public float updateInterval = 0.5f;

    private float deltaTime = 0.0f;
    private float frames = 0;

    private void Update()
    {
        deltaTime += Time.unscaledDeltaTime;
        frames++;

        if (deltaTime > updateInterval)
        {
            float fps = frames / deltaTime;
            fpsText.text = string.Format("{0:0.} fps", fps);

            frames = 0;
            deltaTime = 0;
        }
    }
}
