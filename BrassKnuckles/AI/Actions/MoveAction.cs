using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrassKnuckles.EntityFramework;
using Microsoft.Xna.Framework;

namespace BrassKnuckles.AI.Actions
{
    public class MoveAction : Action
    {
        #region Constants

        private float targetRadius = 1.0f;
        private float slowRadius = 32.0f;

        #endregion

        #region Private fields

        private Entity _entity;
        private Vector2 _goal;
        private PositionComponent _position;
        private VelocityComponent _velocity;

        #endregion

        #region Constructors

        public MoveAction(float expiration, int priority, Entity entity, Vector2 goal)
            : base(expiration, priority)
        {
            _entity = entity;
            _goal = goal;

            ComponentMapper<PositionComponent> positionMapper = new ComponentMapper<PositionComponent>(Director.SharedDirector.EntityWorld);
            _position = positionMapper.Get(entity);

            ComponentMapper<VelocityComponent> velocityMapper = new ComponentMapper<VelocityComponent>(Director.SharedDirector.EntityWorld);
            _velocity = velocityMapper.Get(entity);
        }

        #endregion

        #region Public methods

        public override bool CanInterrupt()
        {
            return true;
        }

        public override bool CanDoBoth(Action otherAction)
        {
            return !(otherAction is MoveAction);
        }

        public override bool IsComplete()
        {
            return (_position.Position == _goal);
        }

        public override void Execute()
        {
            if (_position.Position == _goal)
            {
                return;
            }

            Vector2 direction = (_goal - _position.Position);
            float distance = direction.Length();
            float targetSpeed = _velocity.MaxSpeed;
            if (distance < targetRadius)
            {
                return;
            }
            else if (distance <= slowRadius)
            {
                targetSpeed =_velocity.MaxSpeed * (distance / slowRadius);
            }

            _velocity.Velocity = Vector2.Normalize(direction) * targetSpeed;
        }

        #endregion
    }
}
