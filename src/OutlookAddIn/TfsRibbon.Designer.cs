namespace CP.TfsAssistant.OutlookAddIn
{
    partial class TfsRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public TfsRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TfsRibbon));
            this.tab1 = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.NewItemButton = this.Factory.CreateRibbonButton();
            this.NewItemFromMailButton = this.Factory.CreateRibbonButton();
            this.group2 = this.Factory.CreateRibbonGroup();
            this.SettingsButton = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.group1.SuspendLayout();
            this.group2.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.group1);
            this.tab1.Groups.Add(this.group2);
            this.tab1.Label = "ECS";
            this.tab1.Name = "tab1";
            // 
            // group1
            // 
            this.group1.Items.Add(this.NewItemButton);
            this.group1.Items.Add(this.NewItemFromMailButton);
            this.group1.Label = "Action";
            this.group1.Name = "group1";
            // 
            // NewItemButton
            // 
            this.NewItemButton.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.NewItemButton.Image = ((System.Drawing.Image)(resources.GetObject("NewItemButton.Image")));
            this.NewItemButton.Label = "New Work Item";
            this.NewItemButton.Name = "NewItemButton";
            this.NewItemButton.ShowImage = true;
            this.NewItemButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.NewItemButton_Click);
            // 
            // NewItemFromMailButton
            // 
            this.NewItemFromMailButton.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.NewItemFromMailButton.Image = ((System.Drawing.Image)(resources.GetObject("NewItemFromMailButton.Image")));
            this.NewItemFromMailButton.Label = "New Work Item From Mail";
            this.NewItemFromMailButton.Name = "NewItemFromMailButton";
            this.NewItemFromMailButton.ShowImage = true;
            this.NewItemFromMailButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.NewItemFromMailButton_Click);
            // 
            // group2
            // 
            this.group2.Items.Add(this.SettingsButton);
            this.group2.Label = "Configuration";
            this.group2.Name = "group2";
            // 
            // SettingsButton
            // 
            this.SettingsButton.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.SettingsButton.Image = ((System.Drawing.Image)(resources.GetObject("SettingsButton.Image")));
            this.SettingsButton.Label = "Settings";
            this.SettingsButton.Name = "SettingsButton";
            this.SettingsButton.ShowImage = true;
            this.SettingsButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.SettingsButton_Click);
            // 
            // TfsRibbon
            // 
            this.Name = "TfsRibbon";
            this.RibbonType = "Microsoft.Outlook.Explorer, Microsoft.Outlook.Mail.Read";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.TfsRibbon_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.group2.ResumeLayout(false);
            this.group2.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton NewItemButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton NewItemFromMailButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group2;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton SettingsButton;
    }

    partial class ThisRibbonCollection
    {
        internal TfsRibbon TfsRibbon
        {
            get { return this.GetRibbon<TfsRibbon>(); }
        }
    }
}
