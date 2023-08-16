Imports MySql.Data.MySqlClient
Public Class frmRegister

    Sub clearAll()
        txtName.Clear()
        txtUsername.Clear()
        txtPassword.Clear()
        txtRePassword.Clear()
        txtName.Focus()
    End Sub
    Sub registerNew()
        Try
            konek()
            qry = "SELECT username FROM tbl_users WHERE username = '" & txtUsername.Text & "'"
            mycmd = New MySqlCommand(qry, Myconn)
            mydr = mycmd.ExecuteReader

            If Not mydr.Read Then
                konek()
                qry = "INSERT INTO tbl_users (username,password,name) VALUES (@username,@password,@name)"
                mycmd = New MySqlCommand(qry, Myconn)
                mycmd.Parameters.AddWithValue("@username", txtUsername.Text)
                mycmd.Parameters.AddWithValue("@password", txtPassword.Text)
                mycmd.Parameters.AddWithValue("@name", txtName.Text)
                mycmd.ExecuteNonQuery()

                'use account
                Dim ans = MessageBox.Show("Account Created. Do you want to use the created Account?", "1CISP", MessageBoxButtons.OKCancel)
                If ans = Windows.Forms.DialogResult.OK Then
                    forUsername = txtUsername.Text
                    forName = txtName.Text
                    clearAll()
                    Form1.Show()
                    Me.Close()
                Else
                    clearAll()
                    frmLogin.Show()
                    Me.Close()
                End If

            Else
                MessageBox.Show("Username already registered.", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                clearAll()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Private Sub btnRegister_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRegister.Click
        If txtPassword.Text = txtRePassword.Text Then
            registerNew()
        Else
            MessageBox.Show("Password not match.", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtPassword.Clear()
            txtRePassword.Clear()
            txtPassword.Focus()
        End If
    End Sub
End Class