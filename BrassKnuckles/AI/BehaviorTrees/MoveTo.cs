using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrassKnuckles.EntityFramework;
using Microsoft.Xna.Framework;
using BrassKnuckles.AI.Actions;

namespace BrassKnuckles.AI.BehaviorTrees
{
    public class MoveTo : Task
    {
        #region Private fields

        private Entity _entity;
        private Vector2 _goal;
        private MoveAction _moveAction;

        #endregion

        #region Public methods

        public override TaskResultCode Execute()
        {
            if (_moveAction == null)
            {
                _entity = (Entity)Blackboard.Read("entity");
                _goal = (Vector2)Blackboard.Read("goal");

                _moveAction = new MoveAction(2.0f, 1, _entity, _goal);
            }

            return TaskResultCode.Success;
        }

        #endregion
    }
}
