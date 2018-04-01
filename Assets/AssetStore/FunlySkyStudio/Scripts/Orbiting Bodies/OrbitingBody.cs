using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funly.SkyStudio
{
  public class OrbitingBody : MonoBehaviour
  {
    [HideInInspector, Range(0, 1)]
    public float orbitRotation = 0;

    [HideInInspector, Range(-1, 1)]
    public float orbitTilt = 0;

    [HideInInspector, Range(0, 1)]
    public float bodyPosition = 0;

    // Direction to orbiting body.
    public Vector3 BodyGlobalDirection { get { return transform.right; } }

    [HideInInspector]
    public bool drawOrbitDebugGizmos = false;

    private Light m_bodyLight;
    public Light BodyLight
    {
      get {
        if (m_bodyLight == null) {
          m_bodyLight = transform.GetComponentInChildren<Light>();
        }

        return m_bodyLight;
      }
    }

    void OnDrawGizmos()
    {
      if (drawOrbitDebugGizmos) {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (transform.up * 5.0f));

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + (transform.right * 5.0f));
      }
    }

    public void LayoutOribit()
    {
      transform.localPosition = Vector3.zero;
      transform.localRotation = Quaternion.identity;
      transform.localRotation = Quaternion.Euler(orbitTilt * 180.0f, 0, 0);
      transform.localRotation *= Quaternion.AngleAxis(orbitRotation * 360.0f, Vector3.up);
      transform.localRotation *= Quaternion.AngleAxis(bodyPosition * 360.0f, Vector3.forward);
    }

    void OnValidate()
    {
      LayoutOribit();
    }
  }
}
