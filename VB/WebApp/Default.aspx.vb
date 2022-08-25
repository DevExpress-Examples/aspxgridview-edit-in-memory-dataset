Imports DevExpress.Web
Imports System
Imports System.Collections
Imports System.Data
Imports System.Web.UI

Namespace WebApp

    Public Partial Class [Default]
        Inherits Page

        Private ds As DataSet = Nothing

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
            If Not IsPostBack OrElse Session("DataSet") Is Nothing Then
                ds = New DataSet()
                Dim masterTable As DataTable = New DataTable()
                masterTable.Columns.Add("ID", GetType(Integer))
                masterTable.Columns.Add("Data", GetType(String))
                masterTable.PrimaryKey = New DataColumn() {masterTable.Columns("ID")}
                Dim detailTable As DataTable = New DataTable()
                detailTable.Columns.Add("ID", GetType(Integer))
                detailTable.Columns.Add("MasterID", GetType(Integer))
                detailTable.Columns.Add("Data", GetType(String))
                detailTable.PrimaryKey = New DataColumn() {detailTable.Columns("ID")}
                Dim index As Integer = 0
                For i As Integer = 0 To 20 - 1
                    masterTable.Rows.Add(New Object() {i, "Master Row " & i})
                    For j As Integer = 0 To 5 - 1
                        detailTable.Rows.Add(New Object() {Math.Min(Threading.Interlocked.Increment(index), index - 1), i, "Detail Row " & j})
                    Next
                Next

                ds.Tables.AddRange(New DataTable() {masterTable, detailTable})
                Session("DataSet") = ds
            Else
                ds = CType(Session("DataSet"), DataSet)
            End If

            MasterGridView.DataSource = ds.Tables(0)
            MasterGridView.DataBind()
        End Sub

        Protected Sub DetailGridView_BeforePerformDataSelect(ByVal sender As Object, ByVal e As EventArgs)
            ds = CType(Session("DataSet"), DataSet)
            Dim detailTable As DataTable = ds.Tables(1)
            Dim dv As DataView = New DataView(detailTable)
            Dim detailGridView As ASPxGridView = CType(sender, ASPxGridView)
            dv.RowFilter = "MasterID = " & detailGridView.GetMasterRowKeyValue()
            detailGridView.DataSource = dv
        End Sub

        Protected Sub MasterGridView_RowUpdating(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataUpdatingEventArgs)
            ds = CType(Session("DataSet"), DataSet)
            Dim gridView As ASPxGridView = CType(sender, ASPxGridView)
            Dim dataTable As DataTable = If(gridView.GetMasterRowKeyValue() IsNot Nothing, ds.Tables(1), ds.Tables(0))
            Dim row As DataRow = dataTable.Rows.Find(e.Keys(0))
            Dim enumerator As IDictionaryEnumerator = e.NewValues.GetEnumerator()
            enumerator.Reset()
            While enumerator.MoveNext()
                row(enumerator.Key.ToString()) = enumerator.Value
            End While

            gridView.CancelEdit()
            e.Cancel = True
        End Sub

        Protected Sub MasterGridView_RowInserting(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataInsertingEventArgs)
            ds = CType(Session("DataSet"), DataSet)
            Dim gridView As ASPxGridView = CType(sender, ASPxGridView)
            Dim dataTable As DataTable = If(gridView.GetMasterRowKeyValue() IsNot Nothing, ds.Tables(1), ds.Tables(0))
            Dim row As DataRow = dataTable.NewRow()
            e.NewValues("ID") = GetNewId()
            Dim enumerator As IDictionaryEnumerator = e.NewValues.GetEnumerator()
            enumerator.Reset()
            While enumerator.MoveNext()
                If Not Equals(enumerator.Key.ToString(), "Count") Then row(enumerator.Key.ToString()) = enumerator.Value
            End While

            gridView.CancelEdit()
            e.Cancel = True
            dataTable.Rows.Add(row)
        End Sub

        Protected Sub MasterGridView_RowDeleting(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataDeletingEventArgs)
            Dim i As Integer = MasterGridView.FindVisibleIndexByKeyValue(e.Keys(MasterGridView.KeyFieldName))
            Dim c As Control = MasterGridView.FindDetailRowTemplateControl(i, "ASPxGridView2")
            e.Cancel = True
            ds = CType(Session("DataSet"), DataSet)
            ds.Tables(CInt(0)).Rows.Remove(ds.Tables(CInt(0)).Rows.Find(e.Keys(MasterGridView.KeyFieldName)))
        End Sub

        Private Function GetNewId() As Integer
            ds = CType(Session("DataSet"), DataSet)
            Dim table As DataTable = ds.Tables(0)
            If table.Rows.Count = 0 Then Return 0
            Dim max As Integer = Convert.ToInt32(table.Rows(0)("ID"))
            For i As Integer = 1 To table.Rows.Count - 1
                If Convert.ToInt32(table.Rows(i)("ID")) > max Then max = Convert.ToInt32(table.Rows(i)("ID"))
            Next

            Return max + 1
        End Function
    End Class
End Namespace
