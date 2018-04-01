using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funly.SkyStudio
{
  [Serializable]
  public class KeyframeGroup<T> : System.Object, IKeyframeGroup where T : IBaseKeyframe
  {
    public List<T> keyframes = new List<T>();

    [SerializeField]
    private string m_Name;
    public string name
    {
      get { return m_Name; }
      set { m_Name = value; }
    }

    [SerializeField]
    private string m_Id;
    public string id
    {
      get { return m_Id; }
      set { m_Id = value; }
    }

    public KeyframeGroup(string name)
    {
      this.name = name;
      id = Guid.NewGuid().ToString();
    }

    public void AddKeyFrame(T keyFrame)
    {
      keyframes.Add(keyFrame);
      SortKeyframes();
    }

    public void RemoveKeyFrame(T keyFrame)
    {
      if (keyframes.Count == 1)
      {
        Debug.LogError("You must have at least 1 keyframe in every group.");
        return;
      }

      keyframes.Remove(keyFrame);
      SortKeyframes();
    }

    public T GetKeyframe(int index)
    {
      return keyframes[index];
    }

    public void SortKeyframes()
    {
      keyframes.Sort();
    }

    public float CurveAdjustedBlendingTime(CurveType curve, float t)
    {
      if (curve == CurveType.Linear)
      {
        return t;
      } else if (curve == CurveType.EaseInEaseOut)
      {
        float curveTime = t < .5f ? 2 * t * t : -1 + (4 - 2 * t) * t;
        return Mathf.Clamp01(curveTime);
      }

      return t;
    }

    // Get the keyframe that comes before this time.
    public T GetPreviousKeyFrame(float time)
    {
      T beforeKeyframe;
      T afterKeyframe;

      if (!GetSurroundingKeyFrames(time, out beforeKeyframe, out afterKeyframe))
      {
        return default(T);
      }

      return beforeKeyframe;
    }

    public bool GetSurroundingKeyFrames(float time, out T beforeKeyframe, out T afterKeyframe)
    {
      beforeKeyframe = default(T);
      afterKeyframe = default(T);

      int beforeIndex, afterIndex;

      if (GetSurroundingKeyFrames(time, out beforeIndex, out afterIndex))
      {
        beforeKeyframe = GetKeyframe(beforeIndex);
        afterKeyframe = GetKeyframe(afterIndex);
        return true;
      }
      return false;
    }

    public bool GetSurroundingKeyFrames(float time, out int beforeIndex, out int afterIndex)
    {
      beforeIndex = 0;
      afterIndex = 0;

      if (keyframes.Count == 0)
      {
        Debug.LogError("Can't return nearby keyframes since it's empty.");
        return false;
      }

      if (keyframes.Count == 1)
      {
        return true;
      }

      if (time < keyframes[0].time)
      {
        beforeIndex = keyframes.Count - 1;
        afterIndex = 0;
        return true;
      }

      int keyframeIndex = 0;

      for (int i = 0; i < keyframes.Count; i++)
      {
        if (keyframes[i].time >= time)
        {
          break;
        }
        keyframeIndex = i;
      }

      int nextKeyFrame = (keyframeIndex + 1) % keyframes.Count;

      beforeIndex = keyframeIndex;
      afterIndex = nextKeyFrame;

      return true;
    }

    public static float ProgressBetweenSurroundingKeyframes(float time, BaseKeyframe beforeKey, BaseKeyframe afterKey) {
      return ProgressBetweenSurroundingKeyframes(time, beforeKey.time, afterKey.time);
    }

    // FIXME - Rename to to percent between circular times.
    public static float ProgressBetweenSurroundingKeyframes(float time, float beforeKeyTime, float afterKeyTime)
    {
      if (afterKeyTime > beforeKeyTime && time <= beforeKeyTime)
      {
        return 0;
      }

      float rangeWidth = WidthBetweenCircularValues(beforeKeyTime, afterKeyTime);
      float valueWidth = WidthBetweenCircularValues(beforeKeyTime, time);

      // Find what percentage this time is between the 2 circular keyframes.
      float progress = valueWidth / rangeWidth;

      return Mathf.Clamp01(progress);
    }

    // FIXME - This should really be called distance between circular values.
    public static float WidthBetweenCircularValues(float begin, float end)
    {
      if (begin <= end)
      {
        return end - begin;
      }

      return (1 - begin) + end;
    }

    public void TrimToSingleKeyframe() {
      if (keyframes.Count == 1) {
        return;
      }
      keyframes.RemoveRange(1, keyframes.Count - 1);
    }
  }
}
