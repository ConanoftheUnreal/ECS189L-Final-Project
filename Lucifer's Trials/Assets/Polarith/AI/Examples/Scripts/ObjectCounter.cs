using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Polarith.AI.Package
{
    /// <summary>
    /// A script that helps to keep track of the number of certain objects. All objects that are children of the given
    /// <see cref="ObjectCluster"/> instances are counted. The object number is displayed via the <see
    /// cref="DisplayText"/>.
    /// <para/>
    /// Note, this is just a script used for our example scenes and, therefore, not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [AddComponentMenu("Polarith AI » Move » Package/Object Counter")]
    public sealed class ObjectCounter : MonoBehaviour
    {
        #region Fields =================================================================================================

        /// <summary>
        /// A <see cref="Text"/> object that if assigned displays the number of objects.
        /// </summary>
        [Tooltip("A <see Text object that if assigned displays the number of objects.")]
        public Text DisplayText;

        /// <summary>
        /// For every object in this list the child objects are counted. The sum is number of objects.
        /// </summary>
        [Tooltip("For every object in this list the child objects are counted. The sum is number of objects.")]
        public List<GameObject> ObjectCluster;

        #endregion // Fields

        #region Methods ================================================================================================

        private void Update()
        {
            int count = 0;
            foreach (GameObject obj in ObjectCluster)
            {
                count += obj.transform.childCount;
            }
            if (DisplayText != null)
                DisplayText.text = "Pedestrians: " + count.ToString();
        }

        #endregion // Methods
    } // class ObjectCounter
} // namespace Polarith.AI.Package
