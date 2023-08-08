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
      lblLogoImage = new Label();
      btnPathImage = new Button();
      textRUC = new TextBox();
      lblRUC = new Label();
      lblSuffix = new Label();
      textSuffix = new TextBox();
      groupServer = new GroupBox();
      textNamespace = new TextBox();
      lblNamespace = new Label();
      textDomain = new TextBox();
      lblDomain = new Label();
      btnCancel = new Button();
      lblTitle = new Label();
      btnAccept = new Button();
      primaryContainer.SuspendLayout();
      groupCompany.SuspendLayout();
      groupServer.SuspendLayout();
      SuspendLayout();
      // 
      // primaryContainer
      // 
      resources.ApplyResources(primaryContainer, "primaryContainer");
      primaryContainer.Controls.Add(groupCompany);
      primaryContainer.Controls.Add(groupServer);
      primaryContainer.Name = "primaryContainer";
      // 
      // groupCompany
      // 
      groupCompany.Controls.Add(lblLogoImage);
      groupCompany.Controls.Add(btnPathImage);
      groupCompany.Controls.Add(textRUC);
      groupCompany.Controls.Add(lblRUC);
      groupCompany.Controls.Add(lblSuffix);
      groupCompany.Controls.Add(textSuffix);
      resources.ApplyResources(groupCompany, "groupCompany");
      groupCompany.Name = "groupCompany";
      groupCompany.TabStop = false;
      groupCompany.UseCompatibleTextRendering = true;
      // 
      // lblLogoImage
      // 
      resources.ApplyResources(lblLogoImage, "lblLogoImage");
      lblLogoImage.Name = "lblLogoImage";
      // 
      // btnPathImage
      // 
      resources.ApplyResources(btnPathImage, "btnPathImage");
      btnPathImage.Name = "btnPathImage";
      btnPathImage.UseVisualStyleBackColor = true;
      btnPathImage.Click += btnPathImage_Click;
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
      groupServer.Controls.Add(textNamespace);
      groupServer.Controls.Add(lblNamespace);
      groupServer.Controls.Add(textDomain);
      groupServer.Controls.Add(lblDomain);
      resources.ApplyResources(groupServer, "groupServer");
      groupServer.Name = "groupServer";
      groupServer.TabStop = false;
      // 
      // textNamespace
      // 
      resources.ApplyResources(textNamespace, "textNamespace");
      textNamespace.Name = "textNamespace";
      // 
      // lblNamespace
      // 
      resources.ApplyResources(lblNamespace, "lblNamespace");
      lblNamespace.Name = "lblNamespace";
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
      // btnCancel
      // 
      btnCancel.BackColor = SystemColors.ButtonFace;
      btnCancel.DialogResult = DialogResult.Cancel;
      resources.ApplyResources(btnCancel, "btnCancel");
      btnCancel.Name = "btnCancel";
      btnCancel.UseVisualStyleBackColor = false;
      btnCancel.Click += OnCancel;
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
      FormClosed += OnClose;
      primaryContainer.ResumeLayout(false);
      groupCompany.ResumeLayout(false);
      groupCompany.PerformLayout();
      groupServer.ResumeLayout(false);
      groupServer.PerformLayout();
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
    private Button btnAccept;
    private Button btnCancel;
    private TextBox textNamespace;
    private Label lblNamespace;
    private Label lblLogoImage;
    private Button btnPathImage;
  }
}