using System.Collections.Generic;
using UnityEngine;

namespace Polarith.AI.Package
{
    /// <summary>
    /// If an object enters the trigger collider, it is added to a queue and deactivated. Every time the spawn <see
    /// cref="SpawnDelay"/> runs out, the first object of the queue is re-activated and ported to the <see
    /// cref="SpawnArea"/>.
    /// <para/>
    /// Note, this is just a script used for our example scenes and, therefore, not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [AddComponentMenu("Polarith AI » Move » Package/Teleporter")]
    public sealed class Teleporter : MonoBehaviour
    {
        #region Fields =================================================================================================

        /// <summary>
        /// Defines the area in which the objects spawn after being ported. It should be a trigger collider or else the
        /// object might get stuck.
        /// </summary>
        [Tooltip("Defines the area in which the objects spawn after being ported. It should be a trigger " +
            "collider or else the object might get stuck.")]
        public Collider SpawnArea;

        /// <summary>
        /// Time delay in seconds between appearance of two ported objects.
        /// </summary>
        [Tooltip("Time delay in seconds between appearance of two ported objects.")]
        public float SpawnDelay = 0.0f;

        /// <summary>
        /// Vector3 to declare forward direction. Most likely (0,0,1) for 3D scenarios and (0,1,0) for 2D scenarios.
        /// </summary>
        [Tooltip("Vector3 to declare forward direction. Most likely (0,0,1) for 3D scenarios and (0,1,0) for 2D " +
            "scenarios.")]
        public Vector3 Forward = new Vector3(0, 0, 1);

        /// <summary>
        /// Flag to determine if the item should face towards the teleporter after it was ported.
        /// </summary>
        [Tooltip("Flag to determine if the item should face towards the teleporter after it was ported.")]
        public bool FaceTowards = true;

        //--------------------------------------------------------------------------------------------------------------

        private Queue<Transform> teleportQueue = new Queue<Transform>();
        private float currentTime = -1.0f;

        #endregion // Fields

        #region Methods ================================================================================================

        private void Update()
        {
            if (teleportQueue.Count > 0 && currentTime >= SpawnDelay)
            {
                Transform spawningObject = teleportQueue.Dequeue();

                Vector3 spawnPoint;
                spawnPoint.x = Random.Range(SpawnArea.bounds.min.x, SpawnArea.bounds.max.x);
                spawnPoint.y = Random.Range(SpawnArea.bounds.min.y, SpawnArea.bounds.max.y);
                spawnPoint.z = Random.Range(SpawnArea.bounds.min.z, SpawnArea.bounds.max.z);
                spawningObject.position = spawnPoint;

                // Rotate towards teleporter
                if (FaceTowards)
                {
                    Vector3 cross = Vector3.Cross(Forward, transform.position - spawnPoint);
                    float angle = Vector3.Angle(Forward, transform.position - spawnPoint);
                    // check rotation axis
                    if (Mathf.Abs(cross.x) > Mathf.Abs(cross.y) && Mathf.Abs(cross.x) > Mathf.Abs(cross.z))
                        spawningObject.rotation = Quaternion.Euler(angle * Mathf.Sign(cross.x), 0, 0);
                    else if (Mathf.Abs(cross.y) > Mathf.Abs(cross.z))
                        spawningObject.rotation = Quaternion.Euler(0, angle * Mathf.Sign(cross.y), 0);
                    else
                        spawningObject.rotation = Quaternion.Euler(0, 0, angle * Mathf.Sign(cross.z));
                }

                // Set velocity to zero
                Rigidbody body = spawningObject.GetComponent<Rigidbody>();
                if (body != null)
                    spawningObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

                Rigidbody2D body2D = spawningObject.GetComponent<Rigidbody2D>();
                if (body2D != null)
                    spawningObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

                spawningObject.gameObject.SetActive(true);

                currentTime = 0.0f;
            }

            currentTime += Time.deltaTime;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Despawn(collision.transform);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnTriggerEnter(Collider collision)
        {
            Despawn(collision.transform);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Despawn(Transform transform)
        {
            if (teleportQueue.Count == 0)
                currentTime = 0.0f;

            // It puts the transform in the queue or else it gets the hose again
            teleportQueue.Enqueue(transform);
            transform.gameObject.SetActive(false);
        }

        #endregion // Methods
    } // class Teleporter
} // namespace Polarith.AI.Package
