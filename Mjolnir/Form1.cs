using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Mjolnir
{

    public partial class Form1 : Form
    {
        const string xsdPath = "https://vortx.dev/assets/files/sipment/Layout%2001%20-%20Remessa.xsd";
        //const string xsdPath = "C:\\Layout 04 - RemessaVRS.xsd";
        private OpenFileDialog ofd = new OpenFileDialog();

        public StringBuilder validaXML(XDocument doc)
        {
            XmlSchemaSet schema = new XmlSchemaSet();
            schema.Add("", xsdPath);

            StringBuilder mensagemErro = new StringBuilder();

            doc.Validate(schema, (o, e) =>
            {
                mensagemErro.Append(e.Message);
                mensagemErro.Append(Environment.NewLine);
            });

            return mensagemErro;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = ofd.SafeFileName;
                textBox2.Text = "Validando...";

                XDocument xml = XDocument.Load(ofd.FileName);
                StringBuilder erros = validaXML(xml);


                if (!string.IsNullOrEmpty(erros.ToString()))
                {
                    textBox2.Text = erros.ToString();
                    return;
                }

                textBox2.Text = "Ok";
                return;
            }
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}