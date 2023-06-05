using UnityEngine;

namespace Polarith.AI.Package
{
    /// <summary>
    /// Simple script which deactivates this <see cref="GameObject"/> when trigger colliding with something else.
    /// <para/>
    /// Note, this is just a script used for our example scenes and, therefore, not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [AddComponentMenu("Polarith AI » Move » Package/Trigger Disable")]
    public sealed class TriggerDisable : MonoBehaviour
    {
        #region Fields =================================================================================================

        /// <summary>
        /// If <c>true</c>, the object is destroyed instead of just deactivated.
        /// </summary>
        [Tooltip("If <c>true</c>, the object is destroyed instead of just deactivated.")]
        public bool RemoveObject;

        #endregion // Fields

        #region Methods ================================================================================================

        private void OnTriggerEnter2D(Collider2D other)
        {
            DeactivateGameObject();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnTriggerEnter(Collider other)
        {
            DeactivateGameObject();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void DeactivateGameObject()
        {
            if (RemoveObject)
                Destroy(gameObject);
            else
                gameObject.SetActive(false);
        }

        #endregion // Methods
    } // class TriggerDisable
} // namespace Polarith.AI.Package
