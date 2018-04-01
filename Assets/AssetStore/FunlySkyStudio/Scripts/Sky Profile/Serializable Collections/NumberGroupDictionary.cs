using System;
using System.Collections;
using System.Collections.Generic;
using Funly.SkyStudio;
using UnityEngine;

namespace Funly.SkyStudio
{
  // Subclass to remove templating, so we can serialize this class.
  [Serializable]
  public class NumberGroupDictionary : SerializableDictionary<string, NumberKeyframeGroup>
  {

  }
}

