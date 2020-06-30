using System.Linq;
using UnityEditor;
using UnityEngine;

public class LightProbeCombiner : EditorWindow
{
    private LightProbeGroup[] _source = new LightProbeGroup[0];
    private LightProbeGroup _target;


    void OnEnable()
    {
        Selection.selectionChanged += SelectionChanged;
    }

    void OnDisable()
    {
        Selection.selectionChanged -= SelectionChanged;
    }

    void SelectionChanged()
    {
        _source = Selection.gameObjects.Where(o => o.GetComponent<LightProbeGroup>() != null)
            .Select(o => o.GetComponent<LightProbeGroup>()).Where(g => g != _target).ToArray();

        Repaint();
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Select light probe groups to combine them into target group");
        EditorGUILayout.LabelField($"Selected: {_source.Length}");

        _target = (LightProbeGroup)EditorGUILayout.ObjectField("Target", _target,
                typeof(LightProbeGroup), true);

        EditorGUI.BeginDisabledGroup(_source.Length == 0 || _target == null);
        if (GUILayout.Button("Combine"))
        {
            Combine(_source, _target);
            Selection.activeGameObject = _target.gameObject;
        }
        EditorGUI.EndDisabledGroup();
    }

    void Combine(LightProbeGroup[] source, LightProbeGroup target)
    {
        Undo.RecordObject(target, "Combine light probes");
        target.probePositions = source.SelectMany(g => g.probePositions.Select(v => g.transform.TransformPoint(v))).Select(v => target.transform.InverseTransformPoint(v)).ToArray();

    }


    [MenuItem("Tools/Light Probe Combiner")]
    public static void ShowWindow()
    {
        LightProbeCombiner window = (LightProbeCombiner)GetWindow(typeof(LightProbeCombiner));
        window.Show();
    }
}
