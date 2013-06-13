using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrassKnuckles
{
    /// <summary>
    /// Standard message codes used by objects in TheEngine.
    /// </summary>
    /// <remarks>Recommend reserving values less than 256 for TheEngine and 
    /// only using values 256 and higher for games.</remarks>
    public enum MessageCodes
    {
        /// <summary>
        /// An input binding was triggered. Data is a string containing the name of the <see cref="BrassKnuckles.Input.InputBinding"/>
        /// instance that was triggered.
        /// </summary>
       InputTriggered = 0,
       CollisionInfo = 1
    }
}
