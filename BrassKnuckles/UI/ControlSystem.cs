using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrassKnuckles.EntityFramework;
using BrassKnuckles.EntityFramework.Systems;
using Nuclex.UserInterface;

namespace BrassKnuckles.UI
{
    /// <summary>
    /// Processes <see cref="ControlComponent"/> instances that tie UI controls to entities.
    /// </summary>
    public class ControlSystem : EntitySystem
    {
        #region Private fields

        ComponentMapper<PositionComponent> _positionMapper;
        ComponentMapper<ControlComponent> _controlMapper;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new ControlSystem instance.
        /// </summary>
        public ControlSystem()
            : base(typeof(ControlComponent))
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Initializes the control system.
        /// </summary>
        public override void Initialize()
        {
            _positionMapper = new ComponentMapper<PositionComponent>(World);
            _controlMapper = new ComponentMapper<ControlComponent>(World);

            base.Initialize();
        }

        /// <summary>
        /// Processes the provided <see cref="Entity"/> instance.
        /// </summary>
        /// <param name="entity"><see cref="Entity"/> instance to process.</param>
        public override void Process(Entity entity)
        {
            PositionComponent positionComponent = _positionMapper.Get(entity);
            ControlComponent controlComponent = _controlMapper.Get(entity);

            if ((positionComponent == null) || (controlComponent == null))
            {
                return;
            }

            controlComponent.Control.Bounds.Location = new UniVector(
                new UniScalar(0.0f, positionComponent.PreviousPosition.X + controlComponent.Offset.X + Director.SharedDirector.TransformationMatrix.Translation.X),
                new UniScalar(0.0f, positionComponent.PreviousPosition.Y + controlComponent.Offset.Y + Director.SharedDirector.TransformationMatrix.Translation.Y));
        }

        #endregion
    }
}
