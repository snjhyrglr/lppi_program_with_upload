Imports Microsoft.Reporting.WinForms
Public Class frmReport

    Private Sub frmReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim preparedBy As New ReportParameter("preparedBy", forName)
        Dim coopNeym As New ReportParameter("coopName", coopName)
        Dim baCode As New ReportParameter("batchCode", bCode)
        Dim baDesc As New ReportParameter("batchDesc", bDesc)
        Dim baDate As New ReportParameter("batchDate", bDate)
        Dim repTitle As New ReportParameter("reportTitle", forRepTitle)

        ReportViewer1.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local
        ReportViewer1.LocalReport.ReportPath = System.Environment.CurrentDirectory & "\Reports\Report1.rdlc"
        ReportViewer1.LocalReport.SetParameters(New ReportParameter() {preparedBy})
        ReportViewer1.LocalReport.SetParameters(New ReportParameter() {coopNeym})
        ReportViewer1.LocalReport.SetParameters(New ReportParameter() {baCode})
        ReportViewer1.LocalReport.SetParameters(New ReportParameter() {baDesc})
        ReportViewer1.LocalReport.SetParameters(New ReportParameter() {baDate})
        ReportViewer1.LocalReport.SetParameters(New ReportParameter() {repTitle})
        ReportViewer1.LocalReport.DataSources.Clear()
        ReportViewer1.LocalReport.DataSources.Add(New Microsoft.Reporting.WinForms.ReportDataSource("DataSet1_DataTable1", DS.Tables(0)))
        Me.ReportViewer1.RefreshReport()
    End Sub
End Class