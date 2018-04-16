Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports DevExpress.Web
Imports System.Collections

Partial Public Class _Default
	Inherits System.Web.UI.Page

	Private ds As DataSet = Nothing
	Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
		If (Not IsPostBack) OrElse (Session("DataSet") Is Nothing) Then
			ds = New DataSet()
			Dim masterTable As New DataTable()
			masterTable.Columns.Add("ID", GetType(Integer))
			masterTable.Columns.Add("Data", GetType(String))
			masterTable.PrimaryKey = New DataColumn() { masterTable.Columns("ID") }

			Dim detailTable As New DataTable()
			detailTable.Columns.Add("ID", GetType(Integer))
			detailTable.Columns.Add("MasterID", GetType(Integer))
			detailTable.Columns.Add("Data", GetType(String))
			detailTable.PrimaryKey = New DataColumn() { detailTable.Columns("ID") }
			Dim index As Integer = 0
			For i As Integer = 0 To 19
				masterTable.Rows.Add(New Object() { i, "Master Row " & i })
				For j As Integer = 0 To 4
					detailTable.Rows.Add(New Object() {index, i, "Detail Row " & j})
					index += 1
				Next j
			Next i
			ds.Tables.AddRange(New DataTable() { masterTable, detailTable })
			Session("DataSet") = ds
		Else
			ds = DirectCast(Session("DataSet"), DataSet)
		End If
		ASPxGridView1.DataSource = ds.Tables(0)
		ASPxGridView1.DataBind()
	End Sub
	Protected Sub ASPxGridView2_BeforePerformDataSelect(ByVal sender As Object, ByVal e As EventArgs)
		ds = DirectCast(Session("DataSet"), DataSet)
		Dim detailTable As DataTable = ds.Tables(1)
		Dim dv As New DataView(detailTable)
		Dim detailGridView As ASPxGridView = DirectCast(sender, ASPxGridView)
		dv.RowFilter = "MasterID = " & detailGridView.GetMasterRowKeyValue()
		detailGridView.DataSource = dv
	End Sub
	Protected Sub ASPxGridView1_RowUpdating(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataUpdatingEventArgs)
		ds = DirectCast(Session("DataSet"), DataSet)
		Dim gridView As ASPxGridView = DirectCast(sender, ASPxGridView)
		Dim dataTable As DataTable = If(gridView.GetMasterRowKeyValue() IsNot Nothing, ds.Tables(1), ds.Tables(0))
		Dim row As DataRow = dataTable.Rows.Find(e.Keys(0))
		Dim enumerator As IDictionaryEnumerator = e.NewValues.GetEnumerator()
		enumerator.Reset()
		Do While enumerator.MoveNext()
			row(enumerator.Key.ToString()) = enumerator.Value
		Loop
		gridView.CancelEdit()
		e.Cancel = True
	End Sub
	Protected Sub ASPxGridView1_RowInserting(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataInsertingEventArgs)
		ds = DirectCast(Session("DataSet"), DataSet)
		Dim gridView As ASPxGridView = DirectCast(sender, ASPxGridView)
		Dim dataTable As DataTable = If(gridView.GetMasterRowKeyValue() IsNot Nothing, ds.Tables(1), ds.Tables(0))
		Dim row As DataRow = dataTable.NewRow()
		e.NewValues("ID") = GetNewId()
		Dim enumerator As IDictionaryEnumerator = e.NewValues.GetEnumerator()
		enumerator.Reset()
		Do While enumerator.MoveNext()
			If enumerator.Key.ToString() <> "Count" Then
				row(enumerator.Key.ToString()) = enumerator.Value
			End If
		Loop
		gridView.CancelEdit()
		e.Cancel = True
		dataTable.Rows.Add(row)
	End Sub

	Protected Sub ASPxGridView1_RowDeleting(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataDeletingEventArgs)
		Dim i As Integer = ASPxGridView1.FindVisibleIndexByKeyValue(e.Keys(ASPxGridView1.KeyFieldName))
		Dim c As Control = ASPxGridView1.FindDetailRowTemplateControl(i, "ASPxGridView2")
		e.Cancel = True
		ds = DirectCast(Session("DataSet"), DataSet)
		ds.Tables(0).Rows.Remove(ds.Tables(0).Rows.Find(e.Keys(ASPxGridView1.KeyFieldName)))

	End Sub
	Private Function GetNewId() As Integer
		ds = DirectCast(Session("DataSet"), DataSet)
		Dim table As DataTable= ds.Tables(0)
		If table.Rows.Count = 0 Then
			Return 0
		End If
		Dim max As Integer = Convert.ToInt32(table.Rows(0)("ID"))
		For i As Integer = 1 To table.Rows.Count - 1
			If Convert.ToInt32(table.Rows(i)("ID")) > max Then
				max = Convert.ToInt32(table.Rows(i)("ID"))
			End If
		Next i
		Return max + 1
	End Function
End Class
