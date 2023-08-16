Imports MySql.Data.MySqlClient
Public Class frmAdmin
    Dim forIdLPPI As String

    'Actions
    Sub showAllActions()
        Dim ctr As Integer = 0

        konek()
        qry = "SELECT a.username,b.name,a.tran_code,a.action,a.dt_action FROM tbl_trail a LEFT JOIN tbl_users b on a.username = b.username"
        mycmd = New MySqlCommand(qry, Myconn)
        mydr = mycmd.ExecuteReader

        While mydr.Read
            ctr += 1
            Dim listviewitem As ListViewItem = New ListViewItem(New String() {ctr, mydr(0), mydr(1), mydr(2), mydr(3), mydr(4)})
            lvActions.Items.AddRange(New System.Windows.Forms.ListViewItem() {listviewitem})
        End While
    End Sub
    Sub showByDate()
        lvActions.Items.Clear()

        Dim dtFrom As DateTime = dtActFrom.Text
        Dim xFrom As String = dtFrom.ToString("MM/dd/yyyy")
        Dim dtTo As DateTime = dtActTo.Text
        Dim xTo As String = dtTo.ToString("MM/dd/yyyy")

        Dim ctr As Integer = 0

        konek()
        qry = "SELECT a.username,b.name,a.tran_code,a.action,a.dt_action FROM tbl_trail a LEFT JOIN tbl_users b on a.username = b.username WHERE " & _
        "STR_TO_DATE(a.dt_action,'%m/%d/%Y') BETWEEN STR_TO_DATE('" & xFrom & "','%m/%d/%Y') AND STR_TO_DATE('" & xTo & "','%m/%d/%Y')"
        mycmd = New MySqlCommand(qry, Myconn)
        mydr = mycmd.ExecuteReader

        While mydr.Read
            ctr += 1
            Dim listviewitem As ListViewItem = New ListViewItem(New String() {ctr, mydr(0), mydr(1), mydr(2), mydr(3), mydr(4)})
            lvActions.Items.AddRange(New System.Windows.Forms.ListViewItem() {listviewitem})
        End While
    End Sub

    'Rate & Users
    Sub showAllUsers()
        lvUsers.Items.Clear()
        Dim ctr As Integer = 0
        Dim stat As String

        konek()
        qry = "SELECT * FROM tbl_users"
        mycmd = New MySqlCommand(qry, Myconn)
        mydr = mycmd.ExecuteReader

        While mydr.Read
            If mydr("account_lock") = 0 Then
                stat = "ACTIVE"
            Else
                stat = "LOCKED"
            End If
            ctr += 1
            Dim listviewitem As ListViewItem = New ListViewItem(New String() {ctr, mydr("name"), mydr("username"), stat})
            lvUsers.Items.AddRange(New System.Windows.Forms.ListViewItem() {listviewitem})
        End While
    End Sub

    Sub showRate()
        konek()
        qry = "SELECT * FROM tbl_lppi_rate"
        mycmd = New MySqlCommand(qry, Myconn)
        mydr = mycmd.ExecuteReader

        While mydr.Read
            txtLRate.Text = mydr("lppi_rate")
            txtLSF.Text = mydr("coop_comm")
        End While
    End Sub

    'LPPI
    Sub listAllBatch()
        Dim ctr As Integer = 0

        Try
            konek()
            qry = "SELECT * FROM tbl_batch"
            mycmd = New MySqlCommand(qry, Myconn)
            mydr = mycmd.ExecuteReader

            While mydr.Read
                ctr += 1
                Dim listviewitem As ListViewItem = New ListViewItem(New String() {ctr, mydr(1), mydr(2), mydr(3), mydr(4)})
                lvLBatch.Items.AddRange(New System.Windows.Forms.ListViewItem() {listviewitem})
            End While
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Sub listInsuredPerBatch()
        Dim ctr As Integer = 0
        Dim lamt, gross, net, comm, unu As Double

        lvLList.Items.Clear()
        Try
            konek()
            qry = "SELECT a.id,a.CERT_NO,a.MEM_CODE,b.L_NAME,b.F_NAME,b.M_INITIAL,b.SUFFIX,b.BDAY,a.AGE,b.GENDER,a.LOAN_AMT,c.loans,a.EFFECTIVITY," & _
        "a.EXPIRY,a.TERMS,a.GROSS,a.Unused,a.Commission,a.NET,a.LOAN_TYPE FROM tbl_info a LEFT JOIN tbl_member_info b ON a.MEM_CODE = b.MEMBER_CODE LEFT JOIN tbl_loans c " & _
        "on a.LOAN_TYPE=c.id WHERE BATCH = '" & forIdLPPI & "'"
            mycmd = New MySqlCommand(qry, Myconn)
            mydr = mycmd.ExecuteReader

            While mydr.Read
                ctr += 1
                lamt = mydr("LOAN_AMT")
                gross = mydr("GROSS")
                Net = mydr("NET")
                comm = mydr("Commission")
                unu = mydr("Unused")
                Dim fLoans As String

                If IsDBNull(mydr("loans")) Then
                    fLoans = mydr("LOAN_TYPE")
                Else
                    fLoans = mydr("loans")
                End If
                Dim ListViewItem As ListViewItem = New ListViewItem(New String() {ctr, mydr("id"), mydr("CERT_NO"), mydr("L_NAME"), mydr("F_NAME"), _
                                                                                  mydr("M_INITIAL"), mydr("SUFFIX"), mydr("BDAY"), mydr("AGE"), mydr("GENDER"), _
                                                                                  mydr("EFFECTIVITY"), mydr("EXPIRY"), mydr("TERMS"), fLoans, lamt.ToString("#,##0.00"), _
                                                                                  gross.ToString("#,##0.00"), unu.ToString("#,##0.00"), comm.ToString("#,##0.00"), net.ToString("#,##0.00")})
                lvLList.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem})


            End While

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    'GYRT/GBLISS Members Allowed
    Sub showGYRTAllowed()
        Try
            konek()
            qry = "SELECT gyrt_req_mem FROM tbl_gyrt_rate"
            mycmd = New MySqlCommand(qry, Myconn)
            mydr = mycmd.ExecuteReader

            While mydr.Read
                txtGYRTMem.Text = mydr("gyrt_req_mem")
            End While

            konek()
            qry = "SELECT gbliss_req_mem FROM tbl_gbliss_rate"
            mycmd = New MySqlCommand(qry, Myconn)
            mydr = mycmd.ExecuteReader

            While mydr.Read
                txtGblissMem.Text = mydr("gbliss_req_mem")
            End While
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub frmAdmin_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        showAllActions()
        showAllUsers()
        showRate()
        listAllBatch()
        showGYRTAllowed()
    End Sub

    Private Sub btnGo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGo.Click
        showByDate()
    End Sub

    Private Sub lvUsers_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvUsers.DoubleClick
        Try
            If lvUsers.Items.Count > 0 Then
                txtFullname.Text = lvUsers.Items(lvUsers.FocusedItem.Index).SubItems(1).Text
                txtUsername.Text = lvUsers.Items(lvUsers.FocusedItem.Index).SubItems(2).Text
                cbStatus.Text = lvUsers.Items(lvUsers.FocusedItem.Index).SubItems(3).Text

                lblUsername.Text = lvUsers.Items(lvUsers.FocusedItem.Index).SubItems(2).Text

                konek()
                qry = "SELECT password FROM tbl_users WHERE username = '" & lvUsers.Items(lvUsers.FocusedItem.Index).SubItems(2).Text & "'"
                mycmd = New MySqlCommand(qry, Myconn)
                mydr = mycmd.ExecuteReader
                If mydr.Read Then
                    txtPassword.Text = mydr(0)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub btnUpdateUsers_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdateUsers.Click
        Dim xStat As Integer
        Try
            If Not txtFullname.Text = Nothing Or Not txtUsername.Text = Nothing Or Not txtPassword.Text = Nothing Then
                If cbStatus.Text = "ACTIVE" Then
                    xStat = 0
                Else
                    xStat = 1
                End If

                konek()
                qry = "UPDATE tbl_users set name = '" & txtFullname.Text & "', username = '" & txtUsername.Text & "', " & _
                "password = '" & txtPassword.Text & "', account_lock = " & xStat & " where username = '" & lblUsername.Text & "'"
                mycmd = New MySqlCommand(qry, Myconn)
                mycmd.ExecuteNonQuery()

                MessageBox.Show("User Update.", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                showAllUsers()
                txtPassword.Clear()
                txtUsername.Clear()
                txtFullname.Clear()
            Else
                MessageBox.Show("Fill-up all fields.", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub frmAdmin_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If (e.KeyCode = Keys.E AndAlso e.Modifiers = Keys.Control) Then
            Dim keyAns = InputBox("1 = to Update LPPI Rate" & vbCrLf & _
                                  "2 =  to Update GYRT Minimum Allowed Members" & vbCrLf & _
                                  "3 = to Update GBLISS Minimum Allowed Members", _
                                  "Update")
            If Not keyAns = Nothing Then
                If keyAns = 1 Then
                    txtLRate.Enabled = True
                    txtLSF.Enabled = True
                    btnLUpdate.Enabled = True

                    txtGYRTMem.Enabled = False
                    btnGYRTMem.Enabled = False

                    txtGblissMem.Enabled = False
                    btnGblissMem.Enabled = False
                ElseIf keyAns = 2 Then
                    txtGYRTMem.Enabled = True
                    btnGYRTMem.Enabled = True

                    txtLRate.Enabled = False
                    txtLSF.Enabled = False
                    btnLUpdate.Enabled = False

                    txtGblissMem.Enabled = False
                    btnGblissMem.Enabled = False
                ElseIf keyAns = 3 Then
                    txtGblissMem.Enabled = True
                    btnGblissMem.Enabled = True

                    txtLRate.Enabled = False
                    txtLSF.Enabled = False
                    btnLUpdate.Enabled = False

                    txtGYRTMem.Enabled = False
                    btnGYRTMem.Enabled = False
                End If
            Else
                Exit Sub
            End If

        End If
    End Sub

    Private Sub btnLUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLUpdate.Click
        Try
            konek()
            qry = "UPDATE tbl_lppi_rate set lppi_rate = " & txtLRate.Text & ", coop_comm = " & txtLSF.Text & ""
            mycmd = New MySqlCommand(qry, Myconn)
            mycmd.ExecuteNonQuery()

            MessageBox.Show("LPPI Rate Updated", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
            showRate()
            txtLRate.Enabled = False
            txtLSF.Enabled = False
            btnLUpdate.Enabled = False
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub TabControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabControl1.Click
        showAllActions()
    End Sub

    Private Sub lvLBatch_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvLBatch.DoubleClick
        If lvLBatch.Items.Count > 0 Then
            forIdLPPI = lvLBatch.Items(lvLBatch.FocusedItem.Index).SubItems(1).Text

            listInsuredPerBatch()
        End If
    End Sub

    Private Sub btnGYRTMem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGYRTMem.Click
        Try
            konek()
            qry = "UPDATE tbl_gyrt_rate SET gyrt_req_mem = " & txtGYRTMem.Text & ""
            mycmd = New MySqlCommand(qry, Myconn)
            mycmd.ExecuteNonQuery()
            MessageBox.Show("GYRT Minimum Allowed Members Update.", "", MessageBoxButtons.OK, MessageBoxIcon.Information)

            txtGYRTMem.Enabled = False
            btnGYRTMem.Enabled = False
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub btnGblissMem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGblissMem.Click
        Try
            konek()
            qry = "UPDATE tbl_gbliss_rate SET gbliss_req_mem = " & txtGblissMem.Text & ""
            mycmd = New MySqlCommand(qry, Myconn)
            mycmd.ExecuteNonQuery()
            MessageBox.Show("GYRT Minimum Allowed Members Update.", "", MessageBoxButtons.OK, MessageBoxIcon.Information)

            txtGblissMem.Enabled = False
            btnGblissMem.Enabled = False
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub TabPage3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabPage3.Click

    End Sub
End Class