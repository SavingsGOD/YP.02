
namespace Cybersport
{
    partial class Users
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button4 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.page1 = new System.Windows.Forms.Label();
            this.page2 = new System.Windows.Forms.Label();
            this.page3 = new System.Windows.Forms.Label();
            this.page4 = new System.Windows.Forms.Label();
            this.number_of_pages = new System.Windows.Forms.Label();
            this.search = new System.Windows.Forms.TextBox();
            this.page5 = new System.Windows.Forms.Label();
            this.pointer_arrow1 = new System.Windows.Forms.PictureBox();
            this.pointer_arrow2 = new System.Windows.Forms.PictureBox();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pointer_arrow1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pointer_arrow2)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(15, 57);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(6);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1345, 395);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
            this.dataGridView1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView1_DataBindingComplete);
            this.dataGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyDown);
            this.dataGridView1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dataGridView1_KeyPress);
            this.dataGridView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseDown);
            this.dataGridView1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseMove);
            // 
            // button4
            // 
            this.button4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.button4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(102)))), ((int)(((byte)(0)))));
            this.button4.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button4.Location = new System.Drawing.Point(26, 723);
            this.button4.Margin = new System.Windows.Forms.Padding(6);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(516, 72);
            this.button4.TabIndex = 30;
            this.button4.Text = "Назад";
            this.button4.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.button2.BackColor = System.Drawing.Color.White;
            this.button2.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.Location = new System.Drawing.Point(1444, 723);
            this.button2.Margin = new System.Windows.Forms.Padding(6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(516, 72);
            this.button2.TabIndex = 29;
            this.button2.Text = "Добавить";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // page1
            // 
            this.page1.AutoSize = true;
            this.page1.Location = new System.Drawing.Point(490, 494);
            this.page1.Name = "page1";
            this.page1.Size = new System.Drawing.Size(21, 26);
            this.page1.TabIndex = 34;
            this.page1.Text = "1";
            this.page1.Click += new System.EventHandler(this.page1_Click);
            // 
            // page2
            // 
            this.page2.AutoSize = true;
            this.page2.Location = new System.Drawing.Point(517, 494);
            this.page2.Name = "page2";
            this.page2.Size = new System.Drawing.Size(24, 26);
            this.page2.TabIndex = 35;
            this.page2.Text = "2";
            this.page2.Click += new System.EventHandler(this.page2_Click);
            // 
            // page3
            // 
            this.page3.AutoSize = true;
            this.page3.Location = new System.Drawing.Point(547, 494);
            this.page3.Name = "page3";
            this.page3.Size = new System.Drawing.Size(24, 26);
            this.page3.TabIndex = 36;
            this.page3.Text = "3";
            this.page3.Click += new System.EventHandler(this.page3_Click);
            // 
            // page4
            // 
            this.page4.AutoSize = true;
            this.page4.Location = new System.Drawing.Point(577, 494);
            this.page4.Name = "page4";
            this.page4.Size = new System.Drawing.Size(24, 26);
            this.page4.TabIndex = 37;
            this.page4.Text = "4";
            this.page4.Click += new System.EventHandler(this.page4_Click);
            // 
            // number_of_pages
            // 
            this.number_of_pages.AutoSize = true;
            this.number_of_pages.Location = new System.Drawing.Point(705, 494);
            this.number_of_pages.Name = "number_of_pages";
            this.number_of_pages.Size = new System.Drawing.Size(65, 26);
            this.number_of_pages.TabIndex = 38;
            this.number_of_pages.Text = "label6";
            this.number_of_pages.Click += new System.EventHandler(this.number_of_pages_Click);
            // 
            // search
            // 
            this.search.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(204)))), ((int)(((byte)(153)))));
            this.search.Location = new System.Drawing.Point(15, 14);
            this.search.MaxLength = 20;
            this.search.Name = "search";
            this.search.Size = new System.Drawing.Size(512, 34);
            this.search.TabIndex = 39;
            this.search.TextChanged += new System.EventHandler(this.search_TextChanged);
            // 
            // page5
            // 
            this.page5.AutoSize = true;
            this.page5.Location = new System.Drawing.Point(607, 494);
            this.page5.Name = "page5";
            this.page5.Size = new System.Drawing.Size(24, 26);
            this.page5.TabIndex = 40;
            this.page5.Text = "5";
            this.page5.Click += new System.EventHandler(this.page5_Click);
            // 
            // pointer_arrow1
            // 
            this.pointer_arrow1.Image = global::Cybersport.Properties.Resources.left;
            this.pointer_arrow1.Location = new System.Drawing.Point(422, 484);
            this.pointer_arrow1.Name = "pointer_arrow1";
            this.pointer_arrow1.Size = new System.Drawing.Size(62, 49);
            this.pointer_arrow1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pointer_arrow1.TabIndex = 33;
            this.pointer_arrow1.TabStop = false;
            this.pointer_arrow1.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // pointer_arrow2
            // 
            this.pointer_arrow2.Image = global::Cybersport.Properties.Resources.png_transparent_arrow_direction_pointer_right_arrow_icon;
            this.pointer_arrow2.Location = new System.Drawing.Point(637, 484);
            this.pointer_arrow2.Name = "pointer_arrow2";
            this.pointer_arrow2.Size = new System.Drawing.Size(62, 49);
            this.pointer_arrow2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pointer_arrow2.TabIndex = 32;
            this.pointer_arrow2.TabStop = false;
            this.pointer_arrow2.Click += new System.EventHandler(this.pointer_arrow2_Click);
            // 
            // button3
            // 
            this.button3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(102)))), ((int)(((byte)(0)))));
            this.button3.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button3.Location = new System.Drawing.Point(15, 518);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(243, 36);
            this.button3.TabIndex = 41;
            this.button3.Text = "Назад";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Users
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 26F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1364, 566);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.page5);
            this.Controls.Add(this.search);
            this.Controls.Add(this.number_of_pages);
            this.Controls.Add(this.page4);
            this.Controls.Add(this.page3);
            this.Controls.Add(this.page2);
            this.Controls.Add(this.page1);
            this.Controls.Add(this.pointer_arrow1);
            this.Controls.Add(this.pointer_arrow2);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.dataGridView1);
            this.Font = new System.Drawing.Font("Comic Sans MS", 14.25F);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "Users";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Список пользователей";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Users_FormClosing);
            this.Load += new System.EventHandler(this.Users_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pointer_arrow1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pointer_arrow2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.PictureBox pointer_arrow2;
        private System.Windows.Forms.PictureBox pointer_arrow1;
        private System.Windows.Forms.Label page1;
        private System.Windows.Forms.Label page2;
        private System.Windows.Forms.Label page3;
        private System.Windows.Forms.Label page4;
        private System.Windows.Forms.Label number_of_pages;
        private System.Windows.Forms.TextBox search;
        private System.Windows.Forms.Label page5;
        private System.Windows.Forms.Button button3;
    }
}