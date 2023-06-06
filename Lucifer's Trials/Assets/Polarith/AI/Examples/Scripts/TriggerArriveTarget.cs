using Polarith.AI.Move;
using UnityEngine;

namespace Polarith.AI.Package
{
    /// <summary>
    /// Keeps track objects passing the trigger collider of this object. If a passing object has an <see
    /// cref="AIMArrive"/> behaviour, the <see cref="AIMArrive.Target"/> is set to the object that previously passed the
    /// trigger. Only objects are tracked that have a component of <see cref="TriggerType"/> attached.
    /// <para/>
    /// Note, this is just a script used for our example scenes and, therefore, not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [AddComponentMenu("Polarith AI » Move » Package/Trigger Arrive Target")]
    public sealed class TriggerArriveTarget : MonoBehaviour
    {
        #region Fields =================================================================================================

        /// <summary>
        /// On Trigger, it is checked if the other game object has a component of this type ( <see
        /// cref="GameObject.GetComponent(string)"/>), If no such component can be found, the object is ignored.
        /// </summary>
        public string TriggerType = "VehiclePhysics";

        //--------------------------------------------------------------------------------------------------------------

        private GameObject previous = null;

        #endregion // Fields

        #region Methods ================================================================================================

        private void OnTriggerEnter(Collider collision)
        {
            Transform otherObject = collision.transform;

            Component component = otherObject.GetComponent(TriggerType);
            if (component == null)
                return;

            AIMArrive arrive = otherObject.GetComponentInChildren<AIMArrive>();
            if (previous != null && arrive != null)
                arrive.Target = previous;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnTriggerExit(Collider collision)
        {
            Transform otherObject = collision.transform;

            Component component = otherObject.GetComponent(TriggerType);
            if (component == null)
                return;

            previous = otherObject.gameObject;
        }

        #endregion // Methods
    } // class TriggerArriveTarget
} // namespace Polarith.AI.Package
