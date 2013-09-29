using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using eAnt.eDonkey;

namespace eAnt.Client
{	
	/// <summary>
	/// This forms contains several panel navigable by the user,
	/// the values entered override the default configutation.
	/// </summary>
	public class WizardForm : System.Windows.Forms.Form
	{
		private Panel currentPage = null;
		private InterfacePreferences preferences = null;
		private ConnectionTypes connectionTypes = null;
		private WizardControler wizardControler = null;

		#region Form controls 
		private System.Windows.Forms.Button buttonBack;
		private System.Windows.Forms.Button buttonNext;
		private System.Windows.Forms.Panel page1;
		private System.Windows.Forms.TextBox textBoxNick;
		private System.Windows.Forms.Label labelNick;
		private System.Windows.Forms.Panel page2;
		private System.Windows.Forms.Panel page5;
		private System.Windows.Forms.Button buttonExIncomingFolder;
		private System.Windows.Forms.TextBox textBoxIncomingFolder;
		private System.Windows.Forms.Button buttonRemoveTmp;
		private System.Windows.Forms.ListBox listBoxTmpFolder;
		private System.Windows.Forms.Button buttonExpTemp;
		private System.Windows.Forms.Label labelTempFolder;
		private System.Windows.Forms.Label labelProgress;
		private System.Windows.Forms.Panel page3;
		private System.Windows.Forms.PictureBox logo;
		private System.Windows.Forms.Label labelDownloadFolder;
		private System.Windows.Forms.TextBox textBoxMaxUpSpeed;
		private System.Windows.Forms.TextBox textBoxMaxDownSpeed;
		private System.Windows.Forms.Label labelMaxUpSpeed;
		private System.Windows.Forms.Label labelMaxDownSpeed;
		private System.Windows.Forms.Panel page4;
		private System.Windows.Forms.CheckBox checkBoxAutoConnect;
		private System.Windows.Forms.TextBox textBoxUDPPort;
		private System.Windows.Forms.Label labelUDPPort;
		private System.Windows.Forms.TextBox textBoxTCPPort;
		private System.Windows.Forms.Label labelTCPPort;
		private System.Windows.Forms.Button buttonFinish;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.Label labelEnterUsername;
		private System.Windows.Forms.Label labelPresentation;
		private System.Windows.Forms.Label labelWelcome;
		private System.Windows.Forms.Label labelTipDirs;
		private System.Windows.Forms.Label labelNoteDirs;
		private System.Windows.Forms.Label labelChangeLater;
		private System.Windows.Forms.Label labelCompleted;
		private System.Windows.Forms.Label labelCongratulations;
		private System.Windows.Forms.Label labelTipNetwork;
		private System.Windows.Forms.Label labelNoteNetwork;
		private System.Windows.Forms.Label labelNetwork;
		private System.Windows.Forms.Label labelTipTransLimits;
		private System.Windows.Forms.Label labelNoteTransLimit;
		private System.Windows.Forms.Label labelTranferLimits;
		private System.Windows.Forms.Button buttonSkip;
		private System.Windows.Forms.Label labelFolders;
		private System.Windows.Forms.ComboBox comboConnection;
		private System.Windows.Forms.Label lblConnection;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.PictureBox smallIcon1;
		private System.Windows.Forms.PictureBox smallIcon2;
		private System.Windows.Forms.PictureBox smallIcon3;
		#endregion

		/// <summary>
		/// Variable required by the designer.
		/// </summary>
		private System.ComponentModel.Container components = null;

		//The wizard only configures a few preferences but it must 
		// receive all of them.
		public WizardForm(InterfacePreferences preferences)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//The controler is initialized with a callback method to update the view
			wizardControler = new WizardControler(new WizardControler.StatusChanged(UpdateView));
			currentPage = page1;
			this.preferences = preferences;

			//We take the default values for the folders
			if (preferences != null)
			{
				if (preferences.sharedFolders.Length > 0)
				{
					this.textBoxIncomingFolder.Text = preferences.sharedFolders[0];
				}
				if (preferences.TempFolders.Length > 0)
				{
					this.listBoxTmpFolder.Items.Add(preferences.TempFolders[0]);
				}
			}
			connectionTypes = new ConnectionTypes();
			comboConnection.Items.AddRange(connectionTypes.GetConnectionTypes());
		}

		/// <summary>
		/// Once the wizard window closes the preferences with the user changes
		/// are retrieved with this method
		/// </summary>
		/// <returns>The new preferences</returns>
		public InterfacePreferences GetPreferences()
		{
			return preferences;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(WizardForm));
			this.buttonBack = new System.Windows.Forms.Button();
			this.buttonNext = new System.Windows.Forms.Button();
			this.buttonSkip = new System.Windows.Forms.Button();
			this.page1 = new System.Windows.Forms.Panel();
			this.logo = new System.Windows.Forms.PictureBox();
			this.textBoxNick = new System.Windows.Forms.TextBox();
			this.labelNick = new System.Windows.Forms.Label();
			this.labelEnterUsername = new System.Windows.Forms.Label();
			this.labelPresentation = new System.Windows.Forms.Label();
			this.labelWelcome = new System.Windows.Forms.Label();
			this.labelProgress = new System.Windows.Forms.Label();
			this.page2 = new System.Windows.Forms.Panel();
			this.smallIcon3 = new System.Windows.Forms.PictureBox();
			this.lblConnection = new System.Windows.Forms.Label();
			this.comboConnection = new System.Windows.Forms.ComboBox();
			this.textBoxMaxUpSpeed = new System.Windows.Forms.TextBox();
			this.textBoxMaxDownSpeed = new System.Windows.Forms.TextBox();
			this.labelMaxUpSpeed = new System.Windows.Forms.Label();
			this.labelMaxDownSpeed = new System.Windows.Forms.Label();
			this.labelTipTransLimits = new System.Windows.Forms.Label();
			this.labelNoteTransLimit = new System.Windows.Forms.Label();
			this.labelTranferLimits = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.page3 = new System.Windows.Forms.Panel();
			this.smallIcon1 = new System.Windows.Forms.PictureBox();
			this.labelDownloadFolder = new System.Windows.Forms.Label();
			this.buttonRemoveTmp = new System.Windows.Forms.Button();
			this.listBoxTmpFolder = new System.Windows.Forms.ListBox();
			this.buttonExpTemp = new System.Windows.Forms.Button();
			this.labelTempFolder = new System.Windows.Forms.Label();
			this.buttonExIncomingFolder = new System.Windows.Forms.Button();
			this.textBoxIncomingFolder = new System.Windows.Forms.TextBox();
			this.labelTipDirs = new System.Windows.Forms.Label();
			this.labelNoteDirs = new System.Windows.Forms.Label();
			this.labelFolders = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.page5 = new System.Windows.Forms.Panel();
			this.labelChangeLater = new System.Windows.Forms.Label();
			this.labelCompleted = new System.Windows.Forms.Label();
			this.labelCongratulations = new System.Windows.Forms.Label();
			this.page4 = new System.Windows.Forms.Panel();
			this.smallIcon2 = new System.Windows.Forms.PictureBox();
			this.checkBoxAutoConnect = new System.Windows.Forms.CheckBox();
			this.textBoxUDPPort = new System.Windows.Forms.TextBox();
			this.labelUDPPort = new System.Windows.Forms.Label();
			this.textBoxTCPPort = new System.Windows.Forms.TextBox();
			this.labelTCPPort = new System.Windows.Forms.Label();
			this.labelTipNetwork = new System.Windows.Forms.Label();
			this.labelNoteNetwork = new System.Windows.Forms.Label();
			this.labelNetwork = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.buttonFinish = new System.Windows.Forms.Button();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.page1.SuspendLayout();
			this.page2.SuspendLayout();
			this.page3.SuspendLayout();
			this.page5.SuspendLayout();
			this.page4.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonBack
			// 
			this.buttonBack.BackColor = System.Drawing.SystemColors.Control;
			this.buttonBack.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.buttonBack.Location = new System.Drawing.Point(224, 320);
			this.buttonBack.Name = "buttonBack";
			this.buttonBack.Size = new System.Drawing.Size(80, 23);
			this.buttonBack.TabIndex = 0;
			this.buttonBack.Text = "< Back";
			this.buttonBack.Visible = false;
			this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
			// 
			// buttonNext
			// 
			this.buttonNext.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.buttonNext.Location = new System.Drawing.Point(312, 320);
			this.buttonNext.Name = "buttonNext";
			this.buttonNext.Size = new System.Drawing.Size(80, 23);
			this.buttonNext.TabIndex = 1;
			this.buttonNext.Text = "Next >";
			this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
			// 
			// buttonSkip
			// 
			this.buttonSkip.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.buttonSkip.Location = new System.Drawing.Point(424, 320);
			this.buttonSkip.Name = "buttonSkip";
			this.buttonSkip.Size = new System.Drawing.Size(80, 23);
			this.buttonSkip.TabIndex = 2;
			this.buttonSkip.Text = "Skip";
			this.buttonSkip.Click += new System.EventHandler(this.buttonExit_Click);
			// 
			// page1
			// 
			this.page1.BackColor = System.Drawing.SystemColors.Window;
			this.page1.Controls.Add(this.logo);
			this.page1.Controls.Add(this.textBoxNick);
			this.page1.Controls.Add(this.labelNick);
			this.page1.Controls.Add(this.labelEnterUsername);
			this.page1.Controls.Add(this.labelPresentation);
			this.page1.Controls.Add(this.labelWelcome);
			this.page1.Location = new System.Drawing.Point(0, 0);
			this.page1.Name = "page1";
			this.page1.Size = new System.Drawing.Size(512, 312);
			this.page1.TabIndex = 3;
			// 
			// logo
			// 
			this.logo.Location = new System.Drawing.Point(40, 32);
			this.logo.Name = "logo";
			this.logo.Size = new System.Drawing.Size(120, 100);
			this.logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.logo.TabIndex = 12;
			this.logo.TabStop = false;
			// 
			// textBoxNick
			// 
			this.textBoxNick.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBoxNick.Location = new System.Drawing.Point(184, 256);
			this.textBoxNick.Name = "textBoxNick";
			this.textBoxNick.Size = new System.Drawing.Size(159, 20);
			this.textBoxNick.TabIndex = 11;
			this.textBoxNick.Text = "http://www.Ant.com";
			// 
			// labelNick
			// 
			this.labelNick.Location = new System.Drawing.Point(112, 259);
			this.labelNick.Name = "labelNick";
			this.labelNick.Size = new System.Drawing.Size(59, 14);
			this.labelNick.TabIndex = 10;
			this.labelNick.Text = "Nick:";
			this.labelNick.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelEnterUsername
			// 
			this.labelEnterUsername.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelEnterUsername.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelEnterUsername.Location = new System.Drawing.Point(56, 216);
			this.labelEnterUsername.Name = "labelEnterUsername";
			this.labelEnterUsername.Size = new System.Drawing.Size(392, 32);
			this.labelEnterUsername.TabIndex = 3;
			this.labelEnterUsername.Text = "First, enter the name the other users will see:";
			this.labelEnterUsername.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// labelPresentation
			// 
			this.labelPresentation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelPresentation.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelPresentation.Location = new System.Drawing.Point(40, 144);
			this.labelPresentation.Name = "labelPresentation";
			this.labelPresentation.Size = new System.Drawing.Size(432, 24);
			this.labelPresentation.TabIndex = 1;
			this.labelPresentation.Text = "This wizard will help you get your Ant up and running";
			this.labelPresentation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// labelWelcome
			// 
			this.labelWelcome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelWelcome.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelWelcome.Location = new System.Drawing.Point(208, 64);
			this.labelWelcome.Name = "labelWelcome";
			this.labelWelcome.Size = new System.Drawing.Size(216, 40);
			this.labelWelcome.TabIndex = 0;
			this.labelWelcome.Text = "Welcome to Ant!";
			this.labelWelcome.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// labelProgress
			// 
			this.labelProgress.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelProgress.Location = new System.Drawing.Point(48, 320);
			this.labelProgress.Name = "labelProgress";
			this.labelProgress.Size = new System.Drawing.Size(96, 24);
			this.labelProgress.TabIndex = 4;
			this.labelProgress.Tag = "Step # of 5";
			this.labelProgress.Text = "Step 1 of 5";
			this.labelProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// page2
			// 
			this.page2.BackColor = System.Drawing.SystemColors.Window;
			this.page2.Controls.Add(this.smallIcon3);
			this.page2.Controls.Add(this.lblConnection);
			this.page2.Controls.Add(this.comboConnection);
			this.page2.Controls.Add(this.textBoxMaxUpSpeed);
			this.page2.Controls.Add(this.textBoxMaxDownSpeed);
			this.page2.Controls.Add(this.labelMaxUpSpeed);
			this.page2.Controls.Add(this.labelMaxDownSpeed);
			this.page2.Controls.Add(this.labelTipTransLimits);
			this.page2.Controls.Add(this.labelNoteTransLimit);
			this.page2.Controls.Add(this.labelTranferLimits);
			this.page2.Controls.Add(this.groupBox3);
			this.page2.Location = new System.Drawing.Point(0, 0);
			this.page2.Name = "page2";
			this.page2.Size = new System.Drawing.Size(512, 312);
			this.page2.TabIndex = 14;
			this.page2.Visible = false;
			// 
			// smallIcon3
			// 
			this.smallIcon3.Location = new System.Drawing.Point(440, 32);
			this.smallIcon3.Name = "smallIcon3";
			this.smallIcon3.Size = new System.Drawing.Size(16, 16);
			this.smallIcon3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.smallIcon3.TabIndex = 44;
			this.smallIcon3.TabStop = false;
			// 
			// lblConnection
			// 
			this.lblConnection.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblConnection.Location = new System.Drawing.Point(48, 120);
			this.lblConnection.Name = "lblConnection";
			this.lblConnection.Size = new System.Drawing.Size(144, 14);
			this.lblConnection.TabIndex = 40;
			this.lblConnection.Text = "Connection";
			this.lblConnection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// comboConnection
			// 
			this.comboConnection.DisplayMember = "Modem/Dialup";
			this.comboConnection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboConnection.Location = new System.Drawing.Point(48, 144);
			this.comboConnection.MaxDropDownItems = 6;
			this.comboConnection.Name = "comboConnection";
			this.comboConnection.Size = new System.Drawing.Size(144, 21);
			this.comboConnection.TabIndex = 39;
			this.comboConnection.SelectedIndexChanged += new System.EventHandler(this.comboConnection_SelectedIndexChanged);
			// 
			// textBoxMaxUpSpeed
			// 
			this.textBoxMaxUpSpeed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBoxMaxUpSpeed.Location = new System.Drawing.Point(392, 144);
			this.textBoxMaxUpSpeed.MaxLength = 5;
			this.textBoxMaxUpSpeed.Name = "textBoxMaxUpSpeed";
			this.textBoxMaxUpSpeed.Size = new System.Drawing.Size(32, 20);
			this.textBoxMaxUpSpeed.TabIndex = 38;
			this.textBoxMaxUpSpeed.Text = "";
			this.textBoxMaxUpSpeed.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxMaxUpSpeed_KeyPress);
			// 
			// textBoxMaxDownSpeed
			// 
			this.textBoxMaxDownSpeed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBoxMaxDownSpeed.Location = new System.Drawing.Point(392, 120);
			this.textBoxMaxDownSpeed.MaxLength = 5;
			this.textBoxMaxDownSpeed.Name = "textBoxMaxDownSpeed";
			this.textBoxMaxDownSpeed.Size = new System.Drawing.Size(32, 20);
			this.textBoxMaxDownSpeed.TabIndex = 37;
			this.textBoxMaxDownSpeed.Text = "";
			this.textBoxMaxDownSpeed.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxMaxDownSpeed_KeyPress);
			// 
			// labelMaxUpSpeed
			// 
			this.labelMaxUpSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelMaxUpSpeed.Location = new System.Drawing.Point(248, 144);
			this.labelMaxUpSpeed.Name = "labelMaxUpSpeed";
			this.labelMaxUpSpeed.Size = new System.Drawing.Size(136, 14);
			this.labelMaxUpSpeed.TabIndex = 36;
			this.labelMaxUpSpeed.Text = "Max up speed:";
			this.labelMaxUpSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelMaxDownSpeed
			// 
			this.labelMaxDownSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelMaxDownSpeed.Location = new System.Drawing.Point(248, 120);
			this.labelMaxDownSpeed.Name = "labelMaxDownSpeed";
			this.labelMaxDownSpeed.Size = new System.Drawing.Size(136, 14);
			this.labelMaxDownSpeed.TabIndex = 35;
			this.labelMaxDownSpeed.Text = "Max down speed:";
			this.labelMaxDownSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelTipTransLimits
			// 
			this.labelTipTransLimits.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelTipTransLimits.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelTipTransLimits.Location = new System.Drawing.Point(16, 264);
			this.labelTipTransLimits.Name = "labelTipTransLimits";
			this.labelTipTransLimits.Size = new System.Drawing.Size(488, 40);
			this.labelTipTransLimits.TabIndex = 34;
			this.labelTipTransLimits.Text = "TIP: p2p programs usually reward uploaders, so the more you upload the more you d" +
				"ownload.";
			// 
			// labelNoteTransLimit
			// 
			this.labelNoteTransLimit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelNoteTransLimit.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelNoteTransLimit.Location = new System.Drawing.Point(16, 216);
			this.labelNoteTransLimit.Name = "labelNoteTransLimit";
			this.labelNoteTransLimit.Size = new System.Drawing.Size(488, 44);
			this.labelNoteTransLimit.TabIndex = 33;
			this.labelNoteTransLimit.Text = "NOTE: The download speed cannot be more than four times the upload speed. This re" +
				"striction only applies when you upload 8KBs or less.";
			// 
			// labelTranferLimits
			// 
			this.labelTranferLimits.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelTranferLimits.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelTranferLimits.Location = new System.Drawing.Point(32, 32);
			this.labelTranferLimits.Name = "labelTranferLimits";
			this.labelTranferLimits.Size = new System.Drawing.Size(328, 32);
			this.labelTranferLimits.TabIndex = 28;
			this.labelTranferLimits.Text = "Transfer limits";
			// 
			// groupBox3
			// 
			this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.groupBox3.Location = new System.Drawing.Point(8, 80);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(496, 120);
			this.groupBox3.TabIndex = 41;
			this.groupBox3.TabStop = false;
			// 
			// page3
			// 
			this.page3.BackColor = System.Drawing.SystemColors.Window;
			this.page3.Controls.Add(this.smallIcon1);
			this.page3.Controls.Add(this.labelDownloadFolder);
			this.page3.Controls.Add(this.buttonRemoveTmp);
			this.page3.Controls.Add(this.listBoxTmpFolder);
			this.page3.Controls.Add(this.buttonExpTemp);
			this.page3.Controls.Add(this.labelTempFolder);
			this.page3.Controls.Add(this.buttonExIncomingFolder);
			this.page3.Controls.Add(this.textBoxIncomingFolder);
			this.page3.Controls.Add(this.labelTipDirs);
			this.page3.Controls.Add(this.labelNoteDirs);
			this.page3.Controls.Add(this.labelFolders);
			this.page3.Controls.Add(this.groupBox1);
			this.page3.Location = new System.Drawing.Point(0, 0);
			this.page3.Name = "page3";
			this.page3.Size = new System.Drawing.Size(512, 312);
			this.page3.TabIndex = 15;
			this.page3.Visible = false;
			// 
			// smallIcon1
			// 
			this.smallIcon1.Location = new System.Drawing.Point(416, 8);
			this.smallIcon1.Name = "smallIcon1";
			this.smallIcon1.Size = new System.Drawing.Size(80, 72);
			this.smallIcon1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.smallIcon1.TabIndex = 43;
			this.smallIcon1.TabStop = false;
			// 
			// labelDownloadFolder
			// 
			this.labelDownloadFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelDownloadFolder.Location = new System.Drawing.Point(10, 104);
			this.labelDownloadFolder.Name = "labelDownloadFolder";
			this.labelDownloadFolder.Size = new System.Drawing.Size(136, 14);
			this.labelDownloadFolder.TabIndex = 42;
			this.labelDownloadFolder.Text = "Downloads folder:";
			this.labelDownloadFolder.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// buttonRemoveTmp
			// 
			this.buttonRemoveTmp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonRemoveTmp.Location = new System.Drawing.Point(384, 168);
			this.buttonRemoveTmp.Name = "buttonRemoveTmp";
			this.buttonRemoveTmp.Size = new System.Drawing.Size(112, 24);
			this.buttonRemoveTmp.TabIndex = 41;
			this.buttonRemoveTmp.Text = "Remove";
			this.buttonRemoveTmp.Click += new System.EventHandler(this.buttonRemoveTmp_Click);
			// 
			// listBoxTmpFolder
			// 
			this.listBoxTmpFolder.Location = new System.Drawing.Point(152, 136);
			this.listBoxTmpFolder.Name = "listBoxTmpFolder";
			this.listBoxTmpFolder.Size = new System.Drawing.Size(224, 56);
			this.listBoxTmpFolder.TabIndex = 40;
			// 
			// buttonExpTemp
			// 
			this.buttonExpTemp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonExpTemp.Location = new System.Drawing.Point(384, 136);
			this.buttonExpTemp.Name = "buttonExpTemp";
			this.buttonExpTemp.Size = new System.Drawing.Size(112, 24);
			this.buttonExpTemp.TabIndex = 39;
			this.buttonExpTemp.Text = "Add...";
			this.buttonExpTemp.Click += new System.EventHandler(this.buttonExpTemp_Click);
			// 
			// labelTempFolder
			// 
			this.labelTempFolder.Location = new System.Drawing.Point(16, 144);
			this.labelTempFolder.Name = "labelTempFolder";
			this.labelTempFolder.Size = new System.Drawing.Size(130, 14);
			this.labelTempFolder.TabIndex = 38;
			this.labelTempFolder.Text = "Temp. folder:";
			this.labelTempFolder.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// buttonExIncomingFolder
			// 
			this.buttonExIncomingFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonExIncomingFolder.Location = new System.Drawing.Point(384, 104);
			this.buttonExIncomingFolder.Name = "buttonExIncomingFolder";
			this.buttonExIncomingFolder.Size = new System.Drawing.Size(112, 22);
			this.buttonExIncomingFolder.TabIndex = 37;
			this.buttonExIncomingFolder.Text = "Browse";
			this.buttonExIncomingFolder.Click += new System.EventHandler(this.buttonExIncomingFolder_Click);
			// 
			// textBoxIncomingFolder
			// 
			this.textBoxIncomingFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBoxIncomingFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.textBoxIncomingFolder.Location = new System.Drawing.Point(152, 104);
			this.textBoxIncomingFolder.Name = "textBoxIncomingFolder";
			this.textBoxIncomingFolder.ReadOnly = true;
			this.textBoxIncomingFolder.Size = new System.Drawing.Size(224, 21);
			this.textBoxIncomingFolder.TabIndex = 36;
			this.textBoxIncomingFolder.Text = "";
			// 
			// labelTipDirs
			// 
			this.labelTipDirs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelTipDirs.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelTipDirs.Location = new System.Drawing.Point(16, 264);
			this.labelTipDirs.Name = "labelTipDirs";
			this.labelTipDirs.Size = new System.Drawing.Size(488, 48);
			this.labelTipDirs.TabIndex = 34;
			this.labelTipDirs.Text = "TIP:  If you are migrating from another p2p (i.e. emule), select the temp directo" +
				"ries from that program.";
			// 
			// labelNoteDirs
			// 
			this.labelNoteDirs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelNoteDirs.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelNoteDirs.Location = new System.Drawing.Point(16, 224);
			this.labelNoteDirs.Name = "labelNoteDirs";
			this.labelNoteDirs.Size = new System.Drawing.Size(488, 40);
			this.labelNoteDirs.TabIndex = 33;
			this.labelNoteDirs.Text = "NOTE: Only the first temp directory will be used for new downloads.";
			// 
			// labelFolders
			// 
			this.labelFolders.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelFolders.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelFolders.Location = new System.Drawing.Point(32, 32);
			this.labelFolders.Name = "labelFolders";
			this.labelFolders.Size = new System.Drawing.Size(320, 28);
			this.labelFolders.TabIndex = 28;
			this.labelFolders.Text = "Folders";
			// 
			// groupBox1
			// 
			this.groupBox1.Location = new System.Drawing.Point(8, 80);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(496, 128);
			this.groupBox1.TabIndex = 21;
			this.groupBox1.TabStop = false;
			// 
			// page5
			// 
			this.page5.BackColor = System.Drawing.SystemColors.Window;
			this.page5.Controls.Add(this.labelChangeLater);
			this.page5.Controls.Add(this.labelCompleted);
			this.page5.Controls.Add(this.labelCongratulations);
			this.page5.Location = new System.Drawing.Point(0, 0);
			this.page5.Name = "page5";
			this.page5.Size = new System.Drawing.Size(512, 312);
			this.page5.TabIndex = 16;
			this.page5.Visible = false;
			// 
			// labelChangeLater
			// 
			this.labelChangeLater.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelChangeLater.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelChangeLater.Location = new System.Drawing.Point(16, 200);
			this.labelChangeLater.Name = "labelChangeLater";
			this.labelChangeLater.Size = new System.Drawing.Size(480, 64);
			this.labelChangeLater.TabIndex = 30;
			this.labelChangeLater.Text = "You can later change this settings in the options screen.";
			this.labelChangeLater.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// labelCompleted
			// 
			this.labelCompleted.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelCompleted.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelCompleted.Location = new System.Drawing.Point(16, 144);
			this.labelCompleted.Name = "labelCompleted";
			this.labelCompleted.Size = new System.Drawing.Size(480, 56);
			this.labelCompleted.TabIndex = 29;
			this.labelCompleted.Text = "You have completed this wizard, press Back if you wish to review your selections " +
				"or press Finish to start using Ant.";
			this.labelCompleted.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// labelCongratulations
			// 
			this.labelCongratulations.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelCongratulations.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelCongratulations.Location = new System.Drawing.Point(128, 80);
			this.labelCongratulations.Name = "labelCongratulations";
			this.labelCongratulations.Size = new System.Drawing.Size(264, 32);
			this.labelCongratulations.TabIndex = 28;
			this.labelCongratulations.Text = "Congratulations!!!";
			this.labelCongratulations.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// page4
			// 
			this.page4.BackColor = System.Drawing.SystemColors.Window;
			this.page4.Controls.Add(this.smallIcon2);
			this.page4.Controls.Add(this.checkBoxAutoConnect);
			this.page4.Controls.Add(this.textBoxUDPPort);
			this.page4.Controls.Add(this.labelUDPPort);
			this.page4.Controls.Add(this.textBoxTCPPort);
			this.page4.Controls.Add(this.labelTCPPort);
			this.page4.Controls.Add(this.labelTipNetwork);
			this.page4.Controls.Add(this.labelNoteNetwork);
			this.page4.Controls.Add(this.labelNetwork);
			this.page4.Controls.Add(this.groupBox2);
			this.page4.Location = new System.Drawing.Point(0, 0);
			this.page4.Name = "page4";
			this.page4.Size = new System.Drawing.Size(512, 312);
			this.page4.TabIndex = 19;
			this.page4.Visible = false;
			// 
			// smallIcon2
			// 
			this.smallIcon2.Location = new System.Drawing.Point(416, 8);
			this.smallIcon2.Name = "smallIcon2";
			this.smallIcon2.Size = new System.Drawing.Size(80, 72);
			this.smallIcon2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.smallIcon2.TabIndex = 44;
			this.smallIcon2.TabStop = false;
			// 
			// checkBoxAutoConnect
			// 
			this.checkBoxAutoConnect.Checked = true;
			this.checkBoxAutoConnect.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxAutoConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.checkBoxAutoConnect.Location = new System.Drawing.Point(72, 104);
			this.checkBoxAutoConnect.Name = "checkBoxAutoConnect";
			this.checkBoxAutoConnect.Size = new System.Drawing.Size(156, 16);
			this.checkBoxAutoConnect.TabIndex = 42;
			this.checkBoxAutoConnect.Text = "Auto connect";
			// 
			// textBoxUDPPort
			// 
			this.textBoxUDPPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBoxUDPPort.Location = new System.Drawing.Point(160, 160);
			this.textBoxUDPPort.MaxLength = 5;
			this.textBoxUDPPort.Name = "textBoxUDPPort";
			this.textBoxUDPPort.Size = new System.Drawing.Size(46, 20);
			this.textBoxUDPPort.TabIndex = 41;
			this.textBoxUDPPort.Text = "4672";
			this.textBoxUDPPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxUDPPort_KeyPress);
			// 
			// labelUDPPort
			// 
			this.labelUDPPort.Location = new System.Drawing.Point(72, 160);
			this.labelUDPPort.Name = "labelUDPPort";
			this.labelUDPPort.Size = new System.Drawing.Size(83, 14);
			this.labelUDPPort.TabIndex = 40;
			this.labelUDPPort.Text = "UDP Port:";
			this.labelUDPPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textBoxTCPPort
			// 
			this.textBoxTCPPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBoxTCPPort.Location = new System.Drawing.Point(160, 136);
			this.textBoxTCPPort.MaxLength = 5;
			this.textBoxTCPPort.Name = "textBoxTCPPort";
			this.textBoxTCPPort.Size = new System.Drawing.Size(46, 20);
			this.textBoxTCPPort.TabIndex = 39;
			this.textBoxTCPPort.Text = "4662";
			this.textBoxTCPPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxTCPPort_KeyPress);
			// 
			// labelTCPPort
			// 
			this.labelTCPPort.Location = new System.Drawing.Point(72, 136);
			this.labelTCPPort.Name = "labelTCPPort";
			this.labelTCPPort.Size = new System.Drawing.Size(81, 14);
			this.labelTCPPort.TabIndex = 38;
			this.labelTCPPort.Text = "TCP Port:";
			this.labelTCPPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelTipNetwork
			// 
			this.labelTipNetwork.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelTipNetwork.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelTipNetwork.Location = new System.Drawing.Point(16, 264);
			this.labelTipNetwork.Name = "labelTipNetwork";
			this.labelTipNetwork.Size = new System.Drawing.Size(488, 48);
			this.labelTipNetwork.TabIndex = 37;
			this.labelTipNetwork.Text = "TIP: The default ports (4662 and 4672) are often blocked by ISPs, it is advisable" +
				" to use others.";
			// 
			// labelNoteNetwork
			// 
			this.labelNoteNetwork.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelNoteNetwork.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelNoteNetwork.Location = new System.Drawing.Point(16, 216);
			this.labelNoteNetwork.Name = "labelNoteNetwork";
			this.labelNoteNetwork.Size = new System.Drawing.Size(488, 48);
			this.labelNoteNetwork.TabIndex = 36;
			this.labelNoteNetwork.Text = "NOTE: The ports must be open if you use a firewall, in order to get a HighID. Thi" +
				"s allows you to download from more users.";
			// 
			// labelNetwork
			// 
			this.labelNetwork.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelNetwork.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelNetwork.ForeColor = System.Drawing.SystemColors.ControlText;
			this.labelNetwork.Location = new System.Drawing.Point(32, 32);
			this.labelNetwork.Name = "labelNetwork";
			this.labelNetwork.Size = new System.Drawing.Size(320, 32);
			this.labelNetwork.TabIndex = 35;
			this.labelNetwork.Text = "Network";
			// 
			// groupBox2
			// 
			this.groupBox2.Location = new System.Drawing.Point(8, 80);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(496, 120);
			this.groupBox2.TabIndex = 43;
			this.groupBox2.TabStop = false;
			// 
			// buttonFinish
			// 
			this.buttonFinish.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.buttonFinish.Location = new System.Drawing.Point(312, 320);
			this.buttonFinish.Name = "buttonFinish";
			this.buttonFinish.Size = new System.Drawing.Size(80, 23);
			this.buttonFinish.TabIndex = 20;
			this.buttonFinish.Text = "Finish";
			this.buttonFinish.Visible = false;
			this.buttonFinish.Click += new System.EventHandler(this.buttonFinish_Click);
			// 
			// WizardForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(512, 350);
			this.ControlBox = false;
			this.Controls.Add(this.labelProgress);
			this.Controls.Add(this.buttonSkip);
			this.Controls.Add(this.buttonNext);
			this.Controls.Add(this.buttonBack);
			this.Controls.Add(this.buttonFinish);
			this.Controls.Add(this.page2);
			this.Controls.Add(this.page1);
			this.Controls.Add(this.page5);
			this.Controls.Add(this.page3);
			this.Controls.Add(this.page4);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "WizardForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Ant Configuration Wizard";
			this.Load += new System.EventHandler(this.WizardForm_Load);
			this.page1.ResumeLayout(false);
			this.page2.ResumeLayout(false);
			this.page3.ResumeLayout(false);
			this.page5.ResumeLayout(false);
			this.page4.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Translates to the user language
		/// </summary>
		/// <param name="translation">The object with the translations</param>
		private void m_Globalize(Classes.Globalization translation)
		{
			this.Text = translation["LBL_WIZARDTITLE"];

			//The pattern is stored in the tag, then the ampersand is
			// sustitued by the number of the step in the wizard
			labelProgress.Tag = translation["LBL_PROGRESSWHIZ"] + " " 
				+ WizardControler.NUM_PAGES;
			buttonBack.Text = "< " + translation["LBL_BACK"];
			buttonNext.Text = translation["LBL_NEXT"] + " >";
			buttonSkip.Text = translation["LBL_SKIP"];
			buttonFinish.Text = translation["LBL_FINISH"];
			buttonExIncomingFolder.Text = translation["LBL_EXPSHARED"];
			buttonRemoveTmp.Text = translation["LBL_REMOVETEMP"];
			buttonExpTemp.Text = translation["LBL_EXPTEMP"];
			labelNick.Text = translation["LBL_NICK"]+":";
			labelTempFolder.Text = translation["LBL_TEMPFOLDER"]+":";
			labelDownloadFolder.Text = translation["LBL_DOWNFOLDER"]+":";
			labelMaxUpSpeed.Text = translation["LBL_MAXUPSPEED"]+":";
			labelMaxDownSpeed.Text = translation["LBL_MAXDOWNSPEED"]+":";
			labelUDPPort.Text = translation["LBL_UDPPORT"]+":";
			labelTCPPort.Text = translation["LBL_TCPPORT"]+":";
			labelEnterUsername.Text = translation["LBL_ENTERUSERNAME"];
			labelPresentation.Text = translation["LBL_WHIZPRESENTATION"];
			labelWelcome.Text = translation["LBL_WELCOME"];
			labelTipDirs.Text = translation["LBL_TIPTEMPDIRS"];
			labelNoteDirs.Text = translation["LBL_NOTETEMPDIRS"];
			labelFolders.Text = translation["LBL_FOLDERS"];
			labelChangeLater.Text = translation["LBL_WHIZCHANGELATER"];
			labelCompleted.Text = translation["LBL_WHIZCOMPLETED"];
			labelCongratulations.Text = translation["LBL_COMPLETED"];
			labelTipNetwork.Text = translation["LBL_TIPNETWORK"];
			labelNoteNetwork.Text = translation["LBL_NOTENETWORK"];
			labelNetwork.Text = translation["LBL_NETWORK"];
			labelTipTransLimits.Text = translation["LBL_TIPLIMITS"];
			labelNoteTransLimit.Text = translation["LBL_NOTELIMITS"];
			labelTranferLimits.Text = translation["LBL_TRANFERLIMIT"];
			checkBoxAutoConnect.Text= eAntForm.Globalization["LBL_AUTOCONNECT"];
			lblConnection.Text = eAntForm.Globalization["LBL_CONNECTION"];
		}
		

		/// <summary>
		/// Updates the label that tracks he progress in the Wizard
		/// </summary>
		private void UpdateStatus(byte currentPageNumber)
		{
			string text = (string)labelProgress.Tag;
			labelProgress.Text = text.Replace("#", currentPageNumber.ToString());

			buttonBack.Visible = wizardControler.IsBackAvailable();
			buttonNext.Visible = wizardControler.IsNextAvailable();
			buttonFinish.Visible = wizardControler.IsFinishAvailable();
		}

		/// <summary>
		/// Updates the form with a change in the current page
		/// </summary>
		private void UpdateView(byte currentPageNumber) 
		{			
			if (currentPage != null)
			{
				currentPage.Visible = false;
			}
			switch(currentPageNumber)
			{
				case 1:
					currentPage = page1;
					break;
				case 2:
					currentPage = page2;
					break;
				case 3:
					currentPage = page3;
					break;
				case 4:
					currentPage = page4;
					break;
				case 5:
					currentPage = page5;
					break;
				default:
					MessageBox.Show("Invalid wizard page", "Internal error");
					this.Close();
					break;
			}
			currentPage.Visible = true;
			UpdateStatus(currentPageNumber);
		}

		#region Form event handlers
		private void buttonExit_Click(object sender, System.EventArgs e)
		{
			string warnSkipWizard = eAntForm.Globalization["MSG_SKIPWIZARDWARNING"];
			string AntWizardTitle = eAntForm.Globalization["LBL_WIZARDTITLE"];
			if (MessageBox.Show(warnSkipWizard, AntWizardTitle, MessageBoxButtons.YesNo,MessageBoxIcon.Warning)==DialogResult.Yes)
			{
				this.Close();
			}
		}
		
		private void buttonNext_Click(object sender, System.EventArgs e)
		{
			wizardControler.Next();
		}

		private void buttonBack_Click(object sender, System.EventArgs e)
		{
			wizardControler.Back();
		}

		private void buttonFinish_Click(object sender, System.EventArgs e)
		{
			if (preferences != null) 
			{
				if (textBoxNick.Text != "")
				{
					preferences.UserName = textBoxNick.Text;
				}
				if (textBoxTCPPort.Text != "")
				{
					preferences.TCPPort=Convert.ToUInt16(textBoxTCPPort.Text);
				}
				if (textBoxUDPPort.Text != "")
				{
					preferences.UDPPort=Convert.ToUInt16(textBoxUDPPort.Text);
				}
				preferences.Autoreconect=checkBoxAutoConnect.Checked;
				if (textBoxMaxDownSpeed.Text != "")
				{
					preferences.maxDownloadRate=(float)Convert.ToDouble(textBoxMaxDownSpeed.Text);
				}
				if (textBoxMaxUpSpeed.Text != "")
				{
					preferences.maxUploadRate=(float)Convert.ToDouble(textBoxMaxUpSpeed.Text);
				}
				if (textBoxIncomingFolder.Text != "")
				{
					preferences.sharedFolders=new string[1];
					preferences.sharedFolders[0]=textBoxIncomingFolder.Text;			
				}					
				preferences.TempFolders=new string[listBoxTmpFolder.Items.Count];
				for (int i=0;i<listBoxTmpFolder.Items.Count;i++)
				{
					preferences.TempFolders[i]=(string)listBoxTmpFolder.Items[i];
				}
			}
			this.Close();
		}

		private void buttonExIncomingFolder_Click(object sender, System.EventArgs e)
		{
			folderBrowserDialog1.ShowNewFolderButton=true;
			folderBrowserDialog1.SelectedPath=textBoxIncomingFolder.Text;
			if (folderBrowserDialog1.ShowDialog()==DialogResult.OK)
			{
				textBoxIncomingFolder.Text = folderBrowserDialog1.SelectedPath;
			}
		}

		private void buttonExpTemp_Click(object sender, System.EventArgs e)
		{
			folderBrowserDialog1.ShowNewFolderButton=true;
			if (listBoxTmpFolder.Items.Count > 0)
			{
				folderBrowserDialog1.SelectedPath=(string)listBoxTmpFolder.Items[0];
			}
			if (folderBrowserDialog1.ShowDialog()==DialogResult.OK)
			{
				if (!listBoxTmpFolder.Items.Contains(folderBrowserDialog1.SelectedPath))
					listBoxTmpFolder.Items.Add(folderBrowserDialog1.SelectedPath);
			}
		}

		private void buttonRemoveTmp_Click(object sender, System.EventArgs e)
		{
			if ((listBoxTmpFolder.Items.Count>1)&&(listBoxTmpFolder.SelectedIndex>=0))
			{
				listBoxTmpFolder.Items.RemoveAt(listBoxTmpFolder.SelectedIndex);
			}
		}

		private void textBoxTCPPort_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			e.Handled = checkNumber(e.KeyChar);
		}

		private void textBoxMaxDownSpeed_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			e.Handled = checkNumber(e.KeyChar);				
		}

		private void textBoxMaxUpSpeed_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			e.Handled = checkNumber(e.KeyChar);						
		}

		private void textBoxUDPPort_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			e.Handled = checkNumber(e.KeyChar);		
		}

		private void WizardForm_Load(object sender, System.EventArgs e)
		{
			m_Globalize(eAntForm.Globalization);
			wizardControler.Start();
		}

		private void comboConnection_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			byte index = (byte)comboConnection.SelectedIndex;
			textBoxMaxDownSpeed.Text = connectionTypes.GetDownloadSpeed(index);
			textBoxMaxUpSpeed.Text = connectionTypes.GetUploadSpeed(index);
		}
		#endregion
		
		/// <summary>
		/// Check if a key corresponds to a number
		/// </summary>
		/// <param name="keyChar">The key pressed</param>
		/// <returns>True if the key is a number</returns>
		private bool checkNumber(char keyChar)
		{
			if((keyChar<(char)48 || keyChar>(char)57) && keyChar!=(char)8) 
			{
				return true;
			}
			else 
			{
				return false;
			}
		}	
	}

	internal class WizardControler
	{
		public const byte INITIAL_PAGE = 1;
		public const byte NUM_PAGES = 5;

		internal delegate void StatusChanged(byte currentPageNumber);   // delegate declaration

		private event StatusChanged StatusChangedEvent;

		// The current visible panel of the wizard
		private byte currentPageNumber = INITIAL_PAGE; 

		internal WizardControler(StatusChanged statusChanged)
		{
			StatusChangedEvent += statusChanged;
		}

		/// <summary>
		/// Gets the wizard to the first page
		/// </summary>
		internal void Start()
		{
			currentPageNumber = INITIAL_PAGE;
			StatusChangedEvent(currentPageNumber);
		}

		/// <summary>
		/// Navigates to the next page of the Wizard
		/// </summary>		
		internal void Next()
		{
			if (currentPageNumber < NUM_PAGES)
			{
				currentPageNumber++;
				StatusChangedEvent(currentPageNumber);
			}
		}

		/// <summary>
		/// Navigates back to the preceding page of the wizard
		/// </summary>
		internal void Back()
		{
			if (currentPageNumber > INITIAL_PAGE)
			{
				currentPageNumber--;
				StatusChangedEvent(currentPageNumber);		
			}
		}

		/// <summary>
		/// Returns true if the Wizard is in the last page 
		/// </summary>
		/// <returns>True if the finiah button should available</returns>
		internal bool IsFinishAvailable()
		{
			return currentPageNumber == NUM_PAGES;
		}

		/// <summary>
		/// Returns true if the user can go backward because teh wizard is not on the first page
		/// </summary>
		/// <returns>True if the back button should be available</returns>
		internal bool IsBackAvailable()
		{
			return currentPageNumber != WizardControler.INITIAL_PAGE;
		}

		/// <summary>
		/// Returns true if the user can go foward because the wizard is not on the last page
		/// </summary>
		/// <returns>True if the next button should be available</returns>
		internal bool IsNextAvailable()
		{
			return currentPageNumber != NUM_PAGES;
		}
	}

	/// <summary>
	/// This class contains the different types of connections available in the Wizard
	///  and the corresponding recomended tranfer limits.
	/// </summary>
	internal class ConnectionTypes
	{
		public const byte NUM_CONNECTIONS = 14;
		private ConnectionType[] connections;		
		
		/// <summary>
		/// The types of connections supported is fixed, so they are
		///  inizialized in the constructor and cannot be changed later.
		/// </summary>
		public ConnectionTypes()
		{
			connections = new ConnectionType[NUM_CONNECTIONS];
			connections[0] = new ConnectionType("Dialup/Modem (56k)","4","1");
			connections[1] = new ConnectionType("ADSL (256k/128k)","32","8");
			connections[2] = new ConnectionType("ADSL (512k/128k)","55","9");
			connections[3] = new ConnectionType("ADSL (1024k/300k","120","20");
			connections[4] = new ConnectionType("ADSL (2048k/300k","245","20");
			connections[5] = new ConnectionType("ADSL (4096k/512k","500","40");
			connections[6] = new ConnectionType("Cable (128k)","12","6");
			connections[7] = new ConnectionType("Cable (256k)","32","10");
			connections[8] = new ConnectionType("Cable (300k)","38","18");
			connections[9] = new ConnectionType("Cable (512k)","64","20");
			connections[10] = new ConnectionType("Cable (1024k)","128","35");
			connections[11] = new ConnectionType("ISDN (64k)","8","3");
			connections[12] = new ConnectionType("ISDN2 (128k)","12","4");
			connections[13] = new ConnectionType("T1 (1500k)","256","80");
		}
		
		/// <summary>
		/// Retrieves the description of the connections available
		/// </summary>
		/// <returns>The descriptions of the connections</returns>
		public string[] GetConnectionTypes()
		{
			string[] result = new string[connections.Length];
			for( int i=0; i< connections.Length; i++)
			{
				result[i] = connections[i].Name;
			}
			
			return result;
		}

		/// <summary>
		/// Returns the recommended download speed limit for the type of 
		///  connection given. The number of the connection is the index
		///  in the array of available connections.
		/// </summary>
		/// <param name="type">The index of the connection</param>
		/// <returns>The recommended download speed limit</returns>
		public string GetDownloadSpeed(byte type)
		{
			if (type < 0 || type >= connections.Length)
			{
				return "";
			}
			return connections[type].DownloadSpeed;
		}

		/// <summary>
		/// Return the recommended upload speed limit for the type of 
		///  connection given. The number of the connection is the index
		///  in the array of available connections.
		/// </summary>
		/// <param name="type">The index of the connection</param>
		/// <returns>The recommended upload speed limit</returns>
		public string GetUploadSpeed(byte type)
		{
			if (type < 0 || type >= connections.Length)
			{
				return "";
			}
			return connections[type].UploadSpeed;
		}
	}

	/// <summary>
	/// This class is a Value Object for a Connection Type
	/// </summary>
	internal class ConnectionType
	{
		private string name;
		private string downloadSpeed;
		private string uploadSpeed;

		public ConnectionType(string name, string downloadSpeed, string uploadSpeed)
		{
			this.name = name;
			this.downloadSpeed = downloadSpeed;
			this.uploadSpeed = uploadSpeed;
		}

		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		public string DownloadSpeed
		{
			get
			{
				return downloadSpeed;
			}
			set
			{
				downloadSpeed = value;
			}
		}

		public string UploadSpeed
		{
			get
			{	
				return uploadSpeed;
			}
			set 
			{
				uploadSpeed = value;
			}
		}
	}
}
