using System;
using System.Linq;

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace WiiFly.Editor {
    /// <summary>
    /// Base class for <c>Editor</c>s that have a "Select Implementation" layout.
    /// </summary>
    /// <typeparam name="A">Type of the class that has this editor.</typeparam>
    /// <typeparam name="B">Type of the implementation.</typeparam>
    public abstract class SelectImplementationEditor<A, B> : UnityEditor.Editor where A : UnityEngine.Object {
        #region Fields
        /// <summary>
        /// Array of available implementations.
        /// </summary>
        private Type[] implementations;

        /// <summary>
        /// Index of the currently selected implementation.
        /// </summary>
        private int implementationTypeIndex;

        /// <summary>
        /// Reference to the class that uses this editor.
        /// </summary>
        protected A behaviour;

        /// <summary>
        /// Reference to the reorderable list.
        /// </summary>
        protected ReorderableList reorderableList; 

        /// <summary>
        /// Name of the list attribute.
        /// </summary>
        private string listName;

        /// <summary>
        /// Visible name of the list (displayed on the editor).
        /// </summary>
        private string caption;

        /// <summary>
        /// Indicates if it is not allowed to have multiple instances of the same class.
        /// </summary>
        private bool uniqueImplementations = false;
        #endregion

        #region Unity
        protected virtual void OnEnable() {
            this.behaviour = this.target as A;
            SerializedProperty listProperty = serializedObject.FindProperty(this.listName);
            this.reorderableList = new ReorderableList(this.serializedObject, listProperty, true, true, false, true);
            this.reorderableList.drawHeaderCallback = rect => {
                EditorGUI.LabelField(rect, this.caption, EditorStyles.boldLabel);
            };
            this.reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
                
                EditorGUI.indentLevel = 1;
                SerializedProperty property = listProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(rect, property, new GUIContent(property.managedReferenceFullTypename.Split('.').Last()), true);
            };
            this.reorderableList.elementHeightCallback = (int index) => {
                SerializedProperty property = listProperty.GetArrayElementAtIndex(index);
                return this.reorderableList.elementHeight * property.CountInProperty();
            };
        }

        public override void OnInspectorGUI() {
            this.serializedObject.Update();
            this.DrawInterface();
            this.reorderableList.DoLayoutList();        
            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region Initialization
        protected abstract void AddEditorImplementation(B implementation);

        /// <summary>
        /// Initializes the generic Select Implementation editor with the given values.
        /// </summary>
        /// <param name="listName">Name of the field that stores the selected implementations.</param>
        /// <param name="caption">Caption to be displayed as the title of the list at the inspector.</param>
        /// <param name="uniqueImplementations">Boolean indicating if it is possible or not to have multiple implementations of the same class.</param>
        protected void Initialize(string listName, string caption, bool uniqueImplementations) {
            this.listName = listName;
            this.caption = caption;
            this.uniqueImplementations = uniqueImplementations;
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Creates an implementation of the given type.
        /// </summary>
        /// <param name="type">Type of the implementation to be instantiated.</param>
        /// <returns>Instantiated implementation.</returns>
        protected abstract B CreateImplementation(Type type);

        /// <summary>
        /// Draws the interface of the editor.
        /// </summary>
        private void DrawInterface() {
            if (this.behaviour == null) return;
            
            if (implementations == null || GUILayout.Button("Refresh Implementations")) {
                // Find all implementations of WorldObjectAnimatorComponent using System.Reflection.Module
                implementations = GetImplementations().Where(impl => !impl.IsSubclassOf(typeof(UnityEngine.Object))).ToArray();
            }

            EditorGUILayout.LabelField($"Found {implementations.Length} implementations.");            
            
            // Select implementation from editor popup
            implementationTypeIndex = EditorGUILayout.Popup(new GUIContent("Implementation"),
                implementationTypeIndex, implementations.Select(impl => impl.Name).ToArray());

            if (GUILayout.Button("Create Instance")) {
                if (this.uniqueImplementations && this.Contains(implementations[implementationTypeIndex].FullName)) {
                    Debug.LogWarning("Invalid Operation. This list must contain a unique implementation for each class.");
                } else {
                    this.AddEditorImplementation(this.CreateImplementation(implementations[implementationTypeIndex]));
                }
            }
        }

        /// <summary>
        /// Retreives all the available implementations of the base class.
        /// </summary>
        /// <returns>Array with all the available implementations.</returns>
        private static Type[] GetImplementations() {
            System.Collections.Generic.IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());
            Type interfaceType = typeof(B);
            return types.Where(p => interfaceType.IsAssignableFrom(p) && !p.IsAbstract).ToArray();
        }

        /// <summary>
        /// Checks if the list of instantiated implementations contains the given type.
        /// </summary>
        /// <param name="typeFullname">Fullname of the class type.</param>
        /// <returns>Boolean indicating if the list of instantiated implementations contains the given type.</returns>
        private bool Contains(string typeFullname) {
            for (int i = 0; i < this.reorderableList.serializedProperty.arraySize; ++i) {
                if (this.reorderableList.serializedProperty.GetArrayElementAtIndex(i).managedReferenceFullTypename.Split(' ').Last().Equals(typeFullname))
                    return true;
            }
            return false;
        }
        #endregion
    }
}