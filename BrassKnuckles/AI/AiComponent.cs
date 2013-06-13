using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrassKnuckles.EntityFramework;
using BrassKnuckles.AI.Actions;
using BrassKnuckles.AI.BehaviorTrees;

namespace BrassKnuckles.AI
{
    /// <summary>
    /// Behavioral data for entities.
    /// </summary>
    public class AiComponent : IComponent
    {
        #region Properties

        /// <summary>
        /// Gets the <see cref="ActionManager"/> for this AI.
        /// </summary>
        public ActionManager ActionManager { get; protected set; }

        /// <summary>
        /// Gets or sets the <see cref="ITask"/> instance that is the
        /// root of the AI behavior tree.
        /// </summary>
        public ITask RootBehavior { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an AiComponent instance.
        /// </summary>
        /// <param name="rootBehavior"><see cref="ITask"/> instance that is
        /// the root of the behavior tree.</param>
        public AiComponent(ITask rootBehavior)
        {
            ActionManager = new ActionManager();
            RootBehavior = rootBehavior;
        }

        #endregion
    }
}
