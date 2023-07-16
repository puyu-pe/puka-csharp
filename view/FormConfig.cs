using Newtonsoft.Json;
using puka.util;
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
            string json = @"{
                ""type"": ""extra"",
                ""times"": 1,
                ""printer"": {
                    ""type"": ""linux-usb"",
                    ""name_system"": ""Microsoft Print to PDF"",
                    ""port"": ""9100""
                },
                ""data"": {
                    ""business"": {
                        ""description"": ""Restaurant H. Pollos""
                    },
                    ""titleExtra"": {
                        ""title"": ""DELIVERY : D-1"",
                        ""subtitle"": ""26-08-2020 14:40:30""
                    },
                    ""additional"": [
                        ""FUENTE: INTERNET"",
                        ""CLIENTE: EMERSON ÑAHUINLLA VELASQUEZ"",
                        ""DIRECCIÓN: AV VILLA EL SOL MZ E LT O"",
                        ""CELULAR : 983780014"",
                        ""REFERENCIA : DESVIO DE TIERRA DESPUES DE MECANICA DE MOTOS"",
                        ""PAGARA : 100.00""
                    ],
                    ""items"": [
                        {
                            ""quantity"": 1,
                            ""description"": ""HAWAYANA (FAMILIAR)"",
                            ""commentary"" : ""con arto quesooo"",
                            ""totalPrice"" : 14.50 
                        },
                        {
                            ""quantity"": 1,
                            ""description"": ""HAWAYANA (PERSONAL)"",
                            ""totalPrice"" : 14.50
                        }
                    ]
                }
            }";

            await Task.Run(() => {

                //Program.Logger.Debug("que esta pasando");
                dynamic myData = JsonConvert.DeserializeObject<dynamic>(json);

                Printer printer = new Printer(myData);

                printer.PrintReceipt();

                if (browser.ShowDialog() == DialogResult.OK){
                MessageBox.Show("Que está pasando");
            }
        });
    }

  }
}
