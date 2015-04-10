using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Microsoft.TeamFoundation.WorkItemTracking.WpfControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CP.TfsAssistant.Libraires
{
    /// <summary>
    /// Interaction logic for WorkItemWindow.xaml
    /// </summary>
    public partial class WorkItemWindow : Window
    {
        private WorkItem _workItem;
        private WorkItemControl _workItemControl;
        private bool _changed;

        public WorkItemWindow(WorkItem workItem)
        {
            InitializeComponent();
            _workItem = workItem;
            _workItem.FieldChanged += WorkItem_FieldChanged;
            _workItemControl = new WorkItemControl();
            _workItemControl.Item = workItem;
            this.ContentControl.Content = _workItemControl;
            ChangeTitle();
        }

        private void ChangeTitle()
        {
            if (_workItem != null)
            {
                if (_workItem.Id <= 0)
                {
                    this.Title = string.Format("{0} New {1}", _workItem.Project.Name, _workItem.Type.Name);
                }
                else
                {
                    this.Title = string.Format("{0} {1} {2}", _workItem.Project.Name, _workItem.Type.Name, _workItem.Id);
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (UIHelper.SaveWorkItem(this._workItem))
            {
                ChangeTitle();
                _changed = false;
            }
        }

        private void SaveAndCloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (UIHelper.SaveWorkItem(this._workItem))
            {
                this.CloseMe();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (_changed)
            {
                var result = MessageBox.Show("There are unsaved changes for this work item. \r\nDo you want to save the changes before closing?",
                                                                    "Unsaved Changes", 
                                                                    MessageBoxButton.YesNoCancel, 
                                                                    MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes && UIHelper.SaveWorkItem(this._workItem))
                {
                    this.CloseMe();
                }
                else if (result == MessageBoxResult.No)
                {
                    this.CloseMe();
                }
            }
            else
            {
                this.CloseMe();
            }
        }

        private void WorkItem_FieldChanged(object sender, WorkItemEventArgs e)
        {
            _changed = true;
        }

        private void CloseMe()
        {
            _workItem.FieldChanged -= WorkItem_FieldChanged;
            this.Close();
        }
    }
}
