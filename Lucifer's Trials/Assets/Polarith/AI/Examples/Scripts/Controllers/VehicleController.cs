using Polarith.AI.Move;
using UnityEngine;

namespace Polarith.AI.Package
{
    /// <summary>
    /// An AI controller for <see cref="VehiclePhysics"/>. The <see cref="AimContext"/> is used to get the general
    /// target direction and a corresponding acceleration. If <see cref="AimContextForPriority"/> is set, the decision
    /// from this context is used to slow the vehicle down, e.g. to model priority roads or traffic lights. A state
    /// machine with an Integer parameter "Change" can also be attached to limit the braking for priority to certain
    /// situations.
    /// <para/>
    /// Note, this is just a script for our example scenes and, therefore, not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [AddComponentMenu("Polarith AI » Move » Package/Character/Vehicle Controller")]
    [RequireComponent(typeof(VehiclePhysics))]
    public class VehicleController : MonoBehaviour
    {
        #region Fields =================================================================================================

        /// <summary>
        /// The decision of this AI module is passed to <see cref="VehiclePhysics"/> as general movement (direction,
        /// acceleration).
        /// </summary>
        [Tooltip("The decision of this AI module is passed to VehiclePhysics as general movement (direction, " +
            "acceleration).")]
        public AIMContext AimContext;

        /// <summary>
        /// An optional <see cref="AIMContext"/> instance. If set, this vehicle will brake if the decided value is
        /// greater than 0.1f.
        /// </summary>
        [Tooltip("An optional AIMContext instance. If set, this vehicle will brake if the decided value is greater " +
            "than 0.1f.")]
        public AIMContext AimContextForPriority;

        /// <summary>
        /// An optional <see cref="Animator"/> that is used as state machine. Assuming that is has an int parameter
        /// named "Change", it can be used to limit when the vehicle brakes for priority. Here, the priority brakes are
        /// active only if the value of Change is equal to -1.
        /// </summary>
        [Tooltip("An optional Animator that is used as state machine. Assuming that is has an int parameter named " +
            "'Change', it can be used to limit when the vehicle brakes for priority. Here, the priority brakes are " +
            "active only if the value of Change is equal to -1.")]
        public Animator StateMachine;

        /// <summary>
        /// The <see cref="Rigidbody"/> of the car.
        /// </summary>
        [Tooltip("The Rigidbody of the car.")]
        public Rigidbody Body;

        //--------------------------------------------------------------------------------------------------------------

        private VehiclePhysics vehiclePhysics;

        #endregion // Fields

        #region Methods ================================================================================================

        private void Awake()
        {
            vehiclePhysics = GetComponent<VehiclePhysics>();

            if (Body == null)
                Body = GetComponent<Rigidbody>();

            if (Body == null)
            {
                Debug.LogWarning("VehicleController is deactivated because a reference to the Body (Rigidbody) is " +
                    "missing.");
                enabled = false;
                return;
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        private void FixedUpdate()
        {
            float inputAcceleration = (AimContext.DecidedValues[0] - 0.5f) * 2;
            float inputSteeringAngle = -Vector3.Cross(AimContext.DecidedDirection, transform.forward).y;

            // If the vehicle brakes, always put the pedal to the metal
            if (inputAcceleration < 0.0f)
                inputAcceleration = -1.0f;

            if (AimContextForPriority != null)
            {
                bool usePriority = false;

                // Reduce speed in breaking zones when other car is detected
                if (StateMachine != null && StateMachine.GetInteger("Change") == -1)
                    usePriority = true;
                else if (StateMachine == null)
                    usePriority = true;

                // reduce speed when priority detects an object
                if (usePriority && AimContextForPriority != null && AimContextForPriority.DecidedValues[0] > 0.1f)
                {
                    inputAcceleration = -1.0f;
                }
            }

            // Avoid driving in reverse
            if (Vector3.Angle(Body.velocity, transform.forward) > 90.0f)
                inputAcceleration = 1.0f;

            vehiclePhysics.Move(inputSteeringAngle, inputAcceleration, inputAcceleration);
        }

        #endregion // Methods
    } // class VehicleController
} // namespace Polarith.AI.Package
