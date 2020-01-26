namespace ExpressionEval.TestApp
{
    partial class MainForm
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
            this.FunctionBody = new System.Windows.Forms.TextBox();
            this.SaveButton = new System.Windows.Forms.Button();
            this.ElapsedTime = new System.Windows.Forms.Label();
            this.LoopButton = new System.Windows.Forms.Button();
            this.ReturnTypeCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.functionList = new System.Windows.Forms.ListView();
            this.FunctionName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NumberOfParams = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ReturnType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.edit = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.funcName = new System.Windows.Forms.TextBox();
            this.ResultLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.possibleParams = new System.Windows.Forms.ListView();
            this.ParamType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ParamName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ParamValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Param_name = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.Param_type = new System.Windows.Forms.ComboBox();
            this.add_param = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // FunctionBody
            // 
            this.FunctionBody.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FunctionBody.Location = new System.Drawing.Point(19, 111);
            this.FunctionBody.Multiline = true;
            this.FunctionBody.Name = "FunctionBody";
            this.FunctionBody.Size = new System.Drawing.Size(406, 100);
            this.FunctionBody.TabIndex = 0;
            // 
            // SaveButton
            // 
            this.SaveButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.SaveButton.Location = new System.Drawing.Point(431, 150);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 1;
            this.SaveButton.Text = "SAVE->";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // ElapsedTime
            // 
            this.ElapsedTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ElapsedTime.Location = new System.Drawing.Point(544, 243);
            this.ElapsedTime.Name = "ElapsedTime";
            this.ElapsedTime.Size = new System.Drawing.Size(123, 23);
            this.ElapsedTime.TabIndex = 3;
            this.ElapsedTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LoopButton
            // 
            this.LoopButton.Location = new System.Drawing.Point(348, 243);
            this.LoopButton.Name = "LoopButton";
            this.LoopButton.Size = new System.Drawing.Size(75, 23);
            this.LoopButton.TabIndex = 4;
            this.LoopButton.Text = "Loop";
            this.LoopButton.UseVisualStyleBackColor = true;
           // this.LoopButton.Click += new System.EventHandler(this.LoopButton_Click);
            // 
            // ReturnTypeCombo
            // 
            this.ReturnTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ReturnTypeCombo.FormattingEnabled = true;
            this.ReturnTypeCombo.Location = new System.Drawing.Point(102, 217);
            this.ReturnTypeCombo.Name = "ReturnTypeCombo";
            this.ReturnTypeCombo.Size = new System.Drawing.Size(209, 21);
            this.ReturnTypeCombo.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(531, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Possible functions";
            // 
            // functionList
            // 
            this.functionList.AccessibleName = "functionList";
            this.functionList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.FunctionName,
            this.NumberOfParams,
            this.ReturnType});
            this.functionList.Location = new System.Drawing.Point(512, 40);
            this.functionList.MultiSelect = false;
            this.functionList.Name = "functionList";
            this.functionList.Size = new System.Drawing.Size(177, 186);
            this.functionList.TabIndex = 8;
            this.functionList.UseCompatibleStateImageBehavior = false;
            // 
            // FunctionName
            // 
            this.FunctionName.Text = "FunctionName";
            // 
            // NumberOfParams
            // 
            this.NumberOfParams.Text = "NumberOfParams";
            // 
            // ReturnType
            // 
            this.ReturnType.Text = "ReturnType";
            // 
            // edit
            // 
            this.edit.Location = new System.Drawing.Point(431, 121);
            this.edit.Name = "edit";
            this.edit.Size = new System.Drawing.Size(75, 23);
            this.edit.TabIndex = 9;
            this.edit.Text = "<- EDIT";
            this.edit.UseVisualStyleBackColor = true;
            this.edit.MouseClick += new System.Windows.Forms.MouseEventHandler(this.edit_MouseClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Function name:";
            // 
            // funcName
            // 
            this.funcName.Location = new System.Drawing.Point(102, 14);
            this.funcName.Name = "funcName";
            this.funcName.Size = new System.Drawing.Size(100, 20);
            this.funcName.TabIndex = 11;
            // 
            // ResultLabel
            // 
            this.ResultLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ResultLabel.Location = new System.Drawing.Point(14, 292);
            this.ResultLabel.Name = "ResultLabel";
            this.ResultLabel.Size = new System.Drawing.Size(707, 165);
            this.ResultLabel.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 225);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Return type:";
            // 
            // possibleParams
            // 
            this.possibleParams.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ParamType,
            this.ParamName,
            this.ParamValue});
            this.possibleParams.Location = new System.Drawing.Point(295, 30);
            this.possibleParams.Name = "possibleParams";
            this.possibleParams.Size = new System.Drawing.Size(211, 75);
            this.possibleParams.TabIndex = 13;
            this.possibleParams.UseCompatibleStateImageBehavior = false;
            // 
            // ParamType
            // 
            this.ParamType.Text = "ParamType";
            // 
            // ParamName
            // 
            this.ParamName.Text = "ParamName";
            // 
            // ParamValue
            // 
            this.ParamValue.Text = "ParamValue";
            // 
            // Param_name
            // 
            this.Param_name.Location = new System.Drawing.Point(87, 51);
            this.Param_name.Name = "Param_name";
            this.Param_name.Size = new System.Drawing.Size(100, 20);
            this.Param_name.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Param name:";
            // 
            // Param_type
            // 
            this.Param_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Param_type.FormattingEnabled = true;
            this.Param_type.Location = new System.Drawing.Point(87, 76);
            this.Param_type.Name = "Param_type";
            this.Param_type.Size = new System.Drawing.Size(100, 21);
            this.Param_type.TabIndex = 16;
            // 
            // add_param
            // 
            this.add_param.Location = new System.Drawing.Point(214, 49);
            this.add_param.Name = "add_param";
            this.add_param.Size = new System.Drawing.Size(75, 23);
            this.add_param.TabIndex = 18;
            this.add_param.Text = "ADD->";
            this.add_param.UseVisualStyleBackColor = true;
            this.add_param.Click += new System.EventHandler(this.Add_param_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(214, 76);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 21);
            this.button3.TabIndex = 19;
            this.button3.Text = "REMOVE";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(362, 14);
            this.label6.Name = "label6";
            this.label6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label6.Size = new System.Drawing.Size(83, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Possible params";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 266);
            this.label7.Name = "label7";
            this.label7.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Errors:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Param type:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(442, 248);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(86, 13);
            this.label8.TabIndex = 22;
            this.label8.Text = "Compilation time:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(732, 466);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.add_param);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Param_type);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Param_name);
            this.Controls.Add(this.possibleParams);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.funcName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.edit);
            this.Controls.Add(this.functionList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ReturnTypeCombo);
            this.Controls.Add(this.LoopButton);
            this.Controls.Add(this.ElapsedTime);
            this.Controls.Add(this.ResultLabel);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.FunctionBody);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Expression Evaluation";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox FunctionBody;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Label ElapsedTime;
        private System.Windows.Forms.Button LoopButton;
        private System.Windows.Forms.ComboBox ReturnTypeCombo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView functionList;
        private System.Windows.Forms.Button edit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox funcName;
        private System.Windows.Forms.Label ResultLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView possibleParams;
        private System.Windows.Forms.TextBox Param_name;
        private System.Windows.Forms.Label label4;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ComboBox Param_type;
        private System.Windows.Forms.Button add_param;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ColumnHeader FunctionName;
        private System.Windows.Forms.ColumnHeader NumberOfParams;
        private System.Windows.Forms.ColumnHeader ReturnType;
        private System.Windows.Forms.ColumnHeader ParamType;
        private System.Windows.Forms.ColumnHeader ParamName;
        private System.Windows.Forms.ColumnHeader ParamValue;
    }
}

