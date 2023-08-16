Imports MySql.Data.MySqlClient
Public Class frmLogin
    Dim ctr As Integer = 3
    Dim pass As String
    Dim forAdminPass As String
    Sub newLogin()
        Try
            If ctr > 0 Then
                konek()
                qry = "SELECT * FROM tbl_users where username = '" & txtUsername.Text & "'"
                mycmd = New MySqlCommand(qry, Myconn)
                mydr = mycmd.ExecuteReader

                If mydr.Read Then
                    If mydr("account_lock") = 0 Then
                        pass = mydr("password")
                        If pass = txtPassword.Text Then
                            forName = mydr("name")
                            forUsername = mydr("username")

                            MessageBox.Show("Welcome " & forName, "", MessageBoxButtons.OK, MessageBoxIcon.Information)

                            user = forUsername
                            act = "LOGIN"
                            trail()

                            'frmMainMenu.Show()
                            Form1.Show()
                            Me.Close()
                        Else
                            ctr -= 1
                            MessageBox.Show("Incorrect Password.", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            txtPassword.Clear()
                            txtPassword.Focus()
                        End If
                    Else
                        MessageBox.Show("Account Locked. Please contact IT Admin.", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
                    End If
                    
                Else
                    MessageBox.Show("Incorrect Username.", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Else
                konek()
                qry = "UPDATE tbl_users SET account_lock = 1 WHERE username = '" & txtUsername.Text & "'"
                mycmd = New MySqlCommand(qry, Myconn)
                mycmd.ExecuteNonQuery()

                ctr = 3
                MessageBox.Show("Account Locked. Please contact IT Admin.", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Sub newLogin2()
        Try
            Dim xLock As Integer
            If ctr > 0 Then
                konek()
                qry = "SELECT * FROM tbl_users WHERE username = '" & txtUsername.Text & "' AND password = '" & txtPassword.Text & "'"
                mycmd = New MySqlCommand(qry, Myconn)
                mydr = mycmd.ExecuteReader

                If mydr.Read = True Then
                    xLock = mydr("account_lock")
                    forName = mydr("name")
                    forUsername = mydr("username")
                    If xLock = 1 Then
                        MessageBox.Show("Account is Lock. Somebody call 9-1-1", "", MessageBoxButtons.OK)
                        txtUsername.Clear()
                        txtPassword.Clear()
                        Exit Sub
                    Else
                        konek()
                        qry = "UPDATE tbl_users SET account_lock = 0 WHERE username = '" & txtUsername.Text & "'"
                        mycmd = New MySqlCommand(qry, Myconn)
                        mycmd.ExecuteNonQuery()

                        MessageBox.Show("Welcome " & forName, "", MessageBoxButtons.OK, MessageBoxIcon.Information)

                        user = forUsername
                        act = "LOGIN"
                        tran_id = "User Login"
                        trail()

                        'frmMainMenu.Show()
                        Form1.Show()
                        Me.Close()
                    End If
                Else
                    txtUsername.Clear()
                    txtPassword.Clear()
                    MessageBox.Show("Incorrect Username.", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    ctr -= 1
                    txtUsername.Clear()
                    txtPassword.Clear()
                End If
            Else
                konek()
                qry = "UPDATE tbl_users SET account_lock = 0 WHERE username = '" & txtUsername.Text & "'"
                mycmd = New MySqlCommand(qry, Myconn)
                mycmd.ExecuteNonQuery()

                ctr = 3
                MessageBox.Show("Account Locked. Please contact IT Admin.", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        frmRegister.Show()
    End Sub

    Private Sub btnLogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        newLogin2()
    End Sub

    Private Sub frmLogin_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If (e.KeyCode = Keys.F AndAlso e.Modifiers = Keys.Control) Then
            forAdminPass = InputBox("Please input Admin Password", "Input Password")

            If Not forAdminPass = Nothing Then
                If forAdminPass = "1" Then
                    MessageBox.Show("Welcome Admin", "", MessageBoxButtons.OK)

                    frmAdmin.Show()
                    Me.Close()
                Else
                    MessageBox.Show("Incorrect Admin Password", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Else
                Exit Sub
            End If
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Application.Exit()
    End Sub
End Class