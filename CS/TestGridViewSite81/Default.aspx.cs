using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevExpress.Web;
using System.Collections;

public partial class _Default : System.Web.UI.Page 
{
    DataSet ds = null;
	protected void Page_Init(object sender, EventArgs e)
    {
		if (!IsPostBack || (Session["DataSet"] == null)) {
            ds = new DataSet();
            DataTable masterTable = new DataTable();
            masterTable.Columns.Add("ID", typeof(int));
            masterTable.Columns.Add("Data", typeof(string));
            masterTable.PrimaryKey = new DataColumn[] { masterTable.Columns["ID"] };

            DataTable detailTable = new DataTable();
            detailTable.Columns.Add("ID", typeof(int));
            detailTable.Columns.Add("MasterID", typeof(int));
            detailTable.Columns.Add("Data", typeof(string));
            detailTable.PrimaryKey = new DataColumn[] { detailTable.Columns["ID"] };
            int index = 0;
            for(int i = 0;i < 20;i++) {
                masterTable.Rows.Add(new object[] { i, "Master Row " + i });
                for(int j = 0;j < 5;j++)
                    detailTable.Rows.Add(new object[] { index++, i, "Detail Row " + j });
            }
            ds.Tables.AddRange(new DataTable[] { masterTable, detailTable });
            Session["DataSet"] = ds;
        }
        else
            ds = (DataSet)Session["DataSet"];
        ASPxGridView1.DataSource = ds.Tables[0];
        ASPxGridView1.DataBind();
    }
    protected void ASPxGridView2_BeforePerformDataSelect(object sender, EventArgs e) {
        ds = (DataSet)Session["DataSet"];
        DataTable detailTable = ds.Tables[1];
        DataView dv = new DataView(detailTable);
        ASPxGridView detailGridView = (ASPxGridView)sender;
        dv.RowFilter = "MasterID = " + detailGridView.GetMasterRowKeyValue();
        detailGridView.DataSource = dv;
    }
    protected void ASPxGridView1_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e) {
        ds = (DataSet)Session["DataSet"];
        ASPxGridView gridView = (ASPxGridView)sender;
        DataTable dataTable = gridView.GetMasterRowKeyValue() != null ? ds.Tables[1] : ds.Tables[0];
        DataRow row = dataTable.Rows.Find(e.Keys[0]);
        IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
        enumerator.Reset();
        while(enumerator.MoveNext())
            row[enumerator.Key.ToString()] = enumerator.Value;
        gridView.CancelEdit();
        e.Cancel = true;
    }
    protected void ASPxGridView1_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e) {
        ds = (DataSet)Session["DataSet"];
        ASPxGridView gridView = (ASPxGridView)sender;
		DataTable dataTable = gridView.GetMasterRowKeyValue() != null ? ds.Tables[1] : ds.Tables[0];
        DataRow row = dataTable.NewRow();
        e.NewValues["ID"] = GetNewId();
        IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
        enumerator.Reset();
        while(enumerator.MoveNext())
            if(enumerator.Key.ToString() != "Count")
                row[enumerator.Key.ToString()] = enumerator.Value;
        gridView.CancelEdit();
        e.Cancel = true;
        dataTable.Rows.Add(row);
    }

    protected void ASPxGridView1_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e) {
        int i = ASPxGridView1.FindVisibleIndexByKeyValue(e.Keys[ASPxGridView1.KeyFieldName]);
        Control c = ASPxGridView1.FindDetailRowTemplateControl(i, "ASPxGridView2");
        e.Cancel = true;
        ds = (DataSet)Session["DataSet"];
        ds.Tables[0].Rows.Remove(ds.Tables[0].Rows.Find(e.Keys[ASPxGridView1.KeyFieldName]));
   
    }
    private int GetNewId() {
        ds = (DataSet)Session["DataSet"];
        DataTable table= ds.Tables[0];
        if (table.Rows.Count == 0) return 0;
        int max = Convert.ToInt32(table.Rows[0]["ID"]);
        for (int i = 1; i < table.Rows.Count; i++) {
            if (Convert.ToInt32(table.Rows[i]["ID"]) > max)
                max = Convert.ToInt32(table.Rows[i]["ID"]);
        }
        return max + 1;
    }
}
