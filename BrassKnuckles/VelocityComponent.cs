using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BrassKnuckles.EntityFramework;

namespace BrassKnuckles
{
    public class VelocityComponent : IComponent
    {
        /// <summary>
        /// Gets or sets the entity's Velocity
        /// </summary>
        public Vector2 Velocity { get; set; }
        /// <summary>
        /// Gets or sets the entity's AngularVelocity in Radians/second.
        /// </summary>
        public float AngularVelocity { get; set; }
        /// <summary>
        /// Gets or sets the MaxSpeed value.  Velocity is clamped to this value.
        /// </summary>
        public float MaxSpeed { get; set; }
        /// <summary>
        /// Gets or sets the MaxAngularVelocity value.  The Entity's AngularVelocity is clamped to this value.
        /// </summary>
        public float MaxAngularVelocity { get; set; }

        public VelocityComponent()
        {
            MaxSpeed = float.MaxValue;
            MaxAngularVelocity = float.MaxValue;
        }

        /// <summary>
        /// Resulting velocity of force.
        /// </summary>
        public Vector2 ForceVelocity { get; set; }
        /// <summary>
        /// Time force will last in seconds.
        /// </summary>
        public float ForceDuration { get; set; }
        /// <summary>
        /// Time force has lived in seconds.
        /// </summary>
        public float ForceAge { get; set; }
        /// <summary>
        /// Determines whether or not this force will be applied to the entity;
        /// </summary>
        public bool ForceActive { get; set; }
    }
}
