<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApp.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <dx:ASPxGridView ID="MasterGridView" runat="server" AutoGenerateColumns="False" SettingsPager-PageSize="3"
                KeyFieldName="ID" OnRowUpdating="MasterGridView_RowUpdating" Width="588px" OnRowDeleting="MasterGridView_RowDeleting" OnRowInserting="MasterGridView_RowInserting">
                <Columns>
                    <dx:GridViewCommandColumn VisibleIndex="0" ShowEditButton="True" ShowDeleteButton="True" ShowNewButton="True" />
                    <dx:GridViewDataTextColumn FieldName="ID" ReadOnly="True" VisibleIndex="1">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Data" VisibleIndex="2">
                    </dx:GridViewDataTextColumn>
                </Columns>
                <SettingsDetail ShowDetailRow="True" />
                <Templates>
                    <DetailRow>
                        <dx:ASPxGridView ID="DetailGridView" runat="server" AutoGenerateColumns="False" SettingsPager-PageSize="4"
                            KeyFieldName="ID" OnBeforePerformDataSelect="DetailGridView_BeforePerformDataSelect"
                            OnRowUpdating="MasterGridView_RowUpdating" Width="100%">
                            <Columns>
                                <dx:GridViewCommandColumn VisibleIndex="0" ShowEditButton="True" />
                                <dx:GridViewDataTextColumn FieldName="ID" ReadOnly="True" VisibleIndex="1">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="MasterID" ReadOnly="True" VisibleIndex="2">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="Data" VisibleIndex="3">
                                </dx:GridViewDataTextColumn>
                            </Columns>
                        </dx:ASPxGridView>
                    </DetailRow>
                </Templates>
            </dx:ASPxGridView>
        </div>
    </form>
</body>
</html>
