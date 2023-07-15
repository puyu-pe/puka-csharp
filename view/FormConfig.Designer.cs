namespace puka.view
{
  partial class FormConfig
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfig));
      primaryContainer = new FlowLayoutPanel();
      groupCompany = new GroupBox();
      textRUC = new TextBox();
      lblRUC = new Label();
      lblSuffix = new Label();
      textSuffix = new TextBox();
      groupServer = new GroupBox();
      textDomain = new TextBox();
      lblDomain = new Label();
      groupPUKA = new GroupBox();
      btnOpenFile = new Button();
      textLogs = new TextBox();
      lblLogsDirectory = new Label();
      btnCancel = new Button();
      lblTitle = new Label();
      btnAccept = new Button();
      folderBrowserDialog = new FolderBrowserDialog();
      primaryContainer.SuspendLayout();
      groupCompany.SuspendLayout();
      groupServer.SuspendLayout();
      groupPUKA.SuspendLayout();
      SuspendLayout();
      // 
      // primaryContainer
      // 
      resources.ApplyResources(primaryContainer, "primaryContainer");
      primaryContainer.Controls.Add(groupCompany);
      primaryContainer.Controls.Add(groupServer);
      primaryContainer.Controls.Add(groupPUKA);
      primaryContainer.Name = "primaryContainer";
      // 
      // groupCompany
      // 
      groupCompany.Controls.Add(textRUC);
      groupCompany.Controls.Add(lblRUC);
      groupCompany.Controls.Add(lblSuffix);
      groupCompany.Controls.Add(textSuffix);
      resources.ApplyResources(groupCompany, "groupCompany");
      groupCompany.Name = "groupCompany";
      groupCompany.TabStop = false;
      groupCompany.UseCompatibleTextRendering = true;
      // 
      // textRUC
      // 
      resources.ApplyResources(textRUC, "textRUC");
      textRUC.Name = "textRUC";
      // 
      // lblRUC
      // 
      resources.ApplyResources(lblRUC, "lblRUC");
      lblRUC.Name = "lblRUC";
      // 
      // lblSuffix
      // 
      resources.ApplyResources(lblSuffix, "lblSuffix");
      lblSuffix.Name = "lblSuffix";
      // 
      // textSuffix
      // 
      resources.ApplyResources(textSuffix, "textSuffix");
      textSuffix.Name = "textSuffix";
      // 
      // groupServer
      // 
      groupServer.Controls.Add(textDomain);
      groupServer.Controls.Add(lblDomain);
      resources.ApplyResources(groupServer, "groupServer");
      groupServer.Name = "groupServer";
      groupServer.TabStop = false;
      // 
      // textDomain
      // 
      resources.ApplyResources(textDomain, "textDomain");
      textDomain.Name = "textDomain";
      // 
      // lblDomain
      // 
      resources.ApplyResources(lblDomain, "lblDomain");
      lblDomain.Name = "lblDomain";
      // 
      // groupPUKA
      // 
      resources.ApplyResources(groupPUKA, "groupPUKA");
      groupPUKA.Controls.Add(btnOpenFile);
      groupPUKA.Controls.Add(textLogs);
      groupPUKA.Controls.Add(lblLogsDirectory);
      groupPUKA.Name = "groupPUKA";
      groupPUKA.TabStop = false;
      // 
      // btnOpenFile
      // 
      btnOpenFile.BackColor = SystemColors.ActiveCaption;
      resources.ApplyResources(btnOpenFile, "btnOpenFile");
      btnOpenFile.Name = "btnOpenFile";
      btnOpenFile.UseVisualStyleBackColor = false;
      btnOpenFile.Click += OnClickOpenFolder;
      // 
      // textLogs
      // 
      textLogs.BackColor = SystemColors.ControlLightLight;
      resources.ApplyResources(textLogs, "textLogs");
      textLogs.Name = "textLogs";
      // 
      // lblLogsDirectory
      // 
      resources.ApplyResources(lblLogsDirectory, "lblLogsDirectory");
      lblLogsDirectory.Name = "lblLogsDirectory";
      // 
      // btnCancel
      // 
      btnCancel.BackColor = SystemColors.ButtonFace;
      btnCancel.DialogResult = DialogResult.Cancel;
      resources.ApplyResources(btnCancel, "btnCancel");
      btnCancel.Name = "btnCancel";
      btnCancel.UseVisualStyleBackColor = false;
      // 
      // lblTitle
      // 
      resources.ApplyResources(lblTitle, "lblTitle");
      lblTitle.BorderStyle = BorderStyle.Fixed3D;
      lblTitle.Name = "lblTitle";
      // 
      // btnAccept
      // 
      btnAccept.BackColor = SystemColors.GradientActiveCaption;
      btnAccept.DialogResult = DialogResult.OK;
      resources.ApplyResources(btnAccept, "btnAccept");
      btnAccept.Name = "btnAccept";
      btnAccept.UseVisualStyleBackColor = false;
      // 
      // FormConfig
      // 
      resources.ApplyResources(this, "$this");
      AutoScaleMode = AutoScaleMode.Font;
      BackColor = SystemColors.HighlightText;
      Controls.Add(btnCancel);
      Controls.Add(primaryContainer);
      Controls.Add(lblTitle);
      Controls.Add(btnAccept);
      FormBorderStyle = FormBorderStyle.FixedSingle;
      Name = "FormConfig";
      primaryContainer.ResumeLayout(false);
      primaryContainer.PerformLayout();
      groupCompany.ResumeLayout(false);
      groupCompany.PerformLayout();
      groupServer.ResumeLayout(false);
      groupServer.PerformLayout();
      groupPUKA.ResumeLayout(false);
      groupPUKA.PerformLayout();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private FlowLayoutPanel primaryContainer;
    private TextBox textSuffix;
    private Label lblSuffix;
    private GroupBox groupCompany;
    private TextBox textRUC;
    private Label lblRUC;
    private GroupBox groupServer;
    private Label lblDomain;
    private TextBox textDomain;
    private Label lblTitle;
    private GroupBox groupPUKA;
    private Label lblLogsDirectory;
    private TextBox textLogs;
    private Button btnOpenFile;
    private Button btnAccept;
    private Button btnCancel;
    private FolderBrowserDialog folderBrowserDialog;
  }
}