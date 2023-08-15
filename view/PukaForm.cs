using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using puka.app;

namespace puka.view
{
	public partial class PukaForm : Form
	{
		private string logoImagePathSnapshot;
		public PukaForm()
		{
			logoImagePathSnapshot = "";
			InitializeComponent();
			Init();
		}

		public void Init()
		{
			textRuc.Text = BifrostConfig.GetRuc();
			textSuffix.Text = BifrostConfig.GetSuffix();
			if (BifrostConfig.GetUrl().Length > 0)
			{
				textUrlBifrost.Text = BifrostConfig.GetUrl();
			}
			if (BifrostConfig.GetNamespace().Length > 0)
			{
				textNamespace.Text = BifrostConfig.GetNamespace();
			}
			string? logoPath = UserConfig.Get("logo-path");
			if(!string.IsNullOrEmpty(logoPath)){
				setImagePicture(logoPath);
			}
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
				setImagePicture(logoFileSourcePath);
			}
		}

		private void setImagePicture(string pathImage)
		{
			try
			{
				using (var selectedImage = Image.FromFile(pathImage))
				{
					pictureLogo.SizeMode = PictureBoxSizeMode.Zoom;
					pictureLogo.Image = new Bitmap(selectedImage, pictureLogo.Width, pictureLogo.Height);
				}
			}
			catch (System.Exception e)
			{
				Program.Logger.Debug(e,"No se pudo guardar el logo: " + e.Message);
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

		private void OnAccept(object sender, EventArgs e)
		{
			List<string> errors = new List<string>();
			DialogResult = DialogResult.OK;
			if (!BifrostConfig.TrySetRuc(textRuc.Text.Replace("-", ""), out List<string> e_ruc))
			{
				errors.AddRange(e_ruc);
			}
			if (!BifrostConfig.TrySetSuffix(textSuffix.Text, out List<string> e_suffix))
			{
				errors.AddRange(e_suffix);
			}
			if (!BifrostConfig.TrySetUrl(textUrlBifrost.Text, out List<string> e_url))
			{
				errors.AddRange(e_url);
			}
			if (!BifrostConfig.TrySetNamespace(textNamespace.Text, out List<string> e_namespace))
			{
				errors.AddRange(e_namespace);
			}
			UserConfig.Set("logo-path", logoImagePathSnapshot);

			if (errors.Count > 0)
			{
				if (ShowWarningMessages(errors) == DialogResult.OK)
				{
					Application.Exit();
				}
				else
				{
					DialogResult = DialogResult.None;
				}
			}

		}

		private DialogResult ShowWarningMessages(List<string> errors)
		{
			string warnMessages = "";
			foreach (string warn in errors)
			{
				warnMessages += "* " + warn + "\n";
			}
			warnMessages += "-Puka no iniciara en segundo plano, ¿Desea continuar?";
			return MessageBox.Show(warnMessages, "Adverntencias configuración puka", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
		}

		private void OnCheckedConfigServerChanged(object sender, EventArgs e)
		{
			textUrlBifrost.Enabled = checkBoxActiveServer.Checked;
			textUrlBifrost.ReadOnly = !checkBoxActiveServer.Checked;
			textNamespace.Enabled = checkBoxActiveServer.Checked;
			textNamespace.ReadOnly = !checkBoxActiveServer.Checked;
		}

		private void OnFormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing)
			{
				Application.Exit();
			}
		}
	}
}
