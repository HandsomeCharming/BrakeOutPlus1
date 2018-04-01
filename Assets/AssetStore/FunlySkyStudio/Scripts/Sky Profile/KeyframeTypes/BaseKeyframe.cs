using System;
using System.Collections;
using System.Collections.Generic;
using Funly.SkyStudio;
using UnityEngine;

namespace Funly.SkyStudio
{
  [Serializable]
  public class BaseKeyframe : System.Object, IComparable, IBaseKeyframe
  {
    [SerializeField]
    public string m_Id;
    public string id
    {
      get { return m_Id; }
      set { m_Id = value; }
    }

    // Time this keyframe begins at, (0-1);
    [SerializeField]
    private float m_Time;
    public float time
    {
      get { return m_Time; }
      set { m_Time = value; }
    }

    // Adjust interpolation curve to next keyframe.
    [SerializeField]
    private CurveType m_CurveType = CurveType.Linear;
    public CurveType curveType
    {
      get { return m_CurveType; }
      set { m_CurveType = value; }
    }

    public BaseKeyframe(float time)
    {
      id = Guid.NewGuid().ToString();
      this.time = time;
    }

    public int CompareTo(object other)
    {
      BaseKeyframe otherFrame = other as BaseKeyframe;
      return time.CompareTo(otherFrame.time);
    }
  }
}

