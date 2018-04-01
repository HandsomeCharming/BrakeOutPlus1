using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funly.SkyStudio
{
  public enum CurveType
  {
    Linear,
    EaseInEaseOut,
    OrbitBodyForwardComplete,
  }

  public interface IBaseKeyframe
  {
    string id { get; }
    float time { get; set; }
    CurveType curveType { get; set; }
  }
}

