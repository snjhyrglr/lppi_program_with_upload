Imports MySql.Data.MySqlClient
Public Class frmGBLISSBatch
    Public expD As String
    Public b_code As String
    Sub listCov()

        lvCov.Items.Clear()
        konek()
        qry = "select a.*,count(b.batch) as 'COUNT' from tbl_gbliss_batch a left join tbl_info b on a.BATCH_CODE = b.BATCH group by a.BATCH_CODE"
        mycmd = New MySqlCommand(qry, Myconn)
        mydr = mycmd.ExecuteReader

        While mydr.Read
            Dim ListViewItem As ListViewItem = New ListViewItem(New String() {mydr("BATCH_CODE"), mydr("BATCH_DESCRIPTION"), mydr("DATE"), mydr("EXPIRY"), mydr("BATCH_STATUS")})
            lvCov.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem})
        End While

        mydr = Nothing
        Myconn.Close()

    End Sub
    Sub listMembers()
        frmGYRT.lvMembers.Items.Clear()

        konek()
        qry = "select a.batch,a.member_code,b.L_NAME,b.F_NAME,b.M_INITIAL,b.SUFFIX,b.BDAY,b.GENDER,a.age,a.effectivity,a.expiry,a.premium from tbl_gbliss a " & _
        "left join tbl_member_info b on a.member_code = b.MEMBER_CODE where a.batch = '" & lvCov.Items(lvCov.FocusedItem.Index).SubItems(0).Text & "'"
        mycmd = New MySqlCommand(qry, Myconn)
        mydr = mycmd.ExecuteReader

        While mydr.Read
            Dim lv As ListViewItem = New ListViewItem(New String() {mydr(1), mydr(2), mydr(3), mydr(4), mydr(5), mydr(6), mydr(7), mydr(8), mydr(9), mydr(10), mydr(11)})
            frmGBLISS.lvInsuredMem.Items.AddRange(New System.Windows.Forms.ListViewItem() {lv})
        End While

        mydr = Nothing
        Myconn.Close()
    End Sub
    Sub expiryDate()
        Dim eff As String = txtDate.Text
        Dim exp As Date

        Dim x As DateTime = eff

        exp = x.AddYears(1)
        expD = exp.ToString("MM/dd/yyyy")
    End Sub

    Sub newCode()
        Dim x As DateTime = Date.Today
        Dim y = x.ToString("MMMM")
        Dim codex As String = y.Substring(0, 3)
        Dim yy = x.ToString("yyyy")

        Dim code As String = UCase(codex) & "-" & yy

        konek()
        qry = "SELECT COUNT(*) FROM tbl_gbliss_batch where BATCH_CODE like '%" & UCase(codex) & "%'"
        mycmd = New MySqlCommand(qry, Myconn)
        mydr = mycmd.ExecuteReader

        While mydr.Read
            b_code = code & "-" & "GBLISS-" & mydr(0) + 1
        End While
        txtBatchCode.Text = b_code
    End Sub
    Private Sub frmGBLISSBatch_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        listCov()
        newCode()
        txtDate.Text = Date.Now.ToString("MM/dd/yyyy")
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            konek()
            qry = "SELECT * FROM tbl_gbliss_batch where BATCH_CODE = '" & txtBatchCode.Text & "'"
            mycmd = New MySqlCommand(qry, Myconn)
            mydr = mycmd.ExecuteReader

            If mydr.Read = True Then
                MessageBox.Show("Batch Code Already Exist.")
                txtBatchCode.Clear()
                txtDescription.Clear()
                txtDate.Clear()
                txtBatchCode.Focus()

            Else
                konek()
                expiryDate()
                qry = "INSERT INTO tbl_gbliss_batch VALUES ('','" & UCase(txtBatchCode.Text) & "','" & UCase(txtDescription.Text) & "'," & _
                "'" & txtDate.Text & "','" & expD & "','ACTIVE')"
                mycmd = New MySqlCommand(qry, Myconn)
                mycmd.ExecuteNonQuery()

                MessageBox.Show("Batch Information Successfully Added!")

                konek()
                qry = "SELECT * FROM tbl_gbliss_batch where BATCH_CODE = '" & txtBatchCode.Text & "'"
                mycmd = New MySqlCommand(qry, Myconn)
                mydr = mycmd.ExecuteReader

                If mydr.Read = True Then
                    frmGBLISS.txtCode.Text = mydr(1)
                    frmGBLISS.txtDesc.Text = mydr(2)
                    frmGBLISS.txtStatus.Text = mydr(5)
                    frmGBLISS.txtEffectivity.Text = mydr(3)
                    frmGBLISS.txtExpiry.Text = mydr(4)
                    'Me.Hide()
                    Me.Close()
                End If
            End If
        Catch ex As Exception
            MsgBox("error:" & vbCrLf & vbCrLf & ex.Message)
        End Try
    End Sub

    Private Sub lvCov_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvCov.DoubleClick
        Try
            konek()
            qry = "SELECT DATE FROM tbl_gbliss_batch where BATCH_CODE = '" & lvCov.Items(lvCov.FocusedItem.Index).SubItems(0).Text & "'"
            mycmd = New MySqlCommand(qry, Myconn)
            mydr = mycmd.ExecuteReader

            While mydr.Read
                Dim dateToday = Date.Now.ToString("MM/dd/yyyy")
                Dim bdate = mydr("DATE")
                If Not dateToday = bdate Then
                    frmGBLISS.txtEffectivity.Text = dateToday
                Else
                    frmGBLISS.txtEffectivity.Text = bdate
                End If
            End While
            If lvCov.Items.Count > 0 Then
                frmGBLISS.txtCode.Text = lvCov.Items(lvCov.FocusedItem.Index).SubItems(0).Text
                frmGBLISS.txtDesc.Text = lvCov.Items(lvCov.FocusedItem.Index).SubItems(1).Text
                frmGBLISS.txtStatus.Text = lvCov.Items(lvCov.FocusedItem.Index).SubItems(4).Text
                frmGBLISS.txtExpiry.Text = lvCov.Items(lvCov.FocusedItem.Index).SubItems(3).Text

                frmGBLISS.lvInsuredMem.Items.Clear()
                listMembers()
                frmGBLISS.listMembers()

                Dim tot, total As Double
                For Each lvitem As ListViewItem In frmGYRT.lvInsuredMem.Items
                    If Double.TryParse(lvitem.SubItems(10).Text, tot) Then
                        total += tot
                    End If
                Next
                frmGBLISS.txtTotal.Text = total

                konek()
                qry = "select * from tbl_gbliss where batch = '" & lvCov.Items(lvCov.FocusedItem.Index).SubItems(0).Text & "'"
                mycmd = New MySqlCommand(qry, Myconn)
                mydr = mycmd.ExecuteReader

                While mydr.Read
                    For Each listitem As ListViewItem In frmGYRT.lvMembers.Items
                        If frmGBLISS.lvMembers.Items(0).Text = mydr("member_code") Then
                            listitem.Remove()
                        End If
                    Next

                End While
            End If
            Me.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
End Class