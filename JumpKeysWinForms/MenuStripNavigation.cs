
namespace JumpKeys
{
    public class MenuStripNavigation : ControlNavigationBase
    {
        private int _skipCount = 0;
        private readonly MenuStrip _control;
        //private bool _registered = false;

        internal MenuStripNavigation(MenuStrip control)
        {
            _control = control;

        }

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
                    combo.ComboBox.PreviewKeyDown += ToolStripComboBoxHandlePreviewKeyDown;
                    combo.ComboBox.KeyDown += ToolStripComboBoxHandleKeyDown;
                }
            }

            // register TextBox handlers
            foreach (var item in _control.Items)
            {
                if (item as ToolStripTextBox != null)
                {
                    var textBox = (ToolStripTextBox)item;
                    textBox.TextBox.PreviewKeyDown += ToolStripTextBoxHandlePreviewKeyDown;
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

        private void ToolStripTextBoxHandlePreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                //last item selected
                if (e.Modifiers == Keys.None && _control.Items[_control.Items.Count - 1].Selected)
                {
                    return;
                }

                e.IsInputKey = true;
            }
        }

        public MenuStripNavigation Skip(int skipCount)
        {
            if(skipCount >= 0)
                _skipCount = skipCount;
            return this;
        }

        internal void MenuItemHandlePreviewKeyDown(object s, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                var menuStrip = s as MenuStrip;
                //last item selected
                if (e.Modifiers == Keys.None && menuStrip.Items[menuStrip.Items.Count - 1].Selected)
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
                        SelectToolStripItem(nextItem);
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

        internal void ToolStripComboBoxHandlePreviewKeyDown (object s, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                //last item selected
                if (e.Modifiers == Keys.None && _control.Items[_control.Items.Count - 1].Selected)
                {
                    return;
                }

                // must check last item ???
                e.IsInputKey = true;
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
            while (curIndex < items.Count - 1)
            {
                curIndex += _skipCount+1;

                //index overflow
                if(curIndex > items.Count - 1)
                {
                    curIndex = items.Count - 1;
                }

                return items[curIndex];
            }

            return null;
        }
    }
}
