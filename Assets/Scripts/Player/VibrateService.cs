using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrateService : MonoBehaviour {

    public static VibrateService instance;

    private void Awake() {
        instance = this;
    }

    public void TriggerLight() {
        iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactLight);
    }

    public void TriggerHeavy() {
        iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactHeavy);
    }

    public void TriggerMedium() {
        iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactMedium);
    }
}
