
namespace JumpKeys
{

    public class JKSetup
    {
        //private static Form _form;
        private static Dictionary<Control, ControlNavigationBase> navigationMapping = new();

        public static MenuStripNavigation ForMenuStrip(MenuStrip menuStrip)
        {
            var menuStripNavigation = new MenuStripNavigation(menuStrip);

            if (!navigationMapping.ContainsKey(menuStrip))
            {
                navigationMapping.Add(menuStrip, menuStripNavigation);
                return menuStripNavigation;
            }
            else
            {
                throw new Exception("Navigation already exists for " + menuStrip.Name);
                // implement cleanup
            }
        }

       /* public static void SetupForm(Form form)
        {
            //_form = form;
            // main form catches keystrokes before they reach any of child controls
           // form.KeyPreview = true;
           // hookKeyboardKeys();
        }*/
    }
}