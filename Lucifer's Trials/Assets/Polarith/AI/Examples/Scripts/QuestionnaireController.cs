using UnityEngine;
using UnityEngine.UI;

namespace Polarith.AI.Package
{
    /// <summary>
    /// Controls the questionnaire about the free and pro version of Polarith AI. Functions are related to questions and
    /// answers in the questionnaire scene. Questions and answers are connected via function calls and UI callbacks.
    /// </summary>
    [AddComponentMenu("Polarith AI » Move » Package/Questionnaire Controller")]
    public class QuestionnaireController : MonoBehaviour
    {
        #region Fields =================================================================================================

        [Tooltip("Interest object the agent follows.")]
        [SerializeField]
        private Transform interest;

        #endregion // Fields

        #region Properties =============================================================================================

        /// <summary>
        /// Interest object the agent follows.
        /// </summary>
        public Transform Interest
        {
            get { return interest; }
            set { interest = value; }
        }

        #endregion // Properties

        #region Methods ================================================================================================

        /// <summary>
        /// Change the position of the interest so that the agent moves towards the next question.
        /// </summary>
        /// <param name="target"></param>
        public void MoveInterest(Transform target)
        {
            interest.position = target.position;
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Activates a button by setting interactable to true.
        /// </summary>
        /// <param name="button">The button that will be enabled in canvas.</param>
        public void ActivateButton(Button button)
        {
            button.interactable = true;
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Deactivates a button by setting interactable to false.
        /// </summary>
        /// <param name="button">The button that will be disabled in canvas.</param>
        public void DisableButton(Button button)
        {
            button.interactable = false;
        }

        #endregion // Methods
    } // class QuestionnaireController
} // namespace Polarith.AI.Package
