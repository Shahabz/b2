﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Animations;

namespace Cinemachine.Editor
{
    [CustomEditor(typeof(CinemachineStateDrivenCamera))]
    internal sealed class CinemachineStateDrivenCameraEditor : CinemachineVirtualCameraBaseEditor
    {
        private CinemachineStateDrivenCamera Target { get { return target as CinemachineStateDrivenCamera; } }
        EmbeddeAssetEditor<CinemachineBlenderSettings> m_BlendsEditor;

        protected override string[] GetExcludedPropertiesInInspector()
        {
            List<string> excluded = new List<string>();
            excluded.AddRange(Target.m_ExcludedPropertiesInInspector);
            excluded.Add(SerializedPropertyHelper.PropertyName(()=>Target.m_LayerIndex));
            excluded.Add(SerializedPropertyHelper.PropertyName(()=>Target.m_DefaultBlend));
            excluded.Add(SerializedPropertyHelper.PropertyName(()=>Target.m_ChildCameras));
            excluded.Add(SerializedPropertyHelper.PropertyName(()=>Target.m_Instructions));
            return excluded.ToArray();
        }

        private static readonly GUIContent activeAfterLabel = new GUIContent("", "Camera is activated only after this state has been active for this amount of time");
        private static readonly GUIContent minDurationLabel = new GUIContent("", "Once camera is activated, it will remain active for at least this long");

        private UnityEditorInternal.ReorderableList mChildList;
        private UnityEditorInternal.ReorderableList mInstructionList;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_BlendsEditor = new EmbeddeAssetEditor<CinemachineBlenderSettings>(
                SerializedPropertyHelper.PropertyName(()=>Target.m_CustomBlends), this);
            m_BlendsEditor.OnChanged = (CinemachineBlenderSettings b) => 
            {
                UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
            };
            m_BlendsEditor.OnCreateEditor = (UnityEditor.Editor ed) =>
            {
                CinemachineBlenderSettingsEditor editor = ed as CinemachineBlenderSettingsEditor;
                if (editor != null)
                    editor.GetAllVirtualCameras = () => { return Target.ChildCameras; };
            };
            mChildList = null;
            mInstructionList = null;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (m_BlendsEditor != null)
                m_BlendsEditor.OnDisable();
        }

        public override void OnInspectorGUI()
        {
            if (mInstructionList == null)
                SetupInstructionList();
            if (mChildList == null)
                SetupChildList();

            // Ordinary properties
            base.OnInspectorGUI();

            // Layer index
            EditorGUI.BeginChangeCheck();
            UpdateTargetStates();
            UpdateCameraCandidates();
            SerializedProperty layerProp = serializedObject.FindProperty(()=>Target.m_LayerIndex);
            int currentLayer = layerProp.intValue;
            int layerSelection = EditorGUILayout.Popup("Layer", currentLayer, mLayerNames); 
            if (currentLayer != layerSelection)
                layerProp.intValue = layerSelection;
            EditorGUILayout.PropertyField(serializedObject.FindProperty(()=>Target.m_DefaultBlend));
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                Target.ValidateInstructions();
            }

            // Blends
            EditorGUILayout.Separator();
            m_BlendsEditor.DrawEditorCombo(
                "Create New Blender Asset", 
                Target.gameObject.name + " Blends", "asset", string.Empty,
                "Custom Blends", false);

            // Instructions
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.Separator();
            mInstructionList.DoLayoutList();

