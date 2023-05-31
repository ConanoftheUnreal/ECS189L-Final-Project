using System;
using UnityEngine;
using UnityEngine.UI;

namespace Polarith.AI.Examples
{
    /// <summary>
    /// Computes and displays the framerate either OnGUI or via a given UI Text object. Is not destroyed on load.
    /// <para/>
    /// Note, this is just a script used for our example scenes and, therefore, not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [AddComponentMenu("Polarith AI » Move » Package/Frames Per Second")]
    public sealed class FramesPerSecond : MonoBehaviour
    {
        #region Fields =================================================================================================

        /// <summary>
        /// A custom <see cref="Text"/> object that can be assigned to display the frames per second instead of
        /// rendering them via OnGui.
        /// </summary>
        [Tooltip("A custom Text object that can be assigned to display the frames per second instead of rendering " +
            "them via OnGui.")]
        public Text DisplayText;

        /// <summary>
        /// Sets the font color.
        /// </summary>
        [Tooltip("Sets the font color.")]
        public Color fontColor = Color.white;

        //--------------------------------------------------------------------------------------------------------------

        private Rect position = new Rect(0.0f, 0.0f, 100.0f, 20.0f);
        private string framerateStr;
        private float deltaTime = 0.0f;

        #endregion // Fields

        #region Methods ================================================================================================

        private void Start()
        {
            DontDestroyOnLoad(this);

            position.x = Screen.width - position.width - 10.0f;
            position.y = 10.0f;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Update()
        {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnGUI()
        {
            float fps = 1.0f / deltaTime;
            framerateStr = String.Format("{0:f1}", fps);

            if (DisplayText == null)
            {
                GUI.contentColor = fontColor;
                GUI.Label(position, string.Format("<b><size=16>FPS: {0}</size></b>", framerateStr));
            }
            else
            {
                DisplayText.text = "FPS: " + framerateStr + " ";
            }
        }

        #endregion // Methods
    } // class FramesPerSecond
} // namespace Polarith.AI.Examples
