<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMainMenu
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMainMenu))
        Me.btnGYRT = New System.Windows.Forms.Button
        Me.btnLPPI = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnLogout = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'btnGYRT
        '
        Me.btnGYRT.BackColor = System.Drawing.Color.LemonChiffon
        Me.btnGYRT.BackgroundImage = CType(resources.GetObject("btnGYRT.BackgroundImage"), System.Drawing.Image)
        Me.btnGYRT.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnGYRT.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGYRT.Location = New System.Drawing.Point(317, 59)
        Me.btnGYRT.Name = "btnGYRT"
        Me.btnGYRT.Size = New System.Drawing.Size(110, 69)
        Me.btnGYRT.TabIndex = 3
        Me.btnGYRT.Text = "GYRT"
        Me.btnGYRT.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGYRT.UseVisualStyleBackColor = False
        '
        'btnLPPI
        '
        Me.btnLPPI.BackColor = System.Drawing.Color.LemonChiffon
        Me.btnLPPI.BackgroundImage = CType(resources.GetObject("btnLPPI.BackgroundImage"), System.Drawing.Image)
        Me.btnLPPI.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnLPPI.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLPPI.Location = New System.Drawing.Point(85, 59)
        Me.btnLPPI.Name = "btnLPPI"
        Me.btnLPPI.Size = New System.Drawing.Size(104, 69)
        Me.btnLPPI.TabIndex = 2
        Me.btnLPPI.Text = "LPPI"
        Me.btnLPPI.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnLPPI.UseVisualStyleBackColor = False
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.LemonChiffon
        Me.Button2.BackgroundImage = CType(resources.GetObject("Button2.BackgroundImage"), System.Drawing.Image)
        Me.Button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.Button2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.Location = New System.Drawing.Point(195, 59)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(116, 69)
        Me.Button2.TabIndex = 4
        Me.Button2.Text = "GBLISS"
        Me.Button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button2.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Modern No. 20", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(151, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(206, 29)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "SELECT PLAN"
        '
        'btnLogout
        '
        Me.btnLogout.BackColor = System.Drawing.Color.Transparent
        Me.btnLogout.FlatAppearance.BorderSize = 0
        Me.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLogout.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, CType(((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic) _
                        Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLogout.ForeColor = System.Drawing.Color.Transparent
        Me.btnLogout.Location = New System.Drawing.Point(385, 156)
        Me.btnLogout.Name = "btnLogout"
        Me.btnLogout.Size = New System.Drawing.Size(121, 42)
        Me.btnLogout.TabIndex = 31
        Me.btnLogout.Text = "Logout"
        Me.btnLogout.UseVisualStyleBackColor = False
        '
        'frmMainMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(510, 202)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnLogout)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.btnGYRT)
        Me.Controls.Add(Me.btnLPPI)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMainMenu"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnGYRT As System.Windows.Forms.Button
    Friend WithEvents btnLPPI As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnLogout As System.Windows.Forms.Button
End Class
