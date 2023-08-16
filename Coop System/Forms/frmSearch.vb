Imports MySql.Data.MySqlClient
Public Class frmSearch

    Public mem_code As String

    Sub listMemberID()
        cbMemCode.DataSource = Nothing
        cbMemCode.Items.Clear()

        konek()
        'qry = "SELECT * FROM tbl_member_info"
        qry = "SELECT concat(L_NAME,',',F_NAME,',',M_INITIAL,',',SUFFIX) as FULL_NAME,MEMBER_CODE FROM tbl_member_info WHERE ACTIVE = 0 ORDER BY L_NAME"
        mycmd = New MySqlCommand(qry, Myconn)
        myda = New MySqlDataAdapter(mycmd)
        mydt = New DataTable("FULL_NAME")

        myda.Fill(mydt)

        Try
            cbMemCode.DataSource = mydt
            'cbMemCode.DisplayMember = "MEMBER_CODE"
            cbMemCode.DisplayMember = "FULL_NAME"
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub frmSearch_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lvCoverages.Items.Clear()
        listMemberID()
        txtFullName.Clear()
    End Sub

    Private Sub cbMemCode_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbMemCode.Validated
        Dim lname As String = ""
        Dim fname As String = ""
        Dim mi As String = ""
        Dim suf As String = ""
        Try
            Dim strArr() As String
            Dim str As String
            Dim count As Integer

            str = cbMemCode.Text
            strArr = str.Split(",")
            For count = 1 To strArr.Length - 1
                lname = strArr(0)
                fname = strArr(1)
                mi = strArr(2)
                suf = strArr(3)
            Next
            konek()
            qry = "SELECT MEMBER_CODE,BDAY FROM tbl_member_info where L_NAME = '" & lname & "' and F_NAME = '" & fname & "' " & _
            "and M_INITIAL = '" & mi & "' and SUFFIX = '" & suf & "'"
            mycmd = New MySqlCommand(qry, Myconn)
            mydr = mycmd.ExecuteReader

            While mydr.Read
                mem_code = mydr("MEMBER_CODE")
                txtFullName.Text = mem_code
            End While
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Sub listSearch()
        Dim ctr As Integer = 0
        Dim lamt, gross, net, comm, unu As Double
        lvCoverages.Items.Clear()
        DS.Tables(0).Clear()

        konek()
        'qry = "SELECT * FROM tbl_info where BATCH = '" & txtCode.Text & "'"
        qry = "SELECT a.id,a.BATCH,a.CERT_NO,a.MEM_CODE,b.L_NAME,b.F_NAME,b.M_INITIAL,b.SUFFIX,b.BDAY,a.AGE,b.GENDER,a.LOAN_AMT,c.loans,a.EFFECTIVITY," & _
        "a.EXPIRY,a.TERMS,a.GROSS,a.Unused,a.Commission,a.NET,a.LOAN_TYPE FROM tbl_info a LEFT JOIN tbl_member_info b ON a.MEM_CODE = b.MEMBER_CODE LEFT JOIN tbl_loans c " & _
        "on a.LOAN_TYPE=c.id WHERE a.MEM_CODE = '" & txtFullName.Text & "' ORDER BY STR_TO_DATE(a.EFFECTIVITY, '%m/%d/%Y')"
        mycmd = New MySqlCommand(qry, Myconn)
        mydr = mycmd.ExecuteReader

        While mydr.Read
            ctr += 1
            lamt = mydr("LOAN_AMT")
            gross = mydr("GROSS")
            net = mydr("NET")
            comm = mydr("Commission")
            unu = mydr("Unused")
            Dim fLoans As String

            If IsDBNull(mydr("loans")) Then
                fLoans = mydr("LOAN_TYPE")
            Else
                fLoans = mydr("loans")
            End If
            Dim ListViewItem As ListViewItem = New ListViewItem(New String() {ctr, mydr("id"), mydr("BATCH"), mydr("CERT_NO"), mydr("MEM_CODE"), mydr("L_NAME"), mydr("F_NAME"), _
                                                                              mydr("M_INITIAL"), mydr("SUFFIX"), mydr("BDAY"), mydr("AGE"), mydr("GENDER"), _
                                                                              lamt.ToString("#,##0.00"), fLoans, mydr("EFFECTIVITY"), mydr("EXPIRY"), _
                                                                              mydr("TERMS"), gross.ToString("#,##0.00"), unu.ToString("#,##0.00"), _
                                                                              comm.ToString("#,##0.00"), net.ToString("#,##0.00")})
            lvCoverages.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem})
        End While

        konek()
        myda.SelectCommand = New MySqlCommand(qry, Myconn)
        myda.Fill(DS.Tables(0))
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not txtFullName.Text = Nothing Then
            listSearch()
        Else
            MessageBox.Show("Please select Member", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            If Not lvCoverages.Items.Count < 0 Then
                konek()
                qry = "SELECT * FROM tbl_member_info WHERE MEMBER_CODE = '" & txtFullName.Text & "'"
                mycmd = New MySqlCommand(qry, Myconn)
                mydr = mycmd.ExecuteReader

                If mydr.Read Then
                    pFname = mydr("F_NAME")
                    pLname = mydr("L_NAME")
                    pMI = mydr("M_INITIAL")
                    pSuffix = mydr("SUFFIX")
                Else
                    MessageBox.Show("No Member Record", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
                frmMemberRpt.ShowDialog()
            Else
                MessageBox.Show("Please select member records.", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        
    End Sub

End Class