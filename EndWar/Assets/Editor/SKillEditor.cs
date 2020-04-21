using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(Skill))]
public class SKillEditor : Editor
{
    LayerMask tempMask;
    Skill _editor;
    int skillTypeTab;
    int skillAbilityTab;
    bool show_Damage = true;

    void OnEnable()
    {
        _editor = target as Skill;
    }

    public override void OnInspectorGUI()
    {
        int mask = InternalEditorUtility.LayerMaskToConcatenatedLayersMask(_editor.layerMask);
        string[] displayOption = InternalEditorUtility.layers;

        _editor.skillName = EditorGUILayout.TextField(new GUIContent("Skill Name", "스킬의 이름을 설정합니다."), _editor.skillName);
        _editor.prefab = (Transform)EditorGUILayout.ObjectField(new GUIContent("Effect Prefab", "이펙트 오브젝트를 설정합니다."), _editor.prefab, typeof(Transform), true);
        EditorGUILayout.Space(5);

        skillTypeTab = GUILayout.Toolbar((int)_editor.type, new string[] { "RADIAL", "AOE", "TARGETING" });
        _editor.type = (SkillType)skillTypeTab;

        _editor.damageCount = EditorGUILayout.IntField(new GUIContent("Damage Count", "피해 횟수를 설정합니다."), _editor.damageCount);
        if (_editor.damageCount < 0)
        {
            _editor.damageCount = 0;
        }

        _editor.damage = new float[_editor.damageCount];

        if (_editor.damageCount > 0)
        {
            EditorGUI.indentLevel += 2;
            show_Damage = EditorGUILayout.Foldout(show_Damage, "Damage");
            if (show_Damage)
            {
                EditorGUI.indentLevel += 2;
                for (int i = 0; i < _editor.damage.Length; i++)
                {
                    _editor.damage[i] = EditorGUILayout.FloatField(new GUIContent("Damage " + i, "피해량을 설정합니다."), _editor.damage[i]);
                    //SerializedProperty property = _editor.damage;
                }
                EditorGUI.indentLevel -= 4;
            }
        }

        EditorGUILayout.Space(5);
        _editor.distance = EditorGUILayout.FloatField(new GUIContent("Distance", "거리 설정을 합니다."), _editor.distance);

        switch (skillTypeTab)
        {
            case 0:
                _editor.viewAngle = EditorGUILayout.FloatField(new GUIContent("View Angle","방위각 설정을 합니다."), _editor.viewAngle);
                break;
            case 1:
                _editor.range = EditorGUILayout.FloatField(new GUIContent("Range", "범위 설정을 합니다."), _editor.range);
                _editor.isCollision = EditorGUILayout.Toggle(new GUIContent("IsCollision" , "범위 안의 충돌을 확인합니다." +
                                                                            "\n충돌이 있을때 true" +
                                                                            "\n충돌이 없을때 false, 공격용"), _editor.isCollision);
                break;
            case 2:
                _editor.targeting = (Transform) EditorGUILayout.ObjectField(new GUIContent("Targeting", "지정당한 대상입니다."), _editor._hit.transform, typeof(Transform), true);
                break;
        }
        EditorGUILayout.Space(5);

        tempMask = EditorGUILayout.MaskField(new GUIContent("LayerMask", "지정될 대상의 레이어를 선택합니다."), mask, displayOption);
        _editor.layerMask = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask);
        EditorGUILayout.Space(5);

        skillAbilityTab = GUILayout.Toolbar((int)_editor.ability, new string[] { "NONE", "AIRBORNE", "STUN", "DOT", "SLOW", "MEZ" });
        _editor.ability = (Skillability)skillAbilityTab;
        if (skillAbilityTab != 0)
            _editor.abilityTime = EditorGUILayout.FloatField(new GUIContent("AbilityTime","상태이상 효과의 지속시간을 설정합니다."), _editor.abilityTime);
        switch (skillAbilityTab)
        {
            case 4:
                _editor.slowing = EditorGUILayout.Slider(new GUIContent("Slowing", "둔화율을 설정합니다."), _editor.slowing, 0f, .9f);
                break;
            case 5:
                _editor.electricShock = EditorGUILayout.Toggle(new GUIContent("Electric Shock", "감전을 설정합니다."), _editor.electricShock);
                _editor.freezing = EditorGUILayout.Toggle(new GUIContent("Freezing", "빙결을 설정합니다."), _editor.freezing);
                break;

        }
    }
}
