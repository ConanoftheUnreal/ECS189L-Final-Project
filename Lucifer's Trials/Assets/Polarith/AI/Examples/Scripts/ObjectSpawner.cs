using UnityEngine;
using UnityEngine.AI;

namespace Polarith.AI.Package
{
    /// <summary>
    /// This component is used to spawn randomly distributed game objects inside a certain area, defined by <see
    /// cref="SpawnArea"/>.The scenes Navmesh is used to further restrict the spawning area inside the <see
    /// cref="SpawnArea"/>.
    /// <para/>
    /// <see cref="SpawnArea"/> defines the spawning area. This area is in the XY-plane if <see cref="XZSpawn"/> is
    /// <c>false</c> and in the XZ-plane if <see cref="XZSpawn"/> is true.
    /// <para/>
    /// The <see cref="ObjectSpawner"/> either spawns a fixed <see cref="objectCount"/> instances on start or
    /// dynamically spawns instances with a given <see cref="SpawnInterval"/>. Furthermore, the object instances may
    /// spawn as children of the <see cref="Parent"/> object.
    /// <para/>
    /// Note, this is just a script used for our example scenes and, therefore, not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [AddComponentMenu("Polarith AI » Move » Package/Object Spawner")]
    public sealed class ObjectSpawner : MonoBehaviour
    {
        #region Fields =================================================================================================

        /// <summary>
        /// The reference to the game object or prefab that is spawned.
        /// </summary>
        [Tooltip("The reference to the game object or prefab that is spawned.")]
        public GameObject SpawningObject;

        /// <summary>
        /// The parent object of the spawned objects. Can be set to <c>null</c> if no parent should be assigned.
        /// </summary>
        [Tooltip("The parent object of the spawned objects. Can be set to null if no parent should be assigned. ")]
        public GameObject Parent;

        /// <summary>
        /// The maximum count of active instances spawned. As long as there are more instances of <see
        /// cref="SpawningObject"/> under the <see cref="Parent"/>, no objects are spawned.
        /// </summary>
        public int MaximumObjects = 50;

        /// <summary>
        /// A rectangle defining the spawning area.
        /// </summary>
        [Tooltip("A rectangle defining the spawning area.")]
        public Rect SpawnArea;

        /// <summary>
        /// If <c>true</c>, the spawning area will be flipped into the XZ-plane. Otherwise, the spawning area is in the
        /// XY-plane.
        /// </summary>
        [Tooltip("If true, the spawning area will be flipped into the XZ-plane. Otherwise, the spawning area is in " +
            "the XY-plane.")]
        public bool XZSpawn;

        /// <summary>
        /// Determines if all objects are either spawned at once or spawned over a given time span.
        /// </summary>
        [Tooltip("Determines if all objects are either spawned at once or spawned over a given time span.")]
        public InstantationType Instantiation = InstantationType.Instant;

        /// <summary>
        /// The time between 2 object spawns. Only used if <see cref="Instantiation"/> is set to <c><see
        /// cref="InstantationType.Interval"/></c>.
        /// </summary>
        [Tooltip("The time between 2 object spawns. Only used if Instantiation is set to InstantationType.OverTime.")]
        public float SpawnInterval;

        /// <summary>
        /// Determines which restrictions are applied to the spawning area, like excluding specific Navigation Areas.
        /// </summary>
        [Tooltip("Determines which restrictions are applied to the spawning area, like excluding specific " +
            "Navigation Areas.")]
        public RestrictionType Restriction = ObjectSpawner.RestrictionType.Nothing;

        /// <summary>
        /// Determines the Navigation Areas where objects can be spawned.
        /// </summary>
        [Polarith.UnityUtils.NavMeshAreaMaskAttribute]
        [Tooltip("Determines the Navigation Areas where objects can be spawned.")]
        public int NavMeshAreaMask = -1;

        /// <summary>
        /// If <c>true</c>, the <see cref="SpawnArea"/> is visualized.
        /// </summary>
        [Tooltip("If true, the Spawn Area is visualized.")]
        public bool EnableGizmo = false;

        //--------------------------------------------------------------------------------------------------------------

        private float currentTime;

        #endregion // Fields

        #region Enums ==================================================================================================

        /// <summary>
        /// Defines which restriction is used to reject random positions in the <see cref="SpawnArea"/>.
        /// </summary>
        [System.Flags]
        public enum RestrictionType
        {
            /// <summary>
            /// No restriction, the game objects can be spawned everywhere inside the <see cref="SpawnArea"/>.
            /// </summary>
            Nothing = 0,

