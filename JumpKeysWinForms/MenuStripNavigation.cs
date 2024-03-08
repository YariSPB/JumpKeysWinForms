
namespace JumpKeys
{
    /// <summary>
    /// a class containing navigation methods and state for a MainStrip instance
    /// </summary>
    public class MenuStripNavigation : ControlNavigationBase
    {
        private bool skipComboBox;
        private bool skipTextBox;
        private int jumpAfterCount = 0;
        private readonly MenuStrip _control;
        //private bool _registered = false;

        internal MenuStripNavigation(MenuStrip control)
        {
            _control = control;

        }

        /// <summary>
        /// Completes navigation setup for a control
        /// </summary>
        public override void Register()
        {
            _control.PreviewKeyDown += MenuItemHandlePreviewKeyDown;
            _control.KeyDown += MenuItemHandleKeyDown;
            // register ComboBox handlers
            foreach (var item in _control.Items)
            {
                if (item as ToolStripComboBox != null)
                {
                    var combo = (ToolStripComboBox)item;
                    combo.ComboBox.PreviewKeyDown += MenuItemHandlePreviewKeyDown;
                    combo.ComboBox.KeyDown += ToolStripComboBoxHandleKeyDown;
                }
            }

            // register TextBox handlers
            foreach (var item in _control.Items)
            {
                if (item as ToolStripTextBox != null)
                {
                    var textBox = (ToolStripTextBox)item;
                    textBox.TextBox.PreviewKeyDown += MenuItemHandlePreviewKeyDown;
                    textBox.TextBox.KeyDown += ToolStripTextBoxHandleKeyDown;
                }
            }
         //   _registered = true;
        }

        private void ToolStripTextBoxHandleKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                bool forward = e.Modifiers == Keys.None;
                ToolStripTextBox toolStripTextBox = FindToolStripTextBoxForm((TextBox)sender);

                if (forward)
                {
                    // deactivate combobox by focusing parent control
                    _control.Focus();
                    var nextItem = FindNextToolStripItem(_control.Items, toolStripTextBox);
                    SelectToolStripItem(nextItem);

                }

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private ToolStripTextBox FindToolStripTextBoxForm(TextBox sender)
        {
            foreach (var item in _control.Items)
            {
                if (item as ToolStripTextBox != null)
                {
                    var toolStripTextBox = (ToolStripTextBox)item;
                    if (toolStripTextBox.TextBox == sender)
                    {
                        return toolStripTextBox;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Jump over comboboxes
        /// </summary>
        /// <returns></returns>
        public MenuStripNavigation SkipComboBox()
        {
            skipComboBox = true;
            return this;
        }

        /// <summary>
        /// Jump over textboxes
        /// </summary>
        /// <returns></returns>
        public MenuStripNavigation SkipTextBox()
        {
            skipTextBox = true;
            return this;
        }

        /// <summary>
        /// Jump to next control after navigating this number of items
        /// </summary>
        /// <param name="count">items count</param>
        /// <returns></returns>
        public MenuStripNavigation JumpAfterCount(int count)
        {
            if(count > 0) {
                jumpAfterCount = count;
            }
            
            return this;
        }

        private void MenuItemHandlePreviewKeyDown(object s, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                var itemCount = jumpAfterCount > 0 ? Math.Min(jumpAfterCount, _control.Items.Count) : _control.Items.Count;
                //last item selected
                if (e.Modifiers == Keys.None && _control.Items[itemCount - 1].Selected)
                {
                    return;
                }

                // must check last item ???
                e.IsInputKey = true;
            }
        }

        internal void MenuItemHandleKeyDown(object s, KeyEventArgs e)
        {
            if(e.KeyCode != Keys.Tab) return;
            bool forward = e.Modifiers == Keys.None;
            var menuStrip = s as MenuStrip;
            // System.Collections.IList listOfItems = menuStrip.Items;
            foreach (var item in menuStrip.Items)
            {
                //doens't stick with combobox or textbox controls anyway
                if (item as ToolStripComboBox != null)
                {
                    continue;
                }

                if (item as ToolStripTextBox != null)
                {
                    continue;
                }

                var toolStripItem = (ToolStripItem)item;
                if (toolStripItem.Selected)
                {
                    if (forward)
                    {
                        var nextItem = FindNextToolStripItem(menuStrip.Items, toolStripItem);
                        if(nextItem != null)
                        {
                            SelectToolStripItem(nextItem);
                        }
                 
                    }

                    break;
                }
            }
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private static void SelectToolStripItem(ToolStripItem nextItem) {
            if (nextItem != null)
            {
                if (nextItem as ToolStripComboBox != null)
                {
                    ToolStripComboBox castItem = (ToolStripComboBox)nextItem;
                    castItem.Focus();
                } else if(nextItem as ToolStripTextBox != null)
                {
                    ToolStripTextBox textBox = (ToolStripTextBox)nextItem;
                    textBox.Focus();
                }
                else
                {
                    nextItem.Select();
                }
            }
        }

        internal void ToolStripComboBoxHandleKeyDown(object s, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Tab)
            {
                bool forward = e.Modifiers == Keys.None;
               // ComboBox combo = s as ComboBox;
                ToolStripComboBox toolStripComboBox = FindToolStripComboBoxForm(s as ComboBox);
                if (forward)
                {
                    // deactivate combobox by focusing parent control
                    _control.Focus();
                    var nextItem = FindNextToolStripItem(_control.Items, (object)toolStripComboBox);
                    SelectToolStripItem(nextItem);
                }

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private ToolStripComboBox FindToolStripComboBoxForm(ComboBox combo)
        {
            foreach (var item in _control.Items)
            {
                if (item as ToolStripComboBox != null)
                {
                    var comboToolStrip = (ToolStripComboBox)item;
                    if (comboToolStrip.ComboBox == combo)
                    {
                        return comboToolStrip;
                    }
                }
            }

            return null;
        }

        private ToolStripItem FindNextToolStripItem(ToolStripItemCollection items, Object item)
        {
            var curIndex = items.IndexOf((ToolStripItem)item);
            var itemCount = jumpAfterCount > 0 ? Math.Min(jumpAfterCount, items.Count) : items.Count;
            while (curIndex < itemCount - 1)
            {
                curIndex ++;

                //index overflow
                if(curIndex > items.Count - 1)
                {
                    curIndex = items.Count - 1;
                }

                if ( skipComboBox == true && items[curIndex] as ToolStripComboBox != null) {
                    continue;
                }

                if (skipTextBox == true && items[curIndex] as ToolStripTextBox != null)
                {
                    continue;
                }

                return items[curIndex];
            }

            return null;
        }
    }
}
