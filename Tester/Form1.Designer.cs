namespace Tester
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.tbServer = new System.Windows.Forms.TextBox();
            this.tbClientID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbClientSecret = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lbFile = new System.Windows.Forms.ListBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.lbActions = new System.Windows.Forms.ListBox();
            this.btnTemplate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server:";
            // 
            // tbServer
            // 
            this.tbServer.Location = new System.Drawing.Point(79, 12);
            this.tbServer.Name = "tbServer";
            this.tbServer.Size = new System.Drawing.Size(200, 25);
            this.tbServer.TabIndex = 1;
            // 
            // tbClientID
            // 
            this.tbClientID.Location = new System.Drawing.Point(411, 12);
            this.tbClientID.Name = "tbClientID";
            this.tbClientID.Size = new System.Drawing.Size(184, 25);
            this.tbClientID.TabIndex = 3;
            this.tbClientID.Text = "332a8c170cde406baca517b6bd756b0a";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(326, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "ClientID:";
            // 
            // tbClientSecret
            // 
            this.tbClientSecret.Location = new System.Drawing.Point(755, 12);
            this.tbClientSecret.Name = "tbClientSecret";
            this.tbClientSecret.Size = new System.Drawing.Size(175, 25);
            this.tbClientSecret.TabIndex = 5;
            this.tbClientSecret.Text = "ad14e68faabb40f7b3fd69d1a0560ee0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(638, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "ClientSecret:";
            // 
            // lbFile
            // 
            this.lbFile.FormattingEnabled = true;
            this.lbFile.ItemHeight = 15;
            this.lbFile.Location = new System.Drawing.Point(13, 81);
            this.lbFile.Name = "lbFile";
            this.lbFile.Size = new System.Drawing.Size(217, 109);
            this.lbFile.TabIndex = 6;
            this.lbFile.DoubleClick += new System.EventHandler(this.lbFile_SelectedIndexChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(136, 43);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(94, 32);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Text = "添加文档";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(13, 43);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(94, 32);
            this.btnLogin.TabIndex = 8;
            this.btnLogin.Text = "登录";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.Button2_Click);
            // 
            // ofd
            // 
            this.ofd.Filter = "Word文档(*.doc,*.docx)|*.doc;*.docx|Excel文档(*.xls,*.xlsx)|*.xls;*.xlsx|PowerPoint文档" +
    "(*.ppt,*.pptx)|*.ppt;*.pptx|所有文件|*.*";
            // 
            // lbActions
            // 
            this.lbActions.FormattingEnabled = true;
            this.lbActions.ItemHeight = 15;
            this.lbActions.Location = new System.Drawing.Point(13, 206);
            this.lbActions.Name = "lbActions";
            this.lbActions.Size = new System.Drawing.Size(217, 409);
            this.lbActions.TabIndex = 9;
            this.lbActions.DoubleClick += new System.EventHandler(this.lbActions_SelectedIndexChanged);
            // 
            // btnTemplate
            // 
            this.btnTemplate.Enabled = false;
            this.btnTemplate.Location = new System.Drawing.Point(254, 44);
            this.btnTemplate.Name = "btnTemplate";
            this.btnTemplate.Size = new System.Drawing.Size(121, 31);
            this.btnTemplate.TabIndex = 10;
            this.btnTemplate.Text = "从模板创建";
            this.btnTemplate.UseVisualStyleBackColor = true;
            this.btnTemplate.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(975, 633);
            this.Controls.Add(this.btnTemplate);
            this.Controls.Add(this.lbActions);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lbFile);
            this.Controls.Add(this.tbClientSecret);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbClientID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbServer);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbServer;
        private System.Windows.Forms.TextBox tbClientID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbClientSecret;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox lbFile;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.ListBox lbActions;
        private System.Windows.Forms.Button btnTemplate;
    }
}

