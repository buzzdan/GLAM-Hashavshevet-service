using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.White;
using TestStack.White.ScreenObjects;
using TestStack.White.UIItems.WindowItems;

namespace UIMacro
{
    internal class NotepadApplication
    {
        private Application application;
        private ScreenRepository screenRepository;

        public static NotepadApplication LaunchNotepad()
        {
            NotepadApplication NotepadApp = new NotepadApplication();
            NotepadApp.application = Application.Launch(@"c:\windows\system32\notepad.exe");
            NotepadApp.screenRepository = new ScreenRepository(NotepadApp.application.ApplicationSession);

            return NotepadApp;
        }

        public NotepadWindow GetNotepadWindow()
        {
            var allOpenWindows = application.GetWindows();
            Window notepad = allOpenWindows[0];
            return new NotepadWindow(notepad, screenRepository);
        }

        public void Kill()
        {
            application.Kill();
        }
    }
}