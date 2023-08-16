Imports Microsoft.Reporting.WinForms
Public Class frmReportGYRT
    Dim preparedBy As New ReportParameter("preparedBy", forName)
    Dim coopNeym As New ReportParameter("coopName", coopName)
    Dim d_Effect As New ReportParameter("d_Effect", frmGYRT.txtEffectivity.Text)
    Dim d_Expire As New ReportParameter("d_Expire", frmGYRT.txtExpiry.Text)
    Dim sumPrem As New ReportParameter("sumPrem", frmGYRT.txtTotal.Text)
    Private Sub frmReportGYRT_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ReportViewer1.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local
        ReportViewer1.LocalReport.ReportPath = System.Environment.CurrentDirectory & "\Reports\rptGYRT.rdlc"
        ReportViewer1.LocalReport.SetParameters(New ReportParameter() {preparedBy})
        ReportViewer1.LocalReport.SetParameters(New ReportParameter() {coopNeym})
        ReportViewer1.LocalReport.SetParameters(New ReportParameter() {d_Effect})
        ReportViewer1.LocalReport.SetParameters(New ReportParameter() {d_Expire})
        ReportViewer1.LocalReport.SetParameters(New ReportParameter() {sumPrem})
        ReportViewer1.LocalReport.DataSources.Clear()
        ReportViewer1.LocalReport.DataSources.Add(New Microsoft.Reporting.WinForms.ReportDataSource("DataSet1_DataTable2", DS.Tables(1)))
        Me.ReportViewer1.RefreshReport()
    End Sub
End Class