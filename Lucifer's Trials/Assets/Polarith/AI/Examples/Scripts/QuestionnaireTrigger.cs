using UnityEngine;

namespace Polarith.AI.Package
{
    /// <summary>
    /// A script to redirect the question pointer to another target when the agent enters the trigger.
    /// </summary>
    [AddComponentMenu("Polarith AI » Move » Package/Questionnaire Triger")]
    public class QuestionnaireTrigger : MonoBehaviour
    {
        #region Fields =================================================================================================

        [Tooltip("Next position of the QuestionPointer as soon as the trigger is entered.")]
        [SerializeField]
        private Transform nextTarget;

        [Tooltip("Interest object the agents tries to follow. Acts as a pointer to the current question.")]
        [SerializeField]
        private Transform questionPointer;

        [Tooltip("If true the trigger will open the Polarith page in the assetstore. Otherwise it sets the position " +
            "of the question pointer to the next target.")]
        [SerializeField]
        private bool pro = false;

        #endregion // Fields

        #region Properties =============================================================================================

        /// <summary>
        /// Next position of the QuestionPointer as soon as the trigger is entered.
        /// </summary>
        public Transform NextTarget
        {
            get { return nextTarget; }
            set { nextTarget = value; }
        }

        /// <summary>
        /// Interest object the agents tries to follow. Acts as a pointer to the current question.
        /// </summary>
        public Transform QuestionPointer
        {
            get { return questionPointer; }
            set { questionPointer = value; }
        }

        /// <summary>
        /// If true the trigger will open the Polarith page in the assetstore. Otherwise it sets the position of the 
        /// question pointer to the next target.
        /// </summary>
        public bool Pro
        {
            get { return pro; }
            set { pro = value; }
        }

        #endregion // Properties

        #region Methods ================================================================================================

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (Pro)
            {
                Application.OpenURL(
                    "https://assetstore.unity.com/packages/tools/ai/polarith-ai-pro-movement-pathfinding-steering-71465"
                    );
            }
            else
            {
                questionPointer.position = nextTarget.position;
            }
        }

        #endregion // Methods
    } // class QuestionnaireTrigger
} // namespace Polarith.AI.Package
