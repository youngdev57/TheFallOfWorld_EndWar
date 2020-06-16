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
        _editor.ShowRange();

        int mask = InternalEditorUtility.LayerMaskToConcatenatedLayersMask(_editor.layerMask);
        string[] displayOption = InternalEditorUtility.layers;

        _editor.skillName = EditorGUILayout.TextField(new GUIContent("Skill Name", "스킬의 이름을 설정합니다."), _editor.skillName);
        EditorGUILayout.Space(5);

        skillTypeTab = GUILayout.Toolbar((int)_editor.type, new string[] { "RADIAL", "AOE", "NONTARGET" });
        _editor.type = (SkillType)skillTypeTab;

        if (skillTypeTab != 2)
        {
            _editor.n_damageCount = EditorGUILayout.IntField(new GUIContent("Damage Count", "피해 횟수를 설정합니다."), _editor.n_damageCount);
            if (_editor.n_damageCount < 0)
            {
                _editor.n_damageCount = 0;
            }

            if (_editor.damage == null || _editor.damage.Length != _editor.n_damageCount)
                _editor.damage = new int[_editor.n_damageCount];

            _editor.damage = _editor.damage;

            if (_editor.n_damageCount > 0)
            {
                EditorGUI.indentLevel += 2;
                show_Damage = EditorGUILayout.Foldout(show_Damage, "Damage");
                if (show_Damage)
                {
                    EditorGUI.indentLevel += 2;
                    for (int i = 0; i < _editor.damage.Length; i++)
                    {
                        _editor.damage[i] = EditorGUILayout.IntField(new GUIContent("Damage " + i, "피해량을 설정합니다."), _editor.damage[i]);
                        //SerializedProperty property = _editor.damage;
                    }
                    EditorGUI.indentLevel -= 2;
                }
                EditorGUI.indentLevel -= 2;
            }

            EditorGUILayout.Space(5);
            _editor.distance = EditorGUILayout.FloatField(new GUIContent("Distance", "거리 설정을 합니다."), _editor.distance);
        }
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
                _editor.nonTargetDamage = EditorGUILayout.IntField(new GUIContent("NonTargetDamage", "데미지를 설정합니다."), _editor.nonTargetDamage);
                _editor.seconds = EditorGUILayout.FloatField(new GUIContent("Seconds", "지속되는 시간을 설정합니다."), _editor.seconds);
                _editor.speed = EditorGUILayout.IntField(new GUIContent("Speed", "날라가는 속도를 설정합니다."), _editor.speed);
                break;
        }
        EditorGUILayout.Space(5);

        if (skillTypeTab != 2)
        {
            tempMask = EditorGUILayout.MaskField(new GUIContent("LayerMask", "지정될 대상의 레이어를 선택합니다.\n" +
                                                                "RADIAL : 피해를 받을 적을 설정합니다.\n" +
                                                                "AOE : 바닥을 설정합니다."), mask, displayOption);
            _editor.layerMask = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask);
            EditorGUILayout.Space(5);
        }

        skillAbilityTab = GUILayout.Toolbar((int)_editor.ability, new string[] { "NONE", "STUN", "DOT", "SLOW", "MEZ" });
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
