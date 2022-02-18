namespace InplaceEditBoxLib.Views
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using InplaceEditBoxLib.Interfaces;
    using UserNotification.View;
    using UserNotification.ViewModel;

    /// <summary>
    /// EditBox is a custom cotrol that can switch between two modes: 
    /// editing and normal. When it is in editing mode, the content is
    /// displayed in a TextBox that provides editing capbabilities. When 
    /// the EditBox is in normal, its content is displayed in a TextBlock
    /// that is not editable.
    /// 
    /// This control is designed to be used in an item that is part of a
    /// ItemsControl collection (ListView, GridView, ListBox, TreeView).
    /// 
    /// This code used part of ATC Avalon Team's work
    /// (http://blogs.msdn.com/atc_avalon_team/archive/2006/03/14/550934.aspx)
    /// </summary>
    [TemplatePart(Name = "PART_TextBlockPart", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_MeasureTextBlock", Type = typeof(TextBlock))]
    public class EditBox : Control
    {
        #region fields
        private TextBlock _PART_MeasureTextBlock;
        private TextBlock _PART_TextBlock;

        #region dependency properties
        /// <summary>
        /// Implements the backing store of the <see cref="DisplayTextForegroundBrush"/>
        /// dependency property which can be used to set the foreground color of the textblock
        /// portion in the control.
        /// </summary>
        public static readonly DependencyProperty DisplayTextForegroundBrushProperty =
            DependencyProperty.Register("DisplayTextForegroundBrush", typeof(SolidColorBrush),
                typeof(EditBox), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0,0,0))));

        /// <summary>
        /// TextProperty DependencyProperty should be used to indicate
        /// the string that should be edit in the <seealso cref="EditBox"/> control.
        /// </summary>
        private static readonly DependencyProperty TextProperty =
                DependencyProperty.Register(
                        "Text",
                        typeof(string),
                        typeof(EditBox),
                        new FrameworkPropertyMetadata(string.Empty,
                          new PropertyChangedCallback(OnTextChangedCallback)));

        /// <summary>
        /// Backing storage of DisplayText dependency property should be used to indicate
        /// the string that should displayed when <seealso cref="EditBox"/>
        /// control is not in edit mode.
        /// </summary>
        private static readonly DependencyProperty DisplayTextProperty =
                DependencyProperty.Register("DisplayText",
                                            typeof(string),
                                            typeof(EditBox),
                                            new PropertyMetadata(string.Empty));

        /// <summary>
        /// IsEditingProperty DependencyProperty
        /// </summary>
        private static readonly DependencyProperty IsEditingProperty =
                DependencyProperty.Register(
                        "IsEditing",
                        typeof(bool),
                        typeof(EditBox),
                        new FrameworkPropertyMetadata(false));

        /// <summary>
        /// Implement dependency property to determine whether editing data is allowed or not
        /// (control never enters editing mode if IsReadOnly is set to true [default is false])
        /// </summary>
        private static readonly DependencyProperty mIsReadOnlyProperty =
                DependencyProperty.Register(
                        "IsReadOnly",
                        typeof(bool),
                        typeof(EditBox),
                        new FrameworkPropertyMetadata(false));

        /// <summary>
        /// Send a Rename command request to the ViewModel if renaming has been executed
        /// 
        /// 1> Control entered Edit mode
        /// 2> String changed
        /// 3> Control left Edit Mode (with Enter or F2)
        /// </summary>
        public static readonly DependencyProperty RenameCommandProperty =
            DependencyProperty.Register("RenameCommand",
                                          typeof(ICommand),
                                          typeof(EditBox),
                                          new UIPropertyMetadata(null));

        /// <summary>
        /// Bind this parameter to supply additional information (such as item renamed)
        /// in rename command binding.
        /// </summary>
        public object RenameCommandParameter
        {
            get { return (object)GetValue(RenameCommandParameterProperty); }
            set { SetValue(RenameCommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RenameCommandParameter.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty RenameCommandParameterProperty =
            DependencyProperty.Register("RenameCommandParameter",
                typeof(object),
                typeof(EditBox), new PropertyMetadata(null));

        /// <summary>
        /// Send a Rename cancelled command request to the ViewModel if renaming has been cancelled
        /// 
        /// 1> Control entered Edit mode
        /// 2> Control left Edit Mode (with Escape)
        /// </summary>
        public static readonly DependencyProperty RenameCancelledCommandProperty =
            DependencyProperty.Register( "RenameCancelledCommand",
                                          typeof( ICommand ),
                                          typeof( EditBox ),
                                          new UIPropertyMetadata( null ) );

        /// <summary>
        /// Bind this parameter to supply additional information (such as item being renamed)
        /// in rename cancelled command binding.
        /// </summary>
        public object RenameCancelledCommandParameter
        {
            get { return (object) GetValue( RenameCancelledCommandParameterProperty ); }
            set { SetValue( RenameCancelledCommandParameterProperty, value ); }
        }

        // Using a DependencyProperty as the backing store for RenameCancelledCommandParameter.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty RenameCancelledCommandParameterProperty =
            DependencyProperty.Register( "RenameCancelledCommandParameter",
                typeof( object ),
                typeof( EditBox ), new PropertyMetadata( null ) );

        #region InvalidCharacters dependency properties
        /// <summary>
        /// Backing store of dependency property
        /// </summary>
        public static readonly DependencyProperty InvalidInputCharactersProperty =
            DependencyProperty.Register("InvalidInputCharacters",
                                        typeof(string), typeof(EditBox), new PropertyMetadata(null));

        /// <summary>
        /// Backing store of dependency property
        /// </summary>
        public static readonly DependencyProperty InvalidInputCharactersErrorMessageProperty =
            DependencyProperty.Register("InvalidInputCharactersErrorMessage",
                               typeof(string), typeof(EditBox), new PropertyMetadata(null));

        /// <summary>
        /// Backing store of dependency property
        /// </summary>
        public static readonly DependencyProperty InvalidInputCharactersErrorMessageTitleProperty =
            DependencyProperty.Register("InvalidInputCharactersErrorMessageTitle",
                               typeof(string), typeof(EditBox), new PropertyMetadata(null));
        #endregion InvalidCharacters dependency properties

        #region Mouse Events to trigger renaming with timed mouse click gesture
        /// <summary>
        /// This property can be used to enable/disable mouse "double click"
        /// gesture to start the edit mode (edit mode may also be started via
        /// context menu or F2 key only).
        /// </summary>
        public static readonly DependencyProperty IsEditableOnDoubleClickProperty =
            DependencyProperty.Register("IsEditableOnDoubleClick", typeof(bool), typeof(EditBox), new PropertyMetadata(true));

        /// <summary>
        /// The maximum time between the last click and the current one that will allow the user to edit the text block.
        /// This will allow the user to still be able to "double click to close" the TreeViewItem.
        /// Default is 700 ms.
        /// </summary>
        public static readonly DependencyProperty MaximumClickTimeProperty =
            DependencyProperty.Register("MaximumClickTime", typeof(double),
                                        typeof(EditBox), new UIPropertyMetadata(700d));

        /// <summary>
        /// The minimum time between the last click and the current one that will allow the user to edit the text block.
        /// This will help prevent against accidental edits when they are double clicking.
        /// Default is 300 ms.
        /// </summary>
        public static readonly DependencyProperty MinimumClickTimeProperty =
            DependencyProperty.Register("MinimumClickTime",
                                        typeof(double), typeof(EditBox), new UIPropertyMetadata(300d));

        DateTime _LastClicked;
        #endregion Mouse Events to trigger renaming with timed mouse click gesture
        #endregion dependency properties

        /// <summary>
        /// This object is used to secure thread safe methods by using the lock statement
        /// </summary>
        private readonly object _lockObject = new object();

        /// <summary>
        /// This is the adorner that is visible when the
        /// <seealso cref="EditBox"/> is in editing mode.
        /// </summary>
        private EditBoxAdorner _Adorner;

        /// <summary>
        /// A TextBox in the visual tree
        /// </summary>
        private TextBox _TextBox;

        /// <summary>
        /// This refers to the <seealso cref="ItemsControl"/> (TreeView/listView/ListBox)
        /// control that contains the EditBox
        /// </summary>
        private ItemsControl _ParentItemsControl;

        private SimpleNotificationWindow _Tip;

        /// <summary>
        /// This field points to a viewmodel that keeps the command binding and event triggering
        /// to base the notifications on. This enables the viewmodel to send events (notifications)
        /// for processing to the view.
        /// </summary>
        private IEditBox _ViewModel;

        private bool _DestroyNotificationOnFocusChange = false;
        #endregion fields

        #region constructor
        /// <summary>
        /// Static constructor
        /// </summary>
        static EditBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EditBox),
                new FrameworkPropertyMetadata(typeof(EditBox)));
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public EditBox()
        {
            _TextBox = null;

            // do not implement interaction logic for WPF Design-Time
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
                return;

            this.DataContextChanged += this.OnDataContextChanged;

            this.Loaded += EditBox_Loaded;
            this.Unloaded += OnEditBox_Unloaded;
        }

        private void EditBox_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= EditBox_Loaded;

            _ViewModel = DataContext as IEditBox;

            if (_ViewModel != null)
            {
                // Link to show notification pop-up message event
                _ViewModel.ShowNotificationMessage += ViewModel_ShowNotificationMessage;
                _ViewModel.RequestEdit += ViewModel_RequestEdit;
            }
        }
        #endregion constructor

        #region properties
        /// <summary>
        /// Gets/sets the foreground color of the textblock portion in the control.
        /// </summary>
        public SolidColorBrush DisplayTextForegroundBrush
        {
            get { return (SolidColorBrush)GetValue(DisplayTextForegroundBrushProperty); }
            set { SetValue(DisplayTextForegroundBrushProperty, value); }
        }

        /// <summary>
        /// Gets the text value for editing in the
        /// text portion of the EditBox.
        /// </summary>
        public string Text
        {
            private get
            {
                return (string)GetValue(EditBox.TextProperty);
            }

            set
            {
                SetValue(EditBox.TextProperty, value);
            }
        }

        /// <summary>
        /// Gets the text to display.
        /// 
        /// The DisplayText dependency property should be used to indicate
        /// the string that should displayed when <seealso cref="EditBox"/>
        /// control is not in edit mode.
        /// </summary>
        public string DisplayText
        {
            private get { return (string)this.GetValue(DisplayTextProperty); }
            set { this.SetValue(DisplayTextProperty, value); }
        }

        /// <summary>
        /// Implement dependency property to determine whether editing data is allowed or not
        /// (control never enters efiting mode if IsReadOnly is set to true [default is false])
        /// </summary>
        public bool IsReadOnly
        {
            get { return (bool)GetValue(mIsReadOnlyProperty); }
            set { SetValue(mIsReadOnlyProperty, value); }
        }

        /// <summary>
        /// Gets the scrollviewer in which this control is embeded.
        /// </summary>
        internal ScrollViewer ParentScrollViewer { get; private set; }

        /// <summary>
        /// Gets Editing mode which is true if the EditBox control
        /// is in editing mode, otherwise false.
        /// </summary>
        public bool IsEditing
        {
            get
            {
                return (bool)GetValue(EditBox.IsEditingProperty);
            }

            private set
            {
                SetValue(EditBox.IsEditingProperty, value);

                if (_Adorner != null)
                    _Adorner.UpdateVisibilty(value);
            }
        }

        /// <summary>
        /// Gets the command that is executed (if any is bound)
        /// to request a rename process via viemodel delegation.
        /// 
        /// The command parameter is a tuple containing the new name
        /// (as string) and the bound viewmodel on the datacontext
        /// of this control (as object). The CommandParameter is
        /// created by the control itself an needs no extra binding statement.
        /// </summary>
        public ICommand RenameCommand
        {
            get { return (ICommand)GetValue(RenameCommandProperty); }
            set { SetValue(RenameCommandProperty, value); }
        }

        /// <summary>
        /// Gets the command that is executed (if any is bound)
        /// when the rename process is cancelled.
        /// 
        /// The command parameter is the bound viewmodel on the datacontext
        /// of this control (as object). The CommandParameter is
        /// created by the control itself an needs no extra binding statement.
        /// </summary>
        public ICommand RenameCancelledCommand
        {
            get { return (ICommand) GetValue( RenameCancelledCommandProperty ); }
            set { SetValue( RenameCancelledCommandProperty, value ); }
        }

        #region InvalidCharacters dependency properties
        /// <summary>
        /// This property can be used to enable/disable maouse "double click"
        /// gesture to start the edit mode (edit mode may also be started via
        /// context menu or F2 key only).
        /// </summary>
        public bool IsEditableOnDoubleClick
        {
            get { return (bool)GetValue(IsEditableOnDoubleClickProperty); }
            set { SetValue(IsEditableOnDoubleClickProperty, value); }
        }

        /// <summary>
        /// Gets/sets the string dependency property that contains the characters
        /// that are considered to be invalid in the textbox input overlay element.
        /// </summary>
        public string InvalidInputCharacters
        {
            get { return (string)GetValue(InvalidInputCharactersProperty); }
            set { SetValue(InvalidInputCharactersProperty, value); }
        }

        /// <summary>
        /// Gets/sets the string dependency property that contains the error message
        /// that is shown when the user enters an invalid key.
        /// </summary>
        public string InvalidInputCharactersErrorMessage
        {
            get { return (string)GetValue(InvalidInputCharactersErrorMessageProperty); }
            set { SetValue(InvalidInputCharactersErrorMessageProperty, value); }
        }

        /// <summary>
        /// Gets/sets the string dependency property that contains
        /// the title of the error message that is shown when the user
        /// enters an invalid key. This title is similar to a window
        /// caption but it is not a window caption since the error message
        /// is shown in a custom pop-up element.
        /// </summary>
        public string InvalidInputCharactersErrorMessageTitle
        {
            get { return (string)GetValue(InvalidInputCharactersErrorMessageTitleProperty); }
            set { SetValue(InvalidInputCharactersErrorMessageTitleProperty, value); }
        }
        #endregion InvalidCharacters dependency properties

        #region Mouse Events to trigger renaming with timed mouse click gesture
        /// <summary>
        /// The minimum time between the last click and the current one that will allow the user to edit the text block.
        /// This will help prevent against accidental edits when they are double clicking.
        /// </summary>
        public double MinimumClickTime
        {
            get { return (double)GetValue(MinimumClickTimeProperty); }
            set { SetValue(MinimumClickTimeProperty, value); }
        }

        /// <summary>
        /// The maximum time between the last click and the current one that will allow the user to edit the text block.
        /// This will allow the user to still be able to "double click to close" the TreeViewItem.
        /// </summary>
        public double MaximumClickTime
        {
            get { return (double)GetValue(MaximumClickTimeProperty); }
            set { SetValue(MaximumClickTimeProperty, value); }
        }
        #endregion Mouse Events to trigger renaming with timed mouse click gesture
        #endregion properties

        #region methods
        /// <summary>
        /// Called when the tree for the EditBox has been generated.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _PART_TextBlock = this.GetTemplateChild("PART_TextBlock") as TextBlock;
            _PART_MeasureTextBlock = this.GetTemplateChild("PART_MeasureTextBlock") as TextBlock;

            // No TextBlock element -> no adorning element -> no adorener!
            if (_PART_MeasureTextBlock == null || _PART_TextBlock == null)
                return;

            _PART_TextBlock.MouseDown += TextBlock_LeftMouseDown;

            try
            {
                // Doing this here instead of in XAML makes the XAML easier to overview and apply correctly
                // Text="{Binding Path=DisplayText, Mode=OneWay, UpdateSourceTrigger=PropertyChanged, RelativeSource={RelativeSource TemplatedParent}}"
                // Bind TextBox onto editBox control property Text

                Binding binding = new Binding("DisplayText");
                binding.Source = this;
                binding.Mode = BindingMode.OneWay;
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                _PART_TextBlock.SetBinding(TextBlock.TextProperty, binding);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Free notification resources when parent window is being closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEditBox_Unloaded(object sender, EventArgs e)
        {
            this.DestroyTip();

            // Free event hook-up bewteen view and viewmodel
            ChangeViewModelLink(null, true);
        }

        /// <summary>
        /// Method is invoked when the datacontext is changed.
        /// This requires changing event hook-up on attached viewmodel to enable
        /// notification event conversion from viewmodel into view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ChangeViewModelLink(e.NewValue as IEditBox);
        }

        private void ChangeViewModelLink(IEditBox viewmodel, bool destroyEvents=false)
        {
            if (_ViewModel != null)
            {
                _ViewModel.ShowNotificationMessage -= ViewModel_ShowNotificationMessage;
                _ViewModel.RequestEdit -= ViewModel_RequestEdit;
            }

            _ViewModel = viewmodel;

            if (_ViewModel != null)
            {
                // Link to show notification pop-up message event
                _ViewModel.ShowNotificationMessage += ViewModel_ShowNotificationMessage;
                _ViewModel.RequestEdit += ViewModel_RequestEdit;
            }
            else
            {
                if (destroyEvents == false)
                    Console.WriteLine("----> VIEW Cannot find IEditBox interface.");
            }
        }

        /// <summary>
        /// Source: http://www.codeproject.com/Articles/31592/Editable-TextBlock-in-WPF-for-In-place-Editing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBlock_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.IsEditableOnDoubleClick == false)
                return;

            if (e.ChangedButton != MouseButton.Left)
                return;

            double timeBetweenClicks = (DateTime.Now - _LastClicked).TotalMilliseconds;
            _LastClicked = DateTime.Now;

            if (timeBetweenClicks > MinimumClickTime && timeBetweenClicks < MaximumClickTime)
            {
                this.OnSwitchToEditingMode();

                var t = _TextBox as TextBox;

                if (t != null)
                    t.SelectAll();
            }

            e.Handled = false;
        }

        /// <summary>
        /// Method is invoked when the viewmodel tells the view: Start to edit the name of the item we represent.
        /// (eg: Start to rename a folder).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewModel_RequestEdit(object sender, Events.RequestEdit e)
        {
            if (this.IsEditing == false)
            {
                this.OnSwitchToEditingMode();

                var t = _TextBox as TextBox;

                if (t != null)
                    t.SelectAll();
            }
        }

        #region ShowNotification
        /// <summary>
        /// Method is invoked when the viewmodel tells the view: Show another notification to the user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewModel_ShowNotificationMessage(object sender, UserNotification.Events.ShowNotificationEvent e)
        {
            this.ShowNotification(e.Title, e.Message, true);
        }

        /// <summary>
        /// Shows a notification warning to the user to clarify the current application behavior.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="notifiedFromViewmodel"></param>
        private void ShowNotification(string title, string message, bool notifiedFromViewmodel)
        {
            lock (_lockObject)
            {
                _DestroyNotificationOnFocusChange = notifiedFromViewmodel;

                if (_Tip == null)
                {
                    _Tip = new SimpleNotificationWindow();

                    var ownerWindow = this.GetDpObjectFromVisualTree(this, typeof(Window)) as Window;
                    _Tip.Owner = ownerWindow;
                }

                NotificationViewModel vm = new NotificationViewModel()
                {
                    Title = (string.IsNullOrEmpty(title) ? InplaceEditBoxLib.Local.Strings.STR_MSG_InvalidChar_TITLE : title),
                    Message = message,
                    IsTopmost = false
                };

                _Tip.ShowNotification(vm, this);
            }
        }
        #endregion ShowNotification

        #region textbox events
        /// <summary>
        /// Previews input from TextBox and cancels those characters (with pop-up error message)
        /// that do not appear to be valid (based on given array of invalid characters and error message.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Nothing to process if this dependency property is not set
            if (string.IsNullOrEmpty(this.InvalidInputCharacters) == true)
                return;

            if (e == null)
                return;

            lock (_lockObject)
            {
                if (this.IsEditing == true)
                {
                    foreach (char item in this.InvalidInputCharacters.ToCharArray())
                    {
                        if (string.Compare(e.Text, item.ToString(), false) == 0)
                        {
                            e.Handled = true;
                            break;
                        }
                    }

                    if (e.Handled == true && string.IsNullOrEmpty(this.InvalidInputCharactersErrorMessage) == false)
                    {
                        this.ShowNotification(this.InvalidInputCharactersErrorMessageTitle,
                                              this.InvalidInputCharactersErrorMessage, false);
                    }
                    else
                        this.DestroyTip(); // Entered string seems to be valid so lets put away that pop-up thing
                }
            }
        }

        /// <summary>
        /// This handler method is called when the dependency property <seealso cref="EditBox.TextProperty"/>
        /// is changed in the data source (the ViewModel). The event is raised to tell the view to update its display.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnTextChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(EditBox.TextProperty);
            ////var vm = (EditBox)d;
            ////
            ////vm.Text = (string)e.NewValue;
        }

        /// <summary>
        /// When an EditBox is in editing mode, pressing the ENTER or F2
        /// keys switches the EditBox to normal mode.
        /// </summary>
        private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            lock (_lockObject)
            {
                if (e.Key == Key.Escape)
                {
                    this.OnSwitchToNormalMode();
                    e.Handled = true;

                    return;
                }

                // Cancel editing mode (editing string is OK'ed)
                if (this.IsEditing == true && (e.Key == Key.Enter || e.Key == Key.F2))
                {
                    this.OnSwitchToNormalMode(false);
                    e.Handled = true;

                    return;
                }
            }
        }

        /// <summary>
        /// If an EditBox loses focus while it is in editing mode, 
        /// the EditBox mode switches to normal mode.
        /// </summary>
        private void OnTextBoxLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            this.OnSwitchToNormalMode();
        }

        /// <summary>
        /// Ends the editing mode if textbox loses the focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            this.OnSwitchToNormalMode();
        }
        #endregion textbox events

        /// <summary>
        /// If an EditBox is in editing mode and the content of a ListView is
        /// scrolled, then the EditBox switches to normal mode.
        /// </summary>
        private void OnScrollViewerChanged(object sender, ScrollChangedEventArgs args)
        {
            if (args.HorizontalOffset == 0 && args.VerticalOffset == 0)
                return;

            if (this.IsEditing && Mouse.PrimaryDevice.LeftButton == MouseButtonState.Pressed)
                this.OnSwitchToNormalMode();
        }

        /// <summary>
        /// Sets IsEditing to false when the ViewItem that contains an EditBox changes its size
        /// </summary>
        /// <param name="bCancelEdit"></param>
        private void OnSwitchToNormalMode(bool bCancelEdit = true)
        {
            lock (_lockObject)
            {
                if (_DestroyNotificationOnFocusChange == false)
                    DestroyTip();

                if (IsEditing == true)
                {
                    string sNewName = string.Empty;

                    if (_TextBox != null)
                        sNewName = _TextBox.Text;

                    if (bCancelEdit == false)
                    {
                        if (_TextBox != null)
                        {
                            // Tell the ViewModel (if any) that we'd like to rename this item
                            if (RenameCommand != null)
                            {
                                var tuple = new Tuple<string, object>(sNewName, RenameCommandParameter);
                                RenameCommand.Execute(tuple);
                            }
                        }
                    }
                    else
                    {
                        if (_TextBox != null)
                        {
                            _TextBox.Text = this.Text;

                            // Tell the ViewModel (if any) that renaming of this item has been cancelled
                            if (RenameCancelledCommand != null)
                            {
                                RenameCancelledCommand.Execute(RenameCommandParameter);
                            }
                        }
                    }

                    IsEditing = false;

                    Mouse.RemovePreviewMouseDownOutsideCapturedElementHandler(this, OnMouseDownOutsideElement);
                    ReleaseMouseCapture();

                    _PART_TextBlock.Focus();
                    _PART_TextBlock.Visibility = System.Windows.Visibility.Visible;

                    _PART_MeasureTextBlock.Visibility = System.Windows.Visibility.Hidden;
                }
            }
        }

        /// <summary>
        /// Displays the adorner textbox to let the user edit the text string.
        /// </summary>
        private void OnSwitchToEditingMode()
        {
            lock (_lockObject)
            {
                if (IsReadOnly == false && IsEditing == false)
                {
                    if (_PART_MeasureTextBlock != null && _PART_TextBlock != null)
                    {
                        if (_TextBox == null)
                            HookItemsControlEvents();

                        _PART_TextBlock.Visibility = System.Windows.Visibility.Hidden;
                        _PART_MeasureTextBlock.Visibility = System.Windows.Visibility.Hidden;

                        // Bugfix: Need to do this instead of = this.mPART_TextBlock.Text;
                        // in order to display one string
                        // but use the correct string in edit mode.
                        _TextBox.Text = Text;
                    }

                    IsEditing = true;
                }
            }
        }

        /// <summary>
        /// Walk the visual tree to find the ItemsControl and 
        /// hook into some of its events. This is done to make
        /// sure that editing is cancelled whenever
        /// 
        ///   1> The parent control is scrolling its content
        /// 1.1> The MouseWheel is used
        ///   2> A user clicks outside the adorner control
        ///   3> The parent control changes its size
        /// 
        /// </summary>
        private void HookItemsControlEvents()
        {
            if (_PART_MeasureTextBlock == null)
                return;

            _TextBox = new TextBox();

            _Adorner = new EditBoxAdorner(_PART_MeasureTextBlock, _TextBox, this);
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(_PART_MeasureTextBlock);

            if (layer == null) // This layer does not always seem to be available?
                return;

            layer.Add(_Adorner);

            // try to get the text box focused when layout finishes.
            _TextBox.LayoutUpdated += new EventHandler(this.OnTextBoxLayoutUpdated);

            CaptureMouse();
            Mouse.AddPreviewMouseDownOutsideCapturedElementHandler(this, OnMouseDownOutsideElement);

            _TextBox.PreviewTextInput += OnPreviewTextInput;
            _TextBox.KeyDown += new KeyEventHandler(this.OnTextBoxKeyDown);
            _TextBox.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(this.OnTextBoxLostKeyboardFocus);

            _TextBox.LostFocus += new RoutedEventHandler(this.OnLostFocus);

            _ParentItemsControl = this.GetDpObjectFromVisualTree(this, typeof(ItemsControl)) as ItemsControl;
            Debug.Assert(_ParentItemsControl != null, "DEBUG ISSUE: No FolderTreeView found.");

            if (_ParentItemsControl != null)
            {
                // Handle events on parent control and determine whether to switch to Normal mode or stay in editing mode
                _ParentItemsControl.AddHandler(ScrollViewer.ScrollChangedEvent, new ScrollChangedEventHandler(this.OnScrollViewerChanged));
                _ParentItemsControl.AddHandler(ScrollViewer.MouseWheelEvent, new RoutedEventHandler((s, e) => this.OnSwitchToNormalMode()), true);

                _ParentItemsControl.MouseDown += new MouseButtonEventHandler((s, e) => this.OnSwitchToNormalMode());
                _ParentItemsControl.SizeChanged += new SizeChangedEventHandler((s, e) => this.OnSwitchToNormalMode());

                // Restrict text box to visible area of scrollviewer
                this.ParentScrollViewer = this.GetDpObjectFromVisualTree(_ParentItemsControl, typeof(ScrollViewer)) as ScrollViewer;

                if (this.ParentScrollViewer == null)
                    this.ParentScrollViewer = FindVisualChild<ScrollViewer>(_ParentItemsControl);

                Debug.Assert(this.ParentScrollViewer != null, "DEBUG ISSUE: No ScrollViewer found.");

                if (this.ParentScrollViewer != null)
                    _TextBox.MaxWidth = this.ParentScrollViewer.ViewportWidth;
            }
        }

        /// <summary>
        /// Walk visual tree to find the first DependencyObject of a specific type.
        /// (This method works for finding a ScrollViewer within a TreeView).
        /// </summary>
        private DependencyObject GetDpObjectFromVisualTree(DependencyObject startObject, Type type)
        {
            // Walk the visual tree to get the parent(ItemsControl)
            // of this control
            DependencyObject parent = startObject;
            while (parent != null)
            {
                if (type.IsInstanceOfType(parent))
                    break;
                else
                    parent = VisualTreeHelper.GetParent(parent);
            }

            return parent;
        }

        /// <summary>
        /// Walk visual tree to find the first DependencyObject of a specific type.
        /// (This method works for finding a ScrollViewer within a TreeView).
        /// Source: http://stackoverflow.com/questions/3963341/get-reference-to-my-wpf-listboxs-scrollviewer-in-c
        /// </summary>
        /// <typeparam name="childItem"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);

                if (child != null && child is childItem)
                    return (childItem)child;

                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);

                    if (childOfChild != null)
                        return childOfChild;
                }
            }

            return null;
        }

        /// <summary>
        /// Destroys the notification object to ensure the window is closed when control exits.
        /// </summary>
        private void DestroyTip()
        {
            lock (_lockObject)
            {
                if (_Tip != null)
                {
                    _Tip.HideNotification();
                    _Tip.CloseInvokedByParent();

                    _Tip = null;
                }
            }
        }

        /// <summary>
        /// When Layout finish, if in editable mode, update focus status on TextBox.
        /// </summary>
        private void OnTextBoxLayoutUpdated(object sender, EventArgs e)
        {
            if (_Adorner == null)
                return;

            if (_Adorner.IsTextBoxVisible == true)
            {
                if (_TextBox.IsFocused == false)
                {
                    _TextBox.Focus();
                }
            }
        }

        /// <summary>
        /// Escape Edit Mode when the user clicks outside of the textbox
        /// https://stackoverflow.com/questions/6489032/wpf-remove-focus-when-clicking-outside-of-a-textbox#6489274
        /// 
        /// Thanks for helpful hints to Alaa Ben Fatma.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseDownOutsideElement(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            OnSwitchToNormalMode();
        }
        #endregion methods
    }
}
