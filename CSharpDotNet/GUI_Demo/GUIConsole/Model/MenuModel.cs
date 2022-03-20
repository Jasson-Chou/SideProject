using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIConsole.Model
{
    public class MenuModel
    {
        private string _menu;

        public MenuModel()
        {
            
        }

        public MenuModel(Type menuType)
        {
            MenuType = menuType;
        }

        protected bool TryGetMenuType(int type, out object menuType)
        {
            menuType = Enum.ToObject(MenuType, type);
            return menuType != null;
        }

        public override string ToString()
        {
            return _menu;
        }

        private Type _menuType;
        public Type MenuType
        {
            get => _menuType;
            set
            {
                _menuType = value;
                _menu = string.Empty;
                var menus = Enum.GetNames(MenuType);
                foreach (var menu in menus)
                {
                    var eMenu = Enum.Parse(MenuType, menu);
                    int val = (int)eMenu;
                    _menu += $"[{val}.] {menu}{Environment.NewLine}";
                }
            }
        }

        
    }
}
