namespace InplaceEditBoxLib.Views
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// An adorner class that contains a TextBox to provide editing capability 
    /// for an EditBox control. The editable TextBox resides in the 
    /// AdornerLayer. When the EditBox is in editing mode, the TextBox is given a size 
    /// it with desired size; otherwise, arrange it with size(0,0,0,0).
    /// 
    /// This code used part of ATC Avalon Team's work
    /// (http://blogs.msdn.com/atc_avalon_team/archive/2006/03/14/550934.aspx)
    /// </summary>
    internal sealed class EditBoxAdorner : Adorner
    {
        #region fields
        /// <summary>
        /// Extra padding for the content when it is displayed in the TextBox
        /// </summary>
        private const double ExtraWidth = 15;

        /// <summary>
        /// Visual children
        /// </summary>
        private VisualCollection _VisualChildren;

        /// <summary>
        /// Control that contains both Adorned control and Adorner.
        /// This reference is required to compute the width of the
        /// surrounding scrollviewer.
        /// </summary>
        private EditBox _EditBox;

        /// <summary>
        /// The TextBox that this adorner covers.
        /// </summary>
        private TextBox _TextBox;

        /// <summary>
        /// Whether the EditBox is in editing mode which means the Adorner is visible.
        /// </summary>
        private bool _IsVisible;

        /// <summary>
        /// Canvas that contains the TextBox that provides the ability for it to 
        /// display larger than the current size of the cell so that the entire
        /// contents of the cell can be edited
        /// </summary>
        private Canvas _Canvas;

        /// <summary>
        /// Maximum size of the textbox in dependents of the surrounding scrollviewer
        /// is computed o demand in measure method and invalidated when visibility of Adorner changes.
        /// </summary>
        private double _TextBoxMaxWidth = double.PositiveInfinity;
        #endregion fields

        #region constructor
        /// <summary>
        /// Inialize the EditBoxAdorner.
        /// 
        ///   +---> adorningElement (TextBox)
        ///   |
        /// adornedElement (TextBlock)
        /// </summary>
        /// <param name="adornedElement"></param>
        /// <param name="adorningElement"></param>
        /// <param name="editBox"></param>
        public EditBoxAdorner(UIElement adornedElement,
                              TextBox adorningElement,
                              EditBox editBox)
          : base(adornedElement)
        {
            _TextBox = adorningElement;
            Debug.Assert(_TextBox != null, "No TextBox!");

            _VisualChildren = new VisualCollection(this);

            this.BuildTextBox(editBox);
        }
        #endregion constructor

        #region Properties
        /// <summary>
        /// override property to return infomation about visual tree.
        /// </summary>
        protected override int VisualChildrenCount
        {
            get { return _VisualChildren.Count; }
        }

        /// <summary>
        /// Gets whether the textbox is currently visible or not.
        /// </summary>
        public bool IsTextBoxVisible
        {
            get
            {
                return _IsVisible;
            }
        }
        #endregion Properties

        #region methods
        /// <summary>
        /// Specifies whether a TextBox is visible 
        /// when the IsEditing property changes.
        /// </summary>
        /// <param name="isVisible"></param>
        public void UpdateVisibilty(bool isVisible)
        {
            _IsVisible = isVisible;
            InvalidateMeasure();
            _TextBoxMaxWidth = double.PositiveInfinity;
        }

        /// <summary>
        /// override function to return infomation about visual tree.
        /// </summary>
        protected override Visual GetVisualChild(int index)
        {
            return _VisualChildren[index];
        }

        /// <summary>
        /// override function to arrange elements.
        /// </summary>
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (_IsVisible)
            {
                _TextBox.Arrange(new Rect(-1, -1, finalSize.Width, finalSize.Height));
            }
            else // if there is no editable mode, there is no need to show elements.
            {
                _TextBox.Arrange(new Rect(0, 0, 0, 0));
            }

            return finalSize;
        }

        /// <summary>
        /// Override to measure elements.
        /// </summary>
        protected override Size MeasureOverride(Size constraint)
        {
            _TextBox.IsEnabled = _IsVisible;

            // if in editing mode, measure the space the adorner element should cover.
            if (_IsVisible == true)
            {
                if (double.IsInfinity(_TextBoxMaxWidth) == true)
                {
                    Point position = _TextBox.PointToScreen(new Point(0, 0)),
                    controlPosition = _EditBox.ParentScrollViewer.PointToScreen(new Point(0, 0));

                    position.X = Math.Abs(controlPosition.X - position.X);
                    position.Y = Math.Abs(controlPosition.Y - position.Y);

                    _TextBoxMaxWidth = _EditBox.ParentScrollViewer.ViewportWidth - position.X;
                }

                if (this.AdornedElement.Visibility == System.Windows.Visibility.Collapsed)
                    return new Size(_TextBoxMaxWidth, _TextBox.DesiredSize.Height);

                // 
                if (constraint.Width > _TextBoxMaxWidth)
                {
                    constraint.Width = _TextBoxMaxWidth;
                }

                AdornedElement.Measure(constraint);
                _TextBox.Measure(constraint);

                double desiredWidth = AdornedElement.DesiredSize.Width + ExtraWidth;

                // since the adorner is to cover the EditBox, it should return 
                // the AdornedElement.Width, the extra 15 is to make it more clear.
                if (desiredWidth < _TextBoxMaxWidth)
                    return new Size(desiredWidth, _TextBox.DesiredSize.Height);
                else
                {
                    this.AdornedElement.Visibility = System.Windows.Visibility.Collapsed;

                    return new Size(_TextBoxMaxWidth, _TextBox.DesiredSize.Height);
                }
            }
            else  // no need to show anything if it is not in editable mode.
                return new Size(0, 0);
        }

        /// <summary>
        /// Inialize necessary properties and hook necessary events on TextBox, 
        /// then add it into tree.
        /// </summary>
        private void BuildTextBox(EditBox editBox)
        {
            _EditBox = editBox;

            _Canvas = new Canvas();
            _Canvas.Children.Add(_TextBox);
            _VisualChildren.Add(_Canvas);

            // Bind TextBox onto editBox control property Text
            Binding binding = new Binding("Text");
            binding.Source = editBox;
            binding.Mode = BindingMode.TwoWay;
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            _TextBox.SetBinding(TextBox.TextProperty, binding);

            // Bind Text onto AdornedElement property Text
            binding = new Binding("Text");
            binding.Source = this.AdornedElement;
            binding.Mode = BindingMode.TwoWay;
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            _TextBox.SetBinding(TextBox.TextProperty, binding);
        }
        #endregion methods
    }
}
