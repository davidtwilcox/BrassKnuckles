using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrassKnuckles.AI.BehaviorTrees
{
    public class AlwaysSuccess : Task
    {
        #region Public methods

        public override TaskResultCode Execute()
        {
            return TaskResultCode.Success;
        }

        #endregion
    }
}
