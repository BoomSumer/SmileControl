using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace MyDialog
{
    public class AdornerContainer : Adorner
    {
        private UIElement _child;

        public UIElement Child
        {
            get
            {
                return _child;
            }
            set
            {
                if (value == null)
                {
                    RemoveVisualChild(_child);
                    _child = value;
                }
                else
                {
                    AddVisualChild(value);
                    _child = value;
                }
            }
        }

        protected override int VisualChildrenCount
        {
            get
            {
                if (_child == null)
                {
                    return 0;
                }

                return 1;
            }
        }
        
        public AdornerContainer(UIElement adornedElement)
            : base(adornedElement)
        {
                      
        }

        protected override Size MeasureOverride(Size constraint)
        {
            return base.MeasureOverride(constraint);
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            _child?.Arrange(new Rect(finalSize));
            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index == 0 && _child != null)
            {
                return _child;
            }

            return base.GetVisualChild(index);
        }
    }
}
