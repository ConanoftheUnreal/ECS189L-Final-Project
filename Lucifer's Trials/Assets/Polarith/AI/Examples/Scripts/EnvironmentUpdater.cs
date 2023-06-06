using Polarith.AI.Move;
using UnityEngine;

namespace Polarith.AI.Package
{
    /// <summary>
    /// A script that is used to update the <see cref="AIMEnvironment.GameObjects"/> list. This is done by assigning all
    /// child objects of each game object in the <see cref="GameObjectCollections"/> list to the <see
    /// cref="AIMEnvironment.GameObjects"/> list. However, the size of the <see cref="AIMEnvironment.GameObjects"/>
    /// lists remains unchanged. Therefore, the <see cref="AIMEnvironment.GameObjects"/> list must be set to an
    /// appropriate size before this component is activated.
    /// <para/>
    /// A common technique to improve the performance of a game is pooling. Pooling is a method of avoiding memory
    /// allocation at runtime. This component is a possible solution for combining your pooling method and the
    /// perception pipeline of Polarith AI. By simple changing the parent of an object it can be classified as "dead" or
    /// "alive". If the objects parent is in the <see cref="GameObjectCollections"/> list, it is alive and dead
    /// otherwise.
    /// <para/>
    /// Note, this is just a script used for our example scenes and, therefore, not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [AddComponentMenu("Polarith AI » Move » Package/Environment Updater")]
    public sealed class EnvironmentUpdater : MonoBehaviour
    {
        #region Fields =================================================================================================

        /// <summary>
        /// All children of these objects are added to the specified <see cref="TargetEnvironment"/>.
        /// </summary>
        [Tooltip("All children of these objects are added to the specified 'TargetEnvironment'.")]
        public GameObject[] GameObjectCollections;

        /// <summary>
        /// The <see cref="AIMEnvironment.GameObjects"/> of this environment are updated via this component.
        /// </summary>
        [Tooltip("The AIMEnvironment.GameObjects of this environment are updated via this component.")]
        public AIMEnvironment TargetEnvironment;

        /// <summary>
        /// If <c>true</c>, the objects are refreshed on every update step.
        /// </summary>
        [Tooltip("If <c>true</c>, the objects are refreshed on every update step.")]
        public bool IsDynamic = false;

        //--------------------------------------------------------------------------------------------------------------

        #endregion // Fields

        #region Methods ================================================================================================

        private void Start()
        {
            if (!IsDynamic)
            {
                ProcessObjects();
                enabled = false;
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Update()
        {
            ProcessObjects();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void ProcessObjects()
        {
            // The current AIMEnvironment.Gameobjects index
            int objectIndex = 0;

            for (int i = 0; i < GameObjectCollections.Length; i++)
            {
                for (int j = 0; j < GameObjectCollections[i].transform.childCount; j++)
                {
                    if (objectIndex + j >= TargetEnvironment.GameObjects.Count)
                        return;

                    TargetEnvironment.GameObjects[objectIndex + j] =
                        GameObjectCollections[i].transform.GetChild(j).gameObject;
                }
                objectIndex += GameObjectCollections[i].transform.childCount;
            }
        }

        #endregion // Methods
    } // class EnvironmentUpdater
} // namespace Polarith.AI.Package
