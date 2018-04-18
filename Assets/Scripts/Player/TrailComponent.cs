using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TrailComponent : MonoBehaviour {
    public float m_BoostSpeedMultiplier = 1.0f;

    List<TrailRenderer> m_BoostTrails;
    List<ParticleSystem> m_BoostParticles;
    TrailType m_TrailType;
    float m_OldTrailTime;
    float m_CurrentTrailTime;
    bool m_Boosting = false;

    public enum TrailType
    {
    Trail,
    Particle
    }


	// Use this for initialization
	void Start () {
        m_BoostTrails = new List<TrailRenderer>();
        m_BoostParticles = new List<ParticleSystem>();

        Transform boost = transform.Find("Boost");
        var trails = boost.GetComponentsInChildren<TrailRenderer>();
        if(trails != null)
        {
            m_BoostTrails.AddRange(trails);
        }
        if(boost.GetComponent<TrailRenderer>())
        {
            m_BoostTrails.Add(boost.GetComponent<TrailRenderer>());
        }
        if(m_BoostTrails != null && m_BoostTrails.Count > 0)
        {
            m_OldTrailTime = m_BoostTrails[0].time;
            m_CurrentTrailTime = 0;

            foreach (var trail in m_BoostTrails)
            {
                trail.time = m_CurrentTrailTime;
            }
        }

        var particles = boost.GetComponentsInChildren<ParticleSystem>();
        if (particles != null)
        {
            m_BoostParticles.AddRange(particles);
        }
        if (boost.GetComponent<ParticleSystem>())
        {
            m_BoostParticles.Add(boost.GetComponent<ParticleSystem>());
        }
        if (m_BoostParticles != null && m_BoostParticles.Count > 0)
        {
            foreach (var trail in m_BoostParticles)
            {
                var emi = trail.emission;
                emi.enabled = false;
            }
        }


        if (m_BoostTrails.Count > 0)
        {
            m_TrailType = TrailType.Trail;
        }
        else
        {
            m_TrailType = TrailType.Particle;
        }
    }
	
    public void StartBoost()
    {
        if (!m_Boosting)
        {
            if (m_TrailType == TrailType.Particle)
            {
                foreach (var par in m_BoostParticles)
                {
                    var emi = par.emission;
                    emi.enabled = true;
                }
            }
        }
        m_Boosting = true;
    }

    public void StopBoost()
    {
        if(m_Boosting)
        {
            if (m_TrailType == TrailType.Particle)
            {
                foreach (var par in m_BoostParticles)
                {
                    var emi = par.emission;
                    emi.enabled = false;
                }
            }
        }
        m_Boosting = false;
    }

	// Update is called once per frame
	void Update () {
		if(m_TrailType == TrailType.Trail)
        {
            if(m_Boosting && m_CurrentTrailTime < m_OldTrailTime)
            {
                m_CurrentTrailTime += Time.deltaTime * m_BoostSpeedMultiplier;

                foreach (var trail in m_BoostTrails)
                {
                    trail.time = m_CurrentTrailTime;
                }
            }
            if(!m_Boosting && m_CurrentTrailTime > 0)
            {
                m_CurrentTrailTime -= Time.deltaTime * m_BoostSpeedMultiplier;

                foreach (var trail in m_BoostTrails)
                {
                    trail.time = m_CurrentTrailTime;
                }
            }
        }
	}
}
