using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrassKnuckles.Utility;

namespace BrassKnuckles.EntityFramework.Managers
{
    /// <summary>
    /// Manages <see cref="Entity"/> groups.
    /// </summary>
    public sealed class GroupManager : IManager
    {
        #region Private fields

        private EntityWorld _world;
        private List<Entity> _emptyBag;
        private Dictionary<string, Dictionary<int, Entity>> _entitiesByGroup;
        private Dictionary<int, string> _groupsByEntity;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new GroupManager instance in the provided <see cref="EntityWorld"/>.
        /// </summary>
        /// <param name="world">The <see cref="EntityWorld"/> instance in which the manager will be created.</param>
        public GroupManager(EntityWorld world)
        {
            _world = world;
            _emptyBag = new List<Entity>();
            _entitiesByGroup = new Dictionary<string, Dictionary<int, Entity>>();
            _groupsByEntity = new Dictionary<int, string>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds the provided <see cref="Entity"/> to the specified group.
        /// </summary>
        /// <param name="group">Name of the group.</param>
        /// <param name="entity">The <see cref="Entity"/> instance that will be added to the group.</param>
        /// <remarks>An <see cref="Entity"/> can only belong to one group at a time. If the provided <see cref="Entity"/> is
        /// already a member of a group, it will be removed from the current group before being added to the new group.</remarks>
        public void AddToGroup(string group, Entity entity)
        {
            RemoveFromGroup(entity);

            int entityId = entity.Id;
            if (!_entitiesByGroup.ContainsKey(group))
            {
                _entitiesByGroup.Add(group, new Dictionary<int, Entity>());
            }
            _entitiesByGroup[group].Add(entity.Id, entity);

            _groupsByEntity[entityId] = group;
        }

        /// <summary>
        /// Removes an <see cref="Entity"/> from its group.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> instance to be removed from its group.</param>
        public void RemoveFromGroup(Entity entity)
        {
            int entityId = entity.Id;
            string group;
            if (_groupsByEntity.TryGetValue(entityId, out group))
            {
                _groupsByEntity.Remove(entityId);
                if (_entitiesByGroup.ContainsKey(group))
                {
                    _entitiesByGroup[group].Remove(entityId);
                }
            }
        }

        /// <summary>
        /// Removes all <see cref="Entity"/> instances from the specified group.
        /// </summary>
        /// <param name="group">Name of group to clear.</param>
        public void ClearGroup(string group)
        {
            if (!_entitiesByGroup.ContainsKey(group))
            {
                return;
            }

            foreach (Entity entity in _entitiesByGroup[group].Values)
            {
                RemoveFromGroup(entity);
            }
        }

        /// <summary>
        /// Gets a <see cref="Bag"/> containing all <see cref="Entity"/> instances in the specified group.
        /// </summary>
        /// <param name="group">Group to look up.</param>
        /// <returns>A <see cref="List"/> containing all <see cref="Entity"/> instances in the specified group.</returns>
        public List<Entity> GetEntities(string group)
        {
            List<Entity> entities = new List<Entity>();
            if (_entitiesByGroup.ContainsKey(group))
            {
                entities = _entitiesByGroup[group].Values.ToList();
            }

            return entities;
        }

        /// <summary>
        /// Gets the name of the group to which the provided <see cref="Entity"/> belongs.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> instance to look up.</param>
        /// <returns>The name of the group to which the <see cref="Entity"/> belongs, or an empty string
        /// if it does not belong to a group.</returns>
        public string GetGroupFor(Entity entity)
        {
            string group;
            if (_groupsByEntity.TryGetValue(entity.Id, out group))
            {
                return group;
            }

            return null;
        }

        /// <summary>
        /// Determines if the provided <see cref="Entity"/> belongs to a group.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> instance to look up.</param>
        /// <returns>True if the <see cref="Entity"/> belongs to a group, or false if not.</returns>
        public bool IsGrouped(Entity entity)
        {
            return !string.IsNullOrEmpty(GetGroupFor(entity));
        }

        #endregion
    }
}
