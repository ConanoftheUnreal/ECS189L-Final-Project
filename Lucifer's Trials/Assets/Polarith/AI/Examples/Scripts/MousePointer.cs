using UnityEngine;

namespace Polarith.AI.Package
{
    /// <summary>
    /// Calculates the current mouse position for 2D scenes. It is also possible to display a crosshair at that
    /// position.
    /// <para/>
    /// Note, this is just a script used for our example scenes and, therefore, not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [AddComponentMenu("Polarith AI » Move » Package/Teleporter")]
    public sealed class MousePointer : MonoBehaviour
    {
        #region Fields =================================================================================================

        /// <summary>
        /// An optional reference to a <see cref="SpriteRenderer"/> representing a crosshair.
        /// </summary>
        [Tooltip("An optional reference to a SpriteRenderer representing a crosshair.")]
        public SpriteRenderer CrosshairRenderer;

        //--------------------------------------------------------------------------------------------------------------

        private Vector3 position;

        #endregion // Fields

        #region Properties =============================================================================================

        /// <summary>
        /// Returns the mouse position on the XY-plane (2D scenes). (Read Only)
        /// </summary>
        public Vector3 Position
        {
            get { return position; }
        }

        #endregion // Properties

        #region Methods ================================================================================================

        private void Update()
        {
            // Get camera ray and the interactive plane
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane xy = new Plane(Vector3.forward, new Vector3(0.0f, 0.0f, 0.0f));

            // Cast the ray to get the mouse position on the plane
            float distance;
            xy.Raycast(ray, out distance);
            position = ray.GetPoint(distance);

            // Set the crosshair to mouse position
            if (CrosshairRenderer != null)
            {
                CrosshairRenderer.transform.position
                    = new Vector3(position.x, position.y, CrosshairRenderer.transform.position.z);
            }
        }

        #endregion // Methods
    } // class MousePointer
} // namespace Polarith.AI.Package
