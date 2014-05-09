using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace D3HeroesTool
{
    public static class D3Commands
    {
        public static RoutedCommand InvokeHeroes { get; private set; }
        
        static D3Commands()
        {
            InvokeHeroes = new RoutedCommand("InvokeHeroes", typeof(D3Commands));
        }
    }
}
