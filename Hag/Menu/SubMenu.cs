using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Hag.Menu
{
    class SubMenu : Entity
    {
        public SubMenu(string text, string description)
        {
            base.Name = text;
            base.Description = description;
        }
        public List<Entity> Items = new List<Entity>();
        public int index = 0;

    }
}
