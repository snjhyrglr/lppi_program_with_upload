Imports MySql.Data.MySqlClient
Imports Microsoft.Office.Interop
Public Class frmImport

    Public age As String
    Public terms As Integer
    Public premium As Integer
    Public prem As Double
    Public bday As String = ""
    Public Effective As String = ""
    Public expire As String = ""
    Public membID As String
    Public netPrem As Double
    Public PNet As Double
    Public grossPrem As Double
    Public unused As Double
    Public comm As Double
    Public bal As Double
    Public balance As Double
    Public mem_type As String
    Dim bcode As String
    Public ctr As Integer = 0

    Public Mcode As String
    Public birth As String
    Public effe As String
    Public expir As String
    Public LType As String
    Public LAmt As Double
    Public edad As Integer
    Public termino As Integer
    Public last As String
    Public first As String
    Public ServiceFee As Double

    Public forLoanID As String
    Public loanID As Integer

    Public forMemberCode As String
    Public forLname As String
    Public forFname As String
    Public forMI As String
    Public forSuf As String
    Public forBday As String
    Public forGender As String

    Sub getAge()

        Dim dob As DateTime = birth
        Dim effect As DateTime = effe

        Dim format As String = "ddd d/MMM/ yyy HH:mm"
        Dim elapsed As TimeSpan

        Dim wow = effect.ToString(format)
        Dim wew = dob.ToString(format)

        elapsed = effect - dob

        edad = Math.Round(elapsed.Days / 365.25)

    End Sub
    Sub term()
        Dim date1 As String = effe
        Dim date2 As String = expir
        If termino = Nothing Then
            Dim terms As Integer = DateDiff(DateInterval.Month, CDate(date1), CDate(date2))
            termino = terms
        Else

        End If

    End Sub
    Sub convert()
        Dim bdate As String = DateTime.Parse(birth).ToString("MM/dd/yyyy")
        Dim eff As String = DateTime.Parse(effe).ToString("MM/dd/yyyy")
        Dim exp As String = DateTime.Parse(expir).ToString("MM/dd/yyyy")

        bday = bdate.ToString
        Effective = eff.ToString
        expire = exp.ToString
    End Sub

    Public Sub computePremium()
        premRateStan = 0.6
        coopCommStan = FormatNumber(premRateStan * 0.4334, 2)
      

        Dim plan As String = LType

        Dim term As Double = termino
        Dim loanAmt As Double = LAmt
        Dim prem As Double = 0
        Dim premRate As Double


        If edad <= 65 And edad >= 18 And loanAmt <= 3000000 Then
            grossPremium = (((loanAmt / 1000) * premRateStan) * term)
            ServiceFee = (((loanAmt / 1000) * coopCommStan) * term)
            netPremium = grossPremium - ServiceFee


        ElseIf edad <= 69 And edad >= 66 And loanAmt <= 2000000 Then
            premRate = 1.05
            grossPremium = (((loanAmt / 1000) * premRate) * term)
            ServiceFee = 0
            netPremium = grossPremium - ServiceFee

        ElseIf edad <= 75 And edad >= 70 And loanAmt <= 2000000 Then
            premRate = 3.2
            grossPremium = (((loanAmt / 1000) * premRate) * term)
            ServiceFee = 0
            netPremium = grossPremium - ServiceFee
        Else
            'MessageBox.Show("Not Qualified.")
            netPrem = 0
            ctr = 1
            Exit Sub
        End If

    End Sub
    Private Sub frmImport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Structure ExcelRows
        Dim c1 As String
        Dim c2 As String
        Dim c3 As String
        Dim c4 As String
        Dim c5 As String
        Dim c6 As String
        Dim c7 As String
        Dim c8 As String
        Dim c9 As String
        Dim c10 As String
        Dim c11 As String
        Dim c12 As String
    End Structure
    Private ExcelRowList As List(Of ExcelRows) = New List(Of ExcelRows)
    Private Function GetInfo() As Boolean
        Dim Completed As Boolean = False

        Try

            Dim MyExcel As New Excel.Application
            MyExcel.Workbooks.Open(Me.TextBox1.Text)

            MyExcel.Sheets("sheet1").activate()
            MyExcel.Range("a2").Activate()

            Dim ThisRow As New ExcelRows

            Do
                If MyExcel.ActiveCell.Value > Nothing Or MyExcel.ActiveCell.Text > Nothing Then
                    'member_code
                    ThisRow.c1 = MyExcel.ActiveCell.Value
                    MyExcel.ActiveCell.Offset(0, 1).Activate()

                    'last name
                    ThisRow.c2 = MyExcel.ActiveCell.Value
                    MyExcel.ActiveCell.Offset(0, 1).Activate()

                    'first name
                    ThisRow.c3 = MyExcel.ActiveCell.Value
                    MyExcel.ActiveCell.Offset(0, 1).Activate()

                    'mi
                    ThisRow.c4 = MyExcel.ActiveCell.Value
                    MyExcel.ActiveCell.Offset(0, 1).Activate()

                    'suffix
                    ThisRow.c5 = MyExcel.ActiveCell.Value
                    MyExcel.ActiveCell.Offset(0, 1).Activate()

                    'bday
                    ThisRow.c6 = MyExcel.ActiveCell.Value
                    MyExcel.ActiveCell.Offset(0, 1).Activate()

                    'gender
                    ThisRow.c7 = MyExcel.ActiveCell.Value
                    MyExcel.ActiveCell.Offset(0, 1).Activate()

                    'effec
                    ThisRow.c8 = MyExcel.ActiveCell.Value
                    MyExcel.ActiveCell.Offset(0, 1).Activate()

                    'expire
                    ThisRow.c9 = MyExcel.ActiveCell.Value
                    MyExcel.ActiveCell.Offset(0, 1).Activate()

                    'loan type
                    ThisRow.c10 = MyExcel.ActiveCell.Value
                    MyExcel.ActiveCell.Offset(0, 1).Activate()

                    'loan amount
                    ThisRow.c11 = MyExcel.ActiveCell.Value
                    MyExcel.ActiveCell.Offset(0, 1).Activate()

                    ExcelRowList.Add(ThisRow)
                    MyExcel.ActiveCell.Offset(1, -11).Activate()

                Else
                    Completed = True
                    Exit Do
                End If
            Loop


            MyExcel.Workbooks.Close()
            MyExcel = Nothing

            Return Completed
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Exit Function
        End Try

    End Function

    Private Sub btnImport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImport.Click
        Me.OpenFileDialog1.FileName = Nothing

        If Me.OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            Me.TextBox1.Text = Me.OpenFileDialog1.FileName
        End If

        If GetInfo() = True Then

            Dim ctr_mem As Integer
            Dim xlname, xfname As String
            Dim xbday As String

            konek()
            qry = "SELECT ctr from tbl_member_info order by ctr DESC LIMIT 1"
            mycmd = New MySqlCommand(qry, Myconn)
            mydr = mycmd.ExecuteReader

            While mydr.Read
                ctr_mem = mydr(0)
            End While

            For Each xitem In ExcelRowList
                Dim lvitem As New ListViewItem

                last = xitem.c1
                first = xitem.c2
                birth = xitem.c5

                If last = xlname And first = xfname And birth = xbday Then
                    Mcode = Mcode
                    GoTo xGo
                End If

                konek()
                qry = "SELECT MEMBER_CODE FROM tbl_member_info WHERE L_NAME = '" & last & "' AND F_NAME = '" & first & "' AND BDAY = '" & birth & "'"
                mycmd = New MySqlCommand(qry, Myconn)
                mydr = mycmd.ExecuteReader

                If mydr.Read = True Then
                    Mcode = mydr(0)
                Else
                    ctr_mem += 1
                    Mcode = "LPPI-" & ctr_mem.ToString("00000")
                End If
