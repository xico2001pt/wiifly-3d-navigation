using System;

using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using WiiFly.Camera;
using WiiFly.Camera.Mode;

namespace WiiFly.Editor {
    [CustomEditor(typeof(CameraController))]
    public class CameraControllerEditor : SelectImplementationEditor<CameraController, ICameraMode> {
        #region Unity Methods
        protected void Awake() {
            this.Initialize("_cameraModes", "Camera Modes", true);
        }

        protected override void OnEnable() {
            base.OnEnable();
            this.reorderableList.onRemoveCallback = (ReorderableList list) => {
                int index = list.index;
                if (index < 0 || index > list.count)
                    return;
                
                SerializedProperty serializedProperty = list.serializedProperty;
                
                ICameraMode component = (ICameraMode) serializedProperty.GetArrayElementAtIndex(index).managedReferenceValue;

                serializedProperty.DeleteArrayElementAtIndex(index);
                list.index = -1;
            };
        }

        public override void OnInspectorGUI() {
            DrawPropertiesExcluding(serializedObject, "_cameraModes");
            serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
        }

        #endregion

        #region Initialization
        protected override void AddEditorImplementation(ICameraMode component) {
            this.behaviour.AddMode(component);
        }
        #endregion

        #region Helpers
        protected override ICameraMode CreateImplementation(Type type) {
            object[] args = {};
            object o = Activator.CreateInstance(type, args);
            return (ICameraMode) o;
        }
        #endregion
    }
}