using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrassKnuckles.Input
{
    /// <summary>
    /// Interface for components that will be bound to a set of input states.
    /// </summary>
    public interface IInputMatcher
    {
        #region Methods

        /// <summary>
        /// Determines if the current input state matches the bindings for the component.
        /// </summary>
        /// <returns>True if all bindings match, false if not.</returns>
        bool Matched();

        #endregion
    }
}
