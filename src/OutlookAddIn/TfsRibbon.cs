using Microsoft.Office.Interop.Outlook;
using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CP.TfsAssistant.Libraires;

namespace CP.TfsAssistant.OutlookAddIn
{
    public partial class TfsRibbon
    {
        private MailItem _mailItem;

        private void TfsRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            if (this.RibbonId == "Microsoft.Outlook.Explorer")
            {
                Explorer explorer = Globals.ThisAddIn.Application.ActiveExplorer();
                explorer.SelectionChange += ExplorerSelectionChange;
            }
            else if (this.RibbonId == "Microsoft.Outlook.Mail.Read")
            {
                Inspector inspector = Globals.ThisAddIn.Application.ActiveInspector();
                _mailItem = inspector.CurrentItem as MailItem;
            }
        }

        private void ExplorerSelectionChange()
        {
            _mailItem = null;
            Explorer explorer = Globals.ThisAddIn.Application.ActiveExplorer();
            bool isSingleMailItemSelected = false;
            if (explorer.Selection.Count == 1)
            {
                var selected = explorer.Selection[1];
                if (selected is Microsoft.Office.Interop.Outlook.MailItem)
                {
                    isSingleMailItemSelected = true;
                    _mailItem = selected as MailItem;
                }
            }

            this.NewItemFromMailButton.Enabled = isSingleMailItemSelected;
        }

        private void NewItemButton_Click(object sender, RibbonControlEventArgs e)
        {
            EnsureSettingsValid();
            UIHelper.OpenWorkItemForm();
        }

        private void NewItemFromMailButton_Click(object sender, RibbonControlEventArgs e)
        {
            if (_mailItem == null)
            {
                return;
            }

            EnsureSettingsValid();

            StringBuilder body = new StringBuilder(_mailItem.HTMLBody);
            foreach (Attachment attachment in _mailItem.Attachments)
            {
                var fileName = attachment.FileName;
                if (IsImageFileExtension(Path.GetExtension(fileName)))
                {
                    string attach_content_id = @"http://schemas.microsoft.com/mapi/proptag/0x3712001E";
                    var contentId = attachment.PropertyAccessor.GetProperty(attach_content_id);
                    if (body.ToString().IndexOf(string.Format("src=\"cid:{0}\"", contentId), StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        var tempName = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + Path.GetExtension(fileName));
                        attachment.SaveAsFile(tempName);
                        body.Replace(string.Format("src=\"cid:{0}\"", contentId), string.Format("src=\"file:///{0}\"", tempName));
                    }
                }
            }

            var settings = MySettingsManager.GetSettings<TfsSettings>();
            var fields = new Dictionary<string, object>();
            fields.Add(settings.FieldRefNameForMailBody, body.ToString());
            UIHelper.OpenWorkItemForm(settings.WorkItemType, _mailItem.Subject, fields);
        }

        private bool IsImageFileExtension(string extension)
        {
            var imageExtensions = new List<string>() { ".JPEG", ".JPG", ".BMP", ".PNG", ".ICO", ".PIC" };
            return imageExtensions.Contains(extension.ToUpper());
        }

        private void SettingsButton_Click(object sender, RibbonControlEventArgs e)
        {
            UIHelper.OpenSettingsForm();
        }

        private void EnsureSettingsValid()
        {
            if (!UIHelper.CheckSettings())
            {
                UIHelper.OpenSettingsForm();
            }
        }
    }
}
