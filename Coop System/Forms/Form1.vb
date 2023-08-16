Imports MySql.Data.MySqlClient
Public Class Form1
    Dim grossRate As Double
    Dim net As Double
    Dim gross, comm, unu As Double
    Dim lamt As Double
    Dim term As Double
    Dim ctr As Integer
    Dim ctr2 As Integer = 0



    Sub listLoantypes()
        cbLoanType.Items.Clear()
        konek()
        qry = "SELECT * FROM tbl_loans WHERE active = 0"
        mycmd = New MySqlCommand(qry, Myconn)
        mydr = mycmd.ExecuteReader

        While mydr.Read
            cbLoanType.Items.Add(mydr("loans"))
        End While
    End Sub

    Sub updateWithUnuse()
        Try
            konek()
            qry = "SELECT * FROM tbl_info where MEM_CODE = '" & cbMemCode.Text & "' and BATCH_UNUSED = '" & txtCode.Text & "'"
            mycmd = New MySqlCommand(qry, Myconn)
            mydr = mycmd.ExecuteReader

            If mydr.Read Then
                Dim id As Integer = mydr("id")
                konek()
                qry = "UPDATE tbl_info set BATCH_UNUSED = '', CLOSE = 0 where id = " & id & ""
                mycmd = New MySqlCommand(qry, Myconn)
                mycmd.ExecuteNonQuery()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
      
    End Sub
    Sub countUnuseTerm()
        Try
            konek()
            qry = "SELECT * FROM tbl_info where MEM_CODE = '" & cbMemCode.Text & "' and BATCH != '" & txtCode.Text & "' and CLOSE = 0 and LOAN_TYPE = '" & loanTypeID & "'"
            mycmd = New MySqlCommand(qry, Myconn)
            mydr = mycmd.ExecuteReader

            While mydr.Read
                Dim term As Integer = 0
                Dim fDate As DateTime = mydr("EXPIRY")
                Dim nDate As DateTime = txtEffect.Text

                term = DateDiff(DateInterval.Month, nDate, fDate, )
                unuseTerm = term
                prevNetPrem = mydr("NET")
                prevTerm = mydr("TERMS")
                forID = mydr("id")
                forBatch = mydr("BATCH")
            End While

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Sub computeUnuse()
        Try
            If unuseTerm > 0 Then
                unusePrem = ((prevNetPrem / prevTerm) * unuseTerm)

                konek()
                qry = "UPDATE tbl_info set CLOSE = 1, BATCH_UNUSED = '" & txtCode.Text & "' where id = " & forID & ""
                mycmd = New MySqlCommand(qry, Myconn)
                mycmd.ExecuteNonQuery()
            Else
                unusePrem = 0
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Sub forUnuse()
        Try
            Dim date1 As String
            Dim date2 As String
            Dim date3 As String
            Dim unuseprem2 As Double
            Dim termsx As Double

            konek()
            qry = "SELECT COUNT(*) FROM tbl_info WHERE MEM_CODE = '" & txtFullName.Text & "' AND LOAN_TYPE = " & loanTypeID & ""
            mycmd = New MySqlCommand(qry, Myconn)
            mydr = mycmd.ExecuteReader

            If mydr.Read Then
                If mydr(0) >= 1 Then
                    Dim ans = MessageBox.Show("Do you want to Compute Unused Premium?", "Unused Premium", MessageBoxButtons.YesNo)
                    If ans = Windows.Forms.DialogResult.Yes Then
                        frmUnused.ShowDialog()
                        If Not oldEffect = Nothing Then
                            date1 = txtEffect.Text
                            date2 = oldEffect
                            date3 = oldExpiry
                            If Not date2 = Nothing Then
                                Dim terms As Integer = DateDiff(DateInterval.Month, CDate(date2), CDate(date1))
                                'Dim terms As Integer = DateDiff(DateInterval.Month, CDate(date1), CDate(date3))
                                termsx = DateDiff(DateInterval.Month, CDate(date1), CDate(date3))

                                If termsx <= 0 Then
                                    unusePrem = 0
                                    Exit Sub
                                End If

                                'terms = Int(terms / 30)
                                If terms <= 0 Then
                                    terms = 1
                                End If
                                'oldSF = 0.5
                                unusePrem = ((oldGross - oldSF) - (oldGross - oldSF) / oldTerm * (terms))
                                'unusePrem = ((oldGross - (oldGross * 0.2)) - ((oldGross - (oldGross * 0.2)) / oldTerm * (terms)))
                            Else
                                unusePrem = 0
                                Exit Sub
                            End If
                        End If
                    Else
                        unusePrem = 0
                        Exit Sub
                    End If
                Else
                    unusePrem = 0
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Sub ComputePremium()
        ctr = 0
        Dim premRate As Double = 1
        Dim PNet As Double = 0
        premRateStan = 0.6
        coopCommStan = FormatNumber(premRateStan * 0.4334, 2)

        loanAmt = Val(txtLoanAmt.Text)

        '18 to 65 below 300,000
        If txtAge.Text <= 65 And txtAge.Text >= 18 And loanAmt <= 300000 Then
            grossPremium = (((loanAmt / 1000) * premRateStan) * terms)
            serviceFee = (((loanAmt / 1000) * coopCommStan) * terms)
            netPremium = grossPremium - serviceFee

            comm = serviceFee
            net = netPremium
            gross = grossPremium
            ctr += 1

            '18 to 65 above 300,000
        ElseIf txtAge.Text <= 65 And txtAge.Text >= 18 And loanAmt > 300000 Then
            grossPremium = (((loanAmt / 1000) * premRateStan) * terms)
            serviceFee = (((loanAmt / 1000) * coopCommStan) * terms)
            netPremium = grossPremium - serviceFee

            comm = serviceFee
            net = netPremium
            gross = grossPremium
            ctr += 1

            '66 to 69 below 2,000,000
        ElseIf txtAge.Text <= 69 And txtAge.Text >= 66 And loanAmt <= 2000000 Then
            premRate = 1.05
            grossPremium = (((loanAmt / 1000) * premRateStan) * terms)
            serviceFee = grossPremium * 0
            netPremium = grossPremium - serviceFee

            comm = serviceFee
            net = netPremium
            gross = grossPremium
            ctr += 1

            '70 to 75 below 2,000,000
        ElseIf txtAge.Text <= 75 And txtAge.Text >= 70 And loanAmt <= 2000000 Then
            premRate = 3.2
            grossPremium = (((loanAmt / 1000) * premRateStan) * terms)
            serviceFee = grossPremium * 0
            netPremium = grossPremium - serviceFee

            comm = serviceFee
            net = netPremium
            gross = grossPremium
            ctr += 1

        Else
            MessageBox.Show("Not Qualify.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Sub getAge()
        Dim bday As DateTime = txtBirthdate.Text
        Dim effect As DateTime = txtEffect.Text
        Dim elapsed As TimeSpan
        Dim aMonth As Double

        elapsed = effect - bday

        'txtAge.Text = Math.Round(elapsed.Days / 365.25)
        'txtAge.Text = Math.Floor(effect.Subtract(bday).TotalDays / 365)
        aMonth = DateDiff(DateInterval.Month, bday, effect)
        txtAge.Text = CInt(aMonth / 12)

    End Sub

    Sub getTerm()
        Dim effectDate As DateTime = txtEffect.Text

        txtTerms.Text = 12

        If txtTerms.Text = Nothing Then
            Dim expiryDate As DateTime = txtExpiry.Text

            Dim xterms As Integer = DateDiff(DateInterval.Month, effectDate, expiryDate)
            txtTerms.Text = xterms
            terms = xterms
        Else
            Dim dt As DateTime = effectDate
            Dim expDate As DateTime = dt.AddMonths(txtTerms.Text)
            txtExpiry.Text = expDate.ToString("MM/dd/yyyy")
            txtEffect.Text = effectDate.ToString("MM/dd/yyyy")
            terms = txtTerms.Text
        End If
    End Sub

    Sub loadStandardRate()
        konek()
        qry = "SELECT * FROM tbl_lppi_rate"
        mycmd = New MySqlCommand(qry, Myconn)
        mydr = mycmd.ExecuteReader

        While mydr.Read
            premRateStan = mydr("lppi_rate")
            coopCommStan = mydr("coop_comm")
        End While
    End Sub

    Sub listMemberID()

        cbMemCode.DataSource = Nothing
        cbMemCode.Items.Clear()

        konek()
        'qry = "SELECT * FROM tbl_member_info"
        qry = "SELECT concat(L_NAME,',',F_NAME,',',M_INITIAL,',',SUFFIX) as FULL_NAME,MEMBER_CODE FROM tbl_member_info WHERE ACTIVE = 0"
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

    Sub listAllPerBatch()
        DS.Tables(0).Clear()
        Dim ctr As Integer = 0
        Dim lamt, gross, net, comm As Double
        lvCoverages.Items.Clear()

        konek()
        'qry = "SELECT * FROM tbl_info where BATCH = '" & txtCode.Text & "'"
        qry = "SELECT a.id,a.CERT_NO,a.MEM_CODE,b.L_NAME,b.F_NAME,b.M_INITIAL,b.SUFFIX,b.BDAY,a.AGE,b.GENDER,a.LOAN_AMT,c.loans,a.EFFECTIVITY," & _
        "a.EXPIRY,a.TERMS,a.GROSS,a.Unused,a.Commission,a.NET,a.LOAN_TYPE FROM tbl_info a LEFT JOIN tbl_member_info b ON a.MEM_CODE = b.MEMBER_CODE LEFT JOIN tbl_loans c " & _
        "on a.LOAN_TYPE=c.id WHERE BATCH = '" & txtCode.Text & "' ORDER BY a.id"
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
            Dim ListViewItem As ListViewItem = New ListViewItem(New String() {ctr, mydr("id"), mydr("CERT_NO"), mydr("MEM_CODE"), mydr("L_NAME"), mydr("F_NAME"), _
                                                                              mydr("M_INITIAL"), mydr("SUFFIX"), mydr("BDAY"), mydr("AGE"), mydr("GENDER"), _
                                                                              lamt.ToString("#,##0.00"), fLoans, mydr("EFFECTIVITY"), mydr("EXPIRY"), _
                                                                              mydr("TERMS"), gross.ToString("#,##0.00"), comm.ToString("#,##0.00"), _
                                                                               net.ToString("#,##0.00")})
            lvCoverages.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem})


        End While

        DS.Tables(0).Clear()

        konek()
        qry = "SELECT a.id,a.CERT_NO,a.MEM_CODE,b.L_NAME,b.F_NAME,b.M_INITIAL,b.SUFFIX,b.BDAY,a.AGE,b.GENDER,a.LOAN_AMT,c.loans,a.EFFECTIVITY," & _
        "a.EXPIRY,a.TERMS,a.GROSS,a.Unused,a.Commission,a.NET FROM tbl_info a LEFT JOIN tbl_member_info b ON a.MEM_CODE = b.MEMBER_CODE LEFT JOIN tbl_loans c " & _
        "on a.LOAN_TYPE=c.id WHERE BATCH = '" & txtCode.Text & "' ORDER BY id"
        'qry = "SELECT id,CERT_NO,LASTNAME,FIRSTNAME,MI,SUFFIX,BDAY,AGE,GENDER,LOAN_AMT,LOAN_TYPE,EFFECTIVITY,EXPIRY,TERMS,GROSS,Commission,NET " & _
        '"FROM tbl_info where BATCH = '" & txtCode.Text & "'"
        myda.SelectCommand = New MySqlCommand(qry, Myconn)
        myda.Fill(DS.Tables(0))
    End Sub

    Sub ClearMemDetails()
        txtFname.Clear()
        txtLname.Clear()
        txtMI.Clear()
        txtSuffix.Clear()
        cbGender.Text = ""
        txtBirthdate.Text = ""
        txtAge.Clear()
    End Sub

    Sub ClearLoanDetails()
        txtCertNo.Clear()
        txtLoanAmt.Clear()
        txtEffect.Clear()
        txtExpiry.Clear()
        txtTerms.Clear()
        cbLoanType.Text = ""
        txtPremiumPaid.Clear()
    End Sub

    Sub upto5m()
        Dim ctr As Integer = 0
        Dim expire As DateTime
        Dim loanAmt As Double

        Try
            konek()
            qry = "SELECT EXPIRY,LOAN_AMT FROM tbl_info WHERE MEM_CODE = '" & txtFullName.Text & "'"
            mycmd = New MySqlCommand(qry, Myconn)
            mydr = mycmd.ExecuteReader

            While mydr.Read
                expire = mydr("EXPIRY")
                If expire > Date.Now Then
                    'MessageBox.Show("yes" & vbCrLf & expire & vbCrLf & Date.Now)
                    loanAmt += mydr("LOAN_AMT")
                Else
                    'MessageBox.Show("no")
                End If
            End While
            'MessageBox.Show(loanAmt.ToString("#,##0.00"))
            loanAmt += Val(txtLoanAmt.Text)
            If loanAmt > 5000000 Then
                ctr2 = 1
                Exit Sub
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Sub getLoanID()
        konek()
        qry = "SELECT id FROM tbl_loans WHERE loans = '" & cbLoanType.Text & "'"
        mycmd = New MySqlCommand(qry, Myconn)
        mydr = mycmd.ExecuteReader

        While mydr.Read
            loanTypeID = mydr(0)
        End While
    End Sub

    Private Sub txtLname_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtLname.TextChanged
        TXT(txtLname)
    End Sub

    Private Sub txtFname_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFname.TextChanged
        TXT(txtFname)
    End Sub

    Private Sub txtMI_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMI.TextChanged
        TXT(txtMI)
    End Sub

    Private Sub txtSuffix_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSuffix.TextChanged
        TXT(txtSuffix)
    End Sub

    Private Sub txtLoanAmt_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtLoanAmt.TextChanged
        'number(txtLoanAmt)
    End Sub

    Private Sub txtTerms_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTerms.TextChanged
        number(txtTerms)
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            If txtFullName.Text = "" Then
                If Not txtFname.Text = Nothing Or Not txtLname.Text = Nothing Then
                    konek()
                    qry = "SELECT * FROM tbl_member_info WHERE L_NAME = @lname and F_NAME = @fname"
                    mycmd = New MySqlCommand(qry, Myconn)
                    mycmd.Parameters.AddWithValue("@lname", txtLname.Text)
                    mycmd.Parameters.AddWithValue("@fname", txtFname.Text)
                    mydr = mycmd.ExecuteReader

                    If mydr.Read Then
                        txtFullName.Text = mydr("MEMBER_CODE")
                        txtBirthdate.Text = mydr("BDAY")
                        cbGender.Text = mydr("GENDER")
                    Else
                        konek()
                        'qry = "SELECT count(*) FROM tbl_member_info WHERE ACTIVE = 0 "
                        qry = "SELECT IFNULL((SELECT SUBSTRING(MEMBER_CODE, 6, 5) from tbl_member_info order by ctr DESC limit 1),0)"
                        mycmd = New MySqlCommand(qry, Myconn)
                        mydr = mycmd.ExecuteReader

                        While mydr.Read
                            Dim va As Integer = mydr(0) + 1
                            txtFullName.Text = "LPPI-" & va.ToString("00000")
                        End While
                    End If
                End If
            End If

            If Not cbMemCode.Text = Nothing Or txtLname.Text = Nothing Or txtLoanAmt.Text = Nothing Or txtPremiumPaid.Text = Nothing Then

                konek()
                qry = "SELECT * FROM tbl_member_info WHERE MEMBER_CODE = '" & txtFullName.Text & "'"
                mycmd = New MySqlCommand(qry, Myconn)
                mydr = mycmd.ExecuteReader

                If Not mydr.Read = True Then
                    konek()
                    qry = "INSERT INTO tbl_member_info (MEMBER_CODE,L_NAME,F_NAME,M_INITIAL,SUFFIX,BDAY,GENDER) values (@mem_code,@lname,@fname,@mi,@suff,@bday,@gender)"
                    mycmd = New MySqlCommand(qry, Myconn)
                    mycmd.Parameters.AddWithValue("@mem_code", txtFullName.Text)
                    mycmd.Parameters.AddWithValue("@lname", txtLname.Text)
                    mycmd.Parameters.AddWithValue("@fname", txtFname.Text)
                    mycmd.Parameters.AddWithValue("@mi", txtMI.Text)
                    mycmd.Parameters.AddWithValue("@suff", txtSuffix.Text)
                    mycmd.Parameters.AddWithValue("@bday", txtBirthdate.Text)
                    mycmd.Parameters.AddWithValue("@gender", cbGender.Text)
                    mycmd.ExecuteNonQuery()

                    MessageBox.Show("Member Successfully Added.")
                End If

                getLoanID()
                'forUnuse()
                getTerm()
                ComputePremium()

                txtPremiumPaid.Text = net

                If ctr > 0 Then
                    konek()
                    qry = "INSERT INTO tbl_info (BATCH,CERT_NO,MEM_CODE,AGE,EFFECTIVITY,EXPIRY," & _
                    "TERMS,LOAN_TYPE,LOAN_AMT,GROSS,NET,Unused,Commission) values (@batch,@cert,@memcode,@age,@effect," & _
                    "@expiry,@term,@loan,@amt,@gross,@net,@unuse,@comm)"
                    mycmd = New MySqlCommand(qry, Myconn)
                    mycmd.Parameters.AddWithValue("@batch", txtCode.Text)
                    mycmd.Parameters.AddWithValue("@cert", txtCertNo.Text)
                    mycmd.Parameters.AddWithValue("@memcode", txtFullName.Text)
                    mycmd.Parameters.AddWithValue("@age", txtAge.Text)
                    mycmd.Parameters.AddWithValue("@effect", txtEffect.Text)
                    mycmd.Parameters.AddWithValue("@expiry", txtExpiry.Text)
                    mycmd.Parameters.AddWithValue("@term", txtTerms.Text)
                    mycmd.Parameters.AddWithValue("@loan", loanTypeID)
                    mycmd.Parameters.AddWithValue("@amt", txtLoanAmt.Text)
                    mycmd.Parameters.AddWithValue("@gross", gross)
                    mycmd.Parameters.AddWithValue("@net", net)
                    mycmd.Parameters.AddWithValue("@unuse", 0)
                    mycmd.Parameters.AddWithValue("@comm", comm)
                    mycmd.ExecuteNonQuery()

                    MessageBox.Show("Coverage Successfully Added.")

                    konek()
                    qry = "SELECT * FROM tbl_info ORDER BY id DESC LIMIT 1"
                    mycmd = New MySqlCommand(qry, Myconn)
                    mydr = mycmd.ExecuteReader
                    If mydr.Read Then
                        act = "ADD Loan"
                        tran_id = mydr(0) & ", " & txtCode.Text & ", " & cbMemCode.Text & ", " & txtLoanAmt.Text & ", " & txtPremiumPaid.Text & ""
                        trail()
                    End If

                    ClearMemDetails()
                    ClearLoanDetails()
                    listAllPerBatch()
                    listMemberID()
                    txtFullName.Clear()
                Else
                    Exit Sub
                    listAllPerBatch()
                End If

            End If
            'ComputePremium()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
