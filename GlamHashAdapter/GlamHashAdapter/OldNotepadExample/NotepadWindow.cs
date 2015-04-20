using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;
using TestStack.White;
using TestStack.White.ScreenObjects;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.WindowItems;

namespace UIMacro
{
    public class NotepadWindow : AppScreen
    {
        private TextBox mainTextArea;
        //private Window notepadWindow;

        public NotepadWindow(Window notepadWindow, ScreenRepository repository)
            : base(notepadWindow, repository)
        {
        }

        public TextBox MainTextArea
        {
            get
            {
                if (this.mainTextArea == null)
                {
                    this.mainTextArea = this.Window.Get<TextBox>(SearchCriteria.ByAutomationId("15"));
                }
                return mainTextArea;
            }
        }

        public FontDialog OpenFontDialog()
        {
            //var font = notepadWindow.MenuBar.MenuItem("Format", "Font...");
            var font = this.Window.MenuBar.TopLevelMenu[2].ChildMenus[1];

            //var font = , SearchCriteria.Indexed(1));
            font.Click();
            var fontModalWindow = this.Window.ModalWindows()[0];
            return new FontDialog(fontModalWindow);
        }

        public void SetFontSize(int size)
        {
            var fontDialog = OpenFontDialog();
            fontDialog.SetFontSize(size);
            fontDialog.ClickOk();
        }

        public void TypeAndWait(string text, TimeSpan timeToWait)
        {
            this.Window.Enter(text);
            Thread.Sleep(timeToWait);
        }

        public void WaitWhileBusy()
        {
            this.Window.WaitWhileBusy();
        }
    }
}