<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MQPushMessageDemo.aspx.cs" Inherits="PushNotificationWebAp.MQPushMessageDemo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="Label9" runat="server" Text=""></asp:Label>
        <br />
        <asp:Label ID="Label1" runat="server" Text="URI："></asp:Label><asp:TextBox ID="txtURI" runat="server" Width="808px" ReadOnly="True"></asp:TextBox>
        <br/>
        <asp:Label ID="Label2" runat="server" Text="username："></asp:Label><asp:TextBox ID="txtUserName" runat="server" ReadOnly="True"></asp:TextBox>
        <asp:Label ID="Label3" runat="server" Text="password："></asp:Label><asp:TextBox ID="txtPassword" runat="server" ReadOnly="True"></asp:TextBox>
        <br/>
        <asp:Label ID="Label8" runat="server" Text="MessageFormat："></asp:Label><asp:DropDownList ID="cboFormat" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboFormat_SelectedIndexChanged" Width="109px">
            <asp:ListItem Selected="True">Json</asp:ListItem>
            <asp:ListItem>Fix</asp:ListItem>
        </asp:DropDownList>
        <br />
        <asp:Label ID="Label10" runat="server" Text="Action："></asp:Label>
        <asp:DropDownList ID="ddlAction" runat="server">
            <asp:ListItem Value="Query">查詢</asp:ListItem>
            <asp:ListItem Value="Add">新增</asp:ListItem>
             <asp:ListItem Value="Update">更新</asp:ListItem>
             <asp:ListItem Value="Delete">刪除</asp:ListItem>
        </asp:DropDownList>
        <br />
        <asp:Label ID="Label4" runat="server" Text="Email："></asp:Label><asp:TextBox ID="txtJefferiesTopicName" runat="server" Width="926px">leon08162000@gmail.com;leon08162001@gmail.com;leon08162002@gmail.com</asp:TextBox>
        <br/>
        <asp:Label ID="Label5" runat="server" Text="主旨："></asp:Label><asp:TextBox ID="txtOTATopicName" runat="server" Width="566px" ReadOnly="True">貸款申請核准通知</asp:TextBox>
        <br/>
        <asp:Label ID="Label6" runat="server" Text="Message："></asp:Label><asp:TextBox ID="txtMessage" runat="server" Rows="10" TextMode="MultiLine" Width="746px"></asp:TextBox>
        <br />
        <asp:Label ID="Label7" runat="server" Text="發送筆數："></asp:Label><asp:TextBox ID="txtMessageNums" runat="server" Width="102px">1</asp:TextBox>
        <br />
        <asp:Button ID="btn_Send" runat="server" Text="發送訊息" OnClick="btnPushMessage_Click" />
    </div>
    </form>
</body>
</html>
