using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funly.SkyStudio
{
  public interface IKeyframeGroup
  {
    string name { get; set; }
    string id { get; }
    void SortKeyframes();
  }
}

