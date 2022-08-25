<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.1, Version=8.1.6.0, Culture=neutral, PublicKeyToken=79868b8147b5eae4"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.1, Version=8.1.6.0, Culture=neutral, PublicKeyToken=79868b8147b5eae4"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <dxwgv:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False"
            KeyFieldName="ID" OnRowUpdating="ASPxGridView1_RowUpdating" Width="588px" OnRowDeleting="ASPxGridView1_RowDeleting" OnRowInserting="ASPxGridView1_RowInserting">
            <Columns>
                <dxwgv:GridViewCommandColumn VisibleIndex="0">
                    <EditButton Visible="True">
                    </EditButton>
                    <DeleteButton Visible="True">
                    </DeleteButton>
                    <NewButton Visible="True">
                    </NewButton>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataTextColumn FieldName="ID" ReadOnly="True" VisibleIndex="1">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="Data" VisibleIndex="2">
                </dxwgv:GridViewDataTextColumn>
            </Columns>
            <SettingsDetail ShowDetailRow="True" />
            <Templates>
                <DetailRow>
                    <dxwgv:ASPxGridView ID="ASPxGridView2" runat="server" AutoGenerateColumns="False"
                        KeyFieldName="ID" OnBeforePerformDataSelect="ASPxGridView2_BeforePerformDataSelect"
                        OnRowUpdating="ASPxGridView1_RowUpdating" Width="100%">
                        <Columns>
                            <dxwgv:GridViewCommandColumn VisibleIndex="0">
                                <EditButton Visible="True">
                                </EditButton>
                            </dxwgv:GridViewCommandColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="ID" ReadOnly="True" VisibleIndex="1">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="MasterID" ReadOnly="True" VisibleIndex="2">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="Data" VisibleIndex="3">
                            </dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <SettingsDetail IsDetailGrid="True" />
                    </dxwgv:ASPxGridView>
                </DetailRow>
            </Templates>
        </dxwgv:ASPxGridView>
    
    </div>
    </form>
</body>
</html>
