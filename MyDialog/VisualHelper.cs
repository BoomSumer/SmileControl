using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

namespace MyDialog
{
    public static class VisualHelper
    {
        internal static VisualStateGroup TryGetVisualStateGroup(DependencyObject d, string groupName)
        {
            FrameworkElement implementationRoot = GetImplementationRoot(d);
            if (implementationRoot == null)
            {
                return null;
            }

            return VisualStateManager.GetVisualStateGroups(implementationRoot)?.OfType<VisualStateGroup>().FirstOrDefault((VisualStateGroup group) => string.CompareOrdinal(groupName, group.Name) == 0);
        }

        internal static FrameworkElement GetImplementationRoot(DependencyObject d)
        {
            if (1 != VisualTreeHelper.GetChildrenCount(d))
            {
                return null;
            }

            return VisualTreeHelper.GetChild(d, 0) as FrameworkElement;
        }

        public static T GetChild<T>(DependencyObject d) where T : DependencyObject
        {
            if (d == null)
            {
                return default;
            }

            
            if (d is T t)
            {
                return t;
            }
            
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(d); i++)
            {
                T child = GetChild<T>(VisualTreeHelper.GetChild(d, i));
                if (child != null)
                {
                    return child;
                }
            }

            return null;
        }

        public static T GetParent<T>(DependencyObject d) where T : DependencyObject
        {
            if (d != null)
            {
                T val = d as T;
                if (val == null)
                {
                    if (d is Window)
                    {
                        return null;
                    }

                    return GetParent<T>(VisualTreeHelper.GetParent(d));
                }

                return val;
            }

            return null;
        }

        public static IntPtr GetHandle(this Visual visual)
        {
            return (PresentationSource.FromVisual(visual) as HwndSource)?.Handle ?? IntPtr.Zero;
        }

        internal static void HitTestVisibleElements(Visual visual, HitTestResultCallback resultCallback, HitTestParameters parameters)
        {
            VisualTreeHelper.HitTest(visual, ExcludeNonVisualElements, resultCallback, parameters);
        }

        private static HitTestFilterBehavior ExcludeNonVisualElements(DependencyObject potentialHitTestTarget)
        {
            if (!(potentialHitTestTarget is Visual))
            {
                return HitTestFilterBehavior.ContinueSkipSelfAndChildren;
            }

            UIElement uIElement = potentialHitTestTarget as UIElement;
            if (uIElement == null || (uIElement.IsVisible && uIElement.IsEnabled))
            {
                return HitTestFilterBehavior.Continue;
            }

            return HitTestFilterBehavior.ContinueSkipSelfAndChildren;
        }

        
    }
}
