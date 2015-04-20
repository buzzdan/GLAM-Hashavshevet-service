//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TestStack.White;
//using TestStack.White.Factory;
//using TestStack.White.UIItems;
//using TestStack.White.UIItems.Finders;
//using TestStack.White.UIItems.ListBoxItems;
//using TestStack.White.UIItems.MenuItems;
//using TestStack.White.UIItems.WindowItems;

//namespace UIMacro
//{
//    public class Program
//    {
//        private static void Main(string[] args)
//        {
//            #region Procedural code

//            ////Launchs notepad application and returns its Application object and saves it inside notepadApplication pointer
//            //Application notepadApplication = Application.Launch("notepad.exe");

//            ////Get all open windows of the specific application (notepad)
//            //var allOpenWindows = notepadApplication.GetWindows();

//            ////finds the notepad window by searching it's title in allOpenWindows
//            //Window notepad = allOpenWindows.Find(obj => obj.Title.Contains("Notepad"));
//            //notepad.WaitWhileBusy();
//            //notepad.Enter("hello GLAM!");

//            ////Get the main document (textbox) of notepad by its automation ID (if automationID is missing we can find it by its name)
//            ////how did we find the Automation ID?
//            ////we used "Visual UI Automation Verify" which comes with Windows 8 SDK and available for download from: http://msdn.microsoft.com/en-US/windows/desktop/bg162891
//            ////in "Visual UI Automation Verify" we look for notepad window and inside of it we see ("document", "", "15")
//            ////how do we make sure that we got to the correct UI Element?
//            ////We look in notepad itself and we see red outline around the element we chose in the "Visual UI Automation Verify"
//            //var mainTextArea = notepad.Get<TextBox>(SearchCriteria.ByAutomationId("15"));
//            //Console.WriteLine(mainTextArea.Text);

//            ////we look in the Verify program the name of the menu bar- which is "MenuBar", than we find "MenuItem"
//            //// to get the spesific bottun we write every outern button that we need to stand on it to get to the next one. here: "Format", "Font..."
//            //var font = notepad.MenuBar.MenuItem("Format", "Font...");

//            ////we click on the button we saved
//            //font.Click();

//            ////we save an object with the FOnt dialog (it is called "ModalWindow" since you can't go to any other window while its open
//            //var fontWindow = notepad.ModalWindow("Font");

//            ////we find the 72 font listItem object save it in a pointer
//            //var font72 = fontWindow.Get<ListItem>("72");

//            ////we click on it
//            //font72.Click();

//            //////we find the OK button by its AutomationID
//            ////var okBtn = fontWindow.Get<Button>(SearchCriteria.ByAutomationId("1"));

//            //////we click on it!!!!!
//            ////okBtn.Click();dural code

//            #endregion Procedural code

//            #region OOP code

//            var notepadApplication = NotepadApplication.LaunchNotepad();
//            var notepadWindow = notepadApplication.GetNotepadWindow();
//            notepadWindow.SetFontSize(12);

//            var oneSecond = TimeSpan.FromSeconds(1);
//            notepadWindow.TypeAndWait("Hello Shani", oneSecond);
//            notepadWindow.TypeAndWait("\n", oneSecond);

//            var fontDialog = notepadWindow.OpenFontDialog();
//            fontDialog.SetFontSize(72);
//            fontDialog.ClickOk();

//            notepadWindow.TypeAndWait("\n", oneSecond);
//            notepadWindow.TypeAndWait("Closing in...\n", oneSecond);
//            notepadWindow.TypeAndWait("5, ", oneSecond);
//            notepadWindow.TypeAndWait("4, ", oneSecond);
//            notepadWindow.TypeAndWait("3, ", oneSecond);
//            notepadWindow.TypeAndWait("2, ", oneSecond);
//            notepadWindow.TypeAndWait("1...", oneSecond);

//            notepadApplication.Kill();

//            #endregion OOP code
//        }
//    }
//}