using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MyDialog
{
    public class DialogSession
    {
        private readonly MaskDialog _owner;

        internal DialogSession(MaskDialog owner)
            => _owner = owner ?? throw new ArgumentNullException(nameof(owner));

        /// <summary>
        /// Indicates if the dialog session has ended.  Once ended no further method calls will be permitted.
        /// </summary>
        /// <remarks>
        /// Client code cannot set this directly, this is internally managed.  To end the dialog session use <see cref="Close()"/>.
        /// </remarks>
        public bool IsEnded { get; internal set; }

        /// <summary>
        /// The parameter passed to the <see cref="DialogHost.CloseDialogCommand" /> and return by <see cref="DialogHost.Show(object)"/>
        /// </summary>
        internal object? CloseParameter { get; set; }

        /// <summary>
        /// Gets the <see cref="DialogHost.DialogContent"/> which is currently displayed, so this could be a view model or a UI element.
        /// </summary>
        public object? Content => _owner.Content;

        /// <summary>
        /// Update the current content in the dialog.
        /// </summary>
        /// <param name="content"></param>
        public void UpdateContent(object? content)
        {
            
            _owner.Content = content;
            
        }

        /// <summary>
        /// Closes the dialog.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the dialog session has ended, or a close operation is currently in progress.</exception>
        

        /// <summary>
        /// Closes the dialog. 
        /// </summary>
        /// <param name="parameter">Result parameter which will be returned in <see cref="DialogClosingEventArgs.Parameter"/> or from <see cref="DialogHost.Show(object)"/> method.</param>
        /// <exception cref="InvalidOperationException">Thrown if the dialog session has ended, or a close operation is currently in progress.</exception>
        
    }
}
