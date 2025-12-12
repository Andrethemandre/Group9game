using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DebugTimingGauge : MonoBehaviour
{
    public DebugHitBar hitBar;
    public GameObject hitBarParent;
    public Follower follower;
    public GameObject averageLine;
    public GameObject missMsBarEarly;
    public GameObject missMsBarLate;
    float averageTiming = 0f;
    public int averageTimingMs;
    List<float> hits;
    float widthMs = 0.3f;
    float width = 35f;

    void Start()
    {
        RectTransform rt = GetComponent(typeof(RectTransform)) as RectTransform;
        widthMs = follower.hitMissMargin;
        width = rt.sizeDelta.x;
        Debug.Log("Hit Miss Margin ms: " + widthMs + " width units: " + width);
        hits = new List<float>{ 0f};

        Vector3 missEarly = new Vector3(-follower.hitMargin / widthMs * (width / 2f), 0f, 0f);
        missMsBarEarly.transform.localPosition = missEarly;
        Vector3 missLate = new Vector3(follower.hitMargin / widthMs * (width / 2f), 0f, 0f);
        missMsBarLate.transform.localPosition = missLate;
    }

    void Update()
    {
        Vector3 pos = new Vector3(averageTiming / widthMs * (width / 2f), 0f, 0f);
        averageLine.transform.localPosition = Vector3.Lerp(averageLine.transform.localPosition, pos, 0.3f * Time.deltaTime);
    }

    public void AddHitBar(float offTime)
    {
        Debug.Log("Hit Timing ms: " + (offTime * 1000f).ToString("F0"));
        float xPos = offTime / widthMs * (width / 2f);
        Debug.Log("Hit Timing xPos: " + xPos);
        Vector3 pos = new Vector3(xPos, 0f, 0f);
        
        DebugHitBar cloneHitBar;
        cloneHitBar = Instantiate(hitBar, hitBarParent.transform, false);
        cloneHitBar.transform.localPosition = pos;
        cloneHitBar.color = new Color(
            Mathf.Pow(Mathf.Abs(offTime) / widthMs, 2f) * 0.7f,
            (-1f * Mathf.Pow(Mathf.Abs(offTime) / widthMs, 1.2f) + 1f) * 0.7f,
            0f, 1f
        );
        cloneHitBar.time = (int)(offTime * 1000f);

        if (hits.Count < 10)
        {
            hits.Add(offTime);
        }
        else
        {
            hits.RemoveAt(0);
            hits.Add(offTime);
        }
        averageTiming = hits.Average();
        averageTimingMs = (int)(averageTiming * 1000f);
    }
}
