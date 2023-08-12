using System.Text.RegularExpressions;

namespace puka.view
{
	public partial class FormConfig : Form
	{
		private string pathFileImage = "";

		public FormConfig()
		{
			InitializeComponent();
		}

		public string GetUrlBifrostServer()
		{
			if (!string.IsNullOrEmpty(pathFileImage))
			{
				string fileDestiny = Path.Combine(Directory.GetCurrentDirectory(), "img");

				createFolderImage();

				string nameFileDestiny = Path.Combine(fileDestiny, "logo" + Path.GetExtension(pathFileImage));

				try
				{
					if (File.Exists(nameFileDestiny))
					{
						File.Delete(nameFileDestiny);
					}

					File.Copy(pathFileImage, nameFileDestiny);
					MessageBox.Show("Imagen guardada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Error al guardar la imagen: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}

			string ruc = textRUC.Text.Trim();
			string suffixCompany = textSuffix.Text.Trim();
			string domainBifrost = textDomain.Text.Trim();
			string namespaceBifrost = textNamespace.Text.Trim();
			return String.Format("{0}/{1}-{2}-{3}", domainBifrost, namespaceBifrost, ruc, suffixCompany);
		}

		private void OnCancel(object sender, EventArgs e)
		{
			Dispose();
			Application.Exit();
		}

		private void btnPathImage_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Archivo de imagen (*.png)|*.png";

			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				pathFileImage = openFileDialog.FileName;
				MessageBox.Show(pathFileImage);
			}
		}

		private void createFolderImage()
		{
			string fileDestinyOriginal = Path.Combine(Directory.GetCurrentDirectory(), "img");

			// Crear la carpeta "img" si no existe.
			if (!Directory.Exists(fileDestinyOriginal))
			{
				Directory.CreateDirectory(fileDestinyOriginal);
			}
		}

		private void OnClose(object sender, FormClosedEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing)
			{
				Application.Exit();
			}
		}

		private void OnAccept(object sender, EventArgs e)
		{
			string warnMessage = "";
			DialogResult = DialogResult.OK;
			warnMessage += ValidatingSuffix();
			warnMessage += ValidatingRUC();
			warnMessage += ValidatingDomain();
			warnMessage += ValidatingNamespace();

			if (DialogResult != DialogResult.OK)
			{
				warnMessage += "\n-Si continua, la configuración no se guardara";
				var result = MessageBox.Show(warnMessage, "Advertencia", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
				if(result == DialogResult.OK){
					ClearInput();
					Application.Exit();
				}
			}
		}

		private void ClearInput(){
			textDomain.Clear();
			textNamespace.Clear();
			textRUC.Clear();
			textSuffix.Clear();
		}

		private string ValidatingSuffix()
		{
			string suffix = textSuffix.Text.Trim();
			if (suffix.Length == 0)
			{
				this.DialogResult = DialogResult.None;
				return "El campo sufijo es obligatorio\n";
			}
			bool isValidSuffix = Regex.IsMatch(suffix, @"\d{1}");
			if (!isValidSuffix)
			{
				this.DialogResult = DialogResult.None;
				return string.Format("* El sufijo ${0} solo debe tener un digito\n", suffix);
			}
			return "";
		}

		private string ValidatingRUC()
		{
			string ruc = textRUC.Text.Trim();
			if (ruc.Length == 0)
			{
				this.DialogResult = DialogResult.None;
				return "El campo RUC es obligatorio\n";
			}
			bool isValidRuc = Regex.IsMatch(ruc, @"\d{11}");
			if (!isValidRuc)
			{
				this.DialogResult = DialogResult.None;
				return string.Format("* RUC {0} incorrecto\n", ruc);
			}
			return "";
		}

		private string ValidatingDomain()
		{
			string domain = textDomain.Text.Trim();
			if (domain.Length == 0)
			{
				this.DialogResult = DialogResult.None;
				return "El campo Dominio es obligatorio\n";
			}
			bool isValidDomain = Regex.IsMatch(domain, @"https?://.+");
			if (!isValidDomain)
			{
				this.DialogResult = DialogResult.None;
				return string.Format("* Url del dominio {0} mal formado\n", domain);
			}
			return "";
		}

		private string ValidatingNamespace()
		{
			string namespaceStr = textNamespace.Text.Trim();
			if (namespaceStr.Length == 0)
			{
				this.DialogResult = DialogResult.None;
				return "El campo namespace es obligatorio\n";
			}
			bool isValidNamespace = Regex.IsMatch(namespaceStr, @"[a-zA-Z]+");
			if (!isValidNamespace)
			{
				this.DialogResult = DialogResult.None;
				return string.Format("* Namespace {0} no valido\n¨", namespaceStr);
			}
			return "";
		}
	}
}