            // vcam children
            EditorGUILayout.Separator();
            mChildList.DoLayoutList();
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                Target.ValidateInstructions();
            }
        }

        private string[] mLayerNames;
        private int[] mTargetStates;
        private string[] mTargetStateNames;
        private Dictionary<int, int> mStateIndexLookup;
        private void UpdateTargetStates()
        {
            // Scrape the Animator Controller for states
            AnimatorController ac = (Target.m_AnimatedTarget == null) 
                ? null : Target.m_AnimatedTarget.runtimeAnimatorController as AnimatorController;
            StateCollector collector = new StateCollector();
            collector.CollectStates(ac, Target.m_LayerIndex);
            mTargetStates = collector.mStates.ToArray();
            mTargetStateNames = collector.mStateNames.ToArray();
            mStateIndexLookup = collector.mStateIndexLookup;

            if (ac == null)
                mLayerNames = new string[0];
            else
            {
                mLayerNames = new string[ac.layers.Length];
                for (int i = 0; i < ac.layers.Length; ++i)
                    mLayerNames[i] = ac.layers[i].name;
            }

            // Create the parent map in the target
            List<CinemachineStateDrivenCamera.ParentHash> parents 
                = new List<CinemachineStateDrivenCamera.ParentHash>();
            foreach (var i in collector.mStateParentLookup)
                parents.Add(new CinemachineStateDrivenCamera.ParentHash(i.Key, i.Value));
            Target.m_ParentHash = parents.ToArray();
        }

        class StateCollector
        {
            public List<int> mStates;
            public List<string> mStateNames;
            public Dictionary<int, int> mStateIndexLookup;
            public Dictionary<int, int> mStateParentLookup;

            public void CollectStates(AnimatorController ac, int layerIndex)
            {
                mStates = new List<int>();
                mStateNames = new List<string>();
                mStateIndexLookup = new Dictionary<int, int>();
                mStateParentLookup = new Dictionary<int, int>();

                mStateIndexLookup[0] = mStates.Count;
                mStateNames.Add("(default)");
                mStates.Add(0);

                if (ac != null && layerIndex >= 0 && layerIndex < ac.layers.Length)
                {
                    AnimatorStateMachine fsm = ac.layers[layerIndex].stateMachine;
                    string name = fsm.name;
                    int hash = Animator.StringToHash(name);
                    CollectStatesFromFSM(fsm, name + ".", hash, string.Empty);
                }
            }

            void CollectStatesFromFSM(
                AnimatorStateMachine fsm, string hashPrefix, int parentHash, string displayPrefix)
            {
                ChildAnimatorState[] states = fsm.states;
                for (int i = 0; i < states.Length; i++) 
                {
                    AnimatorState state = states[i].state;
                    int hash = AddState(hashPrefix + state.name, parentHash, displayPrefix + state.name);

                    // Also process clips as pseudo-states, if more than 1 is present.
                    // Since they don't have hashes, we can manufacture some.
                    List<string> clips = CollectClipNames(state.motion);
                    if (clips.Count > 1)
                    {
                        string substatePrefix = displayPrefix + state.name + ".";
                        foreach (string name in clips)
                            AddState(
                                CinemachineStateDrivenCamera.CreateFakeHashName(hash, name), 
                                hash, substatePrefix + name);
                    }
                }

                ChildAnimatorStateMachine[] fsmChildren = fsm.stateMachines;
                foreach (var child in fsmChildren)
                {
                    string name = hashPrefix + child.stateMachine.name;
                    string displayName = displayPrefix + child.stateMachine.name;
                    int hash = AddState(name, parentHash, displayName);
                    CollectStatesFromFSM(child.stateMachine, name + ".", hash, displayName + ".");
                }
            }

            List<string> CollectClipNames(Motion motion)
            {
                List<string> names = new List<string>();
                AnimationClip clip = motion as AnimationClip;
                if (clip != null)
                    names.Add(clip.name);
                BlendTree tree = motion as BlendTree;
                if (tree != null)
                {
                    ChildMotion[] children = tree.children;
                    foreach (var child in children)
                        names.AddRange(CollectClipNames(child.motion));
                }
                return names;
            }

            int AddState(string hashName, int parentHash, string displayName)
            {
                int hash = Animator.StringToHash(hashName);
                if (parentHash != 0)
                    mStateParentLookup[hash] = parentHash;
                mStateIndexLookup[hash] = mStates.Count;
                mStateNames.Add(displayName);
                mStates.Add(hash);
                return hash;
            }
        }

        private int GetStateHashIndex(int stateHash)
        {
            if (stateHash == 0)
                return 0;
            if (!mStateIndexLookup.ContainsKey(stateHash))
                return 0;
            return mStateIndexLookup[stateHash];
        }

        private string[] mCameraCandidates;
        private Dictionary<CinemachineVirtualCameraBase, int> mCameraIndexLookup;
        private void UpdateCameraCandidates()
        {
            List<string> vcams = new List<string>();
            mCameraIndexLookup = new Dictionary<CinemachineVirtualCameraBase, int>();
            vcams.Add("(none)");
            CinemachineVirtualCameraBase[] children = Target.ChildCameras;
            foreach (var c in children)
            {
                mCameraIndexLookup[c] = vcams.Count;
                vcams.Add(c.Name);
            }
            mCameraCandidates = vcams.ToArray();
        }

        private int GetCameraIndex(Object obj)
        {
            if (obj == null || mCameraIndexLookup == null)
                return 0;
            CinemachineVirtualCameraBase vcam = obj as CinemachineVirtualCameraBase;
            if (vcam == null)
                return 0;
            if (!mCameraIndexLookup.ContainsKey(vcam))
                return 0;
            return mCameraIndexLookup[vcam];
        }

        void SetupInstructionList()
        {
            mInstructionList = new UnityEditorInternal.ReorderableList(serializedObject, 
                serializedObject.FindProperty(()=>Target.m_Instructions), 
                true, true, true, true);

            // Needed for accessing field names as strings
            CinemachineStateDrivenCamera.Instruction def = new CinemachineStateDrivenCamera.Instruction();

            float vSpace = 2;
            float hSpace = 3;
            float floatFieldWidth = EditorGUIUtility.singleLineHeight * 2.5f;
            mInstructionList.drawHeaderCallback = (Rect rect) => 
            {  
                rect.width -= (EditorGUIUtility.singleLineHeight + 3*hSpace);
                rect.width -= 2 * floatFieldWidth; rect.width /= 2;
                Vector2 pos = rect.position; pos.x += EditorGUIUtility.singleLineHeight;
                rect.position = pos;
                EditorGUI.LabelField(rect, "State");

                pos.x += rect.width + hSpace; rect.position = pos;
                EditorGUI.LabelField(rect, "Camera");

                pos.x += rect.width + hSpace; rect.width = floatFieldWidth; rect.position = pos;
                EditorGUI.LabelField(rect, "Wait");

                pos.x += rect.width + hSpace; rect.position = pos; 
                EditorGUI.LabelField(rect, "Min");
            };

            mInstructionList.drawElementCallback
                = (Rect rect, int index, bool isActive, bool isFocused) => 
            {
                SerializedProperty instProp 
                    = mInstructionList.serializedProperty.GetArrayElementAtIndex(index);

                rect.y += vSpace; 
                rect.height = EditorGUIUtility.singleLineHeight;
                Vector2 pos = rect.position;
                rect.width -= 3*hSpace;
                rect.width -= 2 * floatFieldWidth; rect.width /= 2;
                SerializedProperty stateSelProp = instProp.FindPropertyRelative(()=>def.m_FullHash);
                int currentState = GetStateHashIndex(stateSelProp.intValue);
                int stateSelection = EditorGUI.Popup(rect, currentState, mTargetStateNames); 
                if (currentState != stateSelection)
                    stateSelProp.intValue = mTargetStates[stateSelection];

                pos.x += rect.width + hSpace; rect.position = pos;
                SerializedProperty vcamSelProp = instProp.FindPropertyRelative(()=>def.m_VirtualCamera);
                int currentVcam = GetCameraIndex(vcamSelProp.objectReferenceValue);
                int vcamSelection = EditorGUI.Popup(rect, currentVcam, mCameraCandidates); 
                if (currentVcam != vcamSelection)
                    vcamSelProp.objectReferenceValue = (vcamSelection == 0) 
                        ? null : Target.ChildCameras[vcamSelection-1];

                pos.x += rect.width + hSpace; rect.width = floatFieldWidth; rect.position = pos;
                SerializedProperty activeAfterProp = instProp.FindPropertyRelative(()=>def.m_ActivateAfter);
                EditorGUI.PropertyField(rect, activeAfterProp, activeAfterLabel);

                pos.x += rect.width + hSpace; rect.position = pos; 
                SerializedProperty minDurationProp = instProp.FindPropertyRelative(()=>def.m_MinDuration);
                EditorGUI.PropertyField(rect, minDurationProp, minDurationLabel);
            };

            mInstructionList.onAddDropdownCallback = (Rect buttonRect, UnityEditorInternal.ReorderableList l) => 
            {  
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("New State"), 
                    false, (object data) => 
                    {
                        ++mInstructionList.serializedProperty.arraySize;
                        serializedObject.ApplyModifiedProperties();
                        Target.ValidateInstructions();
                    }, 
                    null);
                menu.AddItem(new GUIContent("All Unhandled States"), 
                    false, (object data) => 
                    {
                        CinemachineStateDrivenCamera target = Target;
                        int len = mInstructionList.serializedProperty.arraySize;
                        for (int i = 0; i < mTargetStates.Length; ++i)
                        {
                            int hash = mTargetStates[i];
                            if (hash == 0)
                                continue;
                            bool alreadyThere = false;
                            for (int j = 0; j < len; ++j)
                            {
                                if (target.m_Instructions[j].m_FullHash == hash)
                                {
                                    alreadyThere = true;
                                    break;
                                }
                            }
                            if (!alreadyThere)
                            {
                                int index = mInstructionList.serializedProperty.arraySize;
                                ++mInstructionList.serializedProperty.arraySize;
                                SerializedProperty p = mInstructionList.serializedProperty.GetArrayElementAtIndex(index);
                                p.FindPropertyRelative(()=>def.m_FullHash).intValue = hash;
                            }
                        }
                        serializedObject.ApplyModifiedProperties();
                        Target.ValidateInstructions();
                    }, 
                    null);
                menu.ShowAsContext();
            };
        }

        void SetupChildList() 
        {
            float vSpace = 2;
            float hSpace = 3;
            float floatFieldWidth = EditorGUIUtility.singleLineHeight * 2.5f;

            mChildList = new UnityEditorInternal.ReorderableList(serializedObject, 
                serializedObject.FindProperty(()=>Target.m_ChildCameras), 
                true, true, true, true);

            mChildList.drawHeaderCallback = (Rect rect) => 
            {  
                EditorGUI.LabelField(rect, "Virtual Camera Children");
                GUIContent priorityText = new GUIContent("Priority");
                var textDimensions = GUI.skin.label.CalcSize(priorityText);
                rect.x += rect.width - textDimensions.x;
                rect.width = textDimensions.x;
                EditorGUI.LabelField(rect, priorityText);
            };
            mChildList.drawElementCallback
                = (Rect rect, int index, bool isActive, bool isFocused) => 
            {
                rect.y += vSpace;
                Vector2 pos = rect.position;
                rect.width -= floatFieldWidth + hSpace;
                rect.height = EditorGUIUtility.singleLineHeight;
                SerializedProperty element 
                    = mChildList.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(rect, element, GUIContent.none);

                SerializedObject obj = new SerializedObject(element.objectReferenceValue);
                pos.x += rect.width + hSpace; rect.position = pos; 
                rect.width -= floatFieldWidth + hSpace;
                rect.width = floatFieldWidth;
                SerializedProperty priorityProp = obj.FindProperty(()=>Target.m_Priority);
                EditorGUI.PropertyField(rect, priorityProp, GUIContent.none);
                obj.ApplyModifiedProperties();
            };
            mChildList.onChangedCallback = (UnityEditorInternal.ReorderableList l) => 
            { 
                if (l.index < 0 || l.index >= l.serializedProperty.arraySize)
                    return;
                Object o = l.serializedProperty.GetArrayElementAtIndex(
                    l.index).objectReferenceValue;
                CinemachineVirtualCameraBase vcam = (o != null) 
                    ? (o as CinemachineVirtualCameraBase) : null;
                if (vcam != null)
                    vcam.transform.SetSiblingIndex(l.index);
            };
            mChildList.onAddCallback = (UnityEditorInternal.ReorderableList l) => 
            { 
                var index = l.serializedProperty.arraySize;
                var vcam = CinemachineMenu.CreateDefaultVirtualCamera();
                Undo.SetTransformParent(vcam.transform, Target.transform, "");
                vcam.transform.SetSiblingIndex(index);
            };
            mChildList.onRemoveCallback = (UnityEditorInternal.ReorderableList l) => 
            {
                Object o = l.serializedProperty.GetArrayElementAtIndex(
                    l.index).objectReferenceValue;
                CinemachineVirtualCameraBase vcam = (o != null) 
                    ? (o as CinemachineVirtualCameraBase) : null;
                if (vcam != null)
                    Undo.DestroyObjectImmediate(vcam.gameObject);
            };
        }
    }
}
