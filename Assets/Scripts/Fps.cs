using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Fps : MonoBehaviour
{
    private TextMeshProUGUI uiText;
    private int framesNum;
    private float[] frameDeltaTimes;
    private int previousFrameTimeIndex;

    [Header("Visual")]
    [SerializeField] private Color outlineColor;
    [SerializeField] private float outlineWidth;

    [Header("Performance")]
    [SerializeField] private float uiUpdateDelay;



    private void Awake()
    {
        uiText = GetComponent<TextMeshProUGUI>();

        framesNum = 50;
        frameDeltaTimes = new float[framesNum];
        previousFrameTimeIndex = 0;
    }

    private void Start()
    {
        uiText.outlineWidth = outlineWidth;
        uiText.outlineColor = outlineColor;
        StartCoroutine(UpdateUiText());
    }

    private void OnEnable()
    {
        uiText.outlineWidth = outlineWidth;
        uiText.outlineColor = outlineColor;
    }

    private void Update()
    {
        frameDeltaTimes[previousFrameTimeIndex] = Time.deltaTime;
        previousFrameTimeIndex = (previousFrameTimeIndex + 1) % frameDeltaTimes.Length;
    }

    private int CalculateFPS()
    {
        float sum = 0;

        for (int i = 0; i < framesNum; i++)
            sum += frameDeltaTimes[i];

        return (int)(framesNum / sum);
    }

    private IEnumerator UpdateUiText()
    {
        while (true)
        {
            uiText.text = CalculateFPS().ToString();

            yield return new WaitForSeconds(uiUpdateDelay);
        }
    }
}
