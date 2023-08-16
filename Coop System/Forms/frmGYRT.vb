Imports MySql.Data.MySqlClient
Public Class frmGYRT
    Public bday As String
    Public edad As Integer
    Public total As Double = 0
    Public tot As Double
    Public fRate As Double = 0.0
    Public rate As Double

    Sub listMembers()
        lvMembers.Items.Clear()
        konek()

        qry = "select * from tbl_member_info where(MEMBER_CODE) not in (select member_code from tbl_gyrt where batch = '" & txtCode.Text & "')"
        mycmd = New MySqlCommand(qry, Myconn)
        mydr = mycmd.ExecuteReader

        While mydr.Read
            Dim ListViewItem As ListViewItem = New ListViewItem(New String() {mydr("MEMBER_CODE"), mydr("L_NAME"), mydr("F_NAME"), mydr("M_INITIAL"), mydr("SUFFIX"), mydr("BDAY"), mydr("GENDER")})
            lvMembers.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem})
        End While

        mydr = Nothing
        Myconn.Close()


    End Sub
    Sub term()
        Dim date1 As String = txtEffectivity.Text

        Dim dt As DateTime = date1
        Dim expDate As DateTime = dt.AddMonths(12)
        Dim graceper As DateTime = expDate.AddDays(15)

        txtEffectivity.Text = dt.ToString("MM/dd/yyyy")
        txtExpiry.Text = graceper.ToString("MM/dd/yyyy")
    End Sub
    Sub getAge()

        Dim dob As DateTime = bday
        Dim effect As DateTime = txtEffectivity.Text

        Dim format As String = "ddd d/MMM/ yyy HH:mm"
        Dim elapsed As TimeSpan

        Dim wow = effect.ToString(format)
        Dim wew = dob.ToString(format)

        elapsed = effect - dob

        edad = Math.Round(elapsed.Days / 365.25)

    End Sub
    Sub getRate()

        Dim date1 As String = txtToday.Text
        Dim date2 As String = txtExpiry.Text

        Dim terms As Integer = DateDiff(DateInterval.Month, CDate(date1), CDate(date2))

        
        fRate = ((rate / 12) * terms)
    End Sub
    Sub loadRate()
        konek()
        qry = "SELECT gyrt_rate FROM tbl_gyrt_rate"
        mycmd = New MySqlCommand(qry, Myconn)
        mydr = mycmd.ExecuteReader

        While mydr.Read
            rate = mydr(0)
        End While
        txtRate.Text = rate
    End Sub
    Private Sub frmGYRT_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'listMembers()
        loadRate()
        coopName = "Sapang Palay National High School MPC"
    End Sub

    Private Sub btnGet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGet.Click
        If Not txtCode.Text = "" Or Not txtStatus.Text = "" Then

            Dim total As Double = 0
            Dim tot As Double
            Dim fctr As Integer = 1

            getRate()

            If txtStatus.Text = "LOCKED" Then
                MessageBox.Show("BATCH IS LOCKED!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                Exit Sub
            Else
                For Each file As ListViewItem In lvMembers.Items
                    If file.Checked = True Then
                        bday = file.SubItems(5).Text
                        getAge()
                        If Not edad > 65 Then
                            Dim lv As ListViewItem
                            lv = New ListViewItem(New String() {file.SubItems(0).Text, file.SubItems(1).Text, _
                                                                file.SubItems(2).Text, file.SubItems(3).Text, _
                                                                file.SubItems(4).Text, file.SubItems(5).Text, _
                                                                file.SubItems(6).Text, edad, txtToday.Text, _
                                                                txtExpiry.Text, FormatNumber(fRate, 2)})
                            lvInsuredMem.Items.AddRange(New ListViewItem() {lv})

                            file.Remove()
                        Else
                            MessageBox.Show("Member " & file.SubItems(1).Text & ", " & file.SubItems(2).Text & " is overage." & vbCrLf & _
                                            "Age: " & edad, _
                                            "Overage Member", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        End If
                    End If
                Next

            End If

            For Each lvitem As ListViewItem In lvInsuredMem.Items
                If Double.TryParse(lvitem.SubItems(10).Text, tot) Then
                    total += tot
                End If
            Next
            txtTotal.Text = FormatNumber(total, 2)
        Else
            MessageBox.Show("Please select batch first.", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub btnGet2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGet2.Click
        frmGYRTBatch.ShowDialog()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            Dim fCount As Integer

            konek()
            qry = "SELECT gyrt_req_mem FROM tbl_gyrt_rate"
            mycmd = New MySqlCommand(qry, Myconn)
            mydr = mycmd.ExecuteReader

            While mydr.Read
                fCount = mydr(0)
            End While

            If Not lvInsuredMem.Items.Count < fCount Then

                For Each item As ListViewItem In lvInsuredMem.Items
                    konek()
                    qry = "SELECT * FROM tbl_gyrt where member_code = '" & item.SubItems(0).Text & "' and batch = '" & txtCode.Text & "'"
                    mycmd = New MySqlCommand(qry, Myconn)
                    mydr = mycmd.ExecuteReader
                    If mydr.Read = True Then
                        Continue For
                    Else
                        konek()
                        bday = item.SubItems(5).Text
                        getAge()
                        qry = "INSERT INTO tbl_gyrt (batch,member_code,age,effectivity,expiry,premium) values (@batch,@member,@age,@effect,@expire,@premium)"
                        mycmd = New MySqlCommand(qry, Myconn)
                        mycmd.Parameters.AddWithValue("@batch", txtCode.Text)
                        mycmd.Parameters.AddWithValue("@member", item.SubItems(0).Text)
                        mycmd.Parameters.AddWithValue("@age", edad)
                        mycmd.Parameters.AddWithValue("@effect", item.SubItems(8).Text)
                        mycmd.Parameters.AddWithValue("@expire", item.SubItems(9).Text)
                        mycmd.Parameters.AddWithValue("@premium", item.SubItems(10).Text)

                        mycmd.ExecuteNonQuery()
                    End If
                Next

                Dim ans = MessageBox.Show("Do you want to lock this batch?", "GYRT", MessageBoxButtons.YesNo)
                If ans = Windows.Forms.DialogResult.Yes Then
                    konek()
                    qry = "UPDATE tbl_gyrt_batch set BATCH_STATUS = 'LOCKED' WHERE BATCH_CODE = '" & txtCode.Text & "'"
                    mycmd = New MySqlCommand(qry, Myconn)
                    mycmd.ExecuteNonQuery()

                    MessageBox.Show("Coverage Added Successfully. Batch Locked")
                Else
                    MessageBox.Show("Coverage Added Successfully. Batch not Lock")
                End If

                Dim result = MessageBox.Show("Do you want to print the summary?", "Print Summary", MessageBoxButtons.YesNo)
                If result = Windows.Forms.DialogResult.Yes Then
                    konek()
                    qry = "SELECT b.L_NAME,b.F_NAME,b.M_INITIAL,b.SUFFIX,b.BDAY,b.GENDER,a.age,a.premium FROM tbl_gyrt a left join tbl_member_info b " & _
                    "on a.member_code = b.MEMBER_CODE where batch = '" & txtCode.Text & "'"
                    myda = New MySqlDataAdapter
                    myda.SelectCommand = New MySqlCommand(qry, Myconn)
                    myda.Fill(DS.Tables(1))

                    frmReportGYRT.ShowDialog()
                Else
                    Exit Sub
                End If
            Else
                MessageBox.Show("Members should be " & fCount)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub lvInsuredMem_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvInsuredMem.DoubleClick

        Dim lv As ListViewItem
        lv = New ListViewItem(New String() {lvInsuredMem.Items(lvInsuredMem.FocusedItem.Index).SubItems(0).Text, lvInsuredMem.Items(lvInsuredMem.FocusedItem.Index).SubItems(1).Text, _
                                            lvInsuredMem.Items(lvInsuredMem.FocusedItem.Index).SubItems(2).Text, lvInsuredMem.Items(lvInsuredMem.FocusedItem.Index).SubItems(3).Text, _
                                            lvInsuredMem.Items(lvInsuredMem.FocusedItem.Index).SubItems(4).Text, lvInsuredMem.Items(lvInsuredMem.FocusedItem.Index).SubItems(5).Text, _
                                            lvInsuredMem.Items(lvInsuredMem.FocusedItem.Index).SubItems(6).Text})
        lvMembers.Items.AddRange(New ListViewItem() {lv})

        For ctr As Integer = 0 To lvInsuredMem.Items.Count - 1
            lvInsuredMem.Items(lvInsuredMem.FocusedItem.Index).Remove()
            Exit For
        Next
    End Sub

    Private Sub btnReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Dim result = MessageBox.Show("Do you want to print the summary?", "Print Summary", MessageBoxButtons.YesNo)
        If result = Windows.Forms.DialogResult.Yes Then
            konek()
            qry = "SELECT b.L_NAME,b.F_NAME,b.M_INITIAL,b.SUFFIX,b.BDAY,b.GENDER,a.age,a.premium FROM tbl_gyrt a left join tbl_member_info b " & _
            "on a.member_code = b.MEMBER_CODE where batch = '" & txtCode.Text & "'"
            myda = New MySqlDataAdapter
            myda.SelectCommand = New MySqlCommand(qry, Myconn)
            myda.Fill(DS.Tables(1))

            frmReportGYRT.ShowDialog()
        Else
            Exit Sub
        End If
    End Sub

    Private Sub frmGYRT_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        frmMainMenu.Show()
        Me.Hide()
    End Sub

    Private Sub frmGYRT_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.F2 Then
            MessageBox.Show("Please Enter Admin Password to Update Batch.", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
            frmAdminPassGYRT.ShowDialog()
            Exit Sub
        End If

        If Not txtCode.Text = Nothing Then
            If (e.KeyCode = Keys.F1) Then
                frmGYRTNew.ShowDialog()
            End If
        Else
            MessageBox.Show("Please select batch first.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub
End Class