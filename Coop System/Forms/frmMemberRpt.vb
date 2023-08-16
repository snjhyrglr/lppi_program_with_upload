Imports Microsoft.Reporting.WinForms
Public Class frmMemberRpt
    Private Sub frmMemberRpt_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ReportViewer1.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local
        ReportViewer1.LocalReport.ReportPath = System.Environment.CurrentDirectory & "\Reports\memberRpt.rdlc"
        Dim paramFname As New ReportParameter("paramFname", pFname)
        Dim paramLname As New ReportParameter("paramLname", pLname)
        Dim paramMI As New ReportParameter("paramMI", pMI)
        Dim paramSuffix As New ReportParameter("paramSuffix", pSuffix)
        ReportViewer1.LocalReport.SetParameters(New ReportParameter() {paramFname})
        ReportViewer1.LocalReport.SetParameters(New ReportParameter() {paramLname})
        ReportViewer1.LocalReport.SetParameters(New ReportParameter() {paramMI})
        ReportViewer1.LocalReport.SetParameters(New ReportParameter() {paramSuffix})
        ReportViewer1.LocalReport.DataSources.Clear()
        ReportViewer1.LocalReport.DataSources.Add(New Microsoft.Reporting.WinForms.ReportDataSource("DataSet1_DataTable1", DS.Tables(0)))
        Me.ReportViewer1.RefreshReport()
    End Sub
End Class