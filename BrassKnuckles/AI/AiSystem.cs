using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrassKnuckles.EntityFramework.Systems;
using BrassKnuckles.EntityFramework;

namespace BrassKnuckles.AI
{
    /// <summary>
    /// Processes AI behavior.
    /// </summary>
    public class AiSystem : IntervalEntitySystem
    {
        #region Private fields

        private ComponentMapper<AiComponent> _aiMapper;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new AiSystem instance that will update
        /// at the specified interval.
        /// </summary>
        /// <param name="interval">Update interval specified in milliseconds.</param>
        public AiSystem(float interval)
            : base(interval, typeof(AiComponent))
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Initializes the AI system.
        /// </summary>
        public override void Initialize()
        {
            _aiMapper = new ComponentMapper<AiComponent>(World);

            base.Initialize();
        }

        /// <summary>
        /// Processes AI behavor for the provided <see cref="Entity"/> instance.
        /// </summary>
        /// <param name="entity"><see cref="Entity"/> instance to process.</param>
        public override void Process(Entity entity)
        {
            AiComponent ai = _aiMapper.Get(entity);
            if (ai == null)
            {
                return;
            }

            ai.ActionManager.Update(Director.SharedDirector.GameTime);
            ai.RootBehavior.Execute();
        }

        #endregion
    }
}
