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
  public partial class PukaForm : Form
  {
		private string logoImagePathSnapshot;
    public PukaForm()
    {
			logoImagePathSnapshot = "";
      InitializeComponent();
    }

    private void OnSelectingLogo(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog = new()
      {
        Filter = "Archivo de imagen (*.png)|*.png",
      };

      if (openFileDialog.ShowDialog() == DialogResult.OK)
      {
        string userFolderPath = UserConfig.getPukaFolderPath();
        string logoFileSourcePath = openFileDialog.FileName;
        string logoFileDestinyPath = Path.Combine(userFolderPath, "logo_empresa" + Path.GetExtension(logoFileSourcePath));
				logoImagePathSnapshot = logoFileDestinyPath;

        if (File.Exists(logoFileDestinyPath))
        {
          File.Delete(logoFileDestinyPath);
        }
        File.Copy(logoFileSourcePath, logoFileDestinyPath);
        using (var selectedImage = Image.FromFile(logoFileSourcePath))
        {
          pictureLogo.SizeMode = PictureBoxSizeMode.Zoom;
          pictureLogo.Image = new Bitmap(selectedImage, pictureLogo.Width, pictureLogo.Height);
        }
      }

    }

    private void OnCancel(object sender, EventArgs e)
    {
      var dialogResult = MessageBox.Show($"¿Seguro que desea cancelar?, Los cambios no se guardaran", "Advertencia", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
      if (dialogResult == DialogResult.OK)
      {
        Dispose();
        Application.Exit();
      }
    }

    private  void OnAccept(object sender, EventArgs e)
    {
			UserConfig.Add("ruc",textRuc.Text.Replace("-","").Trim());
			UserConfig.Add("suffix",textSuffix.Text.Trim());
			UserConfig.Add("url-bifrost",textUrlBifrost.Text.Trim());
			UserConfig.Add("namespace",textNamespace.Text.Trim());
			UserConfig.Add("logo-path",logoImagePathSnapshot);
		}
    

    private void OnCheckedConfigServerChanged(object sender, EventArgs e)
    {
      textUrlBifrost.Enabled = checkBoxActiveServer.Checked;
      textUrlBifrost.ReadOnly = !checkBoxActiveServer.Checked;
      textNamespace.Enabled = checkBoxActiveServer.Checked;
      textNamespace.ReadOnly = !checkBoxActiveServer.Checked;
    }
  }
}
