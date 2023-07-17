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

                if (browser.ShowDialog() == DialogResult.OK){
                MessageBox.Show("Que está pasando");
            }
        });
    }

  }
}
