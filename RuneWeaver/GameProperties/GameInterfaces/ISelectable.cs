using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuneWeaver.GameProperties.GameInterfaces
{
    /// <summary>
    /// Represents a selectable object.
    /// </summary>
    public interface ISelectable
    {
        /// <summary>
        /// Select the object.
        /// </summary>
        void Select();

        /// <summary>
        /// Deselect the object.
        /// </summary>
        void Deselect();
    }
}
