using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyDialog
{
    public class DialogOpenedEventArgs : RoutedEventArgs
    {
        public DialogOpenedEventArgs(MaskDialog session, RoutedEvent routedEvent)
        {
            if (session == null) throw new ArgumentNullException(nameof(session));

            Session = session;
            RoutedEvent = routedEvent;
        }

        /// <summary>
        /// Allows interaction with the current dialog session.
        /// </summary>
        public MaskDialog Session { get; }
    }
}