exitSub:
    End Sub

    Private Sub btnAddBatch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddBatch.Click
        frmAddBatch.ShowDialog()
    End Sub
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'ComputePremium()
        loadStandardRate()
        listLoantypes()
        listMemberID()
        coopName = "Air Cavaliers Credit Cooperative"

        Dim forTrim As String = "aaa "
        forTrim.Replace(" ", ".")
        'MessageBox.Show(forTrim)
    End Sub

    Private Sub cbMemCode_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbMemCode.Validated
        Dim lname As String = ""
        Dim fname As String = ""
        Dim mi As String = ""
        Dim suf As String = ""
        Try
            ClearLoanDetails()
            ClearMemDetails()

            Dim strArr() As String
            Dim str As String
            Dim count As Integer

            str = cbMemCode.Text
            strArr = str.Split(",")
            If strArr.Length - 1 < 3 Then
                MessageBox.Show("Error", "Input Name", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtFullName.Clear()
                cbMemCode.Text = ""
                Exit Sub
            End If

            For count = 1 To strArr.Length - 1
                lname = strArr(0)
                fname = strArr(1)
                mi = strArr(2)
                suf = strArr(3)
            Next
            konek()
            qry = "SELECT MEMBER_CODE,BDAY FROM tbl_member_info where L_NAME = '" & lname & "' and F_NAME = '" & fname & "' " & _
            "and M_INITIAL = '" & mi & "' and SUFFIX = '" & suf & "' and ACTIVE = 0"
            mycmd = New MySqlCommand(qry, Myconn)
            mydr = mycmd.ExecuteReader

            If mydr.Read = True Then
                txtFullName.Text = mydr(0)
                txtBirthdate.Text = mydr(1)
            Else
                konek()
                'qry = "SELECT count(*) FROM tbl_member_info WHERE ACTIVE = 0 "
                'qry = "SELECT SUBSTRING(MEMBER_CODE, 6, 5) from tbl_member_info order by ctr DESC limit 1 "
                qry = "SELECT ctr from tbl_member_info order by ctr DESC LIMIT 1"
                mycmd = New MySqlCommand(qry, Myconn)
                mydr = mycmd.ExecuteReader

                While mydr.Read
                    Dim va As Integer = mydr(0) + 1
                    txtFullName.Text = "LPPI-" & va.ToString("00000")
                End While
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        Try
            ClearLoanDetails()
            ClearMemDetails()

            konek()
            qry = "SELECT * FROM tbl_member_info where MEMBER_CODE = '" & txtFullName.Text & "'"
            mycmd = New MySqlCommand(qry, Myconn)
            mydr = mycmd.ExecuteReader

            If mydr.Read = True Then
                txtFname.Text = mydr("F_NAME").ToString.Trim
                txtLname.Text = mydr("L_NAME").ToString.Trim
                txtMI.Text = mydr("M_INITIAL").ToString.Trim
                txtSuffix.Text = mydr("SUFFIX")
                txtBirthdate.Text = mydr("BDAY")
                cbGender.Text = mydr("GENDER")
                txtCertNo.Focus()
            Else
                txtFname.Text = fname
                txtLname.Text = lname
                txtMI.Text = mi
                txtSuffix.Text = suf
                txtBirthdate.Focus()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub txtCode_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCode.TextChanged
        listAllPerBatch()
    End Sub

    Private Sub lvCoverages_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvCoverages.DoubleClick
        If lvCoverages.Items.Count > 0 Then
            ClearLoanDetails()
            ClearMemDetails()

            lblID.Text = lvCoverages.Items(lvCoverages.FocusedItem.Index).SubItems(1).Text

            'member details
            txtFullName.Text = lvCoverages.Items(lvCoverages.FocusedItem.Index).SubItems(3).Text
            txtFname.Text = lvCoverages.Items(lvCoverages.FocusedItem.Index).SubItems(5).Text
            txtLname.Text = lvCoverages.Items(lvCoverages.FocusedItem.Index).SubItems(4).Text
            txtMI.Text = lvCoverages.Items(lvCoverages.FocusedItem.Index).SubItems(6).Text
            txtSuffix.Text = lvCoverages.Items(lvCoverages.FocusedItem.Index).SubItems(7).Text
            txtBirthdate.Text = lvCoverages.Items(lvCoverages.FocusedItem.Index).SubItems(8).Text
            txtAge.Text = lvCoverages.Items(lvCoverages.FocusedItem.Index).SubItems(9).Text
            cbGender.Text = lvCoverages.Items(lvCoverages.FocusedItem.Index).SubItems(10).Text

            'loan details
            txtCertNo.Text = lvCoverages.Items(lvCoverages.FocusedItem.Index).SubItems(2).Text
            txtLoanAmt.Text = FormatNumber(lvCoverages.Items(lvCoverages.FocusedItem.Index).SubItems(11).Text, 2).Replace(",", "")
            txtEffect.Text = lvCoverages.Items(lvCoverages.FocusedItem.Index).SubItems(13).Text
            txtExpiry.Text = lvCoverages.Items(lvCoverages.FocusedItem.Index).SubItems(14).Text
            txtTerms.Text = lvCoverages.Items(lvCoverages.FocusedItem.Index).SubItems(15).Text
            cbLoanType.Text = lvCoverages.Items(lvCoverages.FocusedItem.Index).SubItems(12).Text
            txtPremiumPaid.Text = lvCoverages.Items(lvCoverages.FocusedItem.Index).SubItems(18).Text

            'disable add button
            btnAdd.Enabled = False
        End If

    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        'Dim bday As DateTime = txtBirthdate.Text
        'Dim curDate As DateTime = txtEffect.Text

        'Dim age = DateDiff(DateInterval.Month, bday, curDate)
        'Dim age1 As Integer = age / 12
        'Dim age2 = age / 12
        'Dim age3 = age2 Mod 1
        'Dim age4 = Math.Round(curDate.Subtract(bday).TotalDays / 365.25)

        'MessageBox.Show(age1 & " " & age2 & " " & age3 & " " & age4)
        Try
            Dim result As Integer = MessageBox.Show("Do you want to Update this record?", "", MessageBoxButtons.YesNo)
            If result = DialogResult.Yes Then
                If Not cbMemCode.Text Is Nothing Or txtLoanAmt.Text Is Nothing Or txtPremiumPaid.Text Is Nothing Or _
            txtEffect.Text Is Nothing Or txtExpiry.Text Is Nothing Or txtAge.Text Is Nothing Then
                    konek()
                    qry = "UPDATE tbl_member_info set L_NAME = @lname, F_NAME = @fname, M_INITIAL = @mi, SUFFIX = @suf, " & _
                    "BDAY = @bday, GENDER = @gen where MEMBER_CODE = '" & txtFullName.Text & "'"
                    mycmd = New MySqlCommand(qry, Myconn)
                    mycmd.Parameters.AddWithValue("@lname", txtLname.Text)
                    mycmd.Parameters.AddWithValue("@fname", txtFname.Text)
                    mycmd.Parameters.AddWithValue("@mi", txtMI.Text)
                    mycmd.Parameters.AddWithValue("@suf", txtSuffix.Text)
                    mycmd.Parameters.AddWithValue("@bday", txtBirthdate.Text)
                    mycmd.Parameters.AddWithValue("@gen", cbGender.Text)
                    mycmd.ExecuteNonQuery()

                    act = "UPDATE"
                    tran_id = txtFullName.Text & "," & "LNAME=" & txtLname.Text & "," & "FNAME=" & txtFname.Text & "," & "MI=" & txtMI.Text & "," & "SUFFIX=" & _
                             txtSuffix.Text & "," & "BDAY=" & txtBirthdate.Text & "," & "GENDER=" & cbGender.Text
                    trail()

                    getAge()
                    'forUnuse()
                    getTerm()
                    ComputePremium()

                    konek()
                    qry = "UPDATE tbl_info SET CERT_NO = @cert, AGE = @age, EFFECTIVITY = @effect, EXPIRY = @expire, " & _
                    "TERMS = @terms, LOAN_TYPE = @type, LOAN_AMT = @amt, GROSS = @gross, NET = @net, Commission = @comm, " & _
                    "Unused = @unused, BATCH_UNUSED = @oldMem WHERE id = '" & lblID.Text & "'"
                    mycmd = New MySqlCommand(qry, Myconn)
                    mycmd.Parameters.AddWithValue("@cert", txtCertNo.Text)
                    mycmd.Parameters.AddWithValue("@age", txtAge.Text)
                    mycmd.Parameters.AddWithValue("@effect", txtEffect.Text)
                    mycmd.Parameters.AddWithValue("@expire", txtExpiry.Text)
                    mycmd.Parameters.AddWithValue("@terms", txtTerms.Text)
                    mycmd.Parameters.AddWithValue("@type", loanTypeID)
                    mycmd.Parameters.AddWithValue("@amt", txtLoanAmt.Text)
                    mycmd.Parameters.AddWithValue("@gross", gross)
                    mycmd.Parameters.AddWithValue("@net", net)
                    mycmd.Parameters.AddWithValue("@comm", comm)
                    mycmd.Parameters.AddWithValue("@unused", unusePrem)
                    mycmd.Parameters.AddWithValue("@oldMem", unusedID)
                    mycmd.ExecuteNonQuery()

                    MessageBox.Show("RECORD UPDATED.", "", MessageBoxButtons.OK)

                    act = "UPDATE"
                    tran_id = lblID.Text & "," & "CERT NO.=" & txtCertNo.Text & "," & "AGE=" & txtAge.Text & "," & "EFFECT=" & txtEffect.Text & "," & _
                            "EXPIRE=" & txtExpiry.Text & "," & "TERM=" & txtTerms.Text & "," & "LTYPE=" & loanTypeID & "," & "AMT=" & txtLoanAmt.Text & "," & _
                            "GROSS=" & gross & "," & "NET=" & net & "," & "COMM=" & comm & "," & "UNUSE=" & unusePrem & "," & "OLDMEM=" & unusedID
                    trail()

                    ClearMemDetails()
                    ClearLoanDetails()
                    listAllPerBatch()
                    lblID.Text = "-"
                    txtFullName.Clear()
                    cbLoanType.Text = ""
                    btnAdd.Enabled = True
                End If
            Else
                ClearMemDetails()
                ClearLoanDetails()
                lblID.Text = "-"
                txtFullName.Clear()
                cbLoanType.Text = ""
                btnAdd.Enabled = True
            End If
            
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub txtEffect_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtEffect.Validated
        Try
            If txtEffect.Text = "  /  /" Then
                MessageBox.Show("Please input Effective Date.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtEffect.Focus()
                Exit Sub
            End If

            If Not txtBirthdate.Text = Nothing Then
                getAge()
            Else
                MessageBox.Show("Please input bith date.", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btnReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReport.Click
        If Not txtCode.Text = "" Then
            If Not lblStatus.Text = "ACTIVE" Then
                forRepTitle = "LPPI Summary Report of "
                frmReport.ShowDialog()
            Else
                MessageBox.Show("Please Lock the Batch.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Else
            MessageBox.Show("Please select Batch", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If

    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Try
            If Not lblID.Text = Nothing Or Not lblID.Text = "" Then
                Dim result As Integer = MessageBox.Show("Do you want to delete this record?", "", MessageBoxButtons.YesNo)
                If result = DialogResult.Yes Then
                    konek()
                    qry = "DELETE FROM tbl_info where id = '" & lblID.Text & "'"
                    mycmd = New MySqlCommand(qry, Myconn)
                    mycmd.ExecuteNonQuery()

                    MessageBox.Show("RECORD DELETED")

                    ClearMemDetails()
                    ClearLoanDetails()
                    listAllPerBatch()
                    btnAdd.Enabled = True
                    txtFullName.Clear()

                ElseIf result = DialogResult.No Then
                    MessageBox.Show("RECORD NOT DELETED")
                    Exit Sub
                Else
                    MessageBox.Show("CANCEL")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub cbLoanType_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbLoanType.TextChanged
        konek()
        qry = "SELECT * FROM tbl_loans where loans = '" & cbLoanType.Text & "'"
        mycmd = New MySqlCommand(qry, Myconn)
        mydr = mycmd.ExecuteReader

        If mydr.Read Then
            loanTypeID = mydr(0).ToString
        End If
    End Sub

    Private Sub Form1_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        Application.Restart()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        frmSearch.ShowDialog()
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

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try
            forRepTitle = "Unuse Coverage Report of "
            If txtCode.Text = "" Then
                MessageBox.Show("Please select Batch", "1CISP", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Else
                konek()

                DS.Tables(0).Clear()

                qry = "SELECT a.id,a.CERT_NO,a.MEM_CODE,b.L_NAME,b.F_NAME,b.M_INITIAL,b.SUFFIX,b.BDAY,a.AGE,b.GENDER,a.LOAN_AMT," & _
                "c.loans,a.EFFECTIVITY,a.EXPIRY,a.TERMS,a.GROSS,a.Unused,a.Commission,a.NET,a.LOAN_TYPE FROM tbl_info a" & _
                " LEFT JOIN tbl_member_info b on a.MEM_CODE = b.MEMBER_CODE LEFT JOIN tbl_loans c on a.LOAN_TYPE=c.id," & _
                "(select BATCH_UNUSED from tbl_info where BATCH = '" & txtCode.Text & "' and BATCH_UNUSED <> ''" & _
                ") as bb WHERE a.id= bb.BATCH_UNUSED"
                myda.SelectCommand = New MySqlCommand(qry, Myconn)
                myda.Fill(DS.Tables(0))

                frmReport.ShowDialog()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If txtFullName.Text = "" Then
            If Not txtFname.Text = Nothing Or Not txtLname.Text = Nothing Then
                konek()
                qry = "SELECT * FROM tbl_member_info WHERE L_NAME = @lname and F_NAME = @fname"
                mycmd = New MySqlCommand(qry, Myconn)
                mycmd.Parameters.AddWithValue("@lname", txtLname.Text)
                mycmd.Parameters.AddWithValue("@fname", txtFname.Text)
                mydr = mycmd.ExecuteReader

                If mydr.Read Then
                    txtFullName.Text = mydr("MEMBER_CODE")
                    txtBirthdate.Text = mydr("BDAY")
                    cbGender.Text = mydr("GENDER")
                Else
                    konek()
                    'qry = "SELECT count(*) FROM tbl_member_info WHERE ACTIVE = 0 "
                    qry = "SELECT SUBSTRING(MEMBER_CODE, 6, 5) from tbl_member_info order by ctr DESC limit 1 "
                    mycmd = New MySqlCommand(qry, Myconn)
                    mydr = mycmd.ExecuteReader

                    While mydr.Read
                        Dim va As Integer = mydr(0) + 1
                        txtFullName.Text = "LPPI-" & va.ToString("00000")
                    End While
                End If
            End If
        End If
    End Sub

    Private Sub btn_Import_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Import.Click
        If Not txtCode.Text = Nothing Or Not txtCode.Text = "" Then
            frmImport.txtCode.Text = txtCode.Text
            frmImport.txtDesc.Text = txtDescription.Text
            frmImport.txtDate.Text = txtDate.Text
            frmImport.txtStatus.Text = lblStatus.Text
            frmImport.ShowDialog()
        Else
            MessageBox.Show("Please select a batch first.", "1CISP", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If

    End Sub
End Class
