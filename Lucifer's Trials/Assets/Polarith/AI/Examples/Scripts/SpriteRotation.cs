using UnityEngine;

namespace Polarith.AI.Package
{
    /// <summary>
    /// Intended to flip a sprite around the y axis based on its rotation. This can be done to ensure that sprite agents
    /// are not upside down for certain rotations.
    /// <para/>
    /// Note, this is just a script used for our example scenes and, therefore, not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [AddComponentMenu("Polarith AI » Move » Package/Sprite Rotation")]
    public sealed class SpriteRotation : MonoBehaviour
    {
        #region Fields =================================================================================================

        /// <summary>
        /// The transform of the sprite that should be flipped.
        /// </summary>
        [Tooltip("The transform of the sprite that should be flipped.")]
        public Transform SpriteTransform;

        //--------------------------------------------------------------------------------------------------------------

        private float scaleY;

        #endregion // Fields

        #region Methods ================================================================================================

        private void Start()
        {
            if (SpriteTransform == null)
            {
                enabled = false;
                return;
            }

            scaleY = SpriteTransform.localScale.y;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Update()
        {
            float sign = transform.up.normalized.x > 0.0f ? 1.0f : -1.0f;
            SpriteTransform.localScale = new Vector3(
                SpriteTransform.localScale.x,
                scaleY * sign,
                SpriteTransform.localScale.z);
        }

        #endregion // Methods
    } // SpriteRotation
} // namespace Polarith.AI.Package
