using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestStack.White.UIItems.WindowItems;

namespace GlamHashAdapter
{
    public static class Extentions
    {
        public static void Press(this Window window, TestStack.White.WindowsAPI.KeyboardInput.SpecialKeys key, int times = 1)
        {
            for (int i = 0; i < times; i++)
            {
                window.Keyboard.PressSpecialKey(key);
            }
        }
    }
}
