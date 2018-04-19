using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

[RequireComponent(typeof(PostProcessingBehaviour))]
public class CameraEffectManager : MonoBehaviour {

    public static CameraEffectManager current;

    PostProcessingProfile m_Profile;

    float m_BloomIntensityToReach;
    float m_BloomDuration;
    float m_BloomPeakDuration;

    bool m_Autopiloting;
    bool m_HasAutoPilotIEnu;
    bool m_MotionBluring;

    void OnEnable()
    {
        current = this; 
        
        var behaviour = GetComponent<PostProcessingBehaviour>();

        if (behaviour.profile == null)
        {
            enabled = false;
            return;
        }

        m_Profile = Instantiate(behaviour.profile);
        behaviour.profile = m_Profile;

        m_Autopiloting = false;
        m_HasAutoPilotIEnu = false;
        //m_Profile.motionBlur.enabled = true;
    }

    public static void Bloom(float intensity, float duration, float peakDuration = 0.0f)
    {
        if(current != null)
        {
            current.m_BloomIntensityToReach = intensity;
            current.m_BloomDuration = duration;
            current.m_BloomPeakDuration = peakDuration; 
            current.StartBloom();
        }
    }

    void StartBloom()
    {
        current.StartCoroutine(BloomInTime());
    }

    IEnumerator BloomInTime()
    {
        print("bloom");
        float duration = m_BloomDuration;
        BloomModel.Settings settings = m_Profile.bloom.settings;
        while (m_BloomDuration > 0)
        {
            settings.bloom.intensity = Mathf.Lerp(0, m_BloomIntensityToReach, 1.0f - m_BloomDuration / duration);
            m_Profile.bloom.settings = settings;
            m_BloomDuration -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(m_BloomPeakDuration);
        m_BloomDuration = duration;

        while (m_BloomDuration > 0)
        {
            settings.bloom.intensity = Mathf.Lerp(m_BloomIntensityToReach, 0, 1.0f - m_BloomDuration / duration);
            m_Profile.bloom.settings = settings;
            m_BloomDuration -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        settings.bloom.intensity = 0;
        m_Profile.bloom.settings = settings;
    }

    public void StartAutoPilotEffect()
    {
        m_Autopiloting = true;
        if(!m_HasAutoPilotIEnu)
            StartCoroutine(AutoPilotEffect());
    }
    public void StopAutoPilotEffect()
    {
        m_Autopiloting = false;
    }

    public void StartMotionBlur()
    {
        //m_Profile.motionBlur.enabled = true;
        m_MotionBluring = true;
    }

    public void StopMotionBlur()
    {
        //m_Profile.motionBlur.enabled = false;
        m_MotionBluring = false;
    }

    IEnumerator AutoPilotEffect()
    {
        m_HasAutoPilotIEnu = true;
        float fadeInTimeRemain = 0;
        const float fadeInTime = 0.5f;
        const float fadeOutTime = 1.0f;
        const float maxIntensity = 0.5f;

        VignetteModel.Settings vig = m_Profile.vignette.settings;
        vig.intensity = 0;
        GrainModel.Settings grain = m_Profile.grain.settings;
        vig.intensity = 0;

        //fade in
        while (fadeInTimeRemain < fadeInTime)
        {
            float lerpAmount = fadeInTimeRemain / fadeInTime;
            vig.intensity = Mathf.Lerp(0, maxIntensity, lerpAmount);
            grain.intensity = Mathf.Lerp(0, maxIntensity, lerpAmount);
            fadeInTimeRemain += Time.deltaTime;
            m_Profile.vignette.settings = vig;
            m_Profile.grain.settings = grain;
            yield return new WaitForEndOfFrame();
        }

        while(m_Autopiloting)
            yield return new WaitForEndOfFrame();

        //fade out
        fadeInTimeRemain = 0;
        while (fadeInTimeRemain < fadeOutTime)
        {
            float lerpAmount = fadeInTimeRemain / fadeOutTime;
            vig.intensity = Mathf.Lerp(maxIntensity, 0, lerpAmount);
            grain.intensity = Mathf.Lerp(maxIntensity, 0, lerpAmount);
            fadeInTimeRemain += Time.deltaTime;
            m_Profile.vignette.settings = vig;
            m_Profile.grain.settings = grain;
            yield return new WaitForEndOfFrame();
        }

        m_HasAutoPilotIEnu = false;
    }


    // Update is called once per frame
    void Update () {
        if (Player.current.physics.cameraZoomLerpAmount > 0)
        {
			/*
            var set = m_Profile.motionBlur.settings;
            set.frameBlending = Mathf.Lerp(0.0f, 0.5f, Player.current.physics.cameraZoomLerpAmount);
            m_Profile.motionBlur.settings = set;
			*/
        }
        
	}
}
