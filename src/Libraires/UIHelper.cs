namespace CP.TfsAssistant.Libraires
{
    using Microsoft.TeamFoundation.Client;
    using Microsoft.TeamFoundation.WorkItemTracking.Client;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;

    public class UIHelper
    {
        public static bool CheckSettings()
        {
            var settings = MySettingsManager.GetSettings<TfsSettings>();
            if (string.IsNullOrEmpty(settings.ProjectName) || string.IsNullOrEmpty(settings.WorkItemType))
            {
                return false;
            }

            var uri = new Uri(settings.ProjectCollectionUri);
            try
            {
                var project = TfsHelper.GetProject(settings.ProjectCollectionUri, settings.ProjectName);
                var workItemType = project.WorkItemTypes[settings.WorkItemType];
                if (workItemType == null)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed to connect to TFS", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var registered = RegisteredTfsConnections.GetProjectCollection(uri);
            if (registered == null)
            {
                RegisteredTfsConnections.RegisterProjectCollection(new TfsTeamProjectCollection(uri));
            }
            return true;
        }

        public static void OpenSettingsForm()
        {
            var form = new SettingsWindow(new SettingsViewModel());
            form.ShowDialog();
        }

        public static void OpenWorkItemForm()
        {
            var settings = MySettingsManager.GetSettings<TfsSettings>();
            OpenWorkItemForm(settings.WorkItemType);
        }

        /// <summary>
        /// Open a dialog form to create new work item.
        /// </summary>
        /// <param name="workItemTypeName">work item type name</param>
        /// <param name="title">work item title</param>
        /// <param name="fields">A dictionary represents the workitem fields. The key should be the ReferenceName of a field.</param>
        public static void OpenWorkItemForm(string workItemTypeName, string title = null, Dictionary<string, object> fields = null)
        {
            var settings = MySettingsManager.GetSettings<TfsSettings>();
            var uri = new Uri(settings.ProjectCollectionUri);
            var project = TfsHelper.GetProject(settings.ProjectCollectionUri, settings.ProjectName);
            var workItemType = project.WorkItemTypes[workItemTypeName];

            var workItem = new WorkItem(workItemType);
            workItem.IterationPath = settings.Iteration;
            workItem.AreaPath = settings.Area;
            workItem.Title = title;
            if (fields != null)
            {
                foreach (var field in fields)
                {
                    foreach (Field wField in workItem.Fields)
                    {
                        if (wField.ReferenceName == field.Key)
                        {
                            wField.Value = field.Value;
                            break;
                        }
                    }
                }
            }

            var form = new WorkItemWindow(workItem);
            form.ShowDialog();
        }

        public static void OpenWorkItemForm(int id)
        {
            var settings = MySettingsManager.GetSettings<TfsSettings>();
            var workItem = TfsHelper.GetWorkItem(settings.ProjectCollectionUri, id);
            var form = new WorkItemWindow(workItem);
            form.ShowDialog();
        }

        internal static bool SaveWorkItem(WorkItem workItem)
        {
            ArrayList fields = workItem.Validate();
            if (fields.Count > 0)
            {
                StringBuilder error = new StringBuilder();
                error.Append(string.Format("The following {0} fields is in invalid status:", fields.Count)).Append(Environment.NewLine);
                foreach (Field field in fields)
                {
                    error.Append(Environment.NewLine);
                    error.Append(string.Format("{0} - {1}", field.Name, field.Status));
                }
                MessageBox.Show(error.ToString(), "Validation Failded", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            workItem.Save();

            var settings = MySettingsManager.GetSettings<TfsSettings>();
            if (settings.RememberPaths)
            {
                settings.Area = workItem.AreaPath;
                settings.Iteration = workItem.IterationPath;
                MySettingsManager.SaveSingle(settings);
            }
            return true;
        }
    }
}