xGo:
                'last = xitem.c1
                'first = xitem.c2
                'birth = xitem.c5
                'get_Member_Code()
                'Mcode = xitem.c1
                'birth = xitem.c6
                effe = xitem.c7
                expir = xitem.c8
                LType = xitem.c9
                LAmt = xitem.c10
                'last = xitem.c2
                'first = xitem.c3

                convert()
                getAge()
                term()
                computePremium()
                'If edad > 65 Then
                '    Continue For
                'End If

                Dim ListViewItem As ListViewItem = New ListViewItem(New String() {"", "", Mcode, UCase(last), UCase(first), UCase(xitem.c3), "", bday, _
                                                                                  edad, xitem.c6, Effective, expire, termino, LType, _
                                                                                  FormatNumber(LAmt, 2, TriState.False, TriState.True, TriState.True), _
                                                                                  FormatNumber(grossPremium, 2, TriState.False, TriState.True, TriState.True), _
                                                                                  FormatNumber(ServiceFee, 2, TriState.False, TriState.True, TriState.True), _
                                                                                  FormatNumber(netPremium, 2, TriState.False, TriState.True, TriState.True), 0})
                lvNames.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem})

                xlname = last
                xfname = first
                xbday = birth

                btnSave.Enabled = True
            Next
        End If
    End Sub
    Sub get_Member_Code()
        konek()
        qry = "SELECT MEMBER_CODE FROM tbl_member_info WHERE L_NAME = '" & last & "' AND F_NAME = '" & first & "' AND BDAY = '" & birth & "'"
        mycmd = New MySqlCommand(qry, Myconn)
        mydr = mycmd.ExecuteReader

        If mydr.Read = True Then
            Mcode = mydr(0)
        Else
            konek()
            'qry = "SELECT count(*) FROM tbl_member_info WHERE ACTIVE = 0 "
            'qry = "SELECT SUBSTRING(MEMBER_CODE, 6, 5) from tbl_member_info order by ctr DESC limit 1 "
            qry = "SELECT ctr from tbl_member_info order by ctr DESC LIMIT 1"
            mycmd = New MySqlCommand(qry, Myconn)
            mydr = mycmd.ExecuteReader

            While mydr.Read
                Dim va As Integer = mydr(0) + 1
                Mcode = "LPPI-" & va.ToString("00000")
            End While
        End If

    End Sub
    Sub selectLoans()
        konek()
        qry = "select * from tbl_loans where loans = '" & forLoanID & "'"
        mycmd = New MySqlCommand(qry, Myconn)
        mydr = mycmd.ExecuteReader
        If mydr.Read = True Then
            loanID = mydr("id")
        Else
            konek()
            qry = "insert into tbl_loans (loans,active) values (@loans,@active)"
            mycmd = New MySqlCommand(qry, Myconn)
            mycmd.Parameters.AddWithValue("@loans", forLoanID)
            mycmd.Parameters.AddWithValue("@active", 0)
            mycmd.ExecuteNonQuery()

            konek()
            qry = "select * from tbl_loans where loans = '" & forLoanID & "'"
            mycmd = New MySqlCommand(qry, Myconn)
            mydr = mycmd.ExecuteReader
            While mydr.Read
                loanID = mydr("id")
            End While
        End If
    End Sub
    Sub pasokNewMember()
        Try
            konek()
            qry = "select * from tbl_member_info where MEMBER_CODE = '" & forMemberCode & "'"
            mycmd = New MySqlCommand(qry, Myconn)
            mydr = mycmd.ExecuteReader
            If mydr.Read = True Then
                Exit Sub
            Else
                konek()
                qry = "insert into tbl_member_info (MEMBER_CODE,L_NAME,F_NAME,M_INITIAL,SUFFIX,BDAY,GENDER) values (@mc,@lname,@fname,@mi,@suf,@bday,@gender)"
                mycmd = New MySqlCommand(qry, Myconn)
                mycmd.Parameters.AddWithValue("@mc", forMemberCode)
                mycmd.Parameters.AddWithValue("@lname", forLname)
                mycmd.Parameters.AddWithValue("@fname", forFname)
                mycmd.Parameters.AddWithValue("@mi", forMI)
                mycmd.Parameters.AddWithValue("@suf", forSuf)
                mycmd.Parameters.AddWithValue("@bday", forBday)
                mycmd.Parameters.AddWithValue("@gender", forGender)
                mycmd.ExecuteNonQuery()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Try
            For Each item As ListViewItem In lvNames.Items
                Application.DoEvents()

                forLoanID = item.SubItems(13).Text
                selectLoans()

                forMemberCode = item.SubItems(2).Text
                forLname = item.SubItems(3).Text
                forFname = item.SubItems(4).Text
                forMI = item.SubItems(5).Text
                forSuf = item.SubItems(6).Text
                forBday = item.SubItems(7).Text
                forGender = item.SubItems(9).Text
                pasokNewMember()

                konek()
                qry = "INSERT into tbl_info (id,BATCH,CERT_NO,MEM_CODE,AGE,EFFECTIVITY,EXPIRY,TERMS,LOAN_TYPE,LOAN_AMT,GROSS,NET,Commission) values (@id,@batch,@cert_no,@memcode,@age,@effect,@expire,@terms,@type,@amount,@gross,@net,@comm)"
                mycmd = New MySqlCommand(qry, Myconn)
                mycmd.Parameters.AddWithValue("@id", item.SubItems(0).Text)
                mycmd.Parameters.AddWithValue("@cert_no", item.SubItems(1).Text)
                mycmd.Parameters.AddWithValue("@memcode", item.SubItems(2).Text)
                mycmd.Parameters.AddWithValue("@age", item.SubItems(8).Text)
                mycmd.Parameters.AddWithValue("@effect", item.SubItems(10).Text)
                mycmd.Parameters.AddWithValue("@expire", item.SubItems(11).Text)
                mycmd.Parameters.AddWithValue("@terms", item.SubItems(12).Text)
                mycmd.Parameters.AddWithValue("@type", loanID)
                mycmd.Parameters.AddWithValue("@amount", FormatNumber(item.SubItems(14).Text, 2).Replace(",", ""))
                mycmd.Parameters.AddWithValue("@gross", FormatNumber(item.SubItems(15).Text, 2).Replace(",", ""))
                mycmd.Parameters.AddWithValue("@net", FormatNumber(item.SubItems(17).Text, 2).Replace(",", ""))
                mycmd.Parameters.AddWithValue("@comm", FormatNumber(item.SubItems(16).Text, 2).Replace(",", ""))
                mycmd.Parameters.AddWithValue("@batch", txtCode.Text)
                mycmd.ExecuteNonQuery()
            Next
            MessageBox.Show("Data Imported Successfully.", "IMPORT", MessageBoxButtons.OK)
            lvNames.Items.Clear()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
End Class