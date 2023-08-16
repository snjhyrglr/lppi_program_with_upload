Imports MySql.Data.MySqlClient
Public Class frmGYRTAdmin
    Sub listCov()

        lvListBatch.Items.Clear()
        konek()
        qry = "select * from tbl_gyrt_batch"
        mycmd = New MySqlCommand(qry, Myconn)
        mydr = mycmd.ExecuteReader

        While mydr.Read
            Dim ListViewItem As ListViewItem = New ListViewItem(New String() {mydr(0), mydr(1), mydr(2), mydr(3), mydr(4), mydr(5)})
            lvListBatch.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem})
        End While

        mydr = Nothing
        Myconn.Close()

    End Sub
    Sub clear()
        txtCode.Clear()
        txtDescription.Clear()
        txtEffect.Clear()
        txtExpiry.Clear()
        cbStatus.Text = ""
    End Sub
    Private Sub frmGYRTAdmin_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        listCov()
    End Sub

    Private Sub lvListBatch_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvListBatch.DoubleClick
        If lvListBatch.Items.Count > 0 Then
            txtCode.Text = lvListBatch.Items(lvListBatch.FocusedItem.Index).SubItems(1).Text
            txtDescription.Text = lvListBatch.Items(lvListBatch.FocusedItem.Index).SubItems(2).Text
            txtEffect.Text = lvListBatch.Items(lvListBatch.FocusedItem.Index).SubItems(3).Text
            txtExpiry.Text = lvListBatch.Items(lvListBatch.FocusedItem.Index).SubItems(4).Text
            cbStatus.Text = lvListBatch.Items(lvListBatch.FocusedItem.Index).SubItems(5).Text
        End If
    End Sub

    Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        Try
            If Not txtCode.Text = Nothing Or cbStatus.Text = Nothing Then
                konek()
                qry = "UPDATE tbl_gyrt_batch SET BATCH_DESCRIPTION = @desc, BATCH_STATUS = @stat WHERE BATCH_CODE = @code"
                mycmd = New MySqlCommand(qry, Myconn)
                mycmd.Parameters.AddWithValue("@desc", txtDescription.Text)
                mycmd.Parameters.AddWithValue("@stat", cbStatus.Text)
                mycmd.Parameters.AddWithValue("@code", txtCode.Text)
                mycmd.ExecuteNonQuery()

                MessageBox.Show("Batch Updated!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                listCov()
                clear()
            Else
                MessageBox.Show("Batch Code and Status are required fields.", "", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub frmGYRTAdmin_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        Try
            qry = "SELECT * FROM tbl_gyrt_batch WHERE BATCH_CODE = @code"
            mycmd = New MySqlCommand(qry, Myconn)
            mycmd.Parameters.AddWithValue("@code", frmGYRT.txtCode.Text)
            mydr = mycmd.ExecuteReader

            If mydr.Read Then
                frmGYRT.txtCode.Text = mydr("BATCH_CODE")
                frmGYRT.txtDesc.Text = mydr("BATCH_DESCRIPTION")
                frmGYRT.txtStatus.Text = mydr("BATCH_STATUS")
                frmGYRT.txtEffectivity.Text = mydr("DATE")
                frmGYRT.txtExpiry.Text = mydr("EXPIRY")
                frmGYRT.txtToday.Text = Date.Now.ToString("MM/dd/yyyy")
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class