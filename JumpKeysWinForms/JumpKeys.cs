﻿
namespace JumpKeysWinForms
{

    public class JumpKeys
    {
        private static Form _form;
        private static Dictionary<Control, ControlNavigationBase> navigationMapping = new Dictionary<Control, ControlNavigationBase>();

        public static MenuStripNavigation MenuStrip(MenuStrip menuStrip)
        {
            if (menuStrip == null)
            {
                throw new ArgumentNullException();
            }

            var menuStripNavigation = new MenuStripNavigation(menuStrip);

            if (!navigationMapping.ContainsKey(menuStrip))
            {
                navigationMapping.Add(menuStrip, menuStripNavigation);
                return menuStripNavigation;
            }
            else
            {
                throw new Exception("Navigation already exists for " + menuStrip.ToString());
                // implement cleanup
            }
        }




        internal static void MenuItemHandlePreviewKeyDown(object s, PreviewKeyDownEventArgs e)
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

        internal static void MenuItemHandleKeyDown(object s, KeyEventArgs e)
        {
            bool forward = e.Modifiers == Keys.None;
            // MessageBox.Show("tab pressed");
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
                        var nextItem = FindNextToolStripItem(menuStrip.Items, toolStripItem);
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

        private static ToolStripItem? FindNextToolStripItem(ToolStripItemCollection items, ToolStripItem item)
        {
            var curIndex = items.IndexOf(item);
            while (curIndex < items.Count - 1)
            {
                curIndex++;

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

        public static void SetupForm(Form form)
        {
            _form = form;
            // main form catches keystrokes before they reach any of child controls
            form.KeyPreview = true;
            hookKeyboardKeys();
        }

        public static void SayHello()
        {
            MessageBox.Show("Hello JumpKeys");
        }

        private static void hookKeyboardKeys()
        {
            return;
        }
    }
}