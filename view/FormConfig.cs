using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        private void OnClose(object sender, FormClosingEventArgs e)
        {
            //Dispose();
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
    }
}
