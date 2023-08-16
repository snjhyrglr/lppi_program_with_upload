Imports MySql.Data.MySqlClient
Public Class frmGYRTNew

    Sub clear()
        txtLname.Clear()
        txtFname.Clear()
        txtBday.Clear()
        txtMem.Clear()
        txtMI.Clear()
        txtSuf.Clear()
        cbGender.Text = ""
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            konek()
            qry = "SELECT * FROM tbl_member_info WHERE L_NAME = @lname and F_NAME = @fname and BDAY = @bday"
            mycmd = New MySqlCommand(qry, Myconn)
            mycmd.Parameters.AddWithValue("@lname", txtLname.Text)
            mycmd.Parameters.AddWithValue("@fname", txtFname.Text)
            mycmd.Parameters.AddWithValue("@bday", txtBday.Text)
            mydr = mycmd.ExecuteReader

            If Not mydr.Read Then
                konek()
                qry = "SELECT count(*) FROM tbl_member_info"
                mycmd = New MySqlCommand(qry, Myconn)
                mydr = mycmd.ExecuteReader

                While mydr.Read
                    Dim va As Integer = mydr(0) + 1
                    txtMem.Text = "LPPI-" & va.ToString("00000")
                End While

                konek()
                qry = "INSERT INTO tbl_member_info (MEMBER_CODE,L_NAME,F_NAME,M_INITIAL,SUFFIX,BDAY,GENDER) values (@memcode,@last,@first,@mi@suf,@bday,@gen)"
                mycmd = New MySqlCommand(qry, Myconn)
                mycmd.Parameters.AddWithValue("@memcode", txtMem.Text)
                mycmd.Parameters.AddWithValue("@last", txtLname.Text)
                mycmd.Parameters.AddWithValue("@first", txtFname.Text)
                mycmd.Parameters.AddWithValue("@mi", txtMI.Text)
                mycmd.Parameters.AddWithValue("@suf", txtSuf.Text)
                mycmd.Parameters.AddWithValue("@bday", txtBday.Text)
                mycmd.Parameters.AddWithValue("@gen", cbGender.Text)
                mycmd.ExecuteNonQuery()

                MessageBox.Show("New Member", "", MessageBoxButtons.OK, MessageBoxIcon.Information)

                frmGYRT.listMembers()
                clear()
            Else
                txtMem.Text = mydr("MEMBER_CODE")
                MessageBox.Show("Member Already Encoded", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                clear()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
End Class