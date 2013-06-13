using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrassKnuckles.Utility;

namespace BrassKnuckles.EntityFramework.Systems
{
    /// <summary>
    /// System for processing entities in a group.
    /// </summary>
    public abstract class GroupSystem : EntitySystem
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the entity group to process.
        /// </summary>
        protected string Group { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new GroupSystem instance for processing entities in the specified group.
        /// </summary>
        /// <param name="group">Group to work with.</param>
        public GroupSystem(string group)
        {
            Group = group;
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Processes all entities in the group associated with the system.
        /// </summary>
        /// <param name="entities"></param>
        /// <remarks>Does not process entities passed, but instead retrieves entities to process from the 
        /// shared <see cref="GroupManager"/> instance.</remarks>
        protected override void ProcessEntities(Dictionary<int, Entity> entities)
        {
            List<Entity> groupedEntities = World.GroupManager.GetEntities(Group);
            int entityCount = groupedEntities.Count;
            for (int i = 0; i < entityCount; ++i)
            {
                Process(groupedEntities[i]);
            }
        }

        #endregion
    }
}
