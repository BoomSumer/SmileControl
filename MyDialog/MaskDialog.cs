using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;

namespace MyDialog
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:MyDialog"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:MyDialog;assembly=MyDialog"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    /// 
    [TemplatePart(Name = "BackElement", Type = typeof(Border))]
    public class MaskDialog : ContentControl
    {
        static MaskDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaskDialog), new FrameworkPropertyMetadata(typeof(MaskDialog)));
        }

        #region 字段
        private const string BackElement = "PART_BackElement";

        private string? _token;

        private Border? _backElement;

        private AdornerContainer? _container;

        private TaskCompletionSource<object?> _tcs;




        private static readonly Dictionary<string, FrameworkElement> ContainerDic = new();

        private static readonly Dictionary<string, MaskDialog> DialogDict = new();
        #endregion

        #region 附加属性 / 依赖属性

        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(MaskDialog), new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty MaskCanCloseProperty = DependencyProperty.RegisterAttached("MaskCanClose", typeof(bool), typeof(MaskDialog), new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty MaskBrushProperty = DependencyProperty.Register("MaskBrush", typeof(Brush), typeof(MaskDialog), new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty TokenProperty = DependencyProperty.RegisterAttached("Token", typeof(string), typeof(MaskDialog), new PropertyMetadata(default(string), OnTokenChanged));


        #region 属性实例

        public bool IsOpen
        {
            get
            {
                return (bool)GetValue(IsOpenProperty);
            }
            internal set
            {
                SetValue(IsOpenProperty, value);
            }
        }

        public Brush MaskBrush
        {
            get
            {
                return (Brush)GetValue(MaskBrushProperty);
            }
            set
            {
                SetValue(MaskBrushProperty, value);
            }
        }

        public static void SetToken(DependencyObject element, string value)
        {
            element.SetValue(TokenProperty, value);
        }

        public static string GetToken(DependencyObject element)
        {
            return (string)element.GetValue(TokenProperty);
        }

        public static void SetMaskCanClose(DependencyObject element, bool value)
        {
            element.SetValue(MaskCanCloseProperty, ValueBoxes.BooleanBox(value));
        }

        public static bool GetMaskCanClose(DependencyObject element)
        {
            return (bool)element.GetValue(MaskCanCloseProperty);
        }

        #endregion

        #endregion

        #region 属性更改事件
        private static void OnTokenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement frameworkElement = d as FrameworkElement;
            if (frameworkElement != null)
            {
                if (e.NewValue == null)
                {
                    Unregister(frameworkElement);
                }
                else
                {
                    Register(e.NewValue.ToString(), frameworkElement);
                }
            }
        }

        #endregion

        #region 注册 / 取消注册  方法

        public static void Register(string token, FrameworkElement element)
        {
            if (!string.IsNullOrEmpty(token) && element != null)
            {
                ContainerDic[token] = element;
            }
        }

        public static void Unregister(string token, FrameworkElement element)
        {
            if (!string.IsNullOrEmpty(token) && element != null && ContainerDic.ContainsKey(token) && ContainerDic[token] == element)
            {
                ContainerDic.Remove(token);
            }
        }

        public static void Unregister(UIElement element)
        {
            if (element != null)
            {
                KeyValuePair<string, FrameworkElement> keyValuePair = ContainerDic.FirstOrDefault((KeyValuePair<string, FrameworkElement> item) => element == item.Value);
                if (!string.IsNullOrEmpty(keyValuePair.Key))
                {
                    ContainerDic.Remove(keyValuePair.Key);
                }
            }
        }

        public static void Unregister(string token)
        {
            if (!string.IsNullOrEmpty(token) && ContainerDic.ContainsKey(token))
            {
                ContainerDic.Remove(token);
            }
        }

        #endregion

        #region Show 方法

        public static async Task<object?> Show(object content)
        {
            if (content is null)
            {
                return null;
            }
            return await Show(content, "");
        }

        public static async Task<object?> Show(object content, string token)
        {
            MaskDialog dialog = new MaskDialog
            {
                _token = token,
                Content = content
            };

            


            FrameworkElement? _element;
            AdornerDecorator _adornerDecorator;
            if (string.IsNullOrEmpty(token))
            {
                DialogDict[BackElement] = dialog;
                _element = WindowHelper.GetActiveWindow();

            }
            else
            {
                dialog.Close();
                DialogDict[token] = dialog;
                ContainerDic.TryGetValue(token, out _element);
            }
            if (_element is null)
            {
                return null;
            }
            _adornerDecorator = VisualHelper.GetChild<AdornerDecorator>(_element);

            if (_adornerDecorator != null)
            {
                if (_adornerDecorator.Child != null)
                {
                    _adornerDecorator.Child.IsEnabled = false;
                }
                else
                {
                    Grid grid = new Grid();
                    _adornerDecorator.Child = grid;
                }

                AdornerLayer _adornerLayer;

                _adornerLayer = _adornerDecorator.AdornerLayer;

                if (_adornerLayer != null)
                {
                    AdornerContainer adorner = new AdornerContainer(_adornerLayer)
                    {
                        Child = dialog

                    };
                    dialog._container = adorner;
                    dialog.IsOpen = true;
                    _adornerLayer.Add(adorner);


                }
            }
            dialog._tcs = new TaskCompletionSource<object?>();
            return await dialog._tcs.Task;
        }

        #endregion


        #region Close  方法
        
        public static void Close(string token, object parameters)
        {
            MaskDialog? dialog;
            if (DialogDict.TryGetValue(token, out dialog) || DialogDict.TryGetValue(BackElement, out dialog))
            {
                dialog.CloseDialog(parameters);
                dialog.Close();
            }                       
        }
        private void CloseDialog(object parameters)
        {
            _tcs.SetResult(parameters);
        }

        private void Close()
        {
            if (string.IsNullOrEmpty(_token))
            {
                Close(WindowHelper.GetActiveWindow());
            }
            else if (ContainerDic.TryGetValue(_token, out var element))
            {
                Close(element);
                DialogDict.Remove(_token);
            }
        }

        private void Close(FrameworkElement element)
        {
            if (element != null && _container != null)
            {
                var decorator = VisualHelper.GetChild<AdornerDecorator>(element);
                if (decorator != null)
                {
                    if (decorator.Child != null)
                    {
                        decorator.Child.IsEnabled = true;
                    }
                    var layer = decorator.AdornerLayer;
                    layer?.Remove(_container);
                    IsOpen = false;
                }
            }
        }       

        #endregion


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _backElement = GetTemplateChild("BackElement") as Border;
        }
        
    }   
}
