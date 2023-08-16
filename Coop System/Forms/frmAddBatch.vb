Imports MySql.Data.MySqlClient
Public Class frmAddBatch

    Dim b_code As String
    Sub listAllBatch()
        lvListBatch.Items.Clear()
        Dim ctr As Integer = 0

        konek()
        qry = "select * from tbl_batch"
        mycmd = New MySqlCommand(qry, Myconn)
        mydr = mycmd.ExecuteReader

        While mydr.Read
            ctr += 1
            Dim ListViewItem As ListViewItem = New ListViewItem(New String() {ctr, mydr(1), mydr(2), mydr(3), mydr(4)})
            lvListBatch.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem})
        End While
    End Sub
    Sub newCode()
        Dim x As DateTime = Date.Today
        Dim y = x.ToString("MMMM")
        Dim codex As String = y.Substring(0, 3)
        Dim yy = x.ToString("yyyy")
        Dim codey As String = yy.Replace("0", "")

        Dim code As String = UCase(codex) & "-" & codey

        konek()
        qry = "SELECT COUNT(*) FROM tbl_batch where BATCH_CODE like '%" & UCase(codex) & "%'"
        mycmd = New MySqlCommand(qry, Myconn)
        mydr = mycmd.ExecuteReader

        While mydr.Read
            b_code = code & "-" & mydr(0) + 1
        End While
        txtCode.Text = b_code
    End Sub
    Sub clear()
        txtCode.Clear()
        txtBatchDate.Clear()
        txtDescription.Clear()
        cbStatus.Text = ""
        txtCode.ReadOnly = False
    End Sub
    Sub forEditDel()
        Form1.GroupMember.Enabled = True
        Form1.GroupCoverage.Enabled = True
        Form1.cbMemCode.Focus()
    End Sub
    Sub newLoad()
        btnAdd.Enabled = True
        btnEdit.Enabled = False
        btnDelete.Enabled = False

        txtBatchDate.Clear()
        txtDescription.Clear()
    End Sub

    Private Sub frmAddBatch_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        listAllBatch()
        newCode()
        newLoad()
    End Sub

    Private Sub lvListBatch_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvListBatch.DoubleClick
        If lvListBatch.Items.Count > 0 Then
            txtCode.Text = lvListBatch.Items(lvListBatch.FocusedItem.Index).SubItems(1).Text
            txtDescription.Text = lvListBatch.Items(lvListBatch.FocusedItem.Index).SubItems(2).Text
            txtBatchDate.Text = lvListBatch.Items(lvListBatch.FocusedItem.Index).SubItems(3).Text
            cbStatus.Text = lvListBatch.Items(lvListBatch.FocusedItem.Index).SubItems(4).Text

            txtCode.ReadOnly = True

            btnAdd.Enabled = False
            btnEdit.Enabled = True
            btnDelete.Enabled = True
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Not txtCode.Text = Nothing Then
            bCode = txtCode.Text
            bDesc = txtDescription.Text
            bDate = txtBatchDate.Text
            If Not cbStatus.Text = "LOCKED" Then
                Form1.txtCode.Text = txtCode.Text
                Form1.txtDescription.Text = txtDescription.Text
                Form1.txtDate.Text = txtBatchDate.Text
                Form1.lblStatus.Text = cbStatus.Text

                forEditDel()
                Me.Close()
            Else
                Form1.txtCode.Text = txtCode.Text
                Form1.txtDescription.Text = txtDescription.Text
                Form1.txtDate.Text = txtBatchDate.Text
                Form1.lblStatus.Text = cbStatus.Text
                Form1.GroupCoverage.Enabled = False
                Form1.GroupMember.Enabled = False
                Me.Close()
            End If
        Else
            MessageBox.Show("Please select Batch Code.", "1CISP", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            If Not txtCode.Text = Nothing Then
                konek()
                qry = "INSERT INTO tbl_batch (BATCH_CODE,BATCH_DESCRIPTION,DATE,BATCH_STATUS) values " & _
                "(@code,@desc,@date,@stat)"
                mycmd = New MySqlCommand(qry, Myconn)
                mycmd.Parameters.AddWithValue("@code", txtCode.Text)
                mycmd.Parameters.AddWithValue("@desc", txtDescription.Text)
                mycmd.Parameters.AddWithValue("@date", txtBatchDate.Text)
                mycmd.Parameters.AddWithValue("@stat", cbStatus.Text)
                mycmd.ExecuteNonQuery()

                MessageBox.Show("Batch Added Successfully.", "1CISP", MessageBoxButtons.OK)
                clear()
                listAllBatch()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub frmAddBatch_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

    End Sub

    Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        Try
            If Not txtCode.Text = Nothing Then
                frmAdminPass.ShowDialog()
                If Not forUpdateBatch = 0 Then
                    konek()
                    qry = "UPDATE tbl_batch SET BATCH_CODE = @code, BATCH_DESCRIPTION = @desc, DATE = @date, BATCH_STATUS = @stat WHERE BATCH_CODE = @code"
                    mycmd = New MySqlCommand(qry, Myconn)
                    mycmd.Parameters.AddWithValue("@code", txtCode.Text)
                    mycmd.Parameters.AddWithValue("@desc", txtDescription.Text)
                    mycmd.Parameters.AddWithValue("@date", txtBatchDate.Text)
                    mycmd.Parameters.AddWithValue("@stat", cbStatus.Text)
                    mycmd.ExecuteNonQuery()

                    MessageBox.Show("Batch Updated Successfully.", "1CISP", MessageBoxButtons.OK)
                    clear()
                    listAllBatch()
                    forUpdateBatch = 0
                Else
                    MessageBox.Show("Need Admin Password to Update.", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
                    clear()
                End If

            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Try
            If Not txtCode.Text = Nothing Then
                frmAdminPass.ShowDialog()
                If Not forUpdateBatch = 0 Then
                    konek()
                    qry = "DELETE FROM tbl_batch WHERE BATCH_CODE = @code"
                    mycmd = New MySqlCommand(qry, Myconn)
                    mycmd.Parameters.AddWithValue("@code", txtCode.Text)

                    MessageBox.Show("Batch Deleted Successfully.", "1CISP", MessageBoxButtons.OK)
                    clear()
                    listAllBatch()
                    forUpdateBatch = 0
                Else
                    MessageBox.Show("Need Admin Password to Update.", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
                    clear()
                End If

            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
End Class