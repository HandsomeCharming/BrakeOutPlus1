using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEditor;

namespace Funly.SkyStudio
{
  public class KeyframeInspectorWindow : EditorWindow
  {
    public static SkyProfile profile;

    // Color key frame info.
    public static ColorKeyframe colorKeyFrame;
    public static ColorKeyframeGroup colorGroup;

    // Numeric key frame info.
    public static NumberKeyframe numberKeyframe;
    public static NumberKeyframeGroup numberGroup;

    private static bool _isEnabled;
    public static bool IsEnabled { get { return _isEnabled; } }

    private static KeyframeInspectorWindow _instance;

    private static float k_MinWidth = 350.0f;
    private static float k_MinHeight = 125.0f;

    public enum KeyType
    {
      None,
      Color,
      Numeric
    }

    public static KeyType keyType = KeyType.None;

    public static void ShowWindow()
    {
      if (_instance == null) {
				_instance = CreateInstance<KeyframeInspectorWindow>();
				_instance.name = "Keyframe Inspector";
				_instance.titleContent = new GUIContent("Keyframe Inspector");
        _instance.minSize = new Vector2(k_MinWidth, k_MinHeight);
      }
      _instance.ShowUtility();
    }

    private void OnEnable()
    {
      _isEnabled = true;
    }

    private void OnDisable()
    {
      _isEnabled = false;
    }

    public void ForceRepaint()
    {
      Repaint();
    }

    private void Update()
    {
      Repaint();
    }

    public static void SetColorKeyFrame(ColorKeyframe keyFrame, ColorKeyframeGroup group)
    {
      colorKeyFrame = keyFrame;
      colorGroup = group;
      keyType = KeyType.Color;
    }

    public static void SetNumericKeyFrame(NumberKeyframe keyFrame, NumberKeyframeGroup group)
    {
      numberKeyframe = keyFrame;
      numberGroup = group;
      keyType = KeyType.Numeric;
    }

    private void OnGUI()
    {
      if (keyType == KeyType.None)
      {
        ShowEmptyState();
        return;
      }

      GUILayout.BeginVertical();
      GUILayout.Space(5);

      bool didModifyProfile = false;

      // Core layout for this type of keyframe.
      if (keyType == KeyType.Color)
      {
        didModifyProfile = RenderColorGUI();
      } else if (keyType == KeyType.Numeric)
      {
        didModifyProfile = RenderNumericGUI();
      }

      // Time position.
      if (RenderTimeValue())
      {
        didModifyProfile = true;
      }

      // Curve type applies to all keyframes.
      if (RenderBlendingCurveType())
      {
        didModifyProfile = true;
      }

      GUILayout.FlexibleSpace();

      // Buttom buttons.
      GUILayout.BeginHorizontal();

      if (KeyFrameCount() > 1)
      {
        if (GUILayout.Button("Delete Keyframe")) {
          Undo.RecordObject(profile, "Deleting keyframe");

          if (keyType == KeyType.Color) {
            colorGroup.RemoveKeyFrame(colorKeyFrame);
          } else if (keyType == KeyType.Numeric) {
            numberGroup.RemoveKeyFrame(numberKeyframe);
          }

          didModifyProfile = true;
          Close();
        }
      }

      GUILayout.FlexibleSpace();

      if (GUILayout.Button("Close"))
      {
        Close();
      }
      GUILayout.EndHorizontal();

      GUILayout.Space(5);
      GUILayout.EndVertical();

      if (didModifyProfile)
      {
        IKeyframeGroup group = GetActiveGroup();
        group.SortKeyframes();
        EditorUtility.SetDirty(profile);
      }
    }

    private void ShowEmptyState()
    {
      EditorGUILayout.HelpBox("No keyframe selected.", MessageType.Info);
    }

    private int KeyFrameCount()
    {
      if (colorGroup != null)
      {
        return colorGroup.keyframes.Count;
      }

      if (numberGroup != null)
      {
        return numberGroup.keyframes.Count;
      }

      Debug.LogError("Can't get keyframe count");

      return 0;
    }

    private bool RenderColorGUI()
    {
      bool didModify = false;
      if (colorKeyFrame == null || colorGroup == null)
      {
        return didModify;
      }

      EditorGUI.BeginChangeCheck();
      Color selectedColor = EditorGUILayout.ColorField(new GUIContent("Color"), colorKeyFrame.color);
      if (EditorGUI.EndChangeCheck()) {
        Undo.RecordObject(profile, "Keyframe color changed.");
        colorKeyFrame.color = selectedColor;
        didModify = true;
      }

      return didModify;
    }

    private bool RenderNumericGUI()
    {
      bool didModify = false;
      if (numberKeyframe == null || numberGroup == null)
      {
        return didModify;
      }

      EditorGUI.BeginChangeCheck();
      float value = EditorGUILayout.FloatField(new GUIContent("Value"), numberKeyframe.value);
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObject(profile, "Keyframe numeric value changed.");
        numberKeyframe.value = Mathf.Clamp(value, numberGroup.minValue, numberGroup.maxValue);
        didModify = true;
      }

      return didModify;
    }

    public bool RenderBlendingCurveType()
    {
      IBaseKeyframe keyframe = GetActiveKeyframe();

      EditorGUI.BeginChangeCheck();

      CurveType selectedCurveType = (CurveType)EditorGUILayout.EnumPopup(
        new GUIContent("Animation Curve", 
        "Adjust how this keyframe will animate it's value towards the next keyframe." ), keyframe.curveType);

      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObject(profile, "Curve type changed");
        keyframe.curveType = selectedCurveType;
        return true;
      }

      return false;
    }

    public bool RenderTimeValue()
    {
      IBaseKeyframe keyframe = GetActiveKeyframe();

      EditorGUI.BeginChangeCheck();

      float time = EditorGUILayout.FloatField(
        new GUIContent("Time", 
        "Time position this keyframe is at in the current day. This is a value between 0 and 1."),
        keyframe.time);

      if (EditorGUI.EndChangeCheck()) {
        Undo.RecordObject(profile, "Keyframe time changed");
        keyframe.time = Mathf.Clamp01(time);
        return true;
      }

      return false;
    }

    public static string GetActiveKeyframeId() {
      if (IsEnabled == false || keyType == KeyType.None) {
        return null;
      }

      if (keyType == KeyType.Color) {
        return colorKeyFrame.id;
      } else if (keyType == KeyType.Numeric) {
        return numberKeyframe.id;
      }

      return null;
    }

    public IBaseKeyframe GetActiveKeyframe()
    {
      if (keyType == KeyType.Color)
      {
        return colorKeyFrame;
      } else if (keyType == KeyType.Numeric)
      {
        return numberKeyframe;
      }
      return null;
    }

    public IKeyframeGroup GetActiveGroup()
    {
      if (keyType == KeyType.Color) {
        return colorGroup;
      } else if (keyType == KeyType.Numeric) {
        return numberGroup;
      }
      return null;
    }
  }
}
