using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BrassKnuckles.EntityFramework.Systems;
using BrassKnuckles;
using BrassKnuckles.EntityFramework;

namespace BrassKnuckles
{
    public class MovementSystem : EntitySystem
    {
        private ComponentMapper<VelocityComponent> _velocityMapper;
        private ComponentMapper<PositionComponent> _positionMapper;

        public MovementSystem()
            : base(typeof(VelocityComponent), typeof(PositionComponent))
        {
        }

        public override void Initialize()
        {
            _velocityMapper = new ComponentMapper<VelocityComponent>(World);
            _positionMapper = new ComponentMapper<PositionComponent>(World);
        }

        protected override void ProcessEntities(Dictionary<int, Entity> entities)
        {
            base.ProcessEntities(entities);
        }

        public override void Process(Entity entity)
        {
            VelocityComponent v = _velocityMapper.Get(entity);
            PositionComponent p = _positionMapper.Get(entity);

            if (v.ForceActive)
            {
                if (v.ForceAge <= v.ForceDuration)
                {
                    v.ForceAge += World.ElapsedTime;
                    v.Velocity = v.ForceVelocity * (float)Math.Pow((1 - .99), v.ForceAge);
                }
                else
                {
                    v.Velocity = Vector2.Zero;
                    v.ForceActive = false;
                    v.ForceAge = 0;
                }
            }

            //Clamp velocity
            //if (v.Velocity.Length() > v.MaxSpeed)
            //{
            //    v.Velocity = Vector2.Normalize(v.Velocity) * v.MaxSpeed;
            //}
            //Clamp angular velocity
            v.AngularVelocity = MathHelper.Clamp(v.AngularVelocity, -v.MaxAngularVelocity, v.MaxAngularVelocity);

            //Adjust position and rotation based on velocities and time.
            if (p != null && v != null)
            {
                p.Position = new Vector2(p.Position.X + v.Velocity.X * World.ElapsedTime, p.Position.Y + v.Velocity.Y * World.ElapsedTime);
                p.Rotation += v.AngularVelocity * World.ElapsedTime;
            }
        }
    }
}
