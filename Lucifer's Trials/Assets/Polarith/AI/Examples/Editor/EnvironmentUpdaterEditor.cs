using Polarith.AI.Package;
using UnityEditor;

namespace Polarith.AI.PackageEditor
{
    /// <summary>
    /// Custom inspector of <see cref="EnvironmentUpdater"/>.
    /// </summary>
    [CustomEditor(typeof(EnvironmentUpdater))]
    public class EnvironmentUpdaterEditor : Editor
    {
        #region Fields =================================================================================================

        private SerializedProperty gameobjectCollection;
        private SerializedProperty targetEnvironment;
        private SerializedProperty isDynamic;

        #endregion // Fields

        #region Methods ================================================================================================

        /// <summary>
        /// Display the control elements in the inspector.
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(gameobjectCollection, true);
            if (gameobjectCollection.arraySize == 0)
            {
                EditorGUILayout.HelpBox("Without game object references this component won't work. The children of " +
                    "these objects are assigned to the Target Environment. The object itself is ignored.",
                    MessageType.Warning);
                EditorGUILayout.Separator();
            }

            EditorGUILayout.PropertyField(targetEnvironment);
            if (targetEnvironment.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("This value cannot be null. An AIMEnvironment instance must be referenced.",
                    MessageType.Warning);
                EditorGUILayout.Separator();
            }

            EditorGUILayout.PropertyField(isDynamic);

            serializedObject.ApplyModifiedProperties();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnEnable()
        {
            gameobjectCollection = serializedObject.FindProperty("GameObjectCollections");
            targetEnvironment = serializedObject.FindProperty("TargetEnvironment");
            isDynamic = serializedObject.FindProperty("IsDynamic");
        }

        #endregion // Methods
    } // class EnvironmentUpdaterEditor
} // namespace Polarith.AI.PackageEditor
