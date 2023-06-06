using Polarith.AI.Move;
using UnityEngine;

namespace Polarith.AI.Package
{
    /// <summary>
    /// Finds an <see cref="AIMSteeringPerceiver"/> based on the <see cref="PerceiverName"/> and assigns it to an <see
    /// cref="AIMSteeringFilter"/> if it is available. This script is necessary since we cannot use tags in the package.
    /// <para/>
    /// Note, this is just a script used for our example scenes and, therefore, not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [AddComponentMenu("Polarith AI » Move » Package/Perceiver Connector")]
    public sealed class PerceiverConnector : MonoBehaviour
    {
        #region Fields =================================================================================================

        /// <summary>
        /// The name of the <see cref="GameObject"/> that has an <see cref="AIMSteeringPerceiver"/> attached.
        /// </summary>
        public string PerceiverName = "Steering Perceiver";

        #endregion // Fields

        #region Methods ================================================================================================

        private void Start()
        {
            AIMSteeringFilter filter = GetComponent<AIMSteeringFilter>();
            if (filter == null)
                return;
            filter.SteeringPerceiver = GameObject.Find(PerceiverName).GetComponent<AIMSteeringPerceiver>();
        }

        #endregion // Methods
    } // class PerceiverConnector
} // namespace Polarith.AI.Package
