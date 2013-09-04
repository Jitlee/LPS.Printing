using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPS.Controls._PropertyGrid.Parts
{
    public class ExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="eventException">The Exception</param>
        public ExceptionEventArgs(Exception eventException)
        {
            this.EventException = eventException;
        }

        /// <summary>
        /// Gets the Exception
        /// </summary>
        public Exception EventException { get; private set; }
    }
}
