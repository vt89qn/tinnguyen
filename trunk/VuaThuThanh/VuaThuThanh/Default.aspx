<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="VuaThuThanh.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Ext.net" Namespace="Ext.Net" TagPrefix="ext" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="Form1" runat="server">
    <ext:ResourceManager runat="server" />
    <ext:Viewport runat="server" Layout="FitLayout">
        <Items>
            <ext:Panel runat="server" Layout="FitLayout" Header="false" Border="false" BodyPadding="15">
                <Items>
                    <ext:Window ID="WindowLogin" runat="server" Layout="FitLayout" BodyPadding="5" Closable="false"
                        Resizable="false" Draggable="false" Modal="true" Title="Vua Thủ Thành">
                        <Items>
                            <ext:ComboBox ID="txtProfile" runat="server" Editable="false" EmptyText="Chọn Profile"
                                FieldLabel="Profile" LabelWidth="40">
                                <Items>
                                    <ext:ListItem Text="Tin" Value="tin" />
                                    <ext:ListItem Text="Thi" Value="thi" />
                                </Items>
                                <SelectedItems>
                                    <ext:ListItem Text="Tin" Value="tin" />
                                </SelectedItems>
                            </ext:ComboBox>
                        </Items>
                        <Buttons>
                            <ext:Button ID="Button1" runat="server" Text="Login" Icon="Accept">
                                <DirectEvents>
                                    <Click OnEvent="btnLogin_Click">
                                        <EventMask ShowMask="true" Msg="Đang đăng nhập" MinDelay="500" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                        </Buttons>
                    </ext:Window>
                    <ext:Panel ID="WindowWorking" runat="server" Title="Vua Thủ Thành" Layout="VBoxLayout"
                        BodyPadding="15" Hidden="true">
                        <LayoutConfig>
                            <ext:VBoxLayoutConfig Align="Stretch" />
                        </LayoutConfig>
                        <Items>
                            <ext:Button ID="btnChayQuanLenh" runat="server" Text="Thủ Thành" Icon="ApplicationGo"
                                Height="35" Margins="0 0 5 0">
                                <DirectEvents>
                                    <Click OnEvent="btnChayQuanLenh_Click" Timeout="600000">
                                        <EventMask ShowMask="true" Msg="Đang thủ thành" MinDelay="500" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="btnLeoThap" runat="server" Text="Leo Tháp" Icon="ApplicationGo" Height="35"
                                Margins="0 0 5 0">
                                <DirectEvents>
                                    <Click OnEvent="btnLeoThap_Click" Timeout="600000">
                                        <EventMask ShowMask="true" Msg="Đang leo tháp" MinDelay="500" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="btnDuoiTuong" runat="server" Text="Đuổi Tướng" Icon="ApplicationGo"
                                Height="35" Margins="0 0 5 0">
                                <DirectEvents>
                                    <Click OnEvent="btnDuoiTuong_Click" Timeout="600000">
                                        <EventMask ShowMask="true" Msg="Đang đuổi tướng" MinDelay="500" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="btnGhepManhTuong" runat="server" Text="Ghép Mảnh Tướng" Icon="ApplicationGo"
                                Height="35" Margins="0 0 5 0">
                                <DirectEvents>
                                    <Click OnEvent="btnGhepManhTuong_Click" Timeout="600000" />
                                </DirectEvents>
                            </ext:Button>
                            <ext:Panel Layout="HBoxLayout" Border="false">
                                <Defaults>
                                    <ext:Parameter Name="margins" Value="0 5 0 0" Mode="Value" />
                                </Defaults>
                                <Items>
                                    <ext:ComboBox ID="cmbLoai" runat="server" Editable="false" FieldLabel="Tên đồ" LabelWidth="60"
                                        Flex="1" Height="35">
                                        <Items>
                                            <ext:ListItem Text="Thạch Phong" Value="wind_stone" />
                                            <ext:ListItem Text="Thạch Lôi" Value="thunder_stone" />
                                            <ext:ListItem Text="Thạch Quang" Value="light_stone" />
                                            <ext:ListItem Text="Thạch Thủy" Value="water_stone" />
                                            <ext:ListItem Text="Thạch Hỏa" Value="fire_stone" />
                                            <ext:ListItem Text="Thạch Độc" Value="poison_stone" />
                                            <ext:ListItem Text="Kiếm" Value="sword" />
                                            <ext:ListItem Text="Thương" Value="spear" />
                                            <ext:ListItem Text="Cung" Value="bow" />
                                            <ext:ListItem Text="Quạt" Value="fan" />
                                        </Items>
                                        <SelectedItems>
                                            <ext:ListItem Text="Thạch Phong" Value="wind_stone" />
                                        </SelectedItems>
                                    </ext:ComboBox>
                                    <ext:ComboBox ID="cmbCap" runat="server" Editable="false" FieldLabel="Cấp" LabelWidth="60"
                                        Flex="1" Height="35">
                                        <Items>
                                            <ext:ListItem Text="1" Value="01" />
                                            <ext:ListItem Text="2" Value="02" />
                                            <ext:ListItem Text="3" Value="03" />
                                            <ext:ListItem Text="4" Value="04" />
                                            <ext:ListItem Text="5" Value="05" />
                                        </Items>
                                        <SelectedItems>
                                            <ext:ListItem Text="1" Value="01" />
                                        </SelectedItems>
                                    </ext:ComboBox>
                                    <ext:ComboBox ID="cmbSoLuong" runat="server" Editable="false" FieldLabel="Số lượng"
                                        LabelWidth="60" Flex="1" Height="35">
                                        <Items>
                                            <ext:ListItem Text="1" Value="1" />
                                            <ext:ListItem Text="3" Value="3" />
                                            <ext:ListItem Text="5" Value="5" />
                                            <ext:ListItem Text="10" Value="10" />
                                            <ext:ListItem Text="Hết" Value="0" />
                                        </Items>
                                        <SelectedItems>
                                            <ext:ListItem Text="Hết" Value="0" />
                                        </SelectedItems>
                                    </ext:ComboBox>
                                    <ext:Button ID="btnGhepDo" runat="server" Text="Ghép đồ" Icon="ApplicationGo" Height="35"
                                        Margins="0 0 5 0" Flex="1">
                                        <DirectEvents>
                                            <Click OnEvent="btnGhepDo_Click" Timeout="600000">
                                                <EventMask ShowMask="true" Msg="Đang ghép đồ" MinDelay="500" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Panel>
                            <ext:Panel Layout="HBoxLayout" Border="false">
                                <Defaults>
                                    <ext:Parameter Name="margins" Value="0 5 0 0" Mode="Value" />
                                </Defaults>
                                <Items>
                                    <ext:ComboBox ID="txtTenAi" runat="server" Editable="false" FieldLabel="Tên Ải" LabelWidth="60"
                                        Flex="1" Height="35">
                                        <Items>
                                            <ext:ListItem Text="Định Đào" Value="dinh_dao" />
                                            <ext:ListItem Text="Hợp Phì" Value="hop_phi" />
                                            <ext:ListItem Text="Hoa Dung" Value="hoa_dung" />
                                            <ext:ListItem Text="Xích Bích" Value="xich_bich" />
                                            <ext:ListItem Text="Ngưu Chữ" Value="nguu_chu" />
                                            <ext:ListItem Text="Hổ Lao Quan" Value="ho_lao_quan" />
                                            <ext:ListItem Text="Dốc Bắc Vọng" Value="doc_bac_vong" />
                                            <ext:ListItem Text="Di Lăng" Value="di_lang" />
                                        </Items>
                                        <SelectedItems>
                                            <ext:ListItem Text="Hổ Lao Quan" Value="ho_lao_quan" />
                                        </SelectedItems>
                                    </ext:ComboBox>
                                    <ext:ComboBox ID="txtDoKhoAi" runat="server" Editable="false" FieldLabel="Độ Khó"
                                        LabelWidth="60" Flex="1" Height="35">
                                        <Items>
                                            <ext:ListItem Text="1 *" Value="1" />
                                            <ext:ListItem Text="2 *" Value="2" />
                                            <ext:ListItem Text="3 *" Value="3" />
                                        </Items>
                                        <SelectedItems>
                                            <ext:ListItem Text="3 *" Value="3" />
                                        </SelectedItems>
                                    </ext:ComboBox>
                                    <ext:Button ID="btnVuotAi" runat="server" Text="Vượt Ải" Icon="ApplicationGo" Height="35"
                                        Margins="0 0 5 0" Flex="1">
                                        <DirectEvents>
                                            <Click OnEvent="btnVuotAi_Click" Timeout="600000">
                                                <EventMask ShowMask="true" Msg="Đang vượt ải" MinDelay="500" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Panel>
                            <ext:Panel Layout="HBoxLayout" Border="false">
                                <Defaults>
                                    <ext:Parameter Name="margins" Value="0 5 0 0" Mode="Value" />
                                </Defaults>
                                <Items>
                                    <ext:ComboBox ID="txtLoaiQuanLenh" runat="server" Editable="false" EmptyText="Chọn loại cần dùng"
                                        Flex="1" Height="35">
                                        <Items>
                                            <ext:ListItem Text="QL sơn trại" Value="defense_turn_count_restore_00" />
                                            <ext:ListItem Text="QL sơ cấp" Value="defense_turn_count_restore_01" />
                                            <ext:ListItem Text="QL trung cấp" Value="defense_turn_count_restore_02" />
                                            <ext:ListItem Text="QL cao cấp" Value="defense_turn_count_restore_03" />
                                            <ext:ListItem Text="CC sơ cấp" Value="attack_turn_count_restore_01" />
                                            <ext:ListItem Text="CC trung cấp" Value="attack_turn_count_restore_02" />
                                            <ext:ListItem Text="CC cao cấp" Value="attack_turn_count_restore_03" />
                                        </Items>
                                    </ext:ComboBox>
                                    <ext:TextField ID="txtSoluongQL" runat="server" Text="1" EmptyText="Số lượng cần dùng"
                                        Flex="1" Height="35" />
                                    <ext:Button ID="btnNuotQuanLenh" runat="server" Text="Nuốt Quân Lệnh / Cờ Chiến"
                                        Icon="ApplicationGo" AnchorHorizontal="100%" Height="35" Margins="0 0 5 0" Flex="2">
                                        <DirectEvents>
                                            <Click OnEvent="btnNuotQuanLenh_Click" Timeout="600000">
                                                <EventMask ShowMask="true" Msg="Đang nuốt quân lệnh / cờ chiến" MinDelay="500" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Panel>
                            <ext:TextArea Flex="1" runat="server" ReadOnly="true" ID="txtStatus" />
                        </Items>
                    </ext:Panel>
                    <ext:Window runat="server" ID="windowGhepManhTuong" BodyPadding="5" Modal="true"
                        Layout="AnchorLayout" Hidden="true">
                        <Items>
                            <ext:ComboBox ID="txtChonManhTuong" runat="server" Editable="false" EmptyText="Chọn mảnh tướng"
                                TypeAhead="true" DisplayField="manhtuong_name" ValueField="manhtuong_id" Flex="1"
                                Height="35" AnchorHorizontal="100%" Margins="5 0 0 0">
                                <Store>
                                    <ext:Store runat="server" ID="manhTuongStore">
                                        <Model>
                                            <ext:Model runat="server" IDProperty="Id">
                                                <Fields>
                                                    <ext:ModelField Name="manhtuong_id" Type="String" ServerMapping="manhtuong_id" />
                                                    <ext:ModelField Name="manhtuong_name" Type="String" ServerMapping="manhtuong_name" />
                                                </Fields>
                                            </ext:Model>
                                        </Model>
                                    </ext:Store>
                                </Store>
                                <DirectEvents>
                                    <Change OnEvent="txtChonManhTuong_Change" Timeout="600000" />
                                </DirectEvents>
                            </ext:ComboBox>
                            <ext:TextField ID="txtSoLuongManhTuong" runat="server" EmptyText="Số lượng cần dùng"
                                AnchorHorizontal="100%" Margins="5 0 0 0" />
                            <ext:Button ID="btnChonManhTuong" runat="server" Text="Chọn" Icon="ApplicationGo"
                                AnchorHorizontal="100%" Margins="5 0 0 0" Height="35">
                                <DirectEvents>
                                    <Click OnEvent="btnChonManhTuong_Click" Timeout="600000" />
                                </DirectEvents>
                            </ext:Button>
                            <ext:TextArea ID="txtManhTuongDaChon" runat="server" ReadOnly="true" Margins="5 0 0 0"
                                AnchorHorizontal="100%" Height="100">
                            </ext:TextArea>
                        </Items>
                        <Buttons>
                            <ext:Button runat="server" Icon="Cancel" Text="Hủy">
                                <DirectEvents>
                                    <Click OnEvent="btnGhepManhTuongHuy_Click" />
                                </DirectEvents>
                            </ext:Button>
                        </Buttons>
                    </ext:Window>
                </Items>
            </ext:Panel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
