using Polarith.AI.Package;
using UnityEditor;

namespace Polarith.AI.PackageEditor
{
    /// <summary>
    /// Custom inspector of <see cref="ObjectSpawner"/>.
    /// </summary>
    [CustomEditor(typeof(ObjectSpawner))]
    public class ObjectSpawnerEditor : Editor
    {
        #region Fields =================================================================================================

        private SerializedProperty spawningObject;
        private SerializedProperty parent;
        private SerializedProperty instantiation;
        private SerializedProperty maximumObjects;
        private SerializedProperty spawnArea;
        private SerializedProperty xzSpawn;
        private SerializedProperty spawnInterval;
        private SerializedProperty restriction;
        private SerializedProperty navMeshAreaMask;
        private SerializedProperty enableGizmo;

        #endregion // Fields

        #region Methods ====================================================================================================

        /// <summary>
        /// Display the control elements in the inspector.
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(spawningObject);
            if (spawningObject.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("The spawning objected is required as a template for the spawning objects.",
                    MessageType.Info);
                EditorGUILayout.Separator();
            }

            parent.isExpanded = EditorGUILayout.Foldout(parent.isExpanded, "Where");
            if (parent.isExpanded)
            {
                EditorGUILayout.PropertyField(parent);
                EditorGUILayout.PropertyField(maximumObjects);
                EditorGUILayout.PropertyField(spawnArea);
                EditorGUILayout.PropertyField(xzSpawn);
            }

            instantiation.isExpanded = EditorGUILayout.Foldout(instantiation.isExpanded, "How");
            if (instantiation.isExpanded)
            {
                EditorGUILayout.PropertyField(instantiation);
                if (restriction.enumValueIndex == (int)ObjectSpawner.InstantationType.Interval)
                    EditorGUILayout.PropertyField(spawnInterval);

                EditorGUILayout.PropertyField(restriction);
                if (restriction.enumValueIndex == (int)ObjectSpawner.RestrictionType.NavMeshArea)
                    EditorGUILayout.PropertyField(navMeshAreaMask);

                EditorGUILayout.PropertyField(enableGizmo);
            }

            serializedObject.ApplyModifiedProperties();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnEnable()
        {
            spawningObject = serializedObject.FindProperty("SpawningObject");
            parent = serializedObject.FindProperty("Parent");
            instantiation = serializedObject.FindProperty("Instantiation");
            maximumObjects = serializedObject.FindProperty("MaximumObjects");
            spawnArea = serializedObject.FindProperty("SpawnArea");
            xzSpawn = serializedObject.FindProperty("XZSpawn");
            spawnInterval = serializedObject.FindProperty("SpawnInterval");
            restriction = serializedObject.FindProperty("Restriction");
            navMeshAreaMask = serializedObject.FindProperty("NavMeshAreaMask");
            enableGizmo = serializedObject.FindProperty("EnableGizmo");
        }

        #endregion // Methods
    } // class ObjectSpawnerEditor
} // namespace Polarith.AI.PackageEditor
