namespace puka.view
{
  partial class PukaForm
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
      containerLayout = new TableLayoutPanel();
      headerLayout = new TableLayoutPanel();
      pictureLogo = new PictureBox();
      btnSelectLogo = new Button();
      lblRuc = new Label();
      textRuc = new MaskedTextBox();
      lblSuffix = new Label();
      textSuffix = new NumericUpDown();
      centerLayout = new TableLayoutPanel();
      checkBoxActiveServer = new CheckBox();
      lblUrlBifrost = new Label();
      textUrlBifrost = new TextBox();
      lblNamespace = new Label();
      textNamespace = new TextBox();
      endLayout = new TableLayoutPanel();
      btnCancel = new Button();
      bntAccept = new Button();
      containerLayout.SuspendLayout();
      headerLayout.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)pictureLogo).BeginInit();
      ((System.ComponentModel.ISupportInitialize)textSuffix).BeginInit();
      centerLayout.SuspendLayout();
      endLayout.SuspendLayout();
      SuspendLayout();
      // 
      // containerLayout
      // 
      containerLayout.AutoSize = true;
      containerLayout.ColumnCount = 1;
      containerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
      containerLayout.Controls.Add(headerLayout, 0, 0);
      containerLayout.Controls.Add(centerLayout, 0, 1);
      containerLayout.Controls.Add(endLayout, 0, 2);
      containerLayout.Dock = DockStyle.Fill;
      containerLayout.Location = new Point(0, 0);
      containerLayout.Name = "containerLayout";
      containerLayout.RowCount = 3;
      containerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
      containerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
      containerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
      containerLayout.Size = new Size(604, 653);
      containerLayout.TabIndex = 0;
      // 
      // headerLayout
      // 
      headerLayout.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      headerLayout.BackColor = Color.White;
      headerLayout.ColumnCount = 8;
      headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
      headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
      headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
      headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
      headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
      headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
      headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
      headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
      headerLayout.Controls.Add(pictureLogo, 2, 0);
      headerLayout.Controls.Add(btnSelectLogo, 2, 1);
      headerLayout.Controls.Add(lblRuc, 0, 2);
      headerLayout.Controls.Add(textRuc, 1, 2);
      headerLayout.Controls.Add(lblSuffix, 4, 2);
      headerLayout.Controls.Add(textSuffix, 7, 2);
      headerLayout.Location = new Point(3, 3);
      headerLayout.Name = "headerLayout";
      headerLayout.RowCount = 3;
      headerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
      headerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
      headerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
      headerLayout.Size = new Size(598, 320);
      headerLayout.TabIndex = 0;
      // 
      // pictureLogo
      // 
      pictureLogo.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      headerLayout.SetColumnSpan(pictureLogo, 4);
      pictureLogo.Location = new Point(151, 3);
      pictureLogo.Name = "pictureLogo";
      pictureLogo.Size = new Size(290, 186);
      pictureLogo.TabIndex = 0;
      pictureLogo.TabStop = false;
      // 
      // btnSelectLogo
      // 
      btnSelectLogo.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      btnSelectLogo.BackColor = Color.WhiteSmoke;
      headerLayout.SetColumnSpan(btnSelectLogo, 4);
      btnSelectLogo.Font = new Font("Comic Sans MS", 12F, FontStyle.Bold, GraphicsUnit.Point);
      btnSelectLogo.ForeColor = Color.Black;
      btnSelectLogo.Location = new Point(151, 195);
      btnSelectLogo.Name = "btnSelectLogo";
      btnSelectLogo.Size = new Size(290, 58);
      btnSelectLogo.TabIndex = 1;
      btnSelectLogo.Text = "SELECCIONAR LOGO";
      btnSelectLogo.UseVisualStyleBackColor = false;
      btnSelectLogo.Click += OnSelectingLogo;
      // 
      // lblRuc
      // 
      lblRuc.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      lblRuc.AutoSize = true;
      lblRuc.Font = new Font("Comic Sans MS", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
      lblRuc.Location = new Point(3, 275);
      lblRuc.Name = "lblRuc";
      lblRuc.Size = new Size(68, 25);
      lblRuc.TabIndex = 2;
      lblRuc.Text = "RUC:";
      lblRuc.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // textRuc
      // 
      textRuc.Anchor = AnchorStyles.None;
      headerLayout.SetColumnSpan(textRuc, 3);
      textRuc.Font = new Font("Comic Sans MS", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
      textRuc.Location = new Point(96, 272);
      textRuc.Mask = "00-00000000-0";
      textRuc.Name = "textRuc";
      textRuc.Size = new Size(177, 31);
      textRuc.TabIndex = 3;
      textRuc.TextAlign = HorizontalAlignment.Center;
      // 
      // lblSuffix
      // 
      lblSuffix.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      lblSuffix.AutoSize = true;
      headerLayout.SetColumnSpan(lblSuffix, 3);
      lblSuffix.Font = new Font("Comic Sans MS", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
      lblSuffix.Location = new Point(299, 275);
      lblSuffix.Name = "lblSuffix";
      lblSuffix.Size = new Size(216, 25);
      lblSuffix.TabIndex = 4;
      lblSuffix.Text = "Sufijo sucursal:";
      lblSuffix.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // textSuffix
      // 
      textSuffix.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      textSuffix.Font = new Font("Comic Sans MS", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
      textSuffix.Location = new Point(521, 272);
      textSuffix.Name = "textSuffix";
      textSuffix.Size = new Size(74, 31);
      textSuffix.TabIndex = 5;
      textSuffix.Value = new decimal(new int[] { 1, 0, 0, 0 });
      // 
      // centerLayout
      // 
      centerLayout.ColumnCount = 3;
      centerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
      centerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
      centerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
      centerLayout.Controls.Add(checkBoxActiveServer, 0, 0);
      centerLayout.Controls.Add(lblUrlBifrost, 0, 1);
      centerLayout.Controls.Add(textUrlBifrost, 1, 1);
      centerLayout.Controls.Add(lblNamespace, 0, 2);
      centerLayout.Controls.Add(textNamespace, 1, 2);
      centerLayout.Dock = DockStyle.Fill;
      centerLayout.Location = new Point(3, 329);
      centerLayout.Name = "centerLayout";
      centerLayout.RowCount = 4;
      centerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
      centerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
      centerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
      centerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
      centerLayout.Size = new Size(598, 189);
      centerLayout.TabIndex = 1;
      // 
      // checkBoxActiveServer
      // 
      checkBoxActiveServer.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      checkBoxActiveServer.AutoSize = true;
      centerLayout.SetColumnSpan(checkBoxActiveServer, 2);
      checkBoxActiveServer.FlatAppearance.CheckedBackColor = Color.Gray;
      checkBoxActiveServer.Font = new Font("Comic Sans MS", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
      checkBoxActiveServer.Location = new Point(3, 16);
      checkBoxActiveServer.Name = "checkBoxActiveServer";
      checkBoxActiveServer.Size = new Size(352, 28);
      checkBoxActiveServer.TabIndex = 0;
      checkBoxActiveServer.Text = "Configuración servidor";
      checkBoxActiveServer.UseVisualStyleBackColor = true;
      checkBoxActiveServer.CheckedChanged += OnCheckedConfigServerChanged;
      // 
      // lblUrlBifrost
      // 
      lblUrlBifrost.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      lblUrlBifrost.AutoSize = true;
      lblUrlBifrost.Font = new Font("Comic Sans MS", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
      lblUrlBifrost.Location = new Point(3, 58);
      lblUrlBifrost.Name = "lblUrlBifrost";
      lblUrlBifrost.Size = new Size(113, 25);
      lblUrlBifrost.TabIndex = 1;
      lblUrlBifrost.Text = "Url bifrost:";
      // 
      // textUrlBifrost
      // 
      textUrlBifrost.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      textUrlBifrost.BackColor = SystemColors.Window;
      centerLayout.SetColumnSpan(textUrlBifrost, 2);
      textUrlBifrost.Enabled = false;
      textUrlBifrost.Font = new Font("Comic Sans MS", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
      textUrlBifrost.Location = new Point(122, 55);
      textUrlBifrost.Name = "textUrlBifrost";
      textUrlBifrost.ReadOnly = true;
      textUrlBifrost.Size = new Size(473, 31);
      textUrlBifrost.TabIndex = 2;
      textUrlBifrost.Text = "https://bifrost-io.puyu.pe";
      textUrlBifrost.TextAlign = HorizontalAlignment.Center;
      // 
      // lblNamespace
      // 
      lblNamespace.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      lblNamespace.AutoSize = true;
      lblNamespace.Font = new Font("Comic Sans MS", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
      lblNamespace.Location = new Point(3, 105);
      lblNamespace.Name = "lblNamespace";
      lblNamespace.Size = new Size(113, 25);
      lblNamespace.TabIndex = 3;
      lblNamespace.Text = "Namespace:";
      // 
      // textNamespace
      // 
      textNamespace.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      textNamespace.BackColor = SystemColors.Window;
      centerLayout.SetColumnSpan(textNamespace, 2);
      textNamespace.Enabled = false;
      textNamespace.Font = new Font("Comic Sans MS", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
      textNamespace.Location = new Point(122, 102);
      textNamespace.Name = "textNamespace";
      textNamespace.ReadOnly = true;
      textNamespace.Size = new Size(473, 31);
      textNamespace.TabIndex = 4;
      textNamespace.Text = "yures:printer";
      textNamespace.TextAlign = HorizontalAlignment.Center;
      // 
      // endLayout
      // 
      endLayout.ColumnCount = 2;
      endLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
      endLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
      endLayout.Controls.Add(btnCancel, 0, 0);
      endLayout.Controls.Add(bntAccept, 1, 0);
      endLayout.Dock = DockStyle.Fill;
      endLayout.Location = new Point(3, 524);
      endLayout.Name = "endLayout";
      endLayout.RowCount = 1;
      endLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
      endLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
      endLayout.Size = new Size(598, 126);
      endLayout.TabIndex = 2;
      // 
      // btnCancel
      // 
      btnCancel.BackColor = Color.WhiteSmoke;
      btnCancel.DialogResult = DialogResult.Cancel;
      btnCancel.Dock = DockStyle.Fill;
      btnCancel.Font = new Font("Comic Sans MS", 12F, FontStyle.Bold, GraphicsUnit.Point);
      btnCancel.ForeColor = SystemColors.Desktop;
      btnCancel.Location = new Point(40, 40);
      btnCancel.Margin = new Padding(40);
      btnCancel.Name = "btnCancel";
      btnCancel.Size = new Size(219, 46);
      btnCancel.TabIndex = 0;
      btnCancel.Text = "CANCELAR";
      btnCancel.UseVisualStyleBackColor = false;
      btnCancel.Click += OnCancel;
      // 
      // bntAccept
      // 
      bntAccept.BackColor = SystemColors.GradientActiveCaption;
      bntAccept.DialogResult = DialogResult.OK;
      bntAccept.Dock = DockStyle.Fill;
      bntAccept.Font = new Font("Comic Sans MS", 12F, FontStyle.Bold, GraphicsUnit.Point);
      bntAccept.ForeColor = SystemColors.Desktop;
      bntAccept.Location = new Point(339, 40);
      bntAccept.Margin = new Padding(40);
      bntAccept.Name = "bntAccept";
      bntAccept.Size = new Size(219, 46);
      bntAccept.TabIndex = 1;
      bntAccept.Text = "ACEPTAR";
      bntAccept.UseVisualStyleBackColor = false;
      bntAccept.Click += OnAccept;
      // 
      // PukaForm
      // 
      AutoScaleDimensions = new SizeF(8F, 20F);
      AutoScaleMode = AutoScaleMode.Font;
      AutoSize = true;
      BackColor = Color.White;
      ClientSize = new Size(604, 653);
      Controls.Add(containerLayout);
      DoubleBuffered = true;
      Name = "PukaForm";
      Text = "Configuración PUKA-YURES";
      TopMost = true;
      containerLayout.ResumeLayout(false);
      headerLayout.ResumeLayout(false);
      headerLayout.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)pictureLogo).EndInit();
      ((System.ComponentModel.ISupportInitialize)textSuffix).EndInit();
      centerLayout.ResumeLayout(false);
      centerLayout.PerformLayout();
      endLayout.ResumeLayout(false);
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private TableLayoutPanel containerLayout;
    private TableLayoutPanel headerLayout;
    private PictureBox pictureLogo;
    private Button btnSelectLogo;
    private Label lblRuc;
    private MaskedTextBox textRuc;
    private Label lblSuffix;
    private NumericUpDown textSuffix;
    private TableLayoutPanel centerLayout;
    private CheckBox checkBoxActiveServer;
    private Label lblUrlBifrost;
    private TextBox textUrlBifrost;
    private Label lblNamespace;
    private TextBox textNamespace;
    private TableLayoutPanel endLayout;
    private Button btnCancel;
    private Button bntAccept;
  }
}