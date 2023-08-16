Imports MySql.Data.MySqlClient
Public Class frmAdminPass

    Sub selPassword()
        Try
            konek()
            qry = "SELECT password FROM tbl_users WHERE username = 1"
            mycmd = New MySqlCommand(qry, Myconn)
            mydr = mycmd.ExecuteReader

            If mydr.Read Then
                If TextBox1.Text = mydr(0) Then
                    forUpdateBatch = 1
                    MessageBox.Show("Password Match.", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me.Close()
                Else
                    MessageBox.Show("Password Not Match.", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Else
                MessageBox.Show("Enter Admin Account.", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub TextBox1_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyDown
        If (e.KeyCode = Keys.Enter) Then
            selPassword()
        End If
    End Sub
End Class