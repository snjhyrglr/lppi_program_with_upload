Imports MySql.Data.MySqlClient
Public Class frmMainMenu

    Private Sub btnLPPI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLPPI.Click
        Form1.Show()
        Me.Hide()
    End Sub

    Private Sub btnGYRT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGYRT.Click
        frmGYRT.Show()
        Me.Hide()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        frmGBLISS.Show()
        Me.Hide()
    End Sub

    Private Sub btnLogout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogout.Click
        Try
            Dim result As Integer = MessageBox.Show("Do you want to logout?", "", MessageBoxButtons.YesNo)
            If result = DialogResult.Yes Then
                konek()
                qry = "UPDATE tbl_users SET account_lock = 0 WHERE username = '" & forUsername & "'"
                mycmd = New MySqlCommand(qry, Myconn)
                mycmd.ExecuteNonQuery()

                MessageBox.Show("Logout Successful")

                act = "LOGOUT"
                tran_id = "User Logout"
                trail()

                Application.Exit()
            Else

            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
End Class