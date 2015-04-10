namespace CP.TfsAssistant.Libraires
{
    using Microsoft.TeamFoundation.Client;
    using Microsoft.TeamFoundation.WorkItemTracking.Client;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;

    public class SettingsViewModel : ViewModelBase
    {
        private TfsSettings _settings;

        public SettingsViewModel()
        {
            this.CancelCommand = new DelegateCommand(ExecuteCancelCommand);
            this.SaveAndCloseCommand = new DelegateCommand(ExecuteSaveAndCloseCommand);
            this.ChooseProjectCommand = new DelegateCommand(ExecuteChooseProjectCommand);

            this._settings = MySettingsManager.GetSettings<TfsSettings>();
            this.ProjectCollectionUri = this._settings.ProjectCollectionUri;
            this.ProjectName = this._settings.ProjectName;
            this.WorkItemType = this._settings.WorkItemType;
            this.FieldRefNameForMailBody = this._settings.FieldRefNameForMailBody;
            this.Area = this._settings.Area;
            this.Iteration = this._settings.Iteration;
            this.RememberPaths = this._settings.RememberPaths;
        }

        private string _projectCollectionUri;
        public string ProjectCollectionUri
        {
            get { return _projectCollectionUri; }
            set
            {
                if (!object.Equals(_projectCollectionUri, value))
                {
                    _projectCollectionUri = value;
                    this.OnPropertyChanged(() => this.ProjectCollectionUri);
                }
            }
        }

        private string _projectName;
        public string ProjectName
        {
            get { return _projectName; }
            set
            {
                if (!object.Equals(_projectName, value))
                {
                    _projectName = value;
                    this.OnPropertyChanged(() => this.ProjectName);
                }
            }
        }

        private string _workItemType;
        public string WorkItemType
        {
            get { return _workItemType; }
            set
            {
                if (!object.Equals(_workItemType, value))
                {
                    _workItemType = value;
                    this.OnPropertyChanged(() => this.WorkItemType);
                }
            }
        }

        private string _fieldRefNameForMailBody;
        public string FieldRefNameForMailBody
        {
            get { return _fieldRefNameForMailBody; }
            set
            {
                if (!object.Equals(_fieldRefNameForMailBody, value))
                {
                    _fieldRefNameForMailBody = value;
                    this.OnPropertyChanged(() => this.FieldRefNameForMailBody);
                }
            }
        }

        private string _area;
        public string Area
        {
            get { return _area; }
            set
            {
                if (!object.Equals(_area, value))
                {
                    _area = value;
                    this.OnPropertyChanged(() => this.Area);
                }
            }
        }

        private string _iteration;
        public string Iteration
        {
            get { return _iteration; }
            set
            {
                if (!object.Equals(_iteration, value))
                {
                    _iteration = value;
                    this.OnPropertyChanged(() => this.Iteration);
                }
            }
        }

        private bool _rememberPaths;
        public bool RememberPaths
        {
            get { return _rememberPaths; }
            set
            {
                if (!object.Equals(_rememberPaths, value))
                {
                    _rememberPaths = value;
                    this.OnPropertyChanged(() => this.RememberPaths);
                }
            }
        }

        private ObservableCollection<string> _areas;
        public ObservableCollection<string> Areas
        {
            get { return _areas; }
            set
            {
                if (!object.Equals(_areas, value))
                {
                    _areas = value;
                    this.OnPropertyChanged(() => this.Areas);
                }
            }
        }

        private ObservableCollection<string> _iterations;
        public ObservableCollection<string> Iterations
        {
            get { return _iterations; }
            set
            {
                if (!object.Equals(_iterations, value))
                {
                    _iterations = value;
                    this.OnPropertyChanged(() => this.Iterations);
                }
            }
        }

        private ObservableCollection<string> _workItemTypes;
        public ObservableCollection<string> WorkItemTypes
        {
            get { return _workItemTypes; }
            set
            {
                if (!object.Equals(_workItemTypes, value))
                {
                    _workItemTypes = value;
                    this.OnPropertyChanged(() => this.WorkItemTypes);
                }
            }
        }

        public ICommand CancelCommand { get; private set; }
        public ICommand SaveAndCloseCommand { get; private set; }
        public ICommand ChooseProjectCommand { get; set; }

        public void ExecuteCancelCommand(object parameter)
        {
            var window = parameter as Window;
            window.DialogResult = false;
            window.Close();
        }

        public void ExecuteSaveAndCloseCommand(object parameter)
        {
            _settings.ProjectCollectionUri = this.ProjectCollectionUri;
            _settings.ProjectName = this.ProjectName;
            _settings.WorkItemType = this.WorkItemType;
            _settings.FieldRefNameForMailBody = this.FieldRefNameForMailBody;
            _settings.Area = this.Area;
            _settings.Iteration = this.Iteration;
            _settings.RememberPaths = this.RememberPaths;
            MySettingsManager.SaveSingle(_settings);

            var window = parameter as Window;
            window.DialogResult = true;
            window.Close();
        }

        public void ExecuteChooseProjectCommand(object parameter)
        {
            var form = new TeamProjectPicker(TeamProjectPickerMode.SingleProject, false);
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var connection = form.SelectedTeamProjectCollection;
                this.ProjectCollectionUri = connection.Uri.ToString();
                this.ProjectName = form.SelectedProjects[0].Name;
                var store = connection.GetService<WorkItemStore>();
                var project = store.Projects.Cast<Project>().First(p => p.Name == form.SelectedProjects[0].Name);
                this.Areas = new ObservableCollection<string>(TfsHelper.GetAreaPaths(project));
                this.Iterations = new ObservableCollection<string>(TfsHelper.GetIterationPaths(project));
                this.WorkItemTypes = new ObservableCollection<string>(TfsHelper.GetWorkItemTypes(project));
            }
        }
    }
}