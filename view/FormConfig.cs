using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace puka.view
{
  public partial class FormConfig : Form
  {
		private readonly Ookii.Dialogs.WinForms.VistaFolderBrowserDialog browser ;

    public FormConfig()
    {
     	browser = new Ookii.Dialogs.WinForms.VistaFolderBrowserDialog();
      InitializeComponent();
    }

    public async void OnClickOpenFolder(object? sender, EventArgs e)
    {
      await Task.Run(() => {
     		Program.Logger.Debug("que esta pasando");
				if(browser.ShowDialog() == DialogResult.OK){
				}
      });
    }

  }
}
