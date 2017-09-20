using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tester
{
    public partial class Form1 : Form
    {
        private static string token;

        private static WebClient client = new WebClient();

        public Form1()
        {
            InitializeComponent();
        }

        public static string MD5Encrypt(string value)
        {
            MD5 md5 = MD5.Create();
            byte[] result = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(value));
            StringBuilder builder = new StringBuilder(40);
            for (int i = 0; i < result.Length; i++)
            {
                builder.Append(result[i].ToString("x2"));//加密结果"x2"结果为32位,"x3"结果为48位,"x4"结果为64位

            }
            return builder.ToString();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            var key = client.DownloadString(String.Format("http://{0}/api/authentication",tbServer.Text.Trim()));

            var kv = new System.Collections.Specialized.NameValueCollection();
            kv.Add("ClientID", tbClientID.Text.Trim());
            kv.Add("SecureString", MD5Encrypt(String.Format("{0}{1}", tbClientSecret.Text.Trim(), key)));


            token = Encoding.UTF8.GetString( client.UploadValues(
                String.Format("http://{0}/api/authentication", tbServer.Text.Trim()),
                kv                ));


            btnLogin.Enabled = false;
            btnAdd.Enabled = true;

            //client.Headers.Add(String.Format("X-SWA-ClientID:{0}", tbClientID.Text));
            client.Headers.Add(String.Format("X-SWA-Proof:{0}", token));
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if ( ofd.ShowDialog()== DialogResult.OK)
            {
                var f = ofd.OpenFile();
                byte[] bytes = new byte[f.Length];
                f.Read(bytes, 0, (int)f.Length);

                f.Close();

                var kv = new System.Collections.Specialized.NameValueCollection();
                kv.Add("OwnerId", "14263");
                kv.Add("Caption",ofd.SafeFileName.GetCaption());
                kv.Add("Extension", ofd.SafeFileName.GetExtension());

                var fid = Encoding.UTF8.GetString(client.UploadValues(
                String.Format("http://{0}/api/documents/create", tbServer.Text.Trim()),
                kv));

                client.UploadData(String.Format("http://{0}/api/documents/{1}/contents", tbServer.Text.Trim(), fid), bytes);

                lbFile.Items.Add(fid);
            }
       }

        private void lbFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            var json = client.DownloadString(String.Format("http://{0}/api/documents/{1}/actions", tbServer.Text.Trim(), lbFile.SelectedItem.ToString()));
            var actions = JsonConvert.DeserializeObject<string[]>(json);

            lbActions.Items.Clear();

            for (int i=0; i < actions.Length; i++)
            {
                lbActions.Items.Add(actions[i]);
            }
        }

        private void lbActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            var url = client.DownloadString(String.Format("http://{0}/api/documents/{1}/url?UserId=14263&UserDisplayName=zhixin1007&Action={2}", tbServer.Text.Trim(), lbFile.SelectedItem.ToString(),lbActions.SelectedItem.ToString()));

            System.Diagnostics.Process.Start(url);
        }
    }
}
