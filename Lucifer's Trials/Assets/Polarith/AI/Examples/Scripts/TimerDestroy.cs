using UnityEngine;

namespace Polarith.AI.Package
{
    /// <summary>
    /// Destroys this <see cref="GameObject"/> after the given timer runs out. The timer begins on Start.
    /// <para/>
    /// Note, this is just a script used for our example scenes and, therefore, not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [AddComponentMenu("Polarith AI » Move » Package/Timer Destroy")]
    public sealed class TimerDestroy : MonoBehaviour
    {
        #region Fields =================================================================================================

        /// <summary>
        /// If this timer runs out, the gameObject is destroyed. Counted from start, in seconds.
        /// </summary>
        [Tooltip("If this timer runs out, the gameObject is destroyed. Counted from start, in seconds.")]
        public float Timer = 5.0f;

        #endregion // Fields

        #region Methods ================================================================================================

        private void Start()
        {
            Destroy(gameObject, Timer);
        }

        #endregion // Methods
    } // class TimerDestroy
} // namespace Polarith.AI.Package
