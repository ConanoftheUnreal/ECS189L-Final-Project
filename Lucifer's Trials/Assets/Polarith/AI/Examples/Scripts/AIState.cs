using Polarith.AI.Move;
using System.Collections.Generic;
using UnityEngine;

namespace Polarith.AI.Package
{
    /// <summary>
    /// A <see cref="StateMachineBehaviour"/> which activates all components defined by the <see cref="BehaviourLabel"/>
    /// list OnStateEnter.
    /// <para/>
    /// Note, this is just a script used for our example scenes and, therefore, not part of the actual API. We do not
    /// guarantee that this script is working besides our examples.
    /// </summary>
    [AddComponentMenu("Polarith AI » Move » Package/AI State")]
    public sealed class AIState : StateMachineBehaviour
    {
        #region Fields =================================================================================================

        /// <summary>
        /// The labels correspond to the <see cref="AIMBehaviour.Label"/>. If a label is in this list, the corresponding
        /// behaviour will be activated OnStateEnter.
        /// </summary>
        [Tooltip("The labels correspond to the AIMBehaviour.Label. If a label is in this list, the corresponding " +
            "behaviour will be activated OnStateEnter.")]
        public List<string> BehaviourLabel;

        #endregion // Fields

        #region Methods ================================================================================================

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            // Note: If you know your behaviour configuration won't change during play mode, you should consider
            // pre-caching this array and avoid calling GetComponent during every state change.
            AIMBehaviour[] behaviours = animator.gameObject.GetComponents<AIMBehaviour>();
            foreach (AIMBehaviour b in behaviours)
            {
                if (BehaviourLabel.Contains(b.Label))
                    b.enabled = true;
                else
                    b.enabled = false;
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            // You may need to replace this parameter name.
            animator.SetInteger("Change", 0);
        }

        #endregion // Methods
    } // class AIState
} // namespace Polarith.AI.Package
