using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.WindowItems;

namespace UIMacro
{
    public class FontDialog
    {
        private Window fontDialog;

        public FontDialog(Window fontDialog)
        {
            if (fontDialog == null)
            {
                throw new ArgumentNullException("notepadWindow");
            }

            this.fontDialog = fontDialog;
        }

        public bool IsClosed
        {
            get
            {
                return fontDialog.IsClosed;
            }
        }

        public void ClickOk()
        {
            CheckPreConditions();
            var okBtn = fontDialog.Get<Button>(SearchCriteria.ByAutomationId("1"));
            okBtn.Click();
        }

        public void SetFontSize(int size)
        {
            CheckPreConditions();
            var sizeCombobox = fontDialog.Get<ComboBox>(SearchCriteria.ByAutomationId("1138"));
            var sizeTextbox = sizeCombobox.EditableText = size.ToString();
        }

        private void CheckPreConditions()
        {
            if (fontDialog.IsClosed)
            {
                throw new InvalidOperationException("cannot perfor action since dialog is closed");
            }
        }
    }
}