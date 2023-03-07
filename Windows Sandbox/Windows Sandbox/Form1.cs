using Sandbox_Tool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Windows_Sandbox
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // File Path Text Box active/selected on launch
            this.ActiveControl = textBox1;

            LogThis("Choose Application...");
        }

        public void LogThis(string logString)
        {
            // Logging to the Log Text Box
            textBox3.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] " + logString + "\n");
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Sandboxer class
            Sandboxer appSandbox = new Sandboxer();

            LogThis("Executing " + Path.GetFileName(textBox1.Text));
            try
            {
                // Initialise & Application Execution
                //appSandbox.ApplicationInitialise(textBox1.Text, textBox2.Text, pSet(string.Empty));
            }
            catch (SecurityException ex)
            {
                LogThis("ERROR : " + ex.Action.ToString());
                    Console.WriteLine("--- {0} ERROR ---\n", Path.GetFileNameWithoutExtension(textBox1.Text));
                   if (ex.Action.ToString() == "Demand")
                    {
                        int cutPoint = ex.Message.ToString().IndexOf(",");
                        LogThis("DEMAND : " + ex.Message.ToString().Substring(0, cutPoint) + "'");
                   }
                }


                LogThis("Terminated " + Path.GetFileName(textBox1.Text));
            LogThis("Ready...");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // File Dialog -> Browse
            DialogResult appDialogResult = openFileDialog.ShowDialog();

            if (DialogResult.OK == appDialogResult)
            {
                textBox1.Text = openFileDialog.FileName;
                    button3.Enabled = true;
                    LogThis(Path.GetFileName(textBox1.Text) + " selected.");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // Log Text Box visibility toggle
            if (checkBox1.CheckState == CheckState.Checked)
            {
                checkBox1.Text = "Hide Log";
                textBox3.Visible = true;
            }
            else
            {
                checkBox1.Text = "Show Log";
                textBox3.Visible = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Clear the Log Text Box contents
            textBox3.ResetText();
        }

 
        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void toolTip_MouseEnter(object sender, EventArgs e)
        {
            // Tool Tip/Info Box displayd based on the 'Tag' contents of the Check Box mouse hover
            label3.Text = "Info: \n" + ((System.Windows.Forms.CheckBox)sender).Tag.ToString();
        }

        private void toolTip_MouseLeave(object sender, EventArgs e)
        {
            // Tool Tip/Info Box blank display when no Check Box mouse hover
            label3.Text = "Info: \n" + "...";
        }

            private PermissionSet pSet(string args)
        {
            // Create a Permission Set -> (UN)Restricted depending on user choice
            PermissionSet permSet = checkBox2.CheckState == CheckState.Checked || args.Contains("-un")
                ? new PermissionSet(PermissionState.Unrestricted) : new PermissionSet(PermissionState.None);

            // Default Permissions required by assembly
            permSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
            permSet.AddPermission(new FileIOPermission(FileIOPermissionAccess.Read, textBox1.Text));
            permSet.AddPermission(new FileIOPermission(FileIOPermissionAccess.PathDiscovery, textBox1.Text));

            // Chosen Permissions
            permSet.AddPermission(checkBox8.CheckState == CheckState.Checked || args.Contains("-io")
                ? new FileIOPermission(PermissionState.Unrestricted) : new FileIOPermission(PermissionState.None));

            permSet.AddPermission(checkBox7.CheckState == CheckState.Checked || args.Contains("-ui")
                ? new UIPermission(PermissionState.Unrestricted) : new UIPermission(PermissionState.None));

            permSet.AddPermission(checkBox6.CheckState == CheckState.Checked || args.Contains("-fd")
                ? new FileDialogPermission(PermissionState.Unrestricted) : new FileDialogPermission(PermissionState.None));

            permSet.AddPermission(checkBox5.CheckState == CheckState.Checked || args.Contains("-sec")
                ? new SecurityPermission(PermissionState.Unrestricted) : new SecurityPermission(PermissionState.None));

            permSet.AddPermission(checkBox4.CheckState == CheckState.Checked || args.Contains("-reg")
                ? new RegistryPermission(PermissionState.Unrestricted) : new RegistryPermission(PermissionState.None));

            permSet.AddPermission(checkBox3.CheckState == CheckState.Checked || args.Contains("-web")
                ? new WebPermission(PermissionState.Unrestricted) : new WebPermission(PermissionState.None));

            return permSet;
        }

        public void interfaceMan(string[] args)
        {
            // Help/Permissions list display after the appripriate -h argument entered
            if (args.Contains("-h") || args.Contains("-H"))
            {
                Console.WriteLine(@"Parameters: ""<application path>"" ""<application parameters>"" ""<permissions>""");
                Console.WriteLine("-un = Unrestricted permissions");
                Console.WriteLine("-io = IO permissions");
                Console.WriteLine("-ui = UI permissions");
                Console.WriteLine("-fd = File Dialog permissions");
                Console.WriteLine("-sec = Security permissions");
                Console.WriteLine("-is = Isolated Storage permissions");
                Console.WriteLine("-env = Environment permissions");
                Console.WriteLine("-kc = Key Container permissions");
                Console.WriteLine("-pr = Principal permissions");
                Console.WriteLine("-ref = Reflection permissions");
                Console.WriteLine("-reg = Registry permissions");
                Console.WriteLine("-st = Store permissions");
                Console.WriteLine("-ctd = Check Type Descriptor permissions");
                Console.WriteLine("-web = Web permissions");
            }
            else
            {
                Console.WriteLine("HINT - Use -h for Help & Permissions List");
            }

            // Execute Sandboxer functionality through command line when appropriate amount of arguments given
            if (args.Count() == 3)
            {
                Sandboxer appSandbox = new Sandboxer();
                try
                {
                    appSandbox.ApplicationInitialise(args[0].ToString(), args[1].ToString(), pSet(args[2].ToString()));
                }
                catch (SecurityException ex)
                {
                    Console.WriteLine("ERROR : " + ex.Action.ToString());
                    if (ex.Action.ToString() == "Demand")
                    {
                        int cutPoint = ex.Message.ToString().IndexOf(",");
                        Console.WriteLine("DEMAND : " + ex.Message.ToString().Substring(0, cutPoint) + "'");
                    }
                }
            }

        }

        private void label3_Click_1(object sender, EventArgs e)
        {

        }
    }
}
