

namespace JumpKeysWinForms
{
    public class MenuStripNavigation : ControlNavigationBase
    {
        private int _skipCount = 0;
        private readonly Control _control;
        private bool _registered = false;

        internal MenuStripNavigation(Control control)
        {
            _control = control;
        }

        public override void Register()
        {
            _control.PreviewKeyDown += MenuItemHandlePreviewKeyDown;
            _control.KeyDown += MenuItemHandleKeyDown;
            _registered = true;
        }

        public MenuStripNavigation Skip(int skipCount)
        {
            if(skipCount >= 0)
                _skipCount = skipCount;
            return this;
        }

        internal void MenuItemHandlePreviewKeyDown(object s, PreviewKeyDownEventArgs e)
        {
            if (e.IsInputKey)
            {
                return;
            }

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
            bool forward = e.Modifiers == Keys.None;
            var menuStrip = s as MenuStrip;
            // System.Collections.IList listOfItems = menuStrip.Items;
            foreach (var item in menuStrip.Items)
            {
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
                        var nextItem = findNextToolStripItem(menuStrip.Items, toolStripItem);
                        if (nextItem != null)
                        {
                            if (nextItem as ToolStripComboBox != null)
                            {
                                //ToolStripComboBox castItem = (ToolStripComboBox)nextItem;
                                //castItem.Focus();
                            }
                            else
                            {
                                nextItem.Select();
                                break;
                            }
                        }
                        // break;
                    }

                    break;
                }
            }
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private ToolStripItem? findNextToolStripItem(ToolStripItemCollection items, ToolStripItem item)
        {
            var curIndex = items.IndexOf(item);
            while (curIndex < items.Count - 1)
            {
                curIndex += _skipCount+1;

                //index overflow
                if(curIndex > items.Count - 1)
                {
                    curIndex = items.Count - 1;
                }

                if (items[curIndex] as ToolStripComboBox != null)
                {
                    //ToolStripComboBox curItem = (ToolStripComboBox)items[curIndex];
                    //curItem.Focus();

                    continue;
                }

                if (items[curIndex] as ToolStripTextBox != null)
                {
                    continue;
                }

                return items[curIndex];
            }

            return null;
        }
    }
}
