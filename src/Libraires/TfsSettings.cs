namespace CP.TfsAssistant.Libraires
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;

    public class TfsSettings : ISettings
    {
        public string ProjectCollectionUri { get; set; }
        public string ProjectName { get; set; }
        public string WorkItemType { get; set; }

        [Description("The field reference name of the work item. The field will be filled in with mail body.")]
        public string FieldRefNameForMailBody { get; set; }

        public string Area { get; set; }
        public string Iteration { get; set; }
        public bool RememberPaths { get; set; }

        public void Initialize()
        {
            this.ProjectCollectionUri = "http://xxxxxxxxxxxx/tfs/";
            this.ProjectName = "xxx";
            this.WorkItemType = "Bug";
            this.FieldRefNameForMailBody = "Microsoft.VSTS.TCM.ReproSteps";
            this.RememberPaths = true;
        }
    }
}