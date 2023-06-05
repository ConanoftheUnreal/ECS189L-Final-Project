using UnityEditor;

namespace Polarith.AI.Package
{
    /// <summary>
    /// Custom inspector of <see cref="VehiclePhysics"/>.
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(VehiclePhysics))]
    public class VehiclePhysicsEditor : Editor
    {
        #region Fields =================================================================================================

        private SerializedProperty wheelColliders;
        private SerializedProperty wheelMeshes;
        private SerializedProperty motorWheels;
        private SerializedProperty brakeWheels;
        private SerializedProperty steeringWheels;
        private SerializedProperty maximumSteerAngle;
        private SerializedProperty steerHelper;
        private SerializedProperty tractionControl;
        private SerializedProperty fullTorqueOverAllWheels;
        private SerializedProperty reverseTorque;
        private SerializedProperty downforce;
        private SerializedProperty topspeed;
        private SerializedProperty slipLimit;
        private SerializedProperty brakeTorque;

        #endregion // Fields

        #region Methods ================================================================================================

        /// <summary>
        /// Display the control elements in the inspector.
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(wheelColliders, true);
            EditorGUILayout.PropertyField(wheelMeshes, true);
            if (wheelColliders.arraySize != wheelMeshes.arraySize)
                EditorGUILayout.HelpBox("Unequal size of collider and meshes.", MessageType.Warning);

            EditorGUILayout.PropertyField(motorWheels, true);
            EditorGUILayout.PropertyField(brakeWheels, true);
            EditorGUILayout.PropertyField(steeringWheels, true);
            EditorGUILayout.PropertyField(maximumSteerAngle);
            EditorGUILayout.PropertyField(steerHelper);
            EditorGUILayout.PropertyField(tractionControl);
            EditorGUILayout.PropertyField(fullTorqueOverAllWheels);
            EditorGUILayout.PropertyField(reverseTorque);
            EditorGUILayout.PropertyField(downforce);
            EditorGUILayout.PropertyField(topspeed);
            EditorGUILayout.PropertyField(slipLimit);
            EditorGUILayout.PropertyField(brakeTorque);

            serializedObject.ApplyModifiedProperties();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnEnable()
        {
            wheelColliders = serializedObject.FindProperty("WheelColliders");
            wheelMeshes = serializedObject.FindProperty("WheelMeshes");
            motorWheels = serializedObject.FindProperty("MotorWheels");
            brakeWheels = serializedObject.FindProperty("BrakeWheels");
            steeringWheels = serializedObject.FindProperty("SteeringWheels");
            maximumSteerAngle = serializedObject.FindProperty("MaximumSteerAngle");
            steerHelper = serializedObject.FindProperty("SteerHelper");
            tractionControl = serializedObject.FindProperty("TractionControl");
            fullTorqueOverAllWheels = serializedObject.FindProperty("FullTorqueOverAllWheels");
            reverseTorque = serializedObject.FindProperty("ReverseTorque");
            downforce = serializedObject.FindProperty("Downforce");
            topspeed = serializedObject.FindProperty("Topspeed");
            slipLimit = serializedObject.FindProperty("SlipLimit");
            brakeTorque = serializedObject.FindProperty("BrakeTorque");
        }

        #endregion // Methods
    } // class VehiclePhysicsEditor
} // namespace Polarith.AI.Package
