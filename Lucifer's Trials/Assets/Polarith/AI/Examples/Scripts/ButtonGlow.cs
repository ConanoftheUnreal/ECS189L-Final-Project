using UnityEngine;
using UnityEngine.UI;

namespace Polarith.AI.Package
{
    /// <summary>
    /// Interpolates the color of a Unity UI button within two values in a certain time interval.
    /// </summary>
    [AddComponentMenu("Polarith AI » Move » Package/Button Glow")]
    public class ButtonGlow : MonoBehaviour
    {
        #region Fields==================================================================================================

        [Tooltip("Button that should be changed.")]
        [SerializeField]
        private Button button;

        [Tooltip("Color of the button when glowing.")]
        [SerializeField]
        private Color glowColor;

        [Tooltip("Time interval of the color change.")]
        [SerializeField]
        private float timeInterval;
        

        private Color origColor;
        private ColorBlock colors;
        private float deltaT = 0f;
        private float sign = 1f;

        #endregion // Fields

        #region Properties==============================================================================================

        /// <summary>
        /// Button that should be changed.
        /// </summary>
        public Button Button
        {
            get { return button; }
            set { button = value; }
        }

        /// <summary>
        /// Color of the button when glowing.
        /// </summary>
        public Color GlowColor
        {
            get { return glowColor; }
            set { glowColor = value; }
        }

        /// <summary>
        /// Time interval of the color change.
        /// </summary>
        public float TimeInterval
        {
            get { return timeInterval; }
            set { timeInterval = value; }
        }

        #endregion // Properties

        #region Methods=================================================================================================

        private void Start()
        {
            if (button == null)
                button = GetComponent<Button>();
            origColor = button.colors.normalColor;
            colors = button.colors;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Update()
        {
            if (deltaT < 0)
                sign = 1f;
            if (deltaT > timeInterval)
                sign = -1f;
            deltaT += Time.deltaTime * sign;
            colors.normalColor = Color.Lerp(origColor, glowColor, deltaT);
            button.colors = colors;
        }

        #endregion // Methods
    } // class ButtonGlow
} // namespace Polarith.AI.Package
