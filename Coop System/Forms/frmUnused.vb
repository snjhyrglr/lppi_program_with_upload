Imports MySql.Data.MySqlClient
Public Class frmUnused

    Private Sub frmUnused_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        loadLoans()
    End Sub

    Sub loadLoans()
        Dim ctr As Integer = 0
        Dim lamt, gross, net, comm, unu As Double
        lvCoverages.Items.Clear()
        Try
            konek()
            qry = "SELECT a.id,a.CERT_NO,a.MEM_CODE,b.L_NAME,b.F_NAME,b.M_INITIAL,b.SUFFIX,b.BDAY,a.AGE,b.GENDER,a.LOAN_AMT,c.loans,a.EFFECTIVITY," & _
            "a.EXPIRY,a.TERMS,a.GROSS,a.Unused,a.Commission,a.NET,a.LOAN_TYPE FROM tbl_info a LEFT JOIN tbl_member_info b ON a.MEM_CODE = b.MEMBER_CODE LEFT JOIN tbl_loans c " & _
            "on a.LOAN_TYPE=c.id WHERE a.MEM_CODE = '" & Form1.txtFullName.Text & "' AND a.id != '" & Form1.lblID.Text & "' AND LOAN_TYPE = '" & loanTypeID & "'" & _
            "order by STR_TO_DATE(a.EFFECTIVITY, '%m/%d/%Y')"
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
                                                                                  mydr("TERMS"), gross.ToString("#,##0.00"), unu.ToString("#,##0.00"), _
                                                                                  comm.ToString("#,##0.00"), Net.ToString("#,##0.00")})
                lvCoverages.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem})
            End While
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        
    End Sub

    Private Sub lvCoverages_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvCoverages.DoubleClick
        If lvCoverages.Items.Count >= 0 Then
            unusedID = lvCoverages.Items(lvCoverages.FocusedItem.Index).SubItems(1).Text
            'oldGross = FormatNumber(lvCoverages.Items(lvCoverages.FocusedItem.Index).SubItems(16).Text, 0, TriState.False, TriState.False, TriState.False)
            oldGross = lvCoverages.Items(lvCoverages.FocusedItem.Index).SubItems(16).Text
            oldNet = lvCoverages.Items(lvCoverages.FocusedItem.Index).SubItems(19).Text
            oldSF = lvCoverages.Items(lvCoverages.FocusedItem.Index).SubItems(18).Text
            oldTerm = lvCoverages.Items(lvCoverages.FocusedItem.Index).SubItems(15).Text
            oldEffect = lvCoverages.Items(lvCoverages.FocusedItem.Index).SubItems(13).Text
            oldExpiry = lvCoverages.Items(lvCoverages.FocusedItem.Index).SubItems(14).Text
            memCode = lvCoverages.Items(lvCoverages.FocusedItem.Index).SubItems(3).Text

            Me.Close()
        End If
    End Sub
End Class