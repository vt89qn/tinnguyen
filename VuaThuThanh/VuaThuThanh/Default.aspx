<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="VuaThuThanh.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Thư Viện Vua Thủ Thành</title>
    <link href="CSS/style.css" rel="stylesheet" />
    <link rel="shortcut icon" href="/Images/Misc/favicon.ico" type="image/x-icon">
</head>
<body>
    <form runat="server">
    <ext:ResourceManager runat="server" />
    <ext:Viewport ID="viewMain" runat="server" Layout="FitLayout">
        <Items>
            <ext:Panel ID="panelTuong" runat="server" TitleCollapse="true" Layout="AnchorLayout">
                <Items>
                    <ext:Toolbar AnchorHorizontal="100%" Frame="false">
                        <Items>
                            <ext:Button ID="btnDanhSachTuong" runat="server" Text="Danh Sách Tướng" />
                            <ext:ToolbarSeparator />
                            <ext:Button ID="btnNuoiHeo" runat="server" Text="Nuôi Heo" OnClientClick="Ext.Msg.alert('Vua Thủ Thành', 'Tính năng đang phát triển...');" />
                        </Items>
                    </ext:Toolbar>
                    <ext:FormPanel ID="Panel3" runat="server" Frame="true" PaddingSummary="5px 5px 0"
                        ButtonAlign="Center" Height="120" AnchorHorizontal="100%" TitleCollapse="false">
                        <Items>
                            <ext:Container runat="server" Layout="Column" Height="35">
                                <Items>
                                    <ext:Container runat="server" Layout="FormLayout" ColumnWidth=".25" Padding="5">
                                        <Items>
                                            <ext:TextField ID="txtTenTuong" runat="server" FieldLabel="Tên Tướng" LabelWidth="100"
                                                EmptyText="Điền Tục / Dien Tuc / DienTuc" />
                                        </Items>
                                    </ext:Container>
                                    <ext:Container runat="server" Layout="FormLayout" ColumnWidth=".25" Padding="5">
                                        <Items>
                                            <ext:MultiCombo ID="txtNuoc" runat="server" FieldLabel="Nước" LabelWidth="100" Editable="false">
                                                <Items>
                                                    <ext:ListItem Text="Ngụy" Value="1" />
                                                    <ext:ListItem Text="Thục" Value="2" />
                                                    <ext:ListItem Text="Ngô" Value="3" />
                                                    <ext:ListItem Text="Khác" Value="4" />
                                                </Items>
                                            </ext:MultiCombo>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container runat="server" Layout="FormLayout" ColumnWidth=".25" Padding="5">
                                        <Items>
                                            <ext:MultiCombo ID="txtPhai" runat="server" FieldLabel="Phái" LabelWidth="100" Editable="false">
                                                <Items>
                                                    <ext:ListItem Text="Kiếm" Value="1" />
                                                    <ext:ListItem Text="Thương" Value="2" />
                                                    <ext:ListItem Text="Cung" Value="3" />
                                                    <ext:ListItem Text="Quạt" Value="4" />
                                                </Items>
                                            </ext:MultiCombo>
                                        </Items>
                                    </ext:Container>
                                    <ext:Container runat="server" Layout="FormLayout" ColumnWidth=".25" Padding="5">
                                        <Items>
                                            <ext:MultiCombo ID="txtSoSao" runat="server" FieldLabel="Số Sao" LabelWidth="100"
                                                Editable="false">
                                                <Items>
                                                    <ext:ListItem Text="1" Value="1" />
                                                    <ext:ListItem Text="2" Value="2" />
                                                    <ext:ListItem Text="3" Value="3" />
                                                    <ext:ListItem Text="4" Value="4" />
                                                    <ext:ListItem Text="5" Value="5" />
                                                </Items>
                                            </ext:MultiCombo>
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:Container>
                            <ext:Container runat="server" Layout="Column" Height="35">
                                <Items>
                                    <ext:Container runat="server" Layout="FormLayout" ColumnWidth=".25" Padding="5">
                                        <Items>
                                            <ext:TextField ID="txtLucDanh" runat="server" FieldLabel="Lực Đánh(>=)" LabelWidth="100" />
                                        </Items>
                                    </ext:Container>
                                    <ext:Container runat="server" Layout="FormLayout" ColumnWidth=".25" Padding="5">
                                        <Items>
                                            <ext:TextField ID="txtPhamVi" runat="server" FieldLabel="Phạm Vi(>=)" LabelWidth="100" />
                                        </Items>
                                    </ext:Container>
                                    <ext:Container runat="server" Layout="FormLayout" ColumnWidth=".25" Padding="5">
                                        <Items>
                                            <ext:TextField ID="txtTocDo" runat="server" FieldLabel="Tốc Đánh(>=)" LabelWidth="100" />
                                        </Items>
                                    </ext:Container>
                                    <ext:Container runat="server" Layout="FormLayout" ColumnWidth=".25" Padding="5">
                                        <Items>
                                            <ext:TextField ID="txtTriLuc" runat="server" FieldLabel="Trí Lực(>=)" LabelWidth="100" />
                                        </Items>
                                    </ext:Container>
                                </Items>
                            </ext:Container>
                        </Items>
                        <Buttons>
                            <ext:Button ID="btnLoc" runat="server" Text="Lọc" Height="35" Width="105" Icon="Accept">
                                <DirectEvents>
                                    <Click OnEvent="btnLocTuong_Click" Timeout="0">
                                        <EventMask ShowMask="true" Msg="Đang lọc dữ liệu" MinDelay="100" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                        </Buttons>
                    </ext:FormPanel>
                    <ext:DataView ID="viewTuong" runat="server" DeferInitialRefresh="false" ItemSelector="div.tuong"
                        OverItemCls="tuong-hover" MultiSelect="true" AutoScroll="true" Cls="tuong-view"
                        TrackOver="true" AnchorHorizontal="100%" AnchorVertical="-148">
                        <Store>
                            <ext:Store ID="storeTuong" runat="server">
                                <Model>
                                    <ext:Model ID="modelTuong" runat="server" IDProperty="Pid">
                                        <Fields>
                                            <ext:ModelField Name="Pid" Type="Int" />
                                            <ext:ModelField Name="MaTuong" Type="String" />
                                            <ext:ModelField Name="TenTuong" Type="String" />
                                            <ext:ModelField Name="SoSao" Type="Int" />
                                            <ext:ModelField Name="NuocXid" Type="String" />
                                        </Fields>
                                    </ext:Model>
                                </Model>
                                <%--         <Sorters>
                                    <ext:DataSorter Property="NuocXid" Direction="ASC" />
                                    <ext:DataSorter Property="SoSao" Direction="ASC" />
                                </Sorters>--%>
                            </ext:Store>
                        </Store>
                        <Tpl ID="tplTuong" runat="server">
                            <Html>
                                <tpl for=".">
                                <div class="tuong nuoc-{NuocXid}">
                                        <img src="?tuong={[values.MaTuong]}" />
                                    <strong>{TenTuong}</strong>
                                </div>
                                </tpl>
                            </Html>
                        </Tpl>
                        <DirectEvents>
                            <ItemClick OnEvent="viewTuongItem_Click" Timeout="0">
                                <EventMask ShowMask="true" Msg="Đang tải dữ liệu" MinDelay="100" />
                            </ItemClick>
                        </DirectEvents>
                        <%--           <Plugins>
                            <ext:DataViewAnimated ID="animatedTuong" runat="server" Duration="550" IDProperty="id" />
                        </Plugins>--%>
                    </ext:DataView>
                </Items>
                <BottomBar>
                    <ext:Toolbar EnableOverflow="true" Frame="false">
                        <Items>
                            <ext:Label Text="VuaThuThanh.Net@2014 - Nhóm phát triển fb.com/vuathuthanh.net - Email liên hệ : lienhe@vuathuthanh.net" />
                        </Items>
                    </ext:Toolbar>
                </BottomBar>
            </ext:Panel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