            /// <summary>
            /// The objects can not be spawned on specific regions. These regions are determined with <see
            /// cref="NavMeshAreaMask"/>.
            /// </summary>
            NavMeshArea = 1
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Used to decide how the objects are spawned, on an <see cref="InstantationType.Instant"/> or in a given <see
        /// cref="InstantationType.Interval"/>.
        /// </summary>
        public enum InstantationType
        {
            /// <summary>
            /// All objects are spawned inside the <see cref="Start"/> method. The maximum number of objects is
            /// restricted by <see cref="objectCount"/>.
            /// </summary>
            Instant = 0,

            /// <summary>
            /// The objects are spawned constantly.The interval between the spawning processes is defined by <see
            /// cref="SpawnInterval"/>.
            /// </summary>
            Interval = 1
        }

        #endregion // Enums

        #region Methods ================================================================================================

        /// <summary>
        /// This method starts a spawning process.
        /// </summary>
        /// <returns>
        /// <c>True</c> if an object was spawned. Otherwise <c>false</c>, if the position was rejected.
        /// </returns>
        public bool SpawnObject()
        {
            float xz = 0.0f;
            float xy = 1.0f;
            if (XZSpawn)
            {
                xz = 1.0f;
                xy = 0.0f;
            }

            Vector3 randomPosition = new Vector3(
                       Random.Range(SpawnArea.x, SpawnArea.x + SpawnArea.width),
                       xy * Random.Range(SpawnArea.y, SpawnArea.y + SpawnArea.height),
                       xz * Random.Range(SpawnArea.y, SpawnArea.y + SpawnArea.height));

            if (ValidatePosition(randomPosition))
            {
                GameObject obj = Instantiate(SpawningObject);

                if (Parent != null)
                    obj.transform.SetParent(Parent.transform);

                obj.transform.position = randomPosition;

                return true;
            }
            else
                return false;
        }

        //--------------------------------------------------------------------------------------------------------------

        // Determines if the given position is rejected or not, based on the
        // <see cref="Restriction"/>
        // . Returns
        // <c>true</c>
        // if the position is valid.
        private bool ValidatePosition(Vector3 position)
        {
            if (Restriction == RestrictionType.NavMeshArea)
            {
                // Check if point is on a valid NavMeshArea
                NavMeshHit navMeshAreaHitPoint;

                if (NavMesh.SamplePosition(
                       position,
                       out navMeshAreaHitPoint,
                       0.1f,
                       NavMeshAreaMask))
                {
                    // Check for safety margin
                    NavMeshHit closestHitEdge;
                    NavMesh.FindClosestEdge(position, out closestHitEdge, ~NavMeshAreaMask);

                    if (closestHitEdge.distance > 0.1f)
                        return true;
                }

                return false;
            }
            else
                return true;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            if (Instantiation == InstantationType.Instant)
            {
                for (int i = 0; i < MaximumObjects; i++)
                    SpawnObject();

                enabled = false;
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Update()
        {
            if (Instantiation == InstantationType.Instant || Parent.transform.childCount >= MaximumObjects)
                return;

            if (currentTime >= SpawnInterval && SpawnObject())
                currentTime = 0.0f;

            currentTime += Time.deltaTime;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnDrawGizmosSelected()
        {
            float xz = 0.0f;
            float xy = 1.0f;
            if (XZSpawn)
            {
                xz = 1.0f;
                xy = 0.0f;
            }

            if (EnableGizmo)
            {
                Gizmos.color = Color.green;

                Vector3 pos = gameObject.transform.position;

                // Top
                Gizmos.DrawLine(new Vector3(SpawnArea.x, xy * SpawnArea.y + xz * pos.y, xz * SpawnArea.y + xy * pos.z),
                    new Vector3(SpawnArea.xMax, xy * SpawnArea.y + xz * pos.y, xz * SpawnArea.y + xy * pos.z));
                // Bot
                Gizmos.DrawLine(new Vector3(SpawnArea.x, xy * SpawnArea.yMax + xz * pos.y, xz * SpawnArea.yMax + xy * pos.z),
                    new Vector3(SpawnArea.xMax, xy * SpawnArea.yMax + xz * pos.y, xz * SpawnArea.yMax + xy * pos.z));
                // Left
                Gizmos.DrawLine(new Vector3(SpawnArea.x, xy * SpawnArea.y + xz * pos.y, xz * SpawnArea.y + xy * pos.z),
                    new Vector3(SpawnArea.x, xy * SpawnArea.yMax + xz * pos.y, xz * SpawnArea.yMax + xy * pos.z));
                // Right
                Gizmos.DrawLine(new Vector3(SpawnArea.xMax, xy * SpawnArea.y + xz * pos.y, xz * SpawnArea.y + xy * pos.z),
                    new Vector3(SpawnArea.xMax, xy * SpawnArea.yMax + xz * pos.y, xz * SpawnArea.yMax + xy * pos.z));
            }
        }

        #endregion // Methods
    } // class RandomObjects
} // namespace Polarith.AI.Package
