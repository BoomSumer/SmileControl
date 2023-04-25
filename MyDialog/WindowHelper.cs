using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows;

namespace MyDialog
{
    public static class WindowHelper
    {
        

        

        

        //
        // 摘要:
        //     获取当前应用中处于激活的一个窗口
        public static System.Windows.Window GetActiveWindow()
        {
            IntPtr activeWindow = InteropMethods.GetActiveWindow();
            return Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault((System.Windows.Window x) => x.GetHandle() == activeWindow);
        }

        public static IntPtr CreateHandle()
        {
            return new WindowInteropHelper(new System.Windows.Window()).EnsureHandle();
        }

        public static IntPtr GetHandle(this System.Windows.Window window)
        {
            return new WindowInteropHelper(window).EnsureHandle();
        }

        public static HwndSource GetHwndSource(this System.Windows.Window window)
        {
            return HwndSource.FromHwnd(window.GetHandle());
        }

        
        
    }
}
