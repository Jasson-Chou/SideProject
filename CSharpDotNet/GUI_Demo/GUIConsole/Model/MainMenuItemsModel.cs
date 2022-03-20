using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIConsole.Model
{
    public class MainMenuModel:MenuModel
    {
        /// <summary>
        /// User Definition
        /// </summary>
        public enum Menu
        {
            M1 = 1,
            M2,
            M3,
            Exit = 99,
        }

        public string MenuName { get; }

        public MainMenuModel():base(typeof(Menu))
        {
            MenuName = "Main Menu";
        }

        public bool TryGetMenuType(int type,out Menu menuType)
        {
            menuType = default(Menu);
            if (base.TryGetMenuType(type, out object _menuType))
            {
                menuType = (Menu)type;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return $"{MenuName}{Environment.NewLine}{base.ToString()}";
        }

    }
}
