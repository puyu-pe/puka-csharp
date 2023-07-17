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


    public FormConfig()
    {
      InitializeComponent();
    }

    public string GetUrlBifrostServer()
    {
      string ruc = textRUC.Text.Trim();
      string suffixCompany = textSuffix.Text.Trim();
      string domainBifrost = textDomain.Text.Trim();
      string namespaceBifrost = textNamespace.Text.Trim();
      return String.Format("{0}/{1}-{2}-{3}", domainBifrost, namespaceBifrost, ruc, suffixCompany);
    }

  }
}
