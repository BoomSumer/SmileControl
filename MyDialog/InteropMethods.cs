using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyDialog
{
    internal class InteropMethods
    {
        

        

        [DllImport("user32.dll")]
        internal static extern IntPtr GetActiveWindow();


    }
}
