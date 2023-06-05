using UnityEngine;

namespace Polarith.AI.Package
{
    /// <summary>
    /// Script that handles transition between AI States using an int parameter named 'Change'. 'OnTriggerEnter',
    /// 'Change' is always set to -1 and stays at this while the object is inside the trigger. 'OnTriggerExit', 'Change'
    /// is set to the specified value given by <see cref="Change"/>.
    /// <para/>
    /// Note, this is just a script used for our example scenes and, therefore, not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [AddComponentMenu("Polarith AI » Move » Package/Trigger State Transition")]
    public sealed class TriggerStateTransition : MonoBehaviour
    {
        #region Fields =================================================================================================

        /// <summary>
        /// 'OnTriggerEnter', it is checked if the other game object has a component of this type ( <see
        /// cref="GameObject.GetComponent(string)"/>), If no such component can be found, the object is ignored.
        /// </summary>
        public string TriggerType = "VehiclePhysics";

        /// <summary>
        /// Value to which the Change parameter is set in 'OnTriggerExit'.
        /// </summary>
        [Tooltip("Value to which the Change parameter is set in 'OnTriggerExit'.")]
        public int Change = 0;

        /// <summary>
        /// Probability that a state transition happens.
        /// </summary>
        [Range(0, 1)]
        [Tooltip("Probability that a state transition happens.")]
        public float Probability = 0.5f;

        /// <summary>
        /// If <c>true</c>, a value of -1 is passed to the triggering object's animator instead of <see cref="Change"/>
        /// as long as the other object stays in this collider.
        /// </summary>
        [Tooltip("If true, a value of -1 is passed to the triggering object's animator instead of Change as long as " +
            "the other object stays in this collider.")]
        public bool IsBrakingZone = true;

        #endregion // Fields

        #region Methods ================================================================================================

        private void OnTriggerEnter(Collider collider)
        {
            Component component = collider.GetComponent(TriggerType);
            if (component == null)
                return;

            Animator animator = collider.GetComponentInChildren<Animator>();
            float random = Random.Range(0.0f, 1.0f);
            if (animator != null && random < Probability && IsBrakingZone)
                animator.SetInteger("Change", -1);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnTriggerExit(Collider collider)
        {
            Component component = collider.GetComponent(TriggerType);
            if (component == null)
                return;

            Animator animator = collider.GetComponentInChildren<Animator>();
            float random = Random.Range(0.0f, 1.0f);
            if (animator != null && random < Probability)
                animator.SetInteger("Change", Change);
        }

        #endregion // Methods
    } // class TriggerStateTransition
} // namespace Polarith.AI.Package
