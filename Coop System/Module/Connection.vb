Imports MySql.Data.MySqlClient
Imports System.Configuration
Module Connection
    Public sql_db As String = ConfigurationManager.AppSettings.Get("SQLDb")
    Public sql_user As String = ConfigurationManager.AppSettings.Get("SQLuser")
    Public sql_pass As String = ConfigurationManager.AppSettings.Get("SQLpass")
    Public sql_server As String = ConfigurationManager.AppSettings.Get("SQLServer")
    Public sql_port As String = ConfigurationManager.AppSettings.Get("SQLport")
    Public Myconn As New MySqlConnection

    Public conn As String = "server=" & sql_server & "; port=" & sql_port & "; user id=" & sql_user & "; password=" & sql_pass & "; database=" & sql_db & ""

    Public mydr As MySqlDataReader
    Public myda As MySqlDataAdapter
    Public mycmd As MySqlCommand
    Public mydt As DataTable
    Public qry As String
    Public sample As String
    Public tbl As String
    Public user As String
    Public act As String
    Public tran_id As String

    Public bCode As String
    Public bDesc As String
    Public bDate As String
    Public coopName As String

    Public unuseTerm As Integer
    Public unusePrem As Double
    Public terms As Integer
    Public loanAmt As Double
    Public grossPremium As Double
    Public netPremium As Double
    Public serviceFee As Double
    Public prevNetPrem As Double
    Public prevTerm As Double
    Public forID As Integer
    Public forBatch As String
    Public premRateStan As Double
    Public coopCommStan As Double

    Public forUsername As String
    Public forName As String
    Public forMemCode As String
    Public forRepTitle As String

    Public unusedID As Integer
    Public oldNet As Double
    Public oldGross As Double
    Public oldSF As Double
    Public oldTerm As Integer
    Public oldEffect As String
    Public memCode As String
    Public oldExpiry As String

    Public pFname As String
    Public pLname As String
    Public pMI As String
    Public pSuffix As String

    Public loanTypeID As String

    Public forUpdateBatch As Integer
    Public DS As New DataSet1()

    Public Sub konek()
        If Myconn.State = ConnectionState.Open Then
            Myconn.Close()
            Myconn.ConnectionString = conn
            Myconn.Open()
        Else
            Myconn.ConnectionString = conn
            Myconn.Open()
        End If
    End Sub
    Public Sub trail()
        konek()
        qry = "INSERT into tbl_trail (id,username,action,dt_action,tran_code) values ('','" & user & "','" & act & "','" & Date.Now & "','" & tran_id & "')"
        mycmd = New MySqlCommand(qry, Myconn)
        mycmd.ExecuteNonQuery()
    End Sub
End Module